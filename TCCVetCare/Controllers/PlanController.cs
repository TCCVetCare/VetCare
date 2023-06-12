using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Web;
using System.Web.Mvc;
using TCCVetCare.Models;
using TCCVetCare.Repositories;
using TCCVetCare.ViewModels;
using static TCCVetCare.Models.AuthenticationModel;

namespace TCCVetCare.Controllers
{
    public class PlanController : Controller
    {
        PlanRepository query = new PlanRepository();

        public void loadFormOfPayment()
        {
            List<SelectListItem> payments = new List<SelectListItem>();

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbPayment", con);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    payments.Add(
                        new SelectListItem
                        {
                            Text = rdr[1].ToString(), //nome
                            Value = rdr[0].ToString() //id do autor
                        }
                    );
                }
                con.Close(); //fechando conexÃ£o
            }

            ViewBag.payments = new SelectList(payments, "Value", "Text");
        }

        public void loadAnimal(string idCustomer)
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

        public ActionResult Admin()
        {
            return View();
        }

        [AuthenticationAuthorizeAttribute(UserRole.Admin)]
        public ActionResult CadPlan()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CadPlan(PlanModel plan, HttpPostedFileBase file)
        {
            string arquivo = Path.GetFileName(file.FileName);
            string file2 = "/Files/" + Path.GetFileName(file.FileName);
            string _path = Path.Combine(Server.MapPath("/Files"), arquivo);
            file.SaveAs(_path);
            plan.imagePlan = file2;
            if (!ModelState.IsValid)
            {
                ViewBag.msg = "Erro ao realizar cadastro do plano";
                return View(plan);
            }
            else
            {
                query.insertPlan(plan);
                ViewBag.msg = "Cadastro efetuado com sucesso";
                return RedirectToAction("ListPlan", "Plan");
            }
        }

        public ActionResult ListPlan()
        {
            return View(query.getPlan());
        }

        public ActionResult ListPlanCustomer()
        {
            return View(query.getPlan());
        }


        [AuthenticationAuthorizeAttribute(UserRole.Admin)]
        public ActionResult DeletePlan(int id)
        {
            query.deletePlan(id);
            return RedirectToAction("ListPlan");
        }

        [AuthenticationAuthorizeAttribute(UserRole.Admin)]
        public ActionResult UpdatePlan(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            return View(query.getPlan().Find(model => model.idPlan == id));
        }

        [HttpPost]
        public ActionResult UpdatePlan(int id, PlanModel plan, HttpPostedFileBase file)
        {
            string arquivo = Path.GetFileName(file.FileName);
            string file2 = "/Files/" + Path.GetFileName(file.FileName);
            string _path = Path.Combine(Server.MapPath("/Files"), arquivo);
            file.SaveAs(_path);
            plan.imagePlan = file2;
            if (ModelState.IsValid)
            {
                plan.idPlan = id.ToString();
                query.updatePlan(plan);
                return RedirectToAction("ListPlan", "Plan");
            }
            return View(plan);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CadPlanAnimal(string id)
        {
            string idCustomer = (string)Session["idCustomer"];
            loadAnimal(idCustomer);
            Session["idPlan"] = id;
            loadFormOfPayment();
            return View();
        }

        [HttpPost]
        public ActionResult CadPlanAnimal(PlanPetViewModel planAnimal)
        {
            string idPlan = (string)Session["idPlan"];
            planAnimal.idPlan = idPlan;
            planAnimal.idPet = Request["animal"];

            string idAnimal = planAnimal.idPet;

            query.UpdateIdPlan(idPlan, idAnimal);
            planAnimal.idFormOfPayment = Request["payments"];
            string idCustomer = (string)Session["idCustomer"];
            loadAnimal(idCustomer);
            loadFormOfPayment();


            PlanPetViewModel plan = new PlanPetViewModel()
            {
                idPlan = idPlan,
                idPet = planAnimal.idPet,
                idFormOfPayment = planAnimal.idFormOfPayment

            };

            if (ModelState.IsValid)
            {
                query.insertPlanPet(planAnimal);
                ViewBag.msg = "Compra efetuada com sucesso";
            }
            else
            {
                ViewBag.msg = "Erro ao finalizar a compra";
                return View(planAnimal);
            }
            return View();

        }
    }
}