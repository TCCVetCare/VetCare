using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCCVetCare.Models;
using TCCVetCare.Repositories;
using static TCCVetCare.Models.AuthenticationModel;

namespace TCCVetCare.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        ProductRepository query = new ProductRepository();

        public ActionResult Admin()
        {
            return View();
        }


        public void loadSupplier()
        {
            List<SelectListItem> supplier = new List<SelectListItem>();

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbSupplier", con);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    supplier.Add(
                        new SelectListItem
                        {
                            Text = rdr[1].ToString(), //nome
                            Value = rdr[0].ToString() //id do autor
                        }
                    );
                }
                con.Close(); //fechando conexÃ£o
            }

            ViewBag.supplier = new SelectList(supplier, "Value", "Text");
        }

        [AuthenticationAuthorizeAttribute(UserRole.Admin)]
        public ActionResult RegisterProduct()
        {
            loadSupplier();
            return View();
        }

        [HttpPost]
        public ActionResult RegisterProduct(ProductModel product, HttpPostedFileBase file)
        {
            loadSupplier();
            string arquivo = Path.GetFileName(file.FileName);
            string file2 = "/Files/" + Path.GetFileName(file.FileName);
            string _path = Path.Combine(Server.MapPath("/Files"), arquivo);
            file.SaveAs(_path);
            product.imageProduct = file2;
            product.idSupplier = Request["supplier"];

            if (ModelState.IsValid)
            {
                ViewBag.msg = "Erro ao realizar cadastro do produto";
                return View(product);
            }
            else
            {
                query.insertProduct(product);
                ViewBag.msg = "Cadastro efetuado com sucesso";
                return RedirectToAction("ListProductAdmin", "Product");
            }
        }

        public ActionResult ListProduct()
        {
            return View(query.getProduct());
        }

        public ActionResult ListProductAdmin()
        {
            return View(query.getProduct());
        }

    
        public ActionResult DeleteProduct(int id)
        {
            query.deleteProduct(id);
            return RedirectToAction("ListProductAdmin");
        }

        public ActionResult UpdateProduct(string id)
        {
            loadSupplier();
            return View(query.getProduct().Find(model => model.idProduct == id));
        }

        [HttpPost]
        public ActionResult UpdateProduct(int id, ProductModel product, HttpPostedFileBase file)
        {
            string arquivo = Path.GetFileName(file.FileName);
            string file2 = "/Files/" + Path.GetFileName(file.FileName);
            string _path = Path.Combine(Server.MapPath("/Files"), arquivo);
            file.SaveAs(_path);
            product.imageProduct = file2;
            product.idSupplier = Request["supplier"];
            loadSupplier();
            product.idProduct = id.ToString();
            query.updateProduct(product);

            return RedirectToAction("ListProductAdmin");
        }
    }
}
