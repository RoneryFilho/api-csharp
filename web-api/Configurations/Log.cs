using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace web_api.Configurations
{
    public class Log
    {
        public static string getLogDirectory()
        {
            string fileName = $"{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
            string path = System.Configuration.ConfigurationManager.AppSettings["consultorio-log-path"];
            string fullPath = Path.Combine(path, fileName);
            return fullPath;
        }

    }
}