using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using TCCVetCare.Models;

namespace TCCVetCare.Repositories
{
    public class ServiceRepository
    {
        DatabaseConnection con = new DatabaseConnection();


        public void insertService(ServiceModel service)
        {
            MySqlCommand cmd = new MySqlCommand("insert into tbService values(default, @nameService, @priceService, @descriptionService)", con.ConectarBD());
            cmd.Parameters.Add("@nameService", MySqlDbType.VarChar).Value = service.nameService;
            cmd.Parameters.Add("@priceService", MySqlDbType.VarChar).Value = service.priceService;
            cmd.Parameters.Add("@descriptionService", MySqlDbType.VarChar).Value = service.descriptionService;
            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }

        public List<ServiceModel> getService()
        {
            List<ServiceModel> listServices = new List<ServiceModel>();
            MySqlCommand cmd = new MySqlCommand("select * from tbService", con.ConectarBD());

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
                listServices.Add(
                    new ServiceModel
                    {
                        idService = Convert.ToString(dr["idService"]),
                        nameService = Convert.ToString(dr["nameService"]),
                        priceService = Convert.ToDouble(dr["priceService"]),
                        descriptionService = Convert.ToString(dr["descriptionService"])

                    });
            }
            return listServices;
        }

        public bool deleteService(int id)
        {
            MySqlCommand cmd = new MySqlCommand("delete from tbService where idService=@id", con.ConectarBD());
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


        public bool updateService(ServiceModel service)
        {
            MySqlCommand cmd = new MySqlCommand("update tbService set nameService=@nameService, priceService=@priceService, descriptionService=@descriptionService" +
                " where " +
                "idService=@idService", con.ConectarBD());

            cmd.Parameters.AddWithValue("@nameService", service.nameService);
            cmd.Parameters.AddWithValue("@priceService", service.priceService);
            cmd.Parameters.AddWithValue("@descriptionService", service.descriptionService);
            cmd.Parameters.AddWithValue("@idService", service.idService);

            int i = cmd.ExecuteNonQuery();
            if (i >= 1)
            {
                return true;

            }
            else
            {
                return false;
            }
            con.DesconectarBD();
        }

    }
}