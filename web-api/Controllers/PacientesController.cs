using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace web_api.Controllers
{
    public class PacientesController : ApiController
    {
        // GET: api/Pacientes
        public List<Models.Paciente> Get() {
            Models.Paciente p1 = new Models.Paciente();
            p1.Nome = "marcelo";

            Models.Paciente p2 = new Models.Paciente();
            p2.Nome = "ronery";

            List<Models.Paciente> pacientes = new List<Models.Paciente>();
            pacientes.Add(p1);
            pacientes.Add(p2);

            return pacientes;
        }

        // GET: api/Pacientes/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Pacientes
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Pacientes/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Pacientes/5
        public void Delete(int id)
        {
        }
    }
}
