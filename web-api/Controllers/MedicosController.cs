using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace web_api.Controllers
{
    public class MedicosController : ApiController
    {
        // GET: api/Medicos
        public List<Models.Medico> Get()
        {
            List<Models.Medico> medicos = new List<Models.Medico>();

            string connectionString = @"Server=GLADOS_NOTE\SQLEXPRESS;Database=consultorio;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open(); // abre a conexão com o SGBD

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select codigo, nome, datanascimento, crm from medico;";

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Models.Medico medico = new Models.Medico();
                        medico.Codigo = (int)dr["codigo"];
                        medico.Nome = (string)dr["nome"].ToString();

                        if (dr["datanascimento"] != DBNull.Value) //DBNull.Value porque é o valor null do banco de dados
                            medico.DataNascimento = (DateTime)dr["datanascimento"];
                        else
                            medico.DataNascimento = null;

                        medico.Crm = (string)dr["crm"];
                        medicos.Add(medico);
                    }

                }
            }

            return medicos;
        }
    

        // GET: api/Medicos/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Medicos
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Medicos/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Medicos/5
        public void Delete(int id)
        {
        }
    }
}
