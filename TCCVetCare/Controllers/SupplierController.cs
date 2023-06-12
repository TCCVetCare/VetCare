using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCCVetCare.Models;
using TCCVetCare.Repositories;
using static TCCVetCare.Models.AuthenticationModel;

namespace TCCVetCare.Controllers
{
    public class SupplierController : Controller
    {
        SupplierRepository query = new SupplierRepository();

        public ActionResult Admin()
        {
            return View();
        }

        [AuthenticationAuthorizeAttribute(UserRole.Admin)]
        public ActionResult CadSupplier()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CadSupplier(SupplierModel supplier)
        {
            if (!ModelState.IsValid)
                if (!ModelState.IsValid)
                    return View(supplier);
            string cnpj = query.SelectCnpjSuppler(supplier.cnpjSupplier);
            if (cnpj == supplier.cnpjSupplier)
            {
                ViewBag.msg = "CNPJ já existente";
                return View(supplier);
            }
            else
            {
                query.insertSupplier(supplier);
                ViewBag.msg = "Cadastro efetuado com sucesso";
                return RedirectToAction("ListSupplier", "Supplier");
            }
        }

        [AuthenticationAuthorizeAttribute(UserRole.Admin)]
        public ActionResult ListSupplier()
        {
            return View(query.getSupplier());
        }

        [AuthenticationAuthorizeAttribute(UserRole.Admin)]
        public ActionResult DeleteSupplier(int id)
        {
            query.deleteSupplier(id);
            return RedirectToAction("ListSupplier");
        }

        [AuthenticationAuthorizeAttribute(UserRole.Admin)]
        public ActionResult UpdateSupplier(string id)
        {
            return View(query.getSupplier().Find(model => model.idSupplier == id));
        }

        [HttpPost]
        public ActionResult UpdateSupplier(int id, SupplierModel supplier)
        {
            supplier.idSupplier = id.ToString();
            query.updateSupplier(supplier);
            return RedirectToAction("ListSupplier");
        }
    }
}