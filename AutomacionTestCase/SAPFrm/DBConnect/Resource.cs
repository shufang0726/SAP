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
    
    public partial class Resource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public System.Guid Guid { get; set; }
        public int ResourcePoolId { get; set; }
        public int ResourceStatusId { get; set; }
        public string LastUpdatedByAlias { get; set; }
        public System.DateTime LastUpdatedTime { get; set; }
        public System.DateTime LastHBTime { get; set; }
        public Nullable<int> LatestResourceConfigurationId { get; set; }
        public string PublicKey { get; set; }
        public string LocationBuilding { get; set; }
        public string LocationRoom { get; set; }
        public System.DateTime DateAcquired { get; set; }
        public string PermanentOwnerAlias { get; set; }
        public string CurrentOwnerAlias { get; set; }
        public System.DateTime LastRuntime { get; set; }
        public Nullable<int> ResourceStatusChangedByPipeline { get; set; }
        public string RuntimeVersion { get; set; }
        public int ResourceTypeId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<int> RootId { get; set; }
        public string CustomXml { get; set; }
        public Nullable<int> Crc { get; set; }
        public short ResourcePriority { get; set; }
        public int IsTwinSchedulable { get; set; }
        public bool NeededByScheduler { get; set; }
        public Nullable<int> PushDaemonResourceId { get; set; }
        public string MovedToDatabaseName { get; set; }
        public string MovedToDataStoreName { get; set; }
    
        public virtual ResourcePool ResourcePool { get; set; }
        public virtual ResourceConfiguration ResourceConfiguration { get; set; }
    }
}
