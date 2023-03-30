using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Configuration;
using web_api.Models;
using System.IO;
using System.Text;
using web_api.Configurations;

namespace web_api.Controllers
{
    public class PacientesController : ApiController
    {
        // GET: api/Pacientes
        public IHttpActionResult Get()
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

            return Ok(pacientes);
        }

        // GET: api/Pacientes/5
        public IHttpActionResult Get(int id)
        {
            Models.Paciente paciente = new Models.Paciente();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                conn.Open ();

                using(SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $"select codigo, nome, email from paciente where codigo = @codigo";
                    cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.Int)).Value = id;

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        paciente.Codigo = (int)dr["codigo"];
                        paciente.Nome = (string)dr["nome"];
                        paciente.Email = (string)dr["email"];
                    }
                }
            }

            if(paciente.Codigo == 0)
                return NotFound();

            return Ok(paciente);
        }

        // POST: api/Pacientes
        public IHttpActionResult Post(Models.Paciente paciente)
        {
            try
            {
                if (paciente.Nome.Trim() == "" || paciente.Email.Trim() == "") //return early
                    return BadRequest("Nome e/ou Email do paciente não pode(m) ser vazio(s)");

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = $"insert into paciente (nome, email) values (@nome,@email); select convert(int,@@identity) as codigo;";//convert para garantir que o identity é mesmo um inteiro
                        cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar, 200)).Value = paciente.Nome;
                        cmd.Parameters.Add(new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100)).Value = paciente.Email;

                        paciente.Codigo = (int)cmd.ExecuteScalar();
                    }
                }
                return Ok(paciente);
            }catch(Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(Log.getLogDirectory(), true))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(DateTime.Now);
                    sb.Append(": ");
                    sb.Append(ex.GetType());
                    sw.WriteLine(sb.ToString());
                }

                return InternalServerError();
            }
        }

        // PUT: api/Pacientes/5
        public IHttpActionResult Put(int id, Models.Paciente paciente)
        {
            if (id != paciente.Codigo)
                return BadRequest("Código no parâmetro é diferente do código do paciente");

            if (paciente.Nome.Trim() == "" || paciente.Email.Trim() == "")
                return BadRequest("Nome e/ou Email do paciente não pode(m) ser vazio(s)");

            int linhasAfetadas = 0;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $"update paciente set nome = @nome, email = @email where codigo = @codigo;";
                    cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar, 200)).Value = paciente.Nome;
                    cmd.Parameters.Add(new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100)).Value = paciente.Email;
                    cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.Int)).Value = id;//id é um parametro do método put

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }

            if (linhasAfetadas == 0)
                return NotFound();

            return Ok(paciente);

        }

        // DELETE: api/Pacientes/5
        public IHttpActionResult Delete(int id)
        {
            int linhasAfetadas = 0;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $"delete from paciente where codigo = @codigo;";
                    cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.Int)).Value = id;//id é um parametro do método delete

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }

            if (linhasAfetadas == 0)
                return NotFound(); //se nenhuma linha foi afetada, retorna um not found

            return Ok();
        }
    }
}
