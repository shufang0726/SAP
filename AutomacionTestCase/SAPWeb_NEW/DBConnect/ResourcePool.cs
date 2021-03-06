//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SAPWeb_NEW.DBConnect
{
    using System;
    using System.Collections.Generic;
    
    public partial class ResourcePool
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ResourcePool()
        {
            this.Resources = new HashSet<Resource>();
            this.ResourcePool1 = new HashSet<ResourcePool>();
            this.Schedules = new HashSet<Schedule>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Nullable<int> PushDaemonResourceId { get; set; }
        public string DSUserAlias { get; set; }
        public bool ScheduleAsUnit { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<int> RunId { get; set; }
        public bool IsSmartSchedulerEnabled { get; set; }
        public int ChildCount { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Resource> Resources { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResourcePool> ResourcePool1 { get; set; }
        public virtual ResourcePool ResourcePool2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
