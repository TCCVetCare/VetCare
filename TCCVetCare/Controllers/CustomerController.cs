using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCCVetCare.Models;
using TCCVetCare.Repositories;

namespace TCCVetCare.Controllers
{
    public class CustomerController : Controller
    {
        CustomerRepository query = new CustomerRepository();

        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult CadCustomer()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CadCustomer(CustomerModel customer)
        {
            if (!ModelState.IsValid)
                return View(customer);
            string cpf = query.SelectCPFCustomer(customer.cpfCustomer);
            string email = query.SelectEmailCustomer(customer.emailCustomer);
            if (cpf == customer.cpfCustomer && email == customer.emailCustomer)
            {
                ViewBag.Email = "Email ja existente";
                ViewBag.CPF = "CPF já existente";
                return View(customer);
            }
            else if (cpf == customer.cpfCustomer)
            {
                ViewBag.CPF = "CPF já existente";
                return View(customer);
            }
            else if (email == customer.emailCustomer)
            {
                ViewBag.Email = "Email já existente";
                return View(customer);
            }

            CustomerModel newCustomer = new CustomerModel()
            {
                nameCustomer = customer.nameCustomer,
                cpfCustomer = customer.cpfCustomer,
                emailCustomer = customer.emailCustomer,
                passwordCustomer = customer.passwordCustomer,
                phoneCustomer = customer.phoneCustomer,
                zipCode = customer.zipCode,
                streetName = customer.streetName,
                streetNumber = customer.streetNumber,
                addressComplement = customer.addressComplement,
                neighborhood = customer.neighborhood,
                city = customer.city,
                state = customer.state

            };
            //acCustomer.insertCustomer(newCustomer);
            query.InsertCustomerWithAddress(newCustomer);
            query.InsertAddress(newCustomer);
            return RedirectToAction("Login", "Login");
        }

        //[CustomeAuthorize(UserRole.Admin)]
        public ActionResult ListCustomer()
        {
            return View(query.GetCustomer());
        }


        public ActionResult ListCustomerWithAddress()
        {

            List<CustomerModel> customers = query.GetCustomersWithAddress();
            return View(customers);
        }

        public ActionResult ListCustomerWithAddressUpdate()
        {
            string idCustomer = (string)Session["idCustomer"];
            List<CustomerModel> customers = query.GetCustomersWithAddress(idCustomer);


            return View(customers);
        }


        public ActionResult DeleteCustomer(int id)
        {
            query.DeleteCustomer(id);
            return RedirectToAction("ListCustomerWithAddressUpdate");
        }

        //public ActionResult UpdateCustomer(string id)
        //{
        // return View(acCustomer.GetCustomer().Find(model => model.idCustomer == id));
        //}


        public ActionResult UpdateCustomer(string id, string idAddress)
        {
            Session["addressId"] = idAddress;
            return View(query.GetCustomersWithAddress().Find(model => model.idCustomer == id));
        }


        [HttpPost]
        public ActionResult UpdateCustomer(int id, CustomerModel customer)
        {

            string idAddress = (string)Session["addressId"];
            customer.idCustomer = id.ToString();
            query.UpdateCustomer(customer);

            CustomerModel address = new CustomerModel();

            //address.zipCode = customer.zipCode;
            //address.streetName = customer.streetName;
            //address.streetNumber = customer.streetNumber;
            //address.city = customer.city;
            //address.state = customer.state;
            //address.addressComplement = customer.addressComplement;
            //address.neighborhood = customer.neighborhood;
            //address.idAddress = customer.idAddress;
            customer.idAddress = idAddress;
            query.UpdateAddress(customer);
            return RedirectToAction("ListCustomerWithAddressUpdate");

        }

        public ActionResult cartaoCredito()
        {
            return View();
        }

        public ActionResult boleto()
        {
            return View();
        }
    }
}