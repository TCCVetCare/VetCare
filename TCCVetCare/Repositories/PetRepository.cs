using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using TCCVetCare.Models;

namespace TCCVetCare.Repositories
{
    public class PetRepository
    {
        DatabaseConnection con = new DatabaseConnection();

        public void insertPet(PetModel pet)
        {
            // @idCustomer
            MySqlCommand cmd = new MySqlCommand(
                "insert into tbPet values(default, @namePet, @idCustomer, @idBreedPet, @agePet, @genderPet, @idSpeciesPet, @imagePet, @IdPlan)",
                con.ConectarBD()
            );
            cmd.Parameters.Add("@idCustomer", MySqlDbType.VarChar).Value = pet.idCustomer;
            cmd.Parameters.Add("@namePet", MySqlDbType.VarChar).Value = pet.namePet;
            cmd.Parameters.Add("@idBreedPet", MySqlDbType.VarChar).Value = pet.idBreedPet;
            cmd.Parameters.Add("@idSpeciesPet", MySqlDbType.VarChar).Value = pet.idSpeciesPet;
            cmd.Parameters.Add("@genderPet", MySqlDbType.VarChar).Value = pet.genderPet;
            cmd.Parameters.Add("@agePet", MySqlDbType.VarChar).Value = pet.agePet;
            cmd.Parameters.Add("@imagePet", MySqlDbType.VarChar).Value = pet.imagePet;
            cmd.Parameters.Add("@idPlan", MySqlDbType.VarChar).Value = pet.idPlan;

            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }

        public List<PetModel> getPet()
        {
            List<PetModel> listPet = new List<PetModel>();
            MySqlCommand cmd = new MySqlCommand("select * from tbPet", con.ConectarBD());

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
                listPet.Add(
                    new PetModel
                    {
                        idPet = Convert.ToString(dr["idAnimal"]),
                        idCustomer = Convert.ToString(dr["idCustomer"]),
                        namePet = Convert.ToString(dr["nameAnimal"]),
                        idBreedPet = Convert.ToString(dr["idBreedAnimal"]),
                        idSpeciesPet = Convert.ToString(dr["idSpeciesAnimal"]),
                        genderPet = Convert.ToString(dr["genderAnimal"]),
                        agePet = Convert.ToString(dr["ageAnimal"]),
                        imagePet = Convert.ToString(dr["imageAnimal"])
                    }
                );
            }
            return listPet;
        }

        public List<PetModel> getPetByIdCustomer(string id)
        {
            List<PetModel> listPet = new List<PetModel>();
            MySqlCommand cmd = new MySqlCommand(
                "SELECT * FROM tbPet WHERE idCustomer = @id",
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
                listPet.Add(
                    new PetModel
                    {
                        idPet = Convert.ToString(dr["idAnimal"]),
                        idCustomer = Convert.ToString(dr["idCustomer"]),
                        namePet = Convert.ToString(dr["nameAnimal"]),
                        idBreedPet = Convert.ToString(dr["idBreedAnimal"]),
                        idSpeciesPet = Convert.ToString(dr["idSpeciesAnimal"]),
                        genderPet = Convert.ToString(dr["genderAnimal"]),
                        agePet = Convert.ToString(dr["ageAnimal"]),
                        imagePet = Convert.ToString(dr["imageAnimal"])
                    }
                );
            }

            return listPet;
        }

        public bool deletePet(int id)
        {
            MySqlCommand cmd = new MySqlCommand(
                "delete from tbPet where idAnimal=@id",
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

        public bool updatePet(PetModel pet)
        {
            MySqlCommand cmd = new MySqlCommand(
                "update tbPet set nameAnimal=@namePet, idCustomer=@idCustomer, idBreedAnimal=@idBreedPet,  ageAnimal=@agePet, genderAnimal=@genderPet,"
                    + "idSpeciesAnimal=@idSpeciesPet, imageAnimal=@imagePet where "
                    + "idAnimal=@idPet",
                con.ConectarBD()
            );

            cmd.Parameters.AddWithValue("@namePet", pet.namePet);
            cmd.Parameters.AddWithValue("@idBreedPet", pet.idBreedPet);
            cmd.Parameters.AddWithValue("@idSpeciesPet", pet.idSpeciesPet);
            cmd.Parameters.AddWithValue("@genderPet", pet.genderPet);
            cmd.Parameters.AddWithValue("@agePet", pet.agePet);
            cmd.Parameters.AddWithValue("@idPet", pet.idPet);
            cmd.Parameters.AddWithValue("@imagePet", pet.imagePet);
            cmd.Parameters.AddWithValue("@idCustomer", pet.idCustomer);

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

        public string GetNameCustomerById(PetModel id)
        {
            string nameCustomer = string.Empty;

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT nameCustomer FROM tbCustomer WHERE idCustomer = @idCustomer",
                    con
                );
                cmd.Parameters.AddWithValue("@idCustomer", id);
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    nameCustomer = result.ToString();
                }

                con.Close(); // fechando conexão
            }

            return nameCustomer;
        }
    }
}