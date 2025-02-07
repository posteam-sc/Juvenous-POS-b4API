//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace POS.APP_Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class CancellationLog
    {
        public long Id { get; set; }
        public string TransactionId { get; set; }
        public long TransactionDetailId { get; set; }
        public System.DateTime DateTime { get; set; }
        public int TotalOffsetQty { get; set; }
        public int CancelledQty { get; set; }
        public decimal CancelledAmount { get; set; }
        public int CancelledBy { get; set; }
    
        public virtual Transaction Transaction { get; set; }
        public virtual User User { get; set; }
        public virtual TransactionDetail TransactionDetail { get; set; }
    }
}
