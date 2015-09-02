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
    public partial class CollectionPoint : ITrackable
    {
        public CollectionPoint()
        {
            this.Department = new List<Department>();
        }
    
        [DataMember]
        public int CPID { get; set; }
        [DataMember]
        public string CPName { get; set; }
        [DataMember]
        public string CPAddress { get; set; }
        [DataMember]
        public Nullable<double> CPLat { get; set; }
        [DataMember]
        public Nullable<double> CPLgt { get; set; }
    
        [DataMember]
        public ICollection<Department> Department { get; set; }
    
        [DataMember]
        public TrackingState TrackingState { get; set; }
        [DataMember]
        public ICollection<string> ModifiedProperties { get; set; }
        [JsonProperty, DataMember]
        private Guid EntityIdentifier { get; set; }
    }
}
