using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TCCVetCare.Models;

namespace TCCVetCare.Repositories
{
    public class SupplierRepository
    {
        DatabaseConnection con = new DatabaseConnection();

        public void insertSupplier(SupplierModel supplier)
        {
            MySqlCommand cmd = new MySqlCommand(
                "insert into tbSupplier values(default, @nameSupplier, @cnpjSupplier)",
                con.ConectarBD()
            );
            cmd.Parameters.Add("@nameSupplier", MySqlDbType.VarChar).Value = supplier.nameSupplier;
            cmd.Parameters.Add("@cnpjSupplier", MySqlDbType.VarChar).Value = supplier.cnpjSupplier;
            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }

        public List<SupplierModel> getSupplier()
        {
            List<SupplierModel> listSupplier = new List<SupplierModel>();
            MySqlCommand cmd = new MySqlCommand("select * from tbSupplier", con.ConectarBD());

            //adapter para lista
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

            //tabela virtual
            DataTable db = new DataTable();
            adapter.Fill(db);

            con.DesconectarBD();

            //enquanto existir linhas(registros) no banco
            //o foreach irá adicionar os valors vindo do banco nos atributos da ModelCliente

            foreach (DataRow dr in db.Rows)
            {
                listSupplier.Add(
                    new SupplierModel
                    {
                        idSupplier = Convert.ToString(dr["idSupplier"]),
                        nameSupplier = Convert.ToString(dr["nameSupplier"]),
                        cnpjSupplier = Convert.ToString(dr["cnpj"]),
                    }
                );
            }
            return listSupplier;
        }

        public bool deleteSupplier(int id)
        {
            MySqlCommand cmd = new MySqlCommand(
                "delete from tbSupplier where idSupplier=@id",
                con.ConectarBD()
            );
            cmd.Parameters.AddWithValue("id", id);

            int i = cmd.ExecuteNonQuery();
            con.DesconectarBD();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool updateSupplier(SupplierModel supplier)
        {
            MySqlCommand cmd = new MySqlCommand(
                "update tbSupplier set nameSupplier=@nameSupplier, cnpj=@cnpjSupplier"
                    + " where "
                    + "idSupplier=@idSupplier",
                con.ConectarBD()
            );

            cmd.Parameters.AddWithValue("@idSupplier", supplier.idSupplier);
            cmd.Parameters.AddWithValue("@nameSupplier", supplier.nameSupplier);
            cmd.Parameters.AddWithValue("@cnpjSupplier", supplier.cnpjSupplier);
            int i = cmd.ExecuteNonQuery();
            con.DesconectarBD();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        MySqlConnection cn = new MySqlConnection(
            "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
        );

        public string SelectCnpjSuppler(string cnpj)
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("call spSelectCnpjSuppler(@cnpj);");
            cmd.Parameters.Add("@cnpj", MySqlDbType.String).Value = cnpj;
            cmd.Connection = cn;
            string CNPJ = (string)cmd.ExecuteScalar();
            cn.Close();
            if (CNPJ == null)

                CNPJ = "";
            return CNPJ;
        }
    }
}