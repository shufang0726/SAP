using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Client
{
    using Automation_UIAF;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Edge;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Remote;
    using OpenQA.Selenium.Support.UI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using UIAF;
    using UIAF.Hierarchy;

    public enum BrowserType
    {
        IE = 0,
        Edge = 1,
        Firefox = 2,
        Chrome = 3,
        Safari = 4
    }
    public enum DialogType
    {
        WindowsDialog = 1,
        WebBrowser = 2
    }
    public enum CaptureMechanism
    {
        MSAA = 0,
        UIA = 1
    }

   public class UIAFUtility
    {
        private const int DEFAULT_TIMEOUT = 120;
        private static bool myIsCapture = true;
        private static RemoteWebDriver myDriver;
        private static WebDriverWait myWait;
        private static IJavaScriptExecutor myJsExecutor;
        private static Actions myActions;
        private static string rootWindowHandle;
        private static ImageFormat imageFormat = ImageFormat.Jpeg;
        public static string CaptureFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Capture\\";
        private static string[] langPickerGroup ={
           "en-us", "cs-cz","de-de","es-es","fr-fr","hu-hu","it-it","ja-jp","ko-kr","nl-nl","pl-pl","pt-br","pt-pt","ru-ru","sv-se","tr-tr","zh-cn","zh-tw"};

        private static string generateFileName(string captureCode)
        {
            string langRFCName = UIAFUtility.getValueRegEdit();
            string fileName = CaptureFolder + langRFCName + "\\" + "OMS" + "\\" + TestcaseHandler.CurrentClass + "\\" + TestcaseHandler.CurrentTestcase + "\\";
            if (!Directory.Exists(fileName))
            {
                Directory.CreateDirectory(fileName);
            }
            fileName += "\\" + "ID_" + captureCode.ToString() + ".jpg";
            return fileName;
        }
        public static void CaptureWindow(IntPtr hwnd, string fileName, DialogType dialogType, CaptureMechanism captureMechanism = CaptureMechanism.MSAA)
        {
            int startPointX;
            int startPointY;
            Bitmap bmp = GetImage(hwnd, out startPointX, out startPointY);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            bmp.Save(fileName, imageFormat);

        }
        public static Bitmap GetImage(IntPtr hwnd, out int startPointX, out int startPointY)
        {
            startPointX = 0;
            startPointY = 0;
            Win32API.RECT rc = new Win32API.RECT();
            Win32API.GetWindowRect(hwnd, ref rc);
            Rectangle rect = new Rectangle(rc.left + 8, rc.top, rc.right - rc.left - 15, rc.bottom - rc.top - 8);
            startPointX = rect.X;
            startPointY = rect.Y;
            return GetImage(rect);
        }
        private static Bitmap GetImage(Rectangle rect)
        {
            Bitmap bitmap = null;
            IntPtr displayDC = IntPtr.Zero;
            IntPtr compatibleDC = IntPtr.Zero;
            IntPtr bmp = IntPtr.Zero;

            try
            {

                displayDC = Native.CreateDC("DISPLAY",
                                                   IntPtr.Zero,
                                                   IntPtr.Zero,
                                                   IntPtr.Zero);
                if (displayDC == IntPtr.Zero)
                {
                    throw new Exception("CreateDC failed!");
                }

                compatibleDC = Native.CreateCompatibleDC(displayDC);
                if (compatibleDC == IntPtr.Zero)
                {
                    throw new Exception("CreateCompatibleDC failed!");
                }

                bmp = Native.CreateCompatibleBitmap(displayDC, rect.Width, rect.Height);

                if (Native.SelectObject(compatibleDC, bmp) == IntPtr.Zero)
                {
                    throw new Exception("CreateCompatibleBitmap failed");
                }

                if (0 == Native.BitBlt(compatibleDC, 0, 0, rect.Width, rect.Height,
                                              displayDC, rect.X, rect.Y,
                                              (uint)CopyPixelOperation.SourceCopy | (uint)CopyPixelOperation.CaptureBlt))
                {
                    throw new Exception("BitBlt failed");
                }

                bitmap = System.Drawing.Image.FromHbitmap(bmp);

            }
            finally
            {
                Native.DeleteDC(displayDC);
                Native.DeleteDC(compatibleDC);
                Native.DeleteObject(bmp);
            }

            return bitmap;
        }
        public static void CaptureIbizaUI(IntPtr handler, IWebElement ele, long captureCode)
        {
            string fileName = generateFileName(captureCode.ToString());
            Rectangle rec = new Rectangle(ele.Location.X, ele.Location.Y, ele.Size.Width, ele.Size.Height);
            CaptureIbizaUI(handler, fileName, rec);
        }
        public static void CaptureIbizaUI(IntPtr hwnd, string fileName, Rectangle rect)
        {
            Win32API.RECT rc = new Win32API.RECT();
            Win32API.GetWindowRect(hwnd, ref rc);
            Rectangle imageRect = new Rectangle();
            imageRect.X = rect.X + rc.left;
            imageRect.Y = rect.Y + rc.top;
            imageRect.Width = rect.Width;
            imageRect.Height = rect.Height;

            Bitmap bmp = GetImage(imageRect);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            bmp.Save(fileName, imageFormat);
        }
        public static void CaptureIbizaPopupMessage(IntPtr handler, IWebElement ele, long captureCode)
        {
            string fileName = generateFileName(captureCode.ToString());
            CaptureIbizaMessage(handler, fileName, ele);
        }
        public static void CaptureIbizaMessage(IntPtr hwnd, string fileName, IWebElement element)
        {
            Rectangle rect = new Rectangle();
            rect.X = element.Location.X;
            rect.Y = element.Location.Y;
            rect.Width = element.Size.Width;
            rect.Height = element.Size.Height;
            Win32API.RECT rc = new Win32API.RECT();
            Win32API.GetWindowRect(hwnd, ref rc);
            Rectangle imageRect = new Rectangle();
            imageRect.X = rect.X + rc.left;
            imageRect.Y = rect.Y + rc.top;
            imageRect.Width = rect.Width;
            imageRect.Height = rect.Height;

            Bitmap bmp = GetImage(imageRect);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            bmp.Save(fileName, imageFormat);


        }
        public static void EndProcess(string processName)
        {
            Thread.Sleep(2000);
            string name = Path.GetFileNameWithoutExtension(processName);
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.ToLower() == name.ToLower() && !process.HasExited)
                {
                    process.Kill();
                    Thread.Sleep(3000);
                }
            }
        }
        public static void InitializeWebDriver(BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.Edge:
                    EndProcess("MicrosoftWebDriver.exe");
                    EndProcess("MicrosoftEdge.exe");
                    myDriver = new EdgeDriver();
                    break;
                case BrowserType.IE:
                    EndProcess("IEDriverServer.exe");
                    EndProcess("IEXPLORE.EXE");
                    myDriver = new InternetExplorerDriver();
                    break;
                case BrowserType.Firefox:
                    EndProcess("FIREFOX.EXE");
                    myDriver = new FirefoxDriver();
                    break;
                case BrowserType.Chrome:
                    EndProcess("chromedriver.exe");
                    EndProcess("CHROME.EXE");
                    myDriver = new ChromeDriver();
                    break;
                default:
                    break;
            }

            myWait = new WebDriverWait(myDriver, TimeSpan.FromSeconds(DEFAULT_TIMEOUT));
            myJsExecutor = (IJavaScriptExecutor)myDriver;
            myActions = new Actions(myDriver);
            myDriver.Manage().Window.Maximize();
            rootWindowHandle = myDriver.CurrentWindowHandle;

        }
        private static void setProperty(string v1, string v2)
        {
            throw new NotImplementedException();
        }
        public static RemoteWebDriver CurrentWebDriver
        {
            get
            {
                return myDriver;
            }
        }
        public static WebDriverWait Wait
        {
            get
            {
                if (myDriver != null)
                {
                    if (myWait == null)
                    {
                        myWait = new WebDriverWait(myDriver, TimeSpan.FromSeconds(DEFAULT_TIMEOUT));
                    }
                    return myWait;
                }
                else
                {
                    return null;
                }

            }
        }
        public static IJavaScriptExecutor JsExcutor
        {
            get
            {
                if (myDriver != null)
                {
                    if (myJsExecutor == null)
                    {
                        myJsExecutor = (IJavaScriptExecutor)myDriver;
                    }
                    return myJsExecutor;
                }
                else
                {
                    return null;
                }
            }
        }
        public static Actions Actions
        {
            get
            {
                if (myDriver != null)
                {
                    if (myActions == null)
                    {
                        myActions = new Actions(myDriver);
                    }
                    return myActions;
                }
                else
                {
                    return null;
                }
            }
        }
        public static bool IsCapture
        {
            get { return myIsCapture; }
            set { myIsCapture = value; }
        }
        public static IWebElement CurrentBlade
        {
            get
            {
                return GetElementByCssSelector("div.fxs-journey-layout.fxs-stacklayout-horizontal.fxs-stacklayout>section:last-child");
            }
        }
        private static IWebElement getElement(ISearchContext context, By by)
        {
            if (by == null)
                return null;

            IWebElement element = null;

            //// Some mechanism for trustable click
            if (context != null)
            {
                // Context wait for the element founded
                element = myWait.Until(d =>
                {
                    IWebElement e = null;
                    try
                    {
                        e = context.FindElement(by);
                    }
                    catch (NoSuchElementException)
                    {
                    }

                    return e;
                });

                // Context wait for the element displayed
                //myWait.Until(d => { return element.Displayed; });
            }
            else if (myDriver != null)
            {
                // Driver wait for the element founded
                element = myWait.Until(d =>
                {
                    IWebElement result = null;
                    try
                    {
                        result = myDriver.FindElement(by);
                    }
                    catch (NoSuchElementException)
                    {
                    }

                    return result;
                });

                // Driver wait for the element displayed
                //myWait.Until(d => { return element.Displayed; });
            }

            return element;
        }
        private static ReadOnlyCollection<IWebElement> getElements(ISearchContext context, By by)
        {
            if (context != null)
            {
                return context.FindElements(by);
            }
            else if (myDriver != null)
            {
                return myDriver.FindElements(by);
            }
            else
            {
                return null;
            }
        }
        public static string getValueRegEdit()
        {
            string value;
            try
            {
                Microsoft.Win32.RegistryKey obj = Microsoft.Win32.Registry.CurrentUser;
                Microsoft.Win32.RegistryKey objItem = obj.OpenSubKey(@"Control Panel\International");
                value = objItem.GetValue("LocaleName").ToString().ToLower();
            }
            catch (Exception e)
            {
                return "Read Reg error!";
            }
            return value;
        }
        public static bool EnumWindowsProc(IntPtr hWnd, ref IntPtr lParam)
        {
            IntPtr hChild = IntPtr.Zero;
            StringBuilder buf = new StringBuilder(1024);
            StringBuilder buf2 = new StringBuilder(1024);
            Win32API.GetClassName(hWnd, buf, buf.Capacity);
            switch (buf.ToString())
            {
                case "ApplicationFrameWindow": //normal window
                    hChild = Win32API.FindWindowEx(hWnd, IntPtr.Zero, "Windows.UI.Core.CoreWindow", "Microsoft Edge");

                    if (hChild != IntPtr.Zero)
                    {
                        lParam = hChild;
                        return false;
                    }

                    break;
                case "Windows.UI.Core.CoreWindow": //minimized window 
                    hChild = Win32API.FindWindowEx(hWnd, IntPtr.Zero, "Spartan XAML-To-Trident Input Routing Window", "");
                    Win32API.GetWindowText(hWnd, buf2, buf2.Capacity);
                    if (hChild != IntPtr.Zero && buf2.ToString() == "Microsoft Edge")
                    {
                        lParam = hWnd;
                        return false;
                    }
                    break;
            }

            return true;
        }

        #region Cool Methods
        public static void PauseSecond()
        {
            try
            {
                Thread.Sleep(3000);
            }
            catch (Exception)
            { }
        }
        public static void PauseSecond(double seconds)
        {
            if (seconds <= 0)
                return;

            try
            {
                int time = (int)(seconds * 1000);
                Thread.Sleep(time);
            }
            catch (Exception)
            { }
        }

        public static void SetWhetherCaptureFromArgs(string[] args)
        {
            foreach (string s in args)
            {
                if (s.Equals(@"/nocapture", StringComparison.InvariantCultureIgnoreCase))
                {
                    myIsCapture = false;
                }
            }
        }
        public static void SetBorderAroundElement(IWebElement element)
        {
            if (element != null)
            {
                string JS_BORDER = "arguments[0].style.border='2.5px orange solid';";
                string JS_NO_BORDER = "arguments[0].style.border='';";
                string JS_BGCOLOR = "arguments[0].style.background='#e67e22';";
                string JS_NO_BGCOLOR = "arguments[0].style.background='#FFFFFF';";


                //// If elements already surround border, your action will remove that.
                //// You should avoid this.

                //// Default is set border
                string jsSelectStyle = JS_BORDER;
                string jsUnselectStyle = JS_NO_BORDER;

                //// These elements should set BG color
                if (element.GetAttribute("type") != null && element.GetAttribute("type").Equals("text"))
                {
                    jsSelectStyle = JS_BGCOLOR;
                    jsUnselectStyle = JS_NO_BGCOLOR;
                }
                if (element.GetAttribute("type") != null && element.GetAttribute("type").Equals("password"))
                {
                    jsSelectStyle = JS_BGCOLOR;
                    jsUnselectStyle = JS_NO_BGCOLOR;
                }
                if (element.TagName.Equals("textarea") || element.TagName.Equals("select") ||
                    element.TagName.Equals("tr") || element.TagName.Equals("td"))
                {
                    jsSelectStyle = JS_BGCOLOR;
                    jsUnselectStyle = JS_NO_BGCOLOR;
                }

                //// Do set action
                JsExcutor.ExecuteScript(jsSelectStyle, element);
                PauseSecond(0.5);
                JsExcutor.ExecuteScript(jsUnselectStyle, element);
                PauseSecond(0.5);
            }
        }
        public static void SetBorderAroundElement(ReadOnlyCollection<IWebElement> eCollection)
        {
            string JS_BORDER = "arguments[0].style.border='2.5px orange solid';";
            string JS_NO_BORDER = "arguments[0].style.border='';";

            if (eCollection != null)
            {
                foreach (IWebElement e in eCollection)
                    myJsExecutor.ExecuteScript(JS_BORDER, e);
                PauseSecond(0.5);

                foreach (IWebElement e in eCollection)
                    myJsExecutor.ExecuteScript(JS_NO_BORDER, e);
                PauseSecond(0.5);
            }
        }

        public static void OpenUrl(string url)
        {
            if (url == null || url.Length <= 0)
                return;

            myDriver.Navigate().GoToUrl(url);
        }

        public static void SetElementValue(IWebElement element, string value)
        {
            if (element == null || value == null || value.Length <= 0)
                return;

            JsExcutor.ExecuteScript("arguments[0].value='" + value + "';", element);
            PauseSecond();
        }
        public static void SetElementValueforIframe(IWebElement element, string value)
        {
            if (element == null || value == null || value.Length <= 0)
                return;

            JsExcutor.ExecuteScript("arguments[0].value='" + value + "';", element);
            PauseSecond();
            Global.Type("{SPACEBAR}");
            PauseSecond();
        }

        public static bool Exist(By by)
        {
            return myDriver.FindElements(by).Count != 0;
        }
        public static bool IsExist(By by)
        {
            if (by == null)
                return false;

            bool result = false;
            try
            {
                WebDriverWait innerWait = new WebDriverWait(myDriver, TimeSpan.FromSeconds(3));
                result = innerWait.Until(d => { return myDriver.FindElements(by).Count > 0; });
                if (result)
                {
                    if (myDriver.FindElement(by).Displayed)
                        result = true;
                    else
                        result = false;
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
        public static bool IsExist(string cssSelector)
        {
            return IsExist(By.CssSelector(cssSelector));
        }

        // By CssSelector
        public static IWebElement GetElementByCssSelector(string cssSelector, ISearchContext context = null)
        {
            if (cssSelector == null || cssSelector.Length <= 0)
                return null;

            return getElement(context, By.CssSelector(cssSelector));
        }
        public static ReadOnlyCollection<IWebElement> GetElementsByCssSelector(string cssSelector, ISearchContext context = null)
        {
            if (cssSelector == null || cssSelector.Length <= 0)
                return null;

            return getElements(context, By.CssSelector(cssSelector));
        }

        // By XPath
        public static IWebElement GetElementByXPath(string xPath, ISearchContext context = null)
        {
            if (xPath == null || xPath.Length <= 0)
                return null;

            return getElement(context, By.XPath(xPath));
        }
        public static ReadOnlyCollection<IWebElement> GetElementsByXPath(string xPath, ISearchContext context = null)
        {
            if (xPath == null || xPath.Length <= 0)
                return null;

            return getElements(context, By.XPath(xPath));
        }

        // By InnerText
        public static IWebElement FindElement(IList<IWebElement> elementList, string innerText, bool isContains = false)
        {
            if (elementList == null || innerText == null || innerText.Length <= 0)
                return null;

            IWebElement element = null;
            try
            {
                foreach (IWebElement item in elementList)
                {
                    if (isContains)
                    {
                        if (item.Text.ToLower().Contains(innerText.ToLower()))
                        {
                            element = item;
                            break;
                        }
                    }
                    else
                    {
                        if (item.Text.ToLower() == innerText.ToLower())
                        {
                            element = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            { }

            return element;
        }
        public static IWebElement GetElementByInnerText(string cssSelector, string innerText, bool isPartialMatch = false, ISearchContext context = null)
        {
            if (cssSelector == null || cssSelector.Length <= 0)
                return null;
            if (innerText == null || innerText.Length <= 0)
                return null;

            IList<IWebElement> elementList = GetElementsByCssSelector(cssSelector, context);
            IWebElement targetElement = FindElement(elementList, innerText, isPartialMatch);
            int searchTimes = DEFAULT_TIMEOUT;
            while (targetElement == null && searchTimes > 0)
            {
                PauseSecond(1);
                targetElement = FindElement(elementList, innerText, isPartialMatch);
                searchTimes--;
            }

            if (searchTimes == 0 && targetElement == null)
                throw new Exception("Searching by inner text with timeout 120 seconds.");

            return targetElement;
        }

        // By Name
        public static IWebElement GetElementByName(string elementName, ISearchContext context = null)
        {
            if (elementName == null || elementName.Length <= 0)
                return null;

            return getElement(context, By.Name(elementName));
        }
        public static ReadOnlyCollection<IWebElement> GetElementsByName(string elementName, ISearchContext context = null)
        {
            if (elementName == null || elementName.Length <= 0)
                return null;

            return getElements(context, By.Name(elementName));
        }

        // By Id (No get elements methods)
        public static IWebElement GetElementById(string elementId, ISearchContext context = null)
        {
            if (elementId == null || elementId.Length <= 0)
                return null;

            return getElement(context, By.Id(elementId));
        }

        // By Tag Name
        public static IWebElement GetElementByTag(string elementTagName, ISearchContext context = null)
        {
            if (elementTagName == null || elementTagName.Length <= 0)
                return null;

            return getElement(context, By.TagName(elementTagName));
        }
        public static ReadOnlyCollection<IWebElement> GetElementsByTag(string elementTagName, ISearchContext context = null)
        {
            if (elementTagName == null || elementTagName.Length <= 0)
                return null;

            return getElements(context, By.TagName(elementTagName));
        }

        // By Class Name
        public static IWebElement GetElementByClassName(string className, ISearchContext context = null)
        {
            if (className == null || className.Length <= 0)
                return null;

            return getElement(context, By.ClassName(className));
        }
        public static ReadOnlyCollection<IWebElement> GetElementsByClassName(string className, ISearchContext context = null)
        {
            if (className == null || className.Length <= 0)
                return null;

            return getElements(context, By.ClassName(className));
        }

        // By Link Text
        public static IWebElement GetElementByLinkText(string linkText, ISearchContext context = null)
        {
            if (linkText == null || linkText.Length <= 0)
                return null;

            return getElement(context, By.LinkText(linkText));
        }
        public static IWebElement GetElementByPartialLinkText(string partailLinkText, ISearchContext context = null)
        {
            if (partailLinkText == null || partailLinkText.Length <= 0)
                return null;

            return getElement(context, By.PartialLinkText(partailLinkText));
        }
        public static ReadOnlyCollection<IWebElement> GetElementsByLinkText(string linkText, ISearchContext context = null)
        {
            if (linkText == null || linkText.Length <= 0)
                return null;

            return getElements(context, By.LinkText(linkText));
        }
        public static ReadOnlyCollection<IWebElement> GetElementsByPartialLinkText(string partailLinkText, ISearchContext context = null)
        {
            if (partailLinkText == null || partailLinkText.Length <= 0)
                return null;

            return getElements(context, By.PartialLinkText(partailLinkText));
        }


        //// The "Table" element operation
        public static void ClickTableItem(IWebElement tableElement, int rowNumber, int columnNumber = 1)
        {
            if (tableElement == null || rowNumber <= 0 || columnNumber <= 0)
                return;

            myWait.Until(p => { return tableElement.FindElements(By.TagName("tr")).Count >= 1; });
            IWebElement row = tableElement.FindElement(By.CssSelector(string.Format("tr:nth-child({0})", rowNumber)));
            IWebElement target = GetElementByCssSelector(string.Format("td:nth-child({0})", columnNumber), row);

            if (target != null)
            {
                target.Click();
                PauseSecond(2);
            }

        }
        public static void ClickTableItem(IWebElement tableElement, string innerText, bool isPartialMatch = false)
        {
            if (tableElement == null || innerText == null || innerText.Length <= 0)
                return;

            IWebElement target = GetElementByInnerText("td", innerText, isPartialMatch, tableElement);

            if (target != null)
            {
                target.Click();
                PauseSecond();
            }
        }


        //// The "Alert" element operation
        public static void TryToClsoeAlertByAccept()
        {
            try
            {
                PauseSecond();
                IAlert alert = myDriver.SwitchTo().Alert();
                alert.Accept();
                myDriver.SwitchTo().DefaultContent();
            }
            catch
            { }
        }
        public static void TryToCloseAlertByDismiss()
        {
            try
            {
                PauseSecond();
                IAlert alert = myDriver.SwitchTo().Alert();
                alert.Dismiss();
                myDriver.SwitchTo().DefaultContent();
            }
            catch
            { }
        }


        //// The "Select" element operation
        public static void SelectOptionByValue(IWebElement selectElement, string value)
        {
            if (selectElement == null || value == null)
                return;

            SelectElement select = new SelectElement(selectElement);
            select.SelectByValue(value);
            // Fix bug, it will not refresh visible text after choose any one
            myActions.SendKeys(Keys.Return).Perform();
            PauseSecond();
        }
        public static void SelectOptionByVisibleText(IWebElement selectElement, string visibleText)
        {
            if ((selectElement == null) || (visibleText == null))
                return;

            SelectElement select = new SelectElement(selectElement);
            select.SelectByText(visibleText);
            myActions.SendKeys(Keys.Return).Perform();
            PauseSecond();
        }
        public static void SelectOptionByIndex(IWebElement selectElement, int index)
        {
            if ((selectElement == null) || (index < 0))
                return;

            SelectElement select = new SelectElement(selectElement);
            select.SelectByIndex(index);
            myActions.SendKeys(Keys.Return).Perform();
            PauseSecond();
        }
        #endregion


        #region Mouse Actions
        public static void Click(IWebElement element)
        {
            if (element != null)
            {
                element.Click();
            }
        }
        public static void Click(IWebElement element, int loopTime)
        {
            if (element != null && loopTime != 0)
            {
                for (int times = 0; times < loopTime; times++)
                {
                    element.Click();
                }
            }
        }
        public static void ClickElement(IWebElement element)
        {
            if (myDriver != null)
            {
                JsExcutor.ExecuteScript("arguments[0].click()", element);
            }
        }
        public static void ClickElement(IWebElement element, int loopTime)
        {
            if (myDriver != null && loopTime != 0)
            {
                for (int times = 0; times < loopTime; times++)
                {
                    JsExcutor.ExecuteScript("arguments[0].click()", element);
                }
            }
        }
        public static void ClickSelectElement(IWebElement element)
        {
            if (myDriver != null)
            {

                JsExcutor.ExecuteScript("arguments[0].size = arguments[0].length;", element);

            }
        }
        public static void ClickSelectElement(IWebElement element, int loopTime)
        {
            if (myDriver != null && loopTime != 0)
            {
                for (int times = 0; times < loopTime; times++)
                {
                    JsExcutor.ExecuteScript("arguments[0].size = arguments[0].length;", element);

                }

            }
        }
        public static void ClickElementByJs(IWebElement element)
        {
            if (element == null)
                return;

            JsExcutor.ExecuteScript("arguments[0].click(); return;", element);
        }
        public static void ClickElementByJs(IWebElement element, int loopTime)
        {
            if (element == null)
                return;
            for (int times = 0; times < loopTime; times++)
            {
                JsExcutor.ExecuteScript("arguments[0].click(); return;", element);

            }
        }



        public static void FocusElement(IWebElement element)
        {

            if (myDriver != null)
            {
                JsExcutor.ExecuteScript("arguments[0].focus(); ", element);

            }
        }
        public static void DoubleClick(IWebElement element)
        {
            if (element != null)
            {
                myActions.DoubleClick(element).Perform();
                PauseSecond();
            }
        }
        public static void DoubleClick(IWebElement element, int loopTimes)
        {
            if (element != null)
            {
                for (int times = 0; times < loopTimes; times++)
                {
                    myActions.DoubleClick(element).Perform();
                    PauseSecond();


                }
            }
        }
        public static void HoverWebElement(IWebElement element)
        {
            if (element == null)
                return;

            myActions.MoveToElement(element).Perform();
        }
        public static void HoverWebElementByJs(IWebElement element)
        {
            if (myDriver != null)
            {
                JsExcutor.ExecuteScript("if(document.createEvent){var evObj = document.createEvent('MouseEvents');evObj.initEvent('mouseover',true, false); arguments[0].dispatchEvent(evObj);} else if (document.createEventObject){arguments[0].fireEvent('onmouseover');}", element);
            }
        }
        public static void RightClick(IWebElement element)
        {
            if (element == null)
                return;

            myActions.ContextClick(element).Perform();
        }
        public static void RightClick(IWebElement element, int loopTimes)
        {
            if (element == null)
                return;
            for (int times = 0; times < loopTimes; times++)
            {
                myActions.ContextClick(element).Perform();
            }
        }
        public static void clickOMS(string parentXPath, string ChildXPth)
        {
            Actions action = new Actions(myDriver);
            IWebElement parentElement = myDriver.FindElement(By.XPath(parentXPath));
            if (parentElement.Displayed)
            {
                action.MoveToElement(parentElement).Build().Perform();

            }
            IJavaScriptExecutor js = (IJavaScriptExecutor)myDriver;
            if (Exist(By.XPath(ChildXPth)))
            {
                IWebElement childElement = myDriver.FindElement(By.XPath(ChildXPth));
                js.ExecuteScript("arguments[0].click();", childElement);
                System.Threading.Thread.Sleep(3000);
            }
            else
            {
                throw (new Exception("Cannot find this Element"));
            }

        }
        #endregion


        #region Keyboard Actions
        public static void TypeString(string value)
        {
            if (value == null || value.Length <= 0)
                return;

            myActions.SendKeys(value).Perform();
            PauseSecond();
        }
        public static void TypeEnter()
        {
            myActions.SendKeys(Keys.Enter).Perform();
            PauseSecond();
        }
        public static void TypeEsc()
        {
            myActions.SendKeys(Keys.Escape).Perform();
            PauseSecond();
        }
        public static void TypeDown()
        {
            myActions.SendKeys(Keys.Down).Perform();
            PauseSecond();
        }
        public static void TypeDown(int downCount)
        {
            if (downCount <= 0)
                return;

            for (int i = 0; i < downCount; i++)
            {
                myActions.SendKeys(Keys.Down).Perform();
                PauseSecond(0.2);
            }
        }
        public static void TypePageDown()
        {
            myActions.SendKeys(Keys.PageDown).Perform();
        }
        public static void TypePageDown(int downCount)
        {
            if (downCount <= 0)
                return;

            for (int i = 0; i < downCount; i++)
            {
                myActions.SendKeys(Keys.PageDown).Perform();
                PauseSecond(0.2);
            }
        }
        public static void TypePageUp()
        {
            myActions.SendKeys(Keys.PageUp).Perform();
        }
        public static void TypePageUp(int upCount)
        {
            if (upCount <= 0)
                return;

            for (int i = 0; i < upCount; i++)
            {
                myActions.SendKeys(Keys.PageUp).Perform();
                PauseSecond(0.2);
            }
        }
        public static void TypeTab()
        {
            myActions.SendKeys(Keys.Tab).Perform();
            PauseSecond();
        }
        public static void TypeTab(int tabCount)
        {
            if (tabCount <= 0)
                return;

            for (int i = 0; i < tabCount; i++)
            {
                myActions.SendKeys(Keys.Tab).Perform();
                PauseSecond(0.2);
            }
        }
        public static void TypeSpace()
        {
            myActions.SendKeys(Keys.Space).Perform();
            PauseSecond();
        }
        public static void Refreash()
        {
            if (myDriver != null)
            {
                JsExcutor.ExecuteScript("history.go(0)");
            }
        }
        public static void PullDropDown()
        {
            myActions.SendKeys(Keys.Alt + Keys.ArrowDown).Perform();
            PauseSecond();
        }
        public static void PushDropDown()
        {
            myActions.SendKeys(Keys.Alt + Keys.ArrowUp).Perform();
            PauseSecond();
        }
        public static void ReplaceStrings(IWebElement element, string value)
        {
            element.SendKeys(Keys.Control + "a");
            myActions.SendKeys(Keys.Delete).Perform();
            PauseSecond();
            element.SendKeys(value);
            PauseSecond();
            TypeEnter();
        }
        #endregion


        #region  Intelligence Waiting
        public static void WaitForElementLoaded(string cssSelector)
        {
            if (cssSelector == null || cssSelector.Length <= 0)
                return;

            //// Make sure there has the element
            myWait.Until(p =>
            {
                bool result = false;
                try
                {
                    result = myDriver.FindElements(By.CssSelector(cssSelector)).Count > 0;
                }
                catch (NoSuchElementException)
                { }

                return result;
            });

            //// Make sure the element displayed
            myWait.Until(p =>
            {
                bool result = false;
                try
                {
                    result = myDriver.FindElement(By.CssSelector(cssSelector)).Displayed;
                }
                catch (NoSuchElementException)
                { }

                return result;
            });

            PauseSecond();
        }
        public static void WaitElementEnable(string cssSelector)
        {
            if (cssSelector == null || cssSelector.Length <= 0)
                return;

            IWebElement element = GetElementByCssSelector(cssSelector);
            if (element != null)
            {
                myWait.Until(p =>
                {
                    bool result = false;

                    string classValue = element.GetAttribute("class").Trim().ToLower();
                    if (!classValue.Contains("disable"))
                        result = true;

                    return result;
                });
            }
        }
        #endregion


        #region Scroll Actions
        public static void ScrollBladeDown(int pageDownCount = 1)
        {
            if (pageDownCount <= 0)
                return;

            IWebElement element = GetElementByCssSelector("div[class*='scrollbar']", CurrentBlade);
            for (int i = 0; i < pageDownCount; i++)
            {
                element.SendKeys(Keys.PageDown);
                PauseSecond(0.2);
            }
        }
        public static void ScrollIntoView(IWebElement element)
        {
            if (element != null)
            {
                myActions.MoveToElement(element).Perform();
            }
        }
        public static void ScrollIntoViewByJs(IWebElement element)
        {
            if (element != null)
            {
                JsExcutor.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            }
        }
        public static void MakeElementInScreen(IWebElement element, int downCount)
        {
            if (element == null)
            {
                Log.AddExceptionInfo(new Exception("Cannot find element"));
                throw new Exception("Cannot find element");
            }
            Global.MoveMouse(element.Location.X, 500);
            System.Threading.Thread.Sleep(3000);
            Global.MoveWheel(WheelDirection.Down, downCount);
        }
        public static void MakeElementInScreen(IWebElement element, int downCount, WheelDirection direction)
        {
            if (element == null || downCount < 0)
                return;


            Global.MoveMouse(element.Location.X, 500);
            System.Threading.Thread.Sleep(3000);
            Global.MoveWheel(direction, downCount);
        }

        #endregion


        #region Handle Multi Windows
        public static void GetJumpWindowToDriver()
        {
            myWait.Until(q => { return myDriver.WindowHandles.Count > 1; });

            if (myDriver.CurrentWindowHandle == rootWindowHandle)
            {
                foreach (string handle in myDriver.WindowHandles)
                {
                    if (!handle.Equals(rootWindowHandle))
                    {
                        myDriver.SwitchTo().Window(handle);
                        myWait.Until(q =>
                        {
                            return myJsExecutor.ExecuteScript("return document.readyState;").ToString() == "complete";
                        });
                        myDriver.Manage().Window.Maximize();
                    }
                }
            }
        }
        public static void GetDefaultWindowToDriver()
        {
            myDriver.SwitchTo().Window(rootWindowHandle);
        }
        public static void CloseCurrentPage()
        {
            if (myDriver != null)
            {
                JsExcutor.ExecuteScript("window.opener=null;  window.open('','_self'); window.close(); ");
                GetDefaultWindowToDriver();
            }
        }
        public static void SwitchDriverToPopupWindow()
        {
            GetJumpWindowToDriver();
        }
        public static void MaxsizePopupWindow()
        {
            if (myDriver != null)
            {
                //// Base window is the first that you opened
                SwitchDriverToPopupWindow();
                myDriver.Manage().Window.Maximize();
            }
        }
        public static void CloseNewPopupWindow()
        {
            if (myDriver != null)
            {
                SwitchDriverToPopupWindow();
                myDriver.Close();
                PauseSecond();
                myDriver.SwitchTo().Window(rootWindowHandle);
            }
        }
        #endregion


        #region Capture method

        public static void CaptureElement(IWebElement targetElement, long captureId)
        {
            if (targetElement == null || captureId <= 0)
                return;

            if (myIsCapture)
            {
                MicrosoftOMSWindow.AccObject = null;
                IntPtr handler = MicrosoftOMSWindow.AccObject.Hwnd;
                SetBorderAroundElement(targetElement);
                CaptureIbizaUI(handler, targetElement, captureId);
            }
        }
        public static void Capture_Edge(IWebElement targetElement, long captureId)
        {
            if (targetElement == null || captureId <= 0)
                return;

            if (myIsCapture)
            {
                IntPtr handler = IntPtr.Zero;
                Win32API.WinEnumProc proc = new Win32API.WinEnumProc(EnumWindowsProc);
                Win32API.EnumWindows(proc, ref handler);
                if (handler != IntPtr.Zero)
                {
                    CaptureIbizaUI(handler, targetElement, captureId);
                }
            }


        }
        public static void CapturePartWindowMSAA(AccObject obj, long captureId, DialogType dialogType = DialogType.WebBrowser)
        {
            if (myIsCapture)
            {
                CaptureWindowMSAA(obj, captureId, dialogType);
            }
        }
        public static void CaptureWindowMSAA(AccObject obj, long captureCode, DialogType dialogType)
        {
            try
            {
                string fileName = generateFileName(captureCode.ToString());
                CaptureWindow(obj.Hwnd, fileName, dialogType, CaptureMechanism.MSAA);
            }
            catch
            {
            }
        }
        public static IWebElement GetCaptureWebElement(string[] cssStringList)
        {
            if (cssStringList == null || cssStringList.Length <= 0)
                return null;

            IWebElement targetElement = null;
            foreach (string s in cssStringList)
            {
                targetElement = CurrentWebDriver.FindElement(By.CssSelector(s));
                if (targetElement != null)
                    break;
            }

            if (targetElement == null)
                throw new Exception("Get Capture Target Element Failed!");


            return targetElement;
        }
        public static void CapturePopupMessage(IWebElement element, long captureId)
        {
            if (element == null || captureId <= 0)
                return;

            if (myIsCapture)
            {
                IntPtr handler = IntPtr.Zero;
                Win32API.WinEnumProc proc = new Win32API.WinEnumProc(EnumWindowsProc);
                Win32API.EnumWindows(proc, ref handler);
                if (handler != IntPtr.Zero)
                {
                    SetBorderAroundElement(element);
                    CaptureIbizaPopupMessage(handler, element, captureId);
                }
            }
        }
        public static void CaptureErrorPage(IWebDriver driver, string name)
        {
            try
            {
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                ss.SaveAsFile(string.Format(@"{0}.jpg", name), ScreenshotImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public static void CaptureImage(int x, int y, int width, int height, string captureCode)
        {
            string fileName = generateFileName(captureCode);

            //upperLeftSource
            Point p1 = new Point(x, y);
            //upperLeftDestination
            Point p2 = new Point(x, y);
            using (Bitmap bitmap = new Bitmap(width, height))
            {
                Thread.Sleep(3000);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(p1, new Point(0, 0), new Size(width, height));
                }
                bitmap.Save(fileName);
            }
        }
        public static void CaptureWindow(string captureCode)
        {
            CaptureImage(0, 80, 1300, 900, captureCode);
        }

        #endregion

        public static void Cleanup()
        {
            myDriver.Quit();
        }
    }
}
