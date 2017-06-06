using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDotNet;
using RDotNet.NativeLibrary;

namespace RConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //string rHome = System.Environment.GetEnvironmentVariable("R_HOME");
            //if(string.IsNullOrEmpty(rHome))
            //{
            //    rHome = @"C:\Program Files\R\R-3.4.0\bin\x64";
            //}
                       
            //var oldPath = System.Environment.GetEnvironmentVariable("PATH");
            //var rPath = System.Environment.Is64BitProcess ? @"C:\Program Files\R\R-3.4.0\bin\x64" : @"C:\Program Files\R\R-3.4.0\bin\i386";
            //System.Environment.SetEnvironmentVariable("R_HOME", rHome);
            //System.Environment.SetEnvironmentVariable("PATH", System.Environment.GetEnvironmentVariable("PATH"));
            //var newPath = string.Format("{0}{1}{2}", rPath,System.IO.Path.PathSeparator, oldPath);
            //System.Environment.SetEnvironmentVariable("PATH", newPath);
                       

            try
            {
                REngine.SetEnvironmentVariables();

                using (REngine engine = REngine.GetInstance())
                {
                    // .NET Framework array to R vector.
                    NumericVector group1 = engine.CreateNumericVector(new double[] { 30.02, 29.99, 30.11, 29.97, 30.01, 29.99 });
                    engine.SetSymbol("group1", group1);
                    // Direct parsing from R script.
                    NumericVector group2 = engine.Evaluate("group2 <- c(29.89, 29.93, 29.72, 29.98, 30.02, 29.98)").AsNumeric();

                    // Test difference of mean and get the P-value.
                    GenericVector testResult = engine.Evaluate("t.test(group1, group2)").AsList();
                    double p = testResult["p.value"].AsNumeric().First();

                    Console.WriteLine("Group1: [{0}]", string.Join(", ", group1));
                    Console.WriteLine("Group2: [{0}]", string.Join(", ", group2));
                    Console.WriteLine("P-value = {0:0.000}", p);

                    //Rscript.exe D:\Dev\POC\RScript\RConsole\sampleArg.R 1
                    //http://inut-santa.blogspot.ae/2017/01/executing-r-script-files-from-c-with.html

                    //string[] input = new string[2] { "3","4" };
                    string[] input = new string[6];
                    input[0] = "SOBS-DELL-3470\\MSSQL2016";//model.Server;
                    input[1] = "AdventureWorks2016CTP3";//model.Database;
                    input[2] = "rscript";//model.Username;
                    input[3] = "Rscript@123";//model.Password;
                    input[4] = "C:\\01_Dev\\POC\\RScript\\RScript\\Uploads\\cc.csv";
                    input[5] = "C:\\01_Dev\\POC\\RScript\\RScript\\Output\\Generated.csv";

                    engine.SetCommandLineArguments(input);
                    engine.Evaluate(@"source('C:\\01_Dev\\POC\\RScript\\RScript\\Reference\\model1.R')");
                    //engine.Evaluate(@"source('C:\\01_Dev\\POC\\RScript\\RConsole\\model1.R')");
                    //Rscript  D:\Dev\POC\RScript\RScript\Reference\cmm.R LPT-002384\SQLEXPRESS  AdventureWorks2016CTP3 sa Soders@123 D:\Dev\POC\RScript\RScript\Reference\cc.csv
                    //RScript D:\\Dev\\POC\\RScript\\RScript\\Reference\\cmm1.R LPT-002384\\SQLEXPRESS AdventureWorks2016CTP3 sa Soders@123 D:\\Dev\\POC\\RScript\\RScript\\Reference\\cc.csv
                    Console.ReadKey();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }

        }
    }
}
