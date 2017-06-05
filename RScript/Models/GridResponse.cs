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
        public int Id { get; set; }
     
        public string Code { get; set; }
      
        public string Country { get; set; }

        public decimal Limit { get; set; }
    }
}