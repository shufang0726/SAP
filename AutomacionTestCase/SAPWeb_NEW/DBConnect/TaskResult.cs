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
    
    public partial class TaskResult
    {
        public int Id { get; set; }
        public System.Guid Guid { get; set; }
        public Nullable<int> ResultId { get; set; }
        public int TaskResultStatusId { get; set; }
        public int ResourceId { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public System.DateTime LastUpdatedTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<int> ProcessExitStatus { get; set; }
        public Nullable<int> LogParserExitStatus { get; set; }
        public Nullable<int> TaskId { get; set; }
        public int Pass { get; set; }
        public int Fail { get; set; }
        public int NotApplicable { get; set; }
        public int NotRun { get; set; }
        public Nullable<int> CurrentPipelineStateId { get; set; }
        public Nullable<int> SubResultId { get; set; }
        public short PercentComplete { get; set; }
        public string UserComments { get; set; }
        public bool IsLogExists { get; set; }
        public int InfrastructureFail { get; set; }
        public string TaskName { get; set; }
        public Nullable<int> FailureActionId { get; set; }
        public Nullable<int> TaskTypeId { get; set; }
        public Nullable<int> TaskExecutionPhaseId { get; set; }
        public Nullable<int> SequenceNumber { get; set; }
        public Nullable<bool> IsInfrastructureTask { get; set; }
        public Nullable<bool> IsExecuteOnFailure { get; set; }
        public Nullable<bool> IsDisabled { get; set; }
        public Nullable<bool> IsLRFlag { get; set; }
        public string LogLocation { get; set; }
        public Nullable<int> ResultResourceId { get; set; }
    }
}
