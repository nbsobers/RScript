using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RScript.Models
{
    public class GridResponse<T>
    {
        public GridResponse()
        {
            rows = new List<T>();
        }

       
        public int current { get; set; }
      
        public int[] rowCount { get; set; } 
        public int total { get; set; }
    
        public IList<T>  rows { get; set; }
    }

    public class Model1Response
    {
        public int PurchaseOrderID  { get; set; }

        public int PurchaseOrderDetailID { get; set; }

        public DateTime DueDate { get; set; }

        public Decimal OrderQty { get; set; }

        public int ProductID { get; set; }

        public Decimal UnitPrice { get; set; }

        public Decimal LineTotal { get; set; }

        public int ReceivedQty { get; set; }

        public int RejectedQty { get; set; }

        public int StockedQty { get; set; }

        public DateTime ModifiedDate { get; set; }
        
    }
}