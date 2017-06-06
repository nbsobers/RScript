using CsvHelper;
using RScript.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RDotNet;

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
        public JsonResult GenerateReort(GenerateReportModel model)
        {       
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {  
                        var fileName = Path.GetFileName(file.FileName);
                        var inputFilePath = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                        file.SaveAs(inputFilePath);

                        //R Process
                        REngine.SetEnvironmentVariables();
                        using (REngine engine = REngine.GetInstance())
                        {
                            var rScriptFile = Path.Combine(Server.MapPath("~/Reference/"), "cmm1.R");
                            var outputFile = Path.Combine(Server.MapPath("~/Output/"), "Generated.csv");

                            string[] input = new string[5];                           
                            input[0] = @"LPT-002384\SQLEXPRESS";//model.Server;
                            input[1] = "AdventureWorks2016CTP3";//model.Database;
                            input[2] = "sa";//model.Username;
                            input[3] = "Soders@123";//model.Password;
                            input[4] = inputFilePath;
                            

                            engine.SetCommandLineArguments(input);
                            engine.Evaluate(@"source(" + rScriptFile + ")");
                            //Rscript  D:\Dev\POC\RScript\RScript\Reference\cmm.R LPT-002384\SQLEXPRESS  AdventureWorks2016CTP3 sa Soders@123 D:\Dev\POC\RScript\RScript\Reference\cc.csv
                            engine.Dispose();
                            Console.ReadKey();
                           
                        }
                    }
                }

            }
            catch (Exception ex)
            {

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