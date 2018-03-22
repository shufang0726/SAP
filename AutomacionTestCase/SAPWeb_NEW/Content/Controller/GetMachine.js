app.controller('webCtrl', ['$scope', '$http', '$q', '$window', function ($scope, $http, $q) {
  
    $scope.baseUrl = window.location.protocol + '//' +window.location.host;

    $scope.List = [];
    $scope.JobList = [];
    $scope.jobIDList = [];
    $scope.jobNameList = [];
    $scope.jobID = 'None';
    $scope.JobName = 'None';
    $scope.ShowLoading = false;

    $scope.pool = '';
    $scope.xmlPath = '';

    $scope.checkedMachinceNameList = [];
    $scope.checkedJobIDList = [];
    $scope.MachineList = [];

    $scope.isDisable = true;

    $scope.init = function () {
        $scope.ShowLoading = true;
        $scope.pool = "Web";
        $http({
            method: 'GET',
            url: $scope.baseUrl + '/HLK/ConnectHLK' + '/' + $scope.pool,
        }).then(
                function (data) {
                    for (var i = 0; i < data.data.length; i++) {
                        $scope.List.push(data.data[i]);
                    }
                    //$scope.List.push(data);
                    $scope.ShowLoading = false;
                },
                function (data) {

                }
            );
    }
    $scope.init();


    $scope.getJob = function () {
        $scope.pool = "Web";
        $http({
            method: 'GET',
            url: $scope.baseUrl + '/HLK/GetJobID' + '/' + $scope.pool,
        }).then(
                function (data) {
                    for (var i = 0; i < data.data.length; i++) {
                        $scope.JobList.push(data.data[i]);
                        $scope.jobIDList.push(data.data[i].jid);
                        $scope.jobNameList.push(data.data[i].jname);
                    }
                    //$scope.List.push(data);
                },
                function (data) {

                }
            );
    }
    $scope.getJob();


    $scope.tipBox = function () {
        alert("Choose the path is: 'D:\\Template_case.xml'");
    }

    $scope.loadXML = function () {
        var defer = $q.defer();
        //$scope.xmlPath = angular.element("#uploadfile").val();
        $scope.xmlPath = "D:\\Template_case.xml";
        $scope.pool = "Web";
        var model={
            pool: $scope.pool,
            filePath: $scope.xmlPath
        }
        console.log(model.pool, model.filePath);
        if (model.pool == "Web" && model.filePath != "") {
            $http.post(
                $scope.baseUrl + '/HLKFile/CreateCSFile', model                
            ).then(function (model) {
                defer.resolve(model.data);
            },
            function (data) {
                
            }
            );
        }
    }

    $scope.CreateTsFarm = function () {
        var a = document.getElementById("downLoad");
        a.href = '/Home/DownLoadFile/';
        a.click();
    }

    var selectedJobNames = [];
    $scope.clickJobCheckBox = function ($event, val) {
        var status = $event.target.checked;
        if (status == true && selectedJobNames.indexOf(val) == -1) {
            selectedJobNames.push(val);
            $scope.JobName = val
            for (var i = 0; i < $scope.jobNameList.length; i++) {
                if ($scope.JobName == $scope.jobNameList[i]) {
                    $scope.jobID = $scope.jobIDList[i];
                }
            }
        }

        if (status == false && selectedJobNames.indexOf(val) != -1) {
            selectedJobNames.splice(selectedJobNames.indexOf(val), 1);
        }

        if (selectedJobNames.length > 0) {
            $scope.isDisable = false;
            for (var i = 0; i < selectedJobNames.length; i++) {
                $scope.checkedJobIDList.push($scope.jobIDList[i]);
            }
        } else {
            $scope.isDisable = true;
            $scope.jobID = 'None';
        }
    }

    var selectedNames = [];
    $scope.clickMachineCheckBox = function ($event, val) {
        var status = $event.target.checked;
        if (status == true && selectedNames.indexOf(val) == -1) {
            selectedNames.push(val);           
        }

        if (status == false && selectedNames.indexOf(val) != -1) {
            selectedNames.splice(selectedNames.indexOf(val), 1);
        }

        if (selectedNames.length > 0) {
            $scope.isDisable = false;
            for (var i = 0; i < selectedNames.length; i++) {
                $scope.checkedMachinceNameList.push(selectedNames[i]);
                $scope.MachineList.push(selectedNames[i]);
            }
        } else {
            $scope.isDisable = true;
        }
    }

    $scope.selectAllMachine = function ($event, list) {
        var status = $event.target.checked;
        for (var i = 0; i < list.length; i++) {
            var checkBox = $("#" + list[i].MachineName)[0];
            checkBox.checked = status;
            $scope.checkedMachinceNameList.push(list[i].MachineName);
            $scope.MachineList.push(list[i].MachineName);
            $scope.clickMachineCheckBox($event, list[i].MachineName);
        }
    }

    //$scope.selectAllJob = function ($event, jobList) {
    //    var status = $event.target.checked;
    //    for (var i = 0; i < jobList.length; i++) {
    //        var checkBox = $("#" + jobList[i].jname)[0];
    //        checkBox.checked = status;
    //        $scope.clickCheckBox($event, jobList[i].jname);
    //    }
    //}  

    $scope.runTestCase = function () {
        $scope.pool = "Web";
        $scope.checkedMachinceNameList;
        $scope.checkedJobIDList;
        $scope.MachineList;

        var model = {
            pool: $scope.pool,
            checkedMachinceList: $scope.checkedMachinceNameList,
            jobs_result: $scope.checkedJobIDList,
            machines_result: $scope.MachineList
        }
        var loader = layer.load(0);
        if ($scope.pool == "Web") {
            copyDllFile(model);
            runJob(model);
        }
    }

    function copyDllFile(model) {
        var defer = $q.defer();
        $http.post(
                $scope.baseUrl + '/HLK/CopyDllFile', model
            ).then(function (model) {
                defer.resolve(model.data);
            },
            function (data) {

            }
            );
    }

    function runJob(model) {
        var defer = $q.defer();       
        $http.post(
                $scope.baseUrl + '/HLK/RunJob', model
            ).then(function (model) {
                defer.resolve(model.data);
            },
            function (data) {

            }
            );
    }
}]);