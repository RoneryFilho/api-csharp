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
        public IHttpActionResult Get()
        {
            List<Models.Medico> medicos = new List<Models.Medico>();

            string connectionString = Configurations.SQLServer.getConnectionString();

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

            return Ok(medicos);
        }


        // GET: api/Medicos/5
        public IHttpActionResult Get(int id)
        {
            Models.Medico medico = new Models.Medico();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $"select codigo, nome, datanascimento, crm from medico where  codigo = @codigo;";
                    cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.Int)).Value = id;

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        medico.Nome = (string)dr["nome"];
                        medico.Crm = (string)dr["crm"];
                        medico.DataNascimento = dr["datanascimento"] == DBNull.Value ? null : (DateTime?)dr["datanascimento"];
                        medico.Codigo = (int)dr["codigo"];
                    }
                }
            }
            if (medico.Codigo == 0)
                return NotFound();

            return Ok(medico);
        }

        // POST: api/Medicos
        public IHttpActionResult Post(Models.Medico medico)
        {
            if (medico.Nome.Trim() == "" || medico.Crm.Trim() == "")
                return BadRequest("Nome e/ou CRM do médico não pode(m) estar vazio(s)");

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    SqlDataReader dr = cmd.ExecuteReader();

                    cmd.Connection = conn;
                    cmd.CommandText = "insert into medico (nome,datanascimento, crm) values (@nome, @datanascimento, @crm); select convert(int,@@identity) as codigo;";//convert para garantir que o identity é mesmo um inteiro
                    cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = medico.Nome;
                    medico.DataNascimento = dr["datanascimento"] == DBNull.Value ? null : (DateTime?)dr["datanascimento"];

                    /*
                     if (medico.DataNascimento != null)
                        cmd.Parameters.Add(new SqlParameter("@datanascimento", System.Data.SqlDbType.Date)).Value = medico.DataNascimento;
                    else
                        cmd.Parameters.Add(new SqlParameter("@datanascimento", System.Data.SqlDbType.Date)).Value = DBNull.Value;*/

                    cmd.Parameters.Add(new SqlParameter("@crm", System.Data.SqlDbType.Char)).Value = medico.Crm;

                    medico.Codigo = (int)cmd.ExecuteScalar();
                }
            }
            return Ok(medico);
        }

        // PUT: api/Medicos/5
        public IHttpActionResult Put(int id, Models.Medico medico)
        {
            if (id != medico.Codigo)
                return BadRequest("O código do parâmetro é diferente do código do médico");

            if (medico.Nome.Trim() == "" || medico.Crm.Trim() == "")
                return BadRequest("Nome e/ou CRM não pode(m) ficar vazio(s)");

            int linhasAfetadas = 0;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    SqlDataReader dr = cmd.ExecuteReader();

                    cmd.Connection = conn;
                    cmd.CommandText = $"update medico set nome = @nome, datanascimento = @datanascimento, crm = @crm where codigo = @codigo;";

                    cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = medico.Nome;
                    medico.DataNascimento = dr["datanascimento"] == DBNull.Value ? null : (DateTime?)dr["datanascimento"];

                    /*
                     if (medico.DataNascimento != null)
                        cmd.Parameters.Add(new SqlParameter("@datanascimento", System.Data.SqlDbType.Date)).Value = medico.DataNascimento;
                    else
                        cmd.Parameters.Add(new SqlParameter("@datanascimento", System.Data.SqlDbType.Date)).Value = DBNull.Value;
                    */

                    cmd.Parameters.Add(new SqlParameter("@crm", System.Data.SqlDbType.Char)).Value = medico.Crm;
                    cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.Int)).Value = id;

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }

            if (linhasAfetadas == 0)
                return NotFound();

            return Ok(medico);
        }

        // DELETE: api/Medicos/5
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
                    cmd.CommandText = $"delete from medico where codigo = @codigo;";
                    cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.Int)).Value = id;

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }

            if (linhasAfetadas == 0)
                return NotFound();

            return Ok();
        }
    }
}
