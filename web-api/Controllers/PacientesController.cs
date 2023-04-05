using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Configuration;
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
            try
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
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(Log.getLogDirectory(), true))
                {
                    sw.WriteLine($"\n------\nData:{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} \n Mensagem:{ex.Message} \n StackTrace:{ex.StackTrace} \n InnerException:{ex.InnerException} \n Tipo do erro: {ex.GetType()} \n Source: {ex.Source} \n TargetSite: {ex.TargetSite}");
                }

                return BadRequest();
            }
        }

        // GET: api/Pacientes/5
        public IHttpActionResult Get(int id)
        {
            try
            {
                Models.Paciente paciente = new Models.Paciente();
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
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

                if (paciente.Codigo == 0)
                    return NotFound();

                return Ok(paciente);
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(Log.getLogDirectory(), true))
                {
                    sw.WriteLine($"\n------\nData:{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} \n Mensagem:{ex.Message} \n StackTrace:{ex.StackTrace} \n InnerException:{ex.InnerException} \n Tipo do erro: {ex.GetType()} \n Source: {ex.Source} \n TargetSite: {ex.TargetSite}");
                }

                return BadRequest();
            }
        }

        // POST: api/Pacientes
        public IHttpActionResult Post(Models.Paciente paciente)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(paciente.Nome) || String.IsNullOrWhiteSpace(paciente.Email)) //return early
                    return BadRequest("Nome e/ou Email do paciente não pode(m) ser vazio(s)");
                if(paciente.Nome.Length > 200 || paciente.Email.Length > 100)
                    return BadRequest("Nome e/ou Email do paciente não pode(m) ter mais de 200 e 100 caracteteres respectivamente");

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
                    sw.WriteLine($"\n------\nData:{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} \n Mensagem:{ex.Message} \n StackTrace:{ex.StackTrace} \n InnerException:{ex.InnerException} \n Tipo do erro: {ex.GetType()} \n Source: {ex.Source} \n TargetSite: {ex.TargetSite}");
                }

                return InternalServerError();
            }
        }

        // PUT: api/Pacientes/5
        public IHttpActionResult Put(int id, Models.Paciente paciente)
        {
            try
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
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(Log.getLogDirectory(), true))
                {
                    sw.WriteLine($"\n------\nData:{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} \n Mensagem:{ex.Message} \n StackTrace:{ex.StackTrace} \n InnerException:{ex.InnerException} \n Tipo do erro: {ex.GetType()} \n Source: {ex.Source} \n TargetSite: {ex.TargetSite}");
                }

                return InternalServerError();
            }
        }

        // DELETE: api/Pacientes/5
        public IHttpActionResult Delete(int id)
        {
            try
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
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(Log.getLogDirectory(), true))
                {
                    sw.WriteLine($"\n------\nData:{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} \n Mensagem:{ex.Message} \n StackTrace:{ex.StackTrace} \n InnerException:{ex.InnerException} \n Tipo do erro: {ex.GetType()} \n Source: {ex.Source} \n TargetSite: {ex.TargetSite}");
                }

                return InternalServerError();
            }
        }
    }
}
