using System.Collections.Generic;
using System.Data.SqlClient;

namespace Repositories.Database.SQLServer.ADO
{
    public class Paciente : IRepository<Models.Paciente>
    {
        private readonly SqlConnection conn;

        public Paciente(string connectionString)
        {
            conn = new SqlConnection(connectionString);
        }

        public List<Models.Paciente> get()
        {
            List<Models.Paciente> pacientes = new List<Models.Paciente>();

            using (conn)
            {
                conn.Open();

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

        public Models.Paciente getById(int id)
        {
            Models.Paciente paciente = new Models.Paciente();
            using (conn)
            {
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

            return paciente;
        }

        public void add(Models.Paciente paciente)
        {
            using (conn)
            {
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
        }

        public int update(int id, Models.Paciente paciente)
        {
            int linhasAfetadas = 0;
            using (conn)
            {
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
            return linhasAfetadas;
        }


        public int delete(int id)
        {
            int linhasAfetadas = 0;
            using (conn)
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $"delete from paciente where codigo = @codigo;";
                    cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.Int)).Value = id;//id é um parametro do método delete

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }
            return linhasAfetadas;
        }
    }
}
