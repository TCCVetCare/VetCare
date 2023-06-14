using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using TCCVetCare.Models;

namespace TCCVetCare.Repositories
{
    public class ItemsCartRepository
    {

        DatabaseConnection con = new DatabaseConnection();

        public void insertItem(ItemsCartModel cm)
        {
            MySqlCommand cmd = new MySqlCommand(
                "insert into itemCart  values(default, @idCart, @idProduct, @quantity , @valuePartial)",
                con.ConectarBD()
            );

            cmd.Parameters.Add("@idCart", MySqlDbType.VarChar).Value = cm.idCart;
            cmd.Parameters.Add("@idProduct", MySqlDbType.VarChar).Value = cm.idProduct;
            cmd.Parameters.Add("@quantity", MySqlDbType.VarChar).Value = cm.quantity;
            cmd.Parameters.Add("@valuePartial", MySqlDbType.VarChar).Value = cm.valuePartial;
            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }
    }
}