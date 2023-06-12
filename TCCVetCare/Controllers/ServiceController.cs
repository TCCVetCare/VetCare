using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static TCCVetCare.Models.AuthenticationModel;
using System.Web.Services.Description;
using TCCVetCare.Models;
using TCCVetCare.Repositories;

namespace TCCVetCare.Controllers
{
    public class ServiceController : Controller
    {
        ServiceRepository query = new ServiceRepository();

        public ActionResult Admin()
        {
            return View();
        }

        [AuthenticationAuthorizeAttribute(UserRole.Admin)]
        public ActionResult CadService()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CadService(ServiceModel service)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.msg = "Erro ao realizar cadastro do serviço";
                return View(service);
            }
            else
            {
                query.insertService(service);
                ViewBag.msg = "Cadastro efetuado com sucesso";
                return RedirectToAction("ListService", "Service");
            }
        }

        [AuthenticationAuthorizeAttribute(UserRole.Admin)]
        public ActionResult ListService()
        {
            return View(query.getService());
        }

        [AuthenticationAuthorizeAttribute(UserRole.Admin)]
        public ActionResult DeleteService(int id)
        {
            query.deleteService(id);
            return RedirectToAction("ListService");
        }

        [AuthenticationAuthorizeAttribute(UserRole.Admin)]
        public ActionResult UpdateService(string id)
        {
            return View(query.getService().Find(model => model.idService == id));
        }

        [HttpPost]
        public ActionResult UpdateService(int id, ServiceModel service)
        {
            service.idService = id.ToString();
            query.updateService(service);
            return RedirectToAction("ListService");

        }
    }
}