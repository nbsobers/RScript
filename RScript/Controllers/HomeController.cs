using CsvHelper;
using RScript.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using RDotNet;
using log4net;

namespace RScript.Controllers
{
    public class HomeController : Controller
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));
        private static REngine engine;

        public HomeController()
        {
            try
            {
                if (engine != null) return;

                engine = REngine.GetInstance();
                engine.Initialize();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
           
        }

        public ActionResult Index(int? modelId)
        {
            if (modelId.HasValue)
                ViewBag.modelId = modelId;
            else
                ViewBag.modelId = 1;

            return View();
        }

        [HttpPost]
        public JsonResult GenerateReort(GenerateReportModel model)
        {
            var response = new ResponseModel() {IsSuccess = false };
            try
            {
                log.Debug(model);
                string errorMessage = "Model is not valid: ";

                if (!ModelState.IsValid)
                {                    
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            errorMessage = errorMessage + "; " + error.ErrorMessage;
                        }
                    }

                    if (Request.Files.Count == 0) {
                        errorMessage = errorMessage + "; " + "File not selected";
                    }
                   
                    response.Message = errorMessage;
                    return Json(response);
                }

                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var inputFilePath = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                        file.SaveAs(inputFilePath);

                        //R Process
                        //REngine.SetEnvironmentVariables();
                        //RHelper.InitREngine();
                        //REngine engine = RHelper.engine;
                        //REngine engine = REngine.GetInstance();

                        //using (REngine engine = REngine.GetInstance())
                        //{
                        var rScriptFile = Path.Combine(Server.MapPath("~/Commands/"), string.Format("model{0}.R", model.ModelId));
                        rScriptFile = rScriptFile.Replace("\\", "/");
                        log.Debug(rScriptFile);
                        var outputFile = getOutputFile(); //Path.Combine(Server.MapPath("~/Output/"), "Generated.csv");

                        string[] input = new string[6];
                        input[0] = model.Server;
                        input[1] = model.Database;
                        input[2] = model.Username;
                        input[3] = model.Password;
                        input[4] = inputFilePath;
                        input[5] = outputFile;

                        log.Debug(input);

                        engine.SetCommandLineArguments(input);

                        string expression = string.Format("source('{0}')", rScriptFile);
                        engine.Evaluate(expression);
                        response.IsSuccess = true;
                        // engine.Evaluate("source('C:/01_Dev/POC/RScript/RScript/Reference/model1.R')");
                        //Rscript  D:\Dev\POC\RScript\RScript\Reference\cmm.R LPT-002384\SQLEXPRESS  AdventureWorks2016CTP3 sa Soders@123 D:\Dev\POC\RScript\RScript\Reference\cc.csv
                        //"C:\Program Files\R\R-3.4.0\bin\i386\Rscript"  C:\01_Dev\POC\RScript\RScript\Commands\model1.R SOBS-DELL-3470\MSSQL2016  AdventureWorks2016CTP3 rscript Rscript@123 C:\01_Dev\POC\RScript\RScript\Uploads\cc.csv
                        //"C:\Program Files\R\R-3.4.0\bin\i386\Rscript"  C:\01_Dev\POC\RScript\RScript\Commands\model1.R SOBS-DELL-3470\MSSQL2016  AdventureWorks2016CTP3 rscript Rscript@123 C:\01_Dev\POC\RScript\RScript\Uploads\cc.csv C:\01_Dev\POC\RScript\RScript\Output\Generated.csv
                        //engine.Dispose();

                        //}
                    }
                } else{
                    errorMessage = errorMessage + "; " + "File not selected";
                    response.Message = errorMessage;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);              
                response.Message = ex.Message;
            }

            return Json(response);
        }

        public JsonResult GetData()
        {
            try
            {
                var path = Path.Combine(Server.MapPath("~/Output/"), "Generated.csv");
                var response = new List<Model1Response>();

                if (System.IO.File.Exists(path))
                {
                    using (TextReader fileReader = System.IO.File.OpenText(path))
                    {
                        var csv = new CsvReader(fileReader);
                        csv.Configuration.HasHeaderRecord = true;
                        response = csv.GetRecords<Model1Response>().ToList();
                    }
                }
                var gridRes = new GridResponse<Model1Response>();
                gridRes.current = 1;
                gridRes.rowCount = new int[1];
                gridRes.rowCount[0] = -1;
                gridRes.total = response.Count;
                gridRes.rows = response;

                return Json(gridRes);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(ex);
            }
        }

        public JsonResult ClearData()
        {            
            var response = new ResponseModel() { IsSuccess = false };
            try
            {
                var outputFile = getOutputFile();
                if (System.IO.File.Exists(outputFile))
                {
                    System.IO.File.Delete(outputFile);
                }

                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                log.Error(ex);              
                response.Message = ex.Message;
            }

            return Json(response,JsonRequestBehavior.AllowGet);
        }

        private string getOutputFile()
        {
            var outputFile = Path.Combine(Server.MapPath("~/Output/"), "Generated.csv");
            return outputFile;
        }

    }
}