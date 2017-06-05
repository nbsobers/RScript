using CsvHelper;
using RScript.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RScript.Controllers
{
    public class CSVRequest
    {
        public int Id { get; set; }

        public string Code { get; set; }
    }

    public class HomeController : Controller
    {
        public ActionResult Index(int? modelId)
        {
            if(modelId.HasValue)
                ViewBag.modelId = modelId;
            else
                ViewBag.modelId = 1;

            return View();
        }

        [HttpPost]
        public JsonResult GenerateReort(int modelId)
        {           
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {  
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                    file.SaveAs(path);

                    using (TextReader fileReader = System.IO.File.OpenText(path))
                    {
                        var csv = new CsvReader(fileReader);
                        var records = csv.GetRecords<CSVRequest>().ToList();                       
                    }
                }
            }

            return null;
        }

        public JsonResult GetData()
        {
            var path = Path.Combine(Server.MapPath("~/Output/"), "Generated.csv");
            var response = new List<Model1Response>();
            using (TextReader fileReader = System.IO.File.OpenText(path))
            {
                var csv = new CsvReader(fileReader);
                csv.Configuration.HasHeaderRecord = true;
                response = csv.GetRecords<Model1Response>().ToList();
            }

            var gridRes = new GridResponse<Model1Response>();
            gridRes.current = 1;
            gridRes.rowCount = new int[1];
            gridRes.rowCount[0] = -1;
            gridRes.total = response.Count;
            gridRes.rows = response;

            return Json(gridRes);
        }
      
    }
}