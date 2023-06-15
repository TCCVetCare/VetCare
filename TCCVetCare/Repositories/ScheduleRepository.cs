using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TCCVetCare.Models;

namespace TCCVetCare.Repositories
{
    public class ScheduleRepository
    {
        DatabaseConnection con = new DatabaseConnection();

        public void insertSchedule(ScheduleModel schedule)
        {
            MySqlCommand cmd = new MySqlCommand(
                "insert into tbSchedule values(default, @idCustomer, @idPet, @idService, @dateSchedule, @timeSchedule, @observations)",
                con.ConectarBD()
            );
            cmd.Parameters.Add("@idCustomer", MySqlDbType.VarChar).Value = schedule.idCustomer;
            cmd.Parameters.Add("@idPet", MySqlDbType.VarChar).Value = schedule.idPet;
            cmd.Parameters.Add("@idService", MySqlDbType.VarChar).Value = schedule.idService;
            cmd.Parameters.Add("@dateSchedule", MySqlDbType.VarChar).Value = schedule.dateSchedule;
            cmd.Parameters.Add("@timeSchedule", MySqlDbType.VarChar).Value = schedule.timeSchedule;
            cmd.Parameters.Add("@observations", MySqlDbType.VarChar).Value = schedule.observations;

            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }

        public List<ScheduleModel> GetSchedule()
        {
            List<ScheduleModel> listSchedule = new List<ScheduleModel>();
            MySqlCommand cmd = new MySqlCommand("select * from tbSchedule", con.ConectarBD());

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
                listSchedule.Add(
                    new ScheduleModel
                    {
                        idSchedule = Convert.ToString(dr["idSchedule"]),
                        idCustomer = Convert.ToString(dr["idCustomer"]),
                        idPet = Convert.ToString(dr["idAnimal"]),
                        idService = Convert.ToString(dr["idService"]),
                        dateSchedule = Convert.ToString(dr["dateSchedule"]),
                        timeSchedule = Convert.ToString(dr["timeSchedule"]),
                        observations = Convert.ToString(dr["observations"]),
                    }
                );
            }
            return listSchedule;
        }

        public List<ScheduleModel> GetScheduleByCustomerId(ScheduleModel idCustomer)
        {
            List<ScheduleModel> listSchedule = new List<ScheduleModel>();
            MySqlCommand cmd = new MySqlCommand(
                "SELECT * FROM tbSchedule WHERE idCustomer = @idCustomer",
                con.ConectarBD()
            );
            cmd.Parameters.AddWithValue("@idCustomer", idCustomer);

            //adapter para lista
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

            //tabela virtual
            DataTable db = new DataTable();
            adapter.Fill(db);

            con.DesconectarBD();

            //enquanto existir linhas (registros) no banco
            //o foreach irá adicionar os valores vindos do banco nos atributos da ModelCliente
            foreach (DataRow dr in db.Rows)
            {
                listSchedule.Add(
                    new ScheduleModel
                    {
                        idSchedule = Convert.ToString(dr["idSchedule"]),
                        idCustomer = Convert.ToString(dr["idCustomer"]),
                        idPet = Convert.ToString(dr["idAnimal"]),
                        idService = Convert.ToString(dr["idService"]),
                        dateSchedule = Convert.ToString(dr["dateSchedule"]),
                        timeSchedule = Convert.ToString(dr["timeSchedule"]),
                        observations = Convert.ToString(dr["observations"])
                    }
                );
            }
            return listSchedule;
        }

        public string GetAnimalByScheduleId(ScheduleModel idSchedule)
        {
            string petName = string.Empty;

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT a.nameAnimal FROM tbSchedule s INNER JOIN tbAnimal a ON s.idAnimal = a.idAnimal WHERE s.idSchedule = @idSchedule",
                    con
                );
                cmd.Parameters.AddWithValue("@idSchedule", idSchedule);
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    petName = result.ToString();
                }

                con.Close(); // fechando conexão
            }

            return petName;
        }

        public string GetNameServiceByScheduleId(ScheduleModel idSchedule)
        {
            string serviceName = string.Empty;

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT s.nameService FROM tbSchedule s WHERE s.idSchedule = @idSchedule",
                    con
                );
                cmd.Parameters.AddWithValue("@idSchedule", idSchedule);
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    serviceName = result.ToString();
                }

                con.Close(); // fechando conexão
            }

            return serviceName;
        }

        public bool DeleteSchedule(int id)
        {
            MySqlCommand cmd = new MySqlCommand(
                "delete from tbSchedule where idSchedule=@id",
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

        public bool UpdateSchedule(ScheduleModel schedule)
        {
            MySqlCommand cmd = new MySqlCommand(
                "update tbSchedule set idCustomer=@idCustomer, idAnimal=@idPet, idService=@idService, "
                    + "dateSchedule=@dateSchedule, timeSchedule=@timeSchedule, observations=@observations where "
                    + "idSchedule=@idSchedule",
                con.ConectarBD()
            );
            cmd.Parameters.Add("@idCustomer", MySqlDbType.VarChar).Value = schedule.idCustomer;
            cmd.Parameters.Add("@idPet", MySqlDbType.VarChar).Value = schedule.idPet;
            cmd.Parameters.Add("@idService", MySqlDbType.VarChar).Value = schedule.idService;
            cmd.Parameters.Add("@dateSchedule", MySqlDbType.VarChar).Value = schedule.dateSchedule;
            cmd.Parameters.Add("@timeSchedule", MySqlDbType.VarChar).Value = schedule.timeSchedule;
            cmd.Parameters.Add("@observations", MySqlDbType.VarChar).Value = schedule.observations;
            cmd.Parameters.Add("@idSchedule", MySqlDbType.VarChar).Value = schedule.idSchedule;


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


        public bool UpdateSchedule2(ScheduleModel schedule)
        {
            MySqlCommand cmd = new MySqlCommand(
                "UPDATE tbSchedule "
                + "INNER JOIN tbCustomer ON tbSchedule.idCustomer = tbCustomer.idCustomer "
                + "INNER JOIN tbPet ON tbSchedule.idAnimal = tbPet.idAnimal "
                + "INNER JOIN tbService ON tbSchedule.idService = tbService.idService "
                + "SET tbSchedule.idCustomer=@idCustomer, tbSchedule.idAnimal=@idPet, tbSchedule.idService=@idService, "
                + "tbSchedule.dateSchedule=@dateSchedule, tbSchedule.timeSchedule=@timeSchedule, tbSchedule.observations=@observations "
                + "WHERE tbSchedule.idSchedule=@idSchedule",
                con.ConectarBD()
            );

            cmd.Parameters.AddWithValue("@idCustomer", schedule.idCustomer);
            cmd.Parameters.AddWithValue("@idPet", schedule.idPet);
            cmd.Parameters.AddWithValue("@idService", schedule.idService);
            cmd.Parameters.AddWithValue("@idSchedule", schedule.idSchedule);
            cmd.Parameters.AddWithValue("@dateSchedule", schedule.dateSchedule);
            cmd.Parameters.AddWithValue("@timeSchedule", schedule.timeSchedule);
            cmd.Parameters.AddWithValue("@observations", schedule.observations);

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


        public List<ScheduleModel> getScheduleByIdCustomer(string id)
        {
            List<ScheduleModel> listSchedule = new List<ScheduleModel>();
            MySqlCommand cmd = new MySqlCommand(
                "SELECT * FROM tbSchedule WHERE idCustomer = @id",
                con.ConectarBD()
            );
            cmd.Parameters.AddWithValue("@id", id);

            // adapter para lista
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

            // tabela virtual
            DataTable db = new DataTable();
            adapter.Fill(db);

            con.DesconectarBD();

            // enquanto existir linhas (registros) no banco
            // o foreach irá adicionar os valores vindos do banco nos atributos da classe Animal

            foreach (DataRow dr in db.Rows)
            {
                listSchedule.Add(
                    new ScheduleModel
                    {
                        idSchedule = Convert.ToString(dr["idSchedule"]),
                        idCustomer = Convert.ToString(dr["idCustomer"]),
                        idPet = Convert.ToString(dr["idAnimal"]),
                        idService = Convert.ToString(dr["idService"]),
                        dateSchedule = Convert.ToString(dr["dateSchedule"]),
                        timeSchedule = Convert.ToString(dr["timeSchedule"]),
                        observations = Convert.ToString(dr["observations"])
                    }
                );
            }

            return listSchedule;
        }
    }
}