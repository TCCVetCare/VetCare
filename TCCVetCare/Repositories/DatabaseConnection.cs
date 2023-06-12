using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TCCVetCare.Repositories
{
    public class DatabaseConnection
    {

        MySqlConnection cn = new MySqlConnection("Server=localhost;DataBase=teste2;User=root;pwd=12345678");
        private static String msg;

        public MySqlConnection ConectarBD()
        {
            try
            {
                cn.Open();
            }
            catch (Exception ex)
            {
                msg = "Erro ao conectar" + ex.Message;
            }
            return cn;
        }

        public MySqlConnection DesconectarBD()
        {
            try
            {
                cn.Close();
            }
            catch (Exception ex)
            {
                msg = "Erro ao desconectar" + ex.Message;
            }
            return cn;

        }

    }
}