using RDotNet;

namespace RScript.Controllers
{
    public static class RHelper
    {
        public static REngine engine;

        public static void InitREngine()
        {
            //REngine.SetEnvironmentVariables();

            if (engine == null)
            {
                engine = REngine.GetInstance();
            }
        }
    }
}