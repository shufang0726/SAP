//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SAPFrm.DBConnect
{
    using System;
    using System.Collections.Generic;
    
    public partial class ResourceConfiguration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ResourceConfiguration()
        {
            this.Resources = new HashSet<Resource>();
            this.Results = new HashSet<Result>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsShared { get; set; }
        public System.DateTime LastUpdatedTime { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<int> ResourceId { get; set; }
        public string SysParseXmlPath { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Resource> Resources { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Result> Results { get; set; }
    }
}
