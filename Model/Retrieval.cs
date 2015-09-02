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
    public partial class Retrieval : ITrackable
    {
        public Retrieval()
        {
            this.RetrievalDetail = new List<RetrievalDetail>();
        }
    
        [DataMember]
        public int RetID { get; set; }
        [DataMember]
        public Nullable<System.DateTime> Date { get; set; }
        [DataMember]
        public string EmpID { get; set; }
        [DataMember]
        public string Status { get; set; }
    
        [DataMember]
        public Employee Employee { get; set; }
        [DataMember]
        public ICollection<RetrievalDetail> RetrievalDetail { get; set; }
    
        [DataMember]
        public TrackingState TrackingState { get; set; }
        [DataMember]
        public ICollection<string> ModifiedProperties { get; set; }
        [JsonProperty, DataMember]
        private Guid EntityIdentifier { get; set; }
    }
}
