//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReshimgathiServices
{
    using System;
    using System.Collections.Generic;
    
    public partial class RequestContact
    {
        public System.Guid Id { get; set; }
        public System.Guid RequestFrom { get; set; }
        public string RequestTo1 { get; set; }
        public string RequestTo2 { get; set; }
        public string RequestTo3 { get; set; }
        public string RequestTo4 { get; set; }
        public string RequestTo5 { get; set; }
        public bool IsRequestServed { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
    
        public virtual UserProfileDetail UserProfileDetail { get; set; }
    }
}
