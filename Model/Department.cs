//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using TrackableEntities;
    
    [JsonObject(IsReference = true)]
    [DataContract]
    public partial class Department : ITrackable
    {
        public Department()
        {
            this.Delegate = new List<Delegate>();
            this.Requisition = new List<Requisition>();
        }
    
        [DataMember]
        public string DeptID { get; set; }
        [DataMember]
        public string DeptName { get; set; }
        [DataMember]
        public string CPID { get; set; }
        [DataMember]
        public string Contact { get; set; }
        [DataMember]
        public string DeptHead { get; set; }
        [DataMember]
        public string DeptRep { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
    
        [DataMember]
        public CollectionPoint CollectionPoint { get; set; }
        [DataMember]
        public ICollection<Delegate> Delegate { get; set; }
        [DataMember]
        public ICollection<Requisition> Requisition { get; set; }
    
        [DataMember]
        public TrackingState TrackingState { get; set; }
        [DataMember]
        public ICollection<string> ModifiedProperties { get; set; }
        [JsonProperty, DataMember]
        private Guid EntityIdentifier { get; set; }
    }
}
