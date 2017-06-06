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
