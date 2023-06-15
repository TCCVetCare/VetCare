using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TCCVetCare.Repositories;
using TCCVetCare.ViewModels;
using static TCCVetCare.Models.AuthenticationModel;

namespace TCCVetCare.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly AuthenticationRepository query;

        public AuthenticationController()
        {
            string connectionString = "Server=localhost;DataBase=teste2;User=root;pwd=12345678";
            query = new AuthenticationRepository(connectionString);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AuthenticationViewModel model)
        {
            if (ModelState.IsValid)
            {

                //verificar as credenciais do usuÃ¡rio no banco
                var user = query.GetUserByEmailAndPassword(model);

                if (user != null)
                {
                    //autenticar o usuÃ¡rio e redirecionar para a tela inicial
                    FormsAuthentication.SetAuthCookie(user.role.ToString(), false);
                    if (user.role == UserRole.Admin)
                    {
                        return RedirectToAction("ListAnimal", "Animal");
                    }
                    else if (user.role == UserRole.Customer)
                    {
                        Session["idCustomer"] = user.id;

                        return RedirectToAction("CadAnimal", "Animal", new { idCustomer = user.id });

                    }

                }

            }
            ModelState.AddModelError("", "Credencias invÃ¡lidas");
            return View(model);
        }

        public ActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmLogout()
        {
            if (Request.Form["confirm"] == "true")
            {
                // Executar o logout
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Authentication");
            }
            else
            {
                // Cancelar o logout
                return RedirectToAction("Index", "Home");
            }
        }

    }
}