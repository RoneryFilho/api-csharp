using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Medico
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Crm { get; set; }

        public Medico()
        {
            this.Codigo = 0;
            this.Nome = "";
            this.DataNascimento = null;
            this.Crm = "";
        }
    }
}