using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static TCCVetCare.Models.AuthenticationModel;
using TCCVetCare.Models;
using TCCVetCare.Repositories;

namespace TCCVetCare.Controllers
{
    public class ScheduleController : Controller
    {
        ScheduleRepository query = new ScheduleRepository();

        public void loadCustomer(ScheduleModel id)
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

        public string GetNameCustomerById(ScheduleModel id)
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

        public void loadPet(string idCustomer)
        {
            List<SelectListItem> pet = new List<SelectListItem>();

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM tbPet WHERE idCustomer = @idCustomer",
                    con
                );
                cmd.Parameters.AddWithValue("@idCustomer", idCustomer);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    pet.Add(
                        new SelectListItem { Text = rdr[1].ToString(), Value = rdr[0].ToString() }
                    );
                }
                con.Close();
            }

            ViewBag.pet = new SelectList(pet, "Value", "Text");
        }

        public void loadService()
        {
            List<SelectListItem> service = new List<SelectListItem>();

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbService", con);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    service.Add(
                        new SelectListItem
                        {
                            Text = rdr[1].ToString(), //nome
                            Value = rdr[0].ToString() //id do autor
                        }
                    );
                }
                con.Close(); //fechando conexÃ£o
            }

            ViewBag.service = new SelectList(service, "Value", "Text");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegisterSchedule()
        {
            string idCustomer = (string)Session["idCustomer"];
            loadPet(idCustomer);
            loadService();
            return View();
        }

        [HttpPost]
        public ActionResult RegisterSchedule(ScheduleModel schedule)
        {
            string idCustomer = (string)Session["idCustomer"];
            schedule.idCustomer = idCustomer;
            loadPet(idCustomer);
            loadService();

            schedule.idService = Request["service"];
            schedule.idPet = Request["pet"];

            ScheduleModel newSchedule = new ScheduleModel()
            {
                idCustomer = idCustomer,
                idPet = schedule.idPet,
                idService = schedule.idService,
                dateSchedule = schedule.dateSchedule,
                timeSchedule = schedule.timeSchedule,
                observations = schedule.observations
            };

            if (!ModelState.IsValid)
            {
                query.insertSchedule(newSchedule);
                return RedirectToAction("ListScheduleByCustomerId", "Schedule");
            }

            // Se ocorrer um erro de validação, você pode retornar a View atual com o ModelState inválido
            return View(schedule);
        }

        public ActionResult ListSchedule()
        {
            return View(query.GetSchedule());
        }

        public ActionResult ListScheduleByCustomerId()
        {
            // Verifica se o usuário está autenticado
            if (User.Identity.IsAuthenticated)
            {
                //Obtém o ID do usuário logado
                //string userId = GetCurrentUserId();
                string idCustomer = (string)Session["idCustomer"];

                // Obtém os animais cadastrados com a chave estrangeira igual ao ID do usuário
                List<ScheduleModel> scheduleList = query.getScheduleByIdCustomer(idCustomer);

                ViewBag.servico = idCustomer;

                ScheduleModel schedule = GetNamesScheduleById();

                // Retorna a lista de animais para a view
                return View(scheduleList);
            }

            // Redireciona para a página de login se o usuário não estiver autenticado
            return RedirectToAction("Login", "Authentication");
        }

        [AuthenticationAuthorizeAttribute(UserRole.Admin)]
        public ActionResult ListScheduleAdmin()
        {
            // Verifica se o usuário está autenticado
            if (User.Identity.IsAuthenticated)
            {
                //Obtém o ID do usuário logado
                //string userId = GetCurrentUserId();
                string idCustomer = (string)Session["idCustomer"];

                ViewBag.servico = idCustomer;

                ScheduleModel schedule = GetNamesScheduleById();

                // Retorna a lista de animais para a view
                return View(query.GetSchedule());
            }

            // Redireciona para a página de login se o usuário não estiver autenticado
            return RedirectToAction("Login", "Authentication");
        }

        public ScheduleModel GetNamesScheduleById()
        {
            string idCustomer = (string)Session["idCustomer"];

            // Consulta SQL com INNER JOIN
            string query =
                @"SELECT tbSchedule.idSchedule, tbCustomer.nameCustomer, tbPet.nameAnimal, tbService.nameService, 
                        tbSchedule.dateSchedule, tbSchedule.timeSchedule, tbSchedule.observations
                FROM tbSchedule
                INNER JOIN tbCustomer ON tbSchedule.idCustomer = tbCustomer.idCustomer
                INNER JOIN tbPet ON tbSchedule.idAnimal = tbPet.idAnimal
                INNER JOIN tbService ON tbSchedule.idService = tbService.idService";

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
                            string nameService = reader["nameService"].ToString();
                            string nameAnimal = reader["nameAnimal"].ToString();

                            ViewBag.nameCustomer = nameCustomer;
                            ViewBag.nameAnimal = nameAnimal;
                            ViewBag.nameService = nameService;

                            ScheduleModel schedule = new ScheduleModel();
                            schedule.idCustomer = nameCustomer;
                            schedule.idService = nameService;
                            schedule.idPet = nameAnimal;

                            return schedule;
                        }
                    }
                }
                // Caso o cliente não seja encontrado, retorne uma mensagem de erro ou faça algo apropriado
                ViewBag.customer = "Cliente não encontrado";
            }
            return null;
        }


        public List<ScheduleModel> GetScheduleById()
        {
            string idCustomer = (string)Session["idCustomer"];
            List<ScheduleModel> schedules = new List<ScheduleModel>();

            // Consulta SQL com INNER JOIN
            string query =
                @"SELECT tbSchedule.idSchedule, tbCustomer.nameCustomer, tbPet.nameAnimal, tbService.nameService, 
                tbSchedule.dateSchedule, tbSchedule.timeSchedule, tbSchedule.observations
        FROM tbSchedule
        INNER JOIN tbCustomer ON tbSchedule.idCustomer = tbCustomer.idCustomer
        INNER JOIN tbPet ON tbSchedule.idAnimal = tbPet.idAnimal
        INNER JOIN tbService ON tbSchedule.idService = tbService.idService";

            // Crie uma conexão com o banco de dados e execute a consulta
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;DataBase=teste2;User=root;pwd=12345678"))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCustomer", idCustomer);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ScheduleModel schedule = new ScheduleModel();
                            schedule.idSchedule = Convert.ToString(reader["idSchedule"]);
                            schedule.idCustomer = reader["nameCustomer"].ToString();
                            schedule.idService = reader["nameService"].ToString();
                            schedule.idPet = reader["nameAnimal"].ToString();
                            schedule.dateSchedule = Convert.ToString(reader["dateSchedule"]);
                            schedule.timeSchedule = reader["timeSchedule"].ToString();
                            schedule.observations = reader["observations"].ToString();

                            schedules.Add(schedule);
                        }
                    }
                }
            }

            return schedules;
        }


        public ActionResult DeleteSchedule(int id)
        {
            query.DeleteSchedule(id);
            return RedirectToAction("ListScheduleByCustomerId");
        }

        public ActionResult UpdateSchedule(string id)
        {
            string idCustomer = (string)Session["idCustomer"];
            loadPet(idCustomer);
            loadService();
            return View(query.GetSchedule().Find(model => model.idSchedule == id));

        }

        [HttpPost]
        public ActionResult UpdateSchedule(int id, ScheduleModel schedule)
        {
            string idCustomer = (string)Session["idCustomer"];
            loadPet(idCustomer);
            loadService();
            schedule.idCustomer = idCustomer;
            schedule.idService = Request["service"];
            schedule.idPet = Request["pet"];
            schedule.idSchedule = id.ToString();
            query.UpdateSchedule(schedule);
            return RedirectToAction("ListScheduleByCustomerId");

        }
    }
}