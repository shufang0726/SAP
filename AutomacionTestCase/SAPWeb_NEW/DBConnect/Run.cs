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
    
    public partial class Run
    {
        public int Id { get; set; }
        public System.Guid Guid { get; set; }
        public int RunStatusId { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public System.DateTime LastUpdatedTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public int ScheduleId { get; set; }
        public Nullable<int> CurrentPipelineStateId { get; set; }
        public Nullable<int> CurrentPipelineId { get; set; }
        public string LogLocation { get; set; }
        public Nullable<int> ParentTaskResultId { get; set; }
        public int ConstraintQueryId { get; set; }
        public Nullable<int> RunResourceId { get; set; }
        public Nullable<int> LinkRunId { get; set; }
        public Nullable<bool> SkipSmartScheduler { get; set; }
        public int CurrentPipelineSequenceNumber { get; set; }
        public bool IsRunMovedFromPD { get; set; }
        public string RunXml { get; set; }
        public Nullable<System.Guid> SourceDsLinkGuid { get; set; }
    
        public virtual Schedule Schedule { get; set; }
    }
}
