//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReturnPending
    {
        public int BookID { get; set; }
        public string BookName { get; set; }
        public string BookType { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
        public string DonatedBy { get; set; }
        public System.DateTime IssueDate { get; set; }
        public System.DateTime ReturnDate { get; set; }
        public string EMP_EPFNo { get; set; }
        public string EMP_Name { get; set; }
    }
}
