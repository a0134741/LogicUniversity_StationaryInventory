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
    public partial class RetrievalDetail : ITrackable
    {
        [DataMember]
        public int RetSN { get; set; }
        [DataMember]
        public Nullable<int> RetID { get; set; }
        [DataMember]
        public string ItemID { get; set; }
        [DataMember]
        public Nullable<int> RequestQty { get; set; }
        [DataMember]
        public Nullable<int> ActualQty { get; set; }
    
         
        public Item Item { get; set; }
         
        public Retrieval Retrieval { get; set; }
    
         
        public TrackingState TrackingState { get; set; }
         
        public ICollection<string> ModifiedProperties { get; set; }
         
        private Guid EntityIdentifier { get; set; }
    }
}
