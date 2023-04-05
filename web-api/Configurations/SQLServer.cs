using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api.Configurations
{
    public class SQLServer
    {
        public static string getConnectionString() //método global para acesso da string de conexão, para facilitar manutenções futuras
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["consultorio"].ConnectionString;
        }
    }
}