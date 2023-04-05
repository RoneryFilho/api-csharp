using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Repositories.Database.SQLServer
{
    public class Medico
    {
        public static List<Models.Medico> get(string connectionString)
        {
            List<Models.Medico> medicos = new List<Models.Medico>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connectionString;
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
    }
}
