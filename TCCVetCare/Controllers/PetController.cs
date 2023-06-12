using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCCVetCare.Models;
using TCCVetCare.Repositories;
using TCCVetCare.ViewModels;
using static TCCVetCare.Models.AuthenticationModel;

namespace TCCVetCare.Controllers
{
    public class PetController : Controller
    {
        PetRepository queryPet = new PetRepository();
        CustomerRepository queryCustomer = new CustomerRepository();

        public ActionResult Index()
        {
            return View();
        }

        public void loadCustomer(string id)
        {
            List<SelectListItem> customer = new List<SelectListItem>();

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM tbCustomer WHERE idCustomer = @idCustomer",
                    con
                );
                cmd.Parameters.AddWithValue("@idCustomer", id);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    customer.Add(
                        new SelectListItem
                        {
                            Text = rdr[1].ToString(), // nome
                            Value = rdr[0].ToString() // id do autor
                        }
                    );
                }
                con.Close(); // fechando conexão
            }

            ViewBag.customer = new SelectList(customer, "Value", "Text");
        }

        public void loadBreedPet()
        {
            List<SelectListItem> breedPet = new List<SelectListItem>();

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbBreedAnimal", con);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    breedPet.Add(
                        new SelectListItem
                        {
                            Text = rdr[1].ToString(), //nome
                            Value = rdr[0].ToString() //id do autor
                        }
                    );
                }
                con.Close(); //fechando conexÃ£o
            }

            ViewBag.breedPet = new SelectList(breedPet, "Value", "Text");
        }

        public void loadSpeciesPet()
        {
            List<SelectListItem> speciesPet = new List<SelectListItem>();

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbSpeciesAnimal", con);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    speciesPet.Add(
                        new SelectListItem
                        {
                            Text = rdr[1].ToString(), //nome
                            Value = rdr[0].ToString() //id do autor
                        }
                    );
                }
                con.Close(); //fechando conexÃ£o
            }

            ViewBag.speciesPet = new SelectList(speciesPet, "Value", "Text");
        }

        public ActionResult RegisterPet()
        {
            string idCustomer = (string)Session["idCustomer"];
            loadCustomer(idCustomer);
            loadBreedPet();
            loadSpeciesPet();
            // Verifica se o usuário está autenticado
            if (!User.Identity.IsAuthenticated)
            {
                // Se não estiver autenticado, redireciona para a página de login
                return RedirectToAction("Login", "Authentication");
            }
            return View();
        }

        [HttpPost]
        public ActionResult RegisterPet(PetModel pet, HttpPostedFileBase file, string email) //string idCustomer
        {
            if (!ModelState.IsValid)
            {
                string idCustomer = (string)Session["idCustomer"];
                loadCustomer(idCustomer);
                loadBreedPet();
                loadSpeciesPet();
                pet.idCustomer = Request["customer"];
                pet.idBreedPet = Request["breedPet"];
                pet.idSpeciesPet = Request["speciesPet"];

                Session["idBreedAnimal"] = pet.idBreedPet;
                Session["idSpeciesAnimal"] = pet.idSpeciesPet;
                pet.idCustomer = idCustomer;
                string arquivo = Path.GetFileName(file.FileName);
                string file2 = "/Images/" + Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("/Images"), arquivo);
                file.SaveAs(_path);
                pet.imagePet = file2;

                queryPet.insertPet(pet);
                string idAnimal = pet.idPet;
                ViewBag.msg = "Cadastro efetuado com sucesso";
                return RedirectToAction("ListPetCustomer", "Pet");
            }
            else
            {
                ViewBag.msg = "Erro ao realizar cadastro do animal";
                return View(pet);
            }
        }

        public PetModel GetNameCustomerById()
        {
            string idCustomer = (string)Session["idCustomer"];

            // Consulta SQL com INNER JOIN
            string query =
                @"SELECT tbCustomer.nameCustomer
                     FROM tbCustomer
                     JOIN tbPet ON tbPet.idCustomer = tbCustomer.idCustomer
                     WHERE tbCustomer.idCustomer = @IdCustomer";

            // Crie uma conexão com o banco de dados e execute a consulta
            using (
                MySqlConnection connection = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCustomer", idCustomer);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nameCustomer = reader["nameCustomer"].ToString();
                            ViewBag.nameCustomer = nameCustomer;

                            // Crie uma instância de Animal e defina o nome do cliente
                            PetModel pet = new PetModel();
                            pet.idCustomer = nameCustomer;

                            return pet;
                        }
                    }
                }
                // Caso o cliente não seja encontrado, retorne uma mensagem de erro ou faça algo apropriado
                ViewBag.customer = "Cliente não encontrado";
            }
            return null;
        }

        //
        public List<PlanPetViewModel> GetNamePlanById()
        {
            string idCustomer = (string)Session["idCustomer"];
            List<PetModel> listaAnimais = queryPet.getPetByIdCustomer(idCustomer);
            List<string> listaIdsAnimais = (from a in listaAnimais
                                            select a.idPet).ToList();
            List<string> listaIdsAnimaisSessao = new List<string>(); // Lista para armazenar os IDs na sessão

            foreach (string idAnimal in listaIdsAnimais)
            {
                listaIdsAnimaisSessao.Add(idAnimal); // Adiciona o ID à lista da sessão
            }

            Session["listaIdsAnimais"] = listaIdsAnimaisSessao;

            // Consulta SQL com INNER JOIN
            string query =
                @"SELECT tbPet.idAnimal, tbPlan.namePlan FROM tbPet INNER JOIN tbPlan ON tbPet.idPlan = tbPlan.idPlan WHERE tbPet.idAnimal = @idPet";

            // Crie uma conexão com o banco de dados e execute a consulta
            using (MySqlConnection connection = new MySqlConnection("Server =localhost;DataBase=teste2;User=root;pwd=12345678"))
            {
                connection.Open();
                List<PlanPetViewModel> resultPet = new List<PlanPetViewModel>(); // Lista para armazenar os resultados
                Dictionary<string, string> petPlanMap = new Dictionary<string, string>(); // Dicionário para mapear ID do animal ao valor da raça

                foreach (string idPet in (List<string>)Session["listaIdsAnimais"])
                {
                    // Crie uma nova instância de MySqlCommand para cada iteração do loop
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPet", idPet);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string namePlan = reader["namePlan"].ToString();

                                // Crie uma instância de Animal e defina o valor da propriedade idBreedAnimal
                                PlanPetViewModel pet = new PlanPetViewModel();
                                pet.idPet = reader["idAnimal"].ToString();
                                pet.idPlan = namePlan;

                                resultPet.Add(pet);

                                // Mapeie o ID do animal ao valor da raça
                                petPlanMap[pet.idPet] = namePlan;
                            }
                        }
                    }
                    ViewBag.PlanMap = petPlanMap;
                }


                return resultPet;
            }
        }

        public PlanPetViewModel GetNamePlanById2()
        {
            string idPlan = (string)Session["idPlan"];

            // Consulta SQL com INNER JOIN
            string query =
                @"SELECT tbPlan.namePlan
                      FROM PlanAnimal
                      JOIN tbPlan  ON PlanAnimal.idPlan = tbPlan.idPlan
                      WHERE tbPet.idAnimal = @IdPlan";

            // Crie uma conexão com o banco de dados e execute a consulta
            using (
                MySqlConnection connection = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPlan", idPlan);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string namePlan = reader["namePlan"].ToString();
                            ViewBag.namePlan = namePlan;

                            // Crie uma instância de Animal e defina o nome do cliente
                            PlanPetViewModel planPet = new PlanPetViewModel();
                            planPet.idPlan = namePlan;

                            return planPet;
                        }
                    }
                }
                // Caso o cliente não seja encontrado, retorne uma mensagem de erro ou faça algo apropriado
                ViewBag.namePlan = "plano não encontrado";
            }
            return null;
        }

        public List<PetModel> GetPetBreedNameById()
        {
            string idCustomer = (string)Session["idCustomer"];
            List<PetModel> listaAnimais = queryPet.getPetByIdCustomer(idCustomer);
            List<string> listaIdsAnimais = (from a in listaAnimais
                                            select a.idPet).ToList();
            List<string> listaIdsAnimaisSessao = new List<string>(); // Lista para armazenar os IDs na sessão

            foreach (string idAnimal in listaIdsAnimais)
            {
                listaIdsAnimaisSessao.Add(idAnimal); // Adiciona o ID à lista da sessão
            }

            Session["listaIdsAnimais"] = listaIdsAnimaisSessao;

            // Consulta SQL com INNER JOIN
            string query =
                @"SELECT tbPet.idAnimal, tbBreedAnimal.nameBreed
          FROM tbPet
          INNER JOIN tbBreedAnimal ON tbPet.idBreedAnimal = tbBreedAnimal.idBreedAnimal
          WHERE tbPet.idAnimal = @IdPet";

            // Crie uma conexão com o banco de dados e execute a consulta
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;DataBase=teste2;User=root;pwd=12345678"))
            {
                connection.Open();
                List<PetModel> resultPet = new List<PetModel>(); // Lista para armazenar os resultados
                Dictionary<string, string> petBreedMap = new Dictionary<string, string>(); // Dicionário para mapear ID do animal ao valor da raça

                foreach (string idPet in (List<string>)Session["listaIdsAnimais"])
                {
                    // Crie uma nova instância de MySqlCommand para cada iteração do loop
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPet", idPet);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string nameBreed = reader["nameBreed"].ToString();

                                // Crie uma instância de Animal e defina o valor da propriedade idBreedAnimal
                                PetModel pet = new PetModel();
                                pet.idPet = reader["idAnimal"].ToString();
                                pet.idBreedPet = nameBreed;

                                resultPet.Add(pet);

                                // Mapeie o ID do animal ao valor da raça
                                petBreedMap[pet.idPet] = nameBreed;
                            }
                        }
                    }
                    ViewBag.AnimalBreedMap = petBreedMap;
                }

                return resultPet;
            }
        }


        public List<PetModel> GetPetSpeciesNameById()
        {
            string idCustomer = (string)Session["idCustomer"];
            List<PetModel> listaAnimais = queryPet.getPetByIdCustomer(idCustomer);
            List<string> listaIdsAnimais = (from a in listaAnimais
                                            select a.idPet).ToList();
            List<string> listaIdsAnimaisSessao = new List<string>(); // Lista para armazenar os IDs na sessão

            foreach (string idAnimal in listaIdsAnimais)
            {
                listaIdsAnimaisSessao.Add(idAnimal); // Adiciona o ID à lista da sessão
            }

            Session["listaIdsAnimais"] = listaIdsAnimaisSessao;

            // Consulta SQL com INNER JOIN
            string query =
                @"SELECT  tbPet.idAnimal, tbSpeciesAnimal.nameSpeciesAnimal
      FROM tbPet
      INNER JOIN tbSpeciesAnimal ON tbPet.idSpeciesAnimal = tbSpeciesAnimal.idSpeciesAnimal
      WHERE tbPet.idAnimal = @IdPet";

            // Crie uma conexão com o banco de dados e execute a consulta
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;DataBase=teste2;User=root;pwd=12345678"))
            {
                connection.Open();
                List<PetModel> resultPet = new List<PetModel>(); // Lista para armazenar os resultados
                Dictionary<string, string> petSpeciesMap = new Dictionary<string, string>(); // Dicionário para mapear ID do animal ao valor da raça

                foreach (string idPet in (List<string>)Session["listaIdsAnimais"])
                {
                    // Crie uma nova instância de MySqlCommand para cada iteração do loop
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPet", idPet);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string nameSpeciesAnimal = reader["nameSpeciesAnimal"].ToString();

                                // Crie uma instância de Animal e defina o valor da propriedade idBreedAnimal
                                PetModel pet = new PetModel();
                                pet.idPet = reader["idAnimal"].ToString();
                                pet.idSpeciesPet = nameSpeciesAnimal;

                                resultPet.Add(pet);

                                // Mapeie o ID do animal ao valor da raça
                                petSpeciesMap[pet.idPet] = nameSpeciesAnimal;
                            }
                        }
                    }
                    ViewBag.AnimalSpeciesMap = petSpeciesMap;
                }


                return resultPet;
            }
        }


        public List<PetModel> GetPetBreedNameById2()
        {
            string idCustomer = (string)Session["idCustomer"];
            List<PetModel> listaAnimais = queryPet.getPetByIdCustomer(idCustomer);

            string query =
                @"SELECT p.idAnimal, b.nameBreed
          FROM tbPet p
          INNER JOIN tbBreedAnimal b ON p.idBreedAnimal = b.idBreedAnimal
          WHERE p.idCustomer = @IdCustomer";

            using (MySqlConnection connection = new MySqlConnection("Server=localhost;DataBase=teste2;User=root;pwd=12345678"))
            {
                connection.Open();
                List<PetModel> resultPet = new List<PetModel>();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCustomer", idCustomer);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string idAnimal = reader["idAnimal"].ToString();
                            string nameBreed = reader["nameBreed"].ToString();

                            PetModel pet = new PetModel();
                            pet.idPet = idAnimal;
                            pet.idBreedPet = nameBreed;

                            resultPet.Add(pet);
                        }
                    }
                }

                // Armazenar os valores de nameBreed em uma ViewBag
                List<string> nameBreeds = resultPet.Select(a => a.idBreedPet).ToList();
                ViewBag.NameBreeds = nameBreeds;

                return listaAnimais;
            }
        }


        DatabaseConnection con = new DatabaseConnection();
        public List<PetModel> getAnimalByIdCustomer2(string id)
        {
            List<PetModel> listaAnimais = new List<PetModel>();
            MySqlCommand cmd = new MySqlCommand(
                "SELECT tbPet.*, tbSpeciesAnimal.nameSpeciesAnimal, tbBreedAnimal.nameBreed " +
        "FROM tbPet " +
        "INNER JOIN tbSpeciesAnimal ON tbPet.idSpeciesAnimal = tbSpeciesAnimal.idSpeciesAnimal " +
        "INNER JOIN tbBreedAnimal ON tbPet.idBreedAnimal = tbBreedAnimal.idBreedAnimal " +
        "WHERE tbPet.idCustomer = @id",
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
                listaAnimais.Add(
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
                ViewBag.NameSpeciesAnimal = Convert.ToString(dr["idSpeciesAnimal"]);

                // Adicionar idBreedAnimal em uma ViewBag
                ViewBag.NameBreed = Convert.ToString(dr["idBreedAnimal"]);
            }

            return listaAnimais;
        }

        public ActionResult ListPetCustomer(string idAnimal)
        {
            // Verifica se o usuário está autenticado
            if (User.Identity.IsAuthenticated)
            {
                //Obtém o ID do usuário logado
                //string userId = GetCurrentUserId();
                string idCustomer = (string)Session["idCustomer"];
                string idPlan = (string)Session["idPlan"];

                string idBreedAnimal = (string)Session["idBreedAnimal"];
                string idSpeciesAnimal = (string)Session["idSpeciesAnimal"];

                // Obtém os animais cadastrados com a chave estrangeira igual ao ID do usuário
                //List<Animal> animais = acAnimal.getAnimalByIdCustomer(idCustomer);

                List<PetModel> listaAnimais = queryPet.getPetByIdCustomer(idCustomer);

                GetPetBreedNameById();
                GetPetSpeciesNameById();

                ViewBag.customer = idCustomer;
                ViewBag.breed = idBreedAnimal;
                ViewBag.species = idSpeciesAnimal;

                PetModel pet = GetNameCustomerById();
                GetNamePlanById();


                // Retorna a lista de animais para a view
                return View(listaAnimais);
            }

            // Redireciona para a página de login se o usuário não estiver autenticado
            return RedirectToAction("Login", "Login");
        }

        public ActionResult DeleteAnimal(int id)
        {
            queryPet.deletePet(id);
            return RedirectToAction("ListAnimalCustomer");
        }

        public ActionResult UpdateAnimal(string id)
        {
            return View(queryPet.getPet().Find(model => model.idPet == id));
        }

        [HttpPost]
        public ActionResult UpdateAnimal(int id, PetModel animal, HttpPostedFileBase file)
        {
            string arquivo = Path.GetFileName(file.FileName);
            string file2 = "/Files/" + Path.GetFileName(file.FileName);
            string _path = Path.Combine(Server.MapPath("/Files"), arquivo);
            loadBreedPet();
            loadSpeciesPet();
            animal.idBreedPet = Request["speciesAnimal"];
            animal.idSpeciesPet = Request["breedAnimal"];
            file.SaveAs(_path);
            animal.imagePet = file2;
            string idCustomer = (string)Session["idCustomer"];
            loadCustomer(idCustomer);
            animal.idCustomer = Request["customer"];
            animal.idCustomer = idCustomer;

            animal.idPet = id.ToString();
            queryPet.updatePet(animal);
            return RedirectToAction("ListAnimalCustomer");
            
        }

        [AuthenticationAuthorizeAttribute(UserRole.Admin)]
        public ActionResult ListAnimal()
        {
            return View(queryPet.getPet());
        }
    }
}