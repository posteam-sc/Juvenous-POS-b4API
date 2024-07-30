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
    
    public partial class User
    {
        public User()
        {
            this.Adjustments = new HashSet<Adjustment>();
            this.CancellationLogs = new HashSet<CancellationLog>();
            this.DeleteLogs = new HashSet<DeleteLog>();
            this.Expenses = new HashSet<Expense>();
            this.ProductPriceChanges = new HashSet<ProductPriceChange>();
            this.ProductQuantityChanges = new HashSet<ProductQuantityChange>();
            this.PurchaseDeleteLogs = new HashSet<PurchaseDeleteLog>();
            this.StockInHeaders = new HashSet<StockInHeader>();
            this.StockInHeaders1 = new HashSet<StockInHeader>();
            this.Transactions = new HashSet<Transaction>();
            this.UsePrePaidDebts = new HashSet<UsePrePaidDebt>();
            this.User1 = new HashSet<User>();
            this.User11 = new HashSet<User>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> UserRoleId { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public Nullable<int> ShopId { get; set; }
        public string MenuPermission { get; set; }
        public string UserCodeNo { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual ICollection<Adjustment> Adjustments { get; set; }
        public virtual ICollection<CancellationLog> CancellationLogs { get; set; }
        public virtual ICollection<DeleteLog> DeleteLogs { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<ProductPriceChange> ProductPriceChanges { get; set; }
        public virtual ICollection<ProductQuantityChange> ProductQuantityChanges { get; set; }
        public virtual ICollection<PurchaseDeleteLog> PurchaseDeleteLogs { get; set; }
        public virtual ICollection<StockInHeader> StockInHeaders { get; set; }
        public virtual ICollection<StockInHeader> StockInHeaders1 { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<UsePrePaidDebt> UsePrePaidDebts { get; set; }
        public virtual ICollection<User> User1 { get; set; }
        public virtual User User2 { get; set; }
        public virtual ICollection<User> User11 { get; set; }
        public virtual User User3 { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}
