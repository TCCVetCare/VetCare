using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCCVetCare.Models;

namespace TCCVetCare.Repositories
{
    public class CartRepository
    {
        DatabaseConnection con = new DatabaseConnection();
        MySqlConnection cn = new MySqlConnection(
            "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
        );

        public void insertCart(CartModel cm)
        {
            MySqlCommand cmd = new MySqlCommand(
                "insert into tbCart values(default, @idCustomer, @dateSale, @timeSale , @valueTotal)",
                con.ConectarBD()
            );

            cmd.Parameters.Add("@idCustomer", MySqlDbType.VarChar).Value = cm.idCustomer;
            cmd.Parameters.Add("@dateSale", MySqlDbType.VarChar).Value = cm.dateSale;
            cmd.Parameters.Add("@timeSale", MySqlDbType.VarChar).Value = cm.timeSale;
            cmd.Parameters.Add("@valueTotal", MySqlDbType.VarChar).Value = cm.valueTotal;
            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }

        MySqlDataReader dr;

        public void searchIdSale(CartModel sale)
        {
            MySqlCommand cmd = new MySqlCommand(
                "SELECT idCart FROM tbCart ORDER BY idCart DESC limit 1",
                con.ConectarBD()
            );
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                sale.idSale = dr[0].ToString();
            }
            con.DesconectarBD();
        }
    }
}
