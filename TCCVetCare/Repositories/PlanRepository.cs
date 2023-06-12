using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Web;
using TCCVetCare.Models;
using TCCVetCare.ViewModels;

namespace TCCVetCare.Repositories
{
    public class PlanRepository
    {
        DatabaseConnection con = new DatabaseConnection();

        public void insertPlan(PlanModel plan)
        {
            MySqlCommand cmd = new MySqlCommand("insert into tbPlan values(default, @namePlan, @descriptionPlan, @pricePlan, @imagePlan)", con.ConectarBD());
            cmd.Parameters.Add("@namePlan", MySqlDbType.VarChar).Value = plan.namePlan;
            cmd.Parameters.Add("@descriptionPlan", MySqlDbType.VarChar).Value = plan.descriptionPlan;
            cmd.Parameters.Add("@pricePlan", MySqlDbType.Double).Value = plan.pricePlan;
            cmd.Parameters.Add("@imagePlan", MySqlDbType.VarChar).Value = plan.imagePlan;
            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }

        public List<PlanModel> getPlan()
        {
            List<PlanModel> listPlan = new List<PlanModel>();
            MySqlCommand cmd = new MySqlCommand("select * from tbPlan", con.ConectarBD());

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
                listPlan.Add(
                    new PlanModel
                    {
                        idPlan = Convert.ToString(dr["idPlan"]),
                        namePlan = Convert.ToString(dr["namePlan"]),
                        descriptionPlan = Convert.ToString(dr["descriptionPlan"]),
                        pricePlan = Convert.ToDouble(dr["pricePlan"]),
                        imagePlan = Convert.ToString(dr["imagePlan"])

                    });
            }
            return listPlan;

        }

        public bool deletePlan(int id)
        {
            MySqlCommand cmd = new MySqlCommand("delete from tbPlan where idPlan=@id", con.ConectarBD());
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

        public bool updatePlan(PlanModel plan)
        {
            MySqlCommand cmd = new MySqlCommand("update tbPlan set namePlan=@namePlan, descriptionPlan=@descriptionPlan, pricePlan=@pricePlan, imagePlan=@imagePlan  " +
                " where " +
                "idPlan=@idPlan", con.ConectarBD());

            cmd.Parameters.Add("@idPlan", MySqlDbType.VarChar).Value = plan.idPlan;
            cmd.Parameters.Add("@namePlan", MySqlDbType.VarChar).Value = plan.namePlan;
            cmd.Parameters.Add("@descriptionPlan", MySqlDbType.VarChar).Value = plan.descriptionPlan;
            cmd.Parameters.Add("@pricePlan", MySqlDbType.Double).Value = plan.pricePlan;
            cmd.Parameters.Add("@imagePlan", MySqlDbType.VarChar).Value = plan.imagePlan;
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

        public void insertPlanPet(PlanPetViewModel planPet)
        {
            MySqlCommand cmd = new MySqlCommand("insert into PlanAnimal values(default, @idPlan, @idAnimal, @idFormOfPayment)", con.ConectarBD());
            cmd.Parameters.Add("@idPlan", MySqlDbType.VarChar).Value = planPet.idPlan;
            cmd.Parameters.Add("@idAnimal", MySqlDbType.VarChar).Value = planPet.idPet;
            cmd.Parameters.Add("@idFormOfPayment", MySqlDbType.VarChar).Value = planPet.idFormOfPayment;
            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }

        public void UpdateIdPlan(string idPlan, string idAnimal)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE tbPet SET idPlan = @idPlan WHERE idAnimal = @idAnimal", con.ConectarBD());
            cmd.Parameters.Add("@idPlan", MySqlDbType.Int32).Value = idPlan;
            cmd.Parameters.Add("@idAnimal", MySqlDbType.Int32).Value = idAnimal;
            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }
    }
}