using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Configuration;

namespace web_api.Controllers
{
    public class PacientesController : ApiController
    {
        // GET: api/Pacientes
        public List<Models.Paciente> Get()
        {
            List<Models.Paciente> pacientes = new List<Models.Paciente>();

            string connectionString = Configurations.SQLServer.getConnectionString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open(); // abre a conexão com o SGBD



                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select codigo, nome, email from paciente;";

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Models.Paciente paciente = new Models.Paciente();
                        paciente.Codigo = (int)dr["codigo"];
                        paciente.Nome = (string)dr["nome"];
                        paciente.Email = (string)dr["email"];
                        pacientes.Add(paciente);
                    }

                }
            }

            return pacientes;
        }

        // GET: api/Pacientes/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Pacientes
        public void Post(Models.Paciente paciente)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $"insert into paciente (codigo, nome, email) values (@codigo,@nome,@email)";
                    cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.Int)).Value = paciente.Codigo;
                    cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar, 200)).Value = paciente.Nome;
                    cmd.Parameters.Add(new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100)).Value = paciente.Email;
                    cmd.ExecuteNonQuery();
                }
            }

        }

        // PUT: api/Pacientes/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Pacientes/5
        public void Delete(int id)
        {
        }
    }
}
