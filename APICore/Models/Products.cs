using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICore.Models
{
    public class ProductDetails
    {
        public Products productinfo { get; set; }
        public productimg productimg { get; set; }
    }
    public class Products
    {
        public int pid { get; set; }
        public string barcode { get; set; }
        public string category { get; set; }
        public string brand { get; set; }
        public string productname { get; set; }
        public string description { get; set; }
        public string status { get; set; } // in-stocks , out-of-stock, deleted
        public int quantity { get; set; }
        public decimal price { get; set; }
        public string createdby { get; set; }
        public string updatedby { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
    }

    public class productimg
    {
        public int imgid { get; set; }
        public int pid { get; set; }
        public string image { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
    }

    public class categories
    {
        public int categoryid { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string createdby { get; set; }
        public string updatedby { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }

    }
    public class brands
    {
        public int brandid { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string createdby { get; set; }
        public string updatedby { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }

    }

    public class brandCategory
    {
        public string Brand { get; set; }
        public string Category { get; set; }
    }
}
