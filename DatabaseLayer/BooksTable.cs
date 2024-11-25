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
    
    public partial class BooksTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BooksTable()
        {
            this.BookIssueTables = new HashSet<BookIssueTable>();
        }
    
        public int BookID { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int BookTypeID { get; set; }
        public double Price { get; set; }
        public string DonatedBy { get; set; }
        public bool Availability { get; set; }
        public int UserID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookIssueTable> BookIssueTables { get; set; }
        public virtual BooksTable BooksTable1 { get; set; }
        public virtual BooksTable BooksTable2 { get; set; }
        public virtual BookTypesTable BookTypesTable { get; set; }
        public virtual UserTable UserTable { get; set; }
    }
}