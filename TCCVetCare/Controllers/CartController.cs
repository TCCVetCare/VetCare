using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TCCVetCare.Models;
using TCCVetCare.Repositories;

namespace TCCVetCare.Controllers
{
    public class CartController : Controller
    {
        CartRepository queryCart = new CartRepository();
        ProductRepository queryProduct = new ProductRepository();
        ItemsCartRepository queryItemsCart = new ItemsCartRepository();

        DatabaseConnection con = new DatabaseConnection();
        MySqlConnection cn = new MySqlConnection("Server=localhost;DataBase=teste2;User=root;pwd=12345678");
        public static string idCart;

        public ActionResult addItemsCart(int id, double price)
        {
            CartModel cart = Session["Carrinho"] != null ? (CartModel)Session["Carrinho"] : new CartModel();
            var productItem = queryProduct.GetConsProd(id);
            idCart = id.ToString();

            ProductModel prod = new ProductModel();

            if (productItem != null)
            {
                var itemCart = new ItemsCartModel();
                itemCart.idItemCart = Guid.NewGuid();
                itemCart.idProduct = id.ToString();
                itemCart.product = productItem[0].nameProduct;
                itemCart.quantity = 1;
                itemCart.unitPrice = price;

                List<ItemsCartModel> x = cart.ItemsCart.FindAll(l => l.product == itemCart.product);

                if (x.Count != 0)
                {
                    cart.ItemsCart.FirstOrDefault(p => p.product == productItem[0].nameProduct).quantity += 1;
                    itemCart.valuePartial = itemCart.quantity * itemCart.unitPrice;
                    cart.valueTotal += itemCart.valuePartial;
                    cart.ItemsCart.FirstOrDefault(p => p.product == productItem[0].nameProduct).valuePartial = cart.ItemsCart.FirstOrDefault(p => p.product == productItem[0].nameProduct).quantity * itemCart.unitPrice;

                }

                else
                {
                    itemCart.valuePartial = itemCart.quantity * itemCart.unitPrice;
                    cart.valueTotal += itemCart.valuePartial;
                    cart.ItemsCart.Add(itemCart);
                }

                /*carrinho.ValorTotal = carrinho.ItensPedido.Select(i => i.Produto).Sum(d => d.Valor);*/

                Session["Carrinho"] = cart;
            }

            return RedirectToAction("Carrinho");
        }

        public ActionResult Cart()
        {
            CartModel cart = Session["Carrinho"] != null ? (CartModel)Session["Carrinho"] : new CartModel();

            return View(cart);
        }

        public ActionResult delItemsCart(Guid id)
        {
            var cart = Session["Carrinho"] != null ? (CartModel)Session["Carrinho"] : new CartModel();
            var delItem = cart.ItemsCart.FirstOrDefault(i => i.idItemCart == id);

            cart.valueTotal -= delItem.valuePartial;

            cart.ItemsCart.Remove(delItem);

            Session["Carrinho"] = cart;
            return RedirectToAction("Carrinho");
        }

        public ActionResult saveItemsCart(CartModel x)
        {

            string idCustomer = (string)Session["idCustomer"];
            var cart = Session["Carrinho"] != null ? (CartModel)Session["Carrinho"] : new CartModel();

            CartModel md = new CartModel();
            ItemsCartModel mdV = new ItemsCartModel();

            md.dateSale = DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy");
            md.timeSale = DateTime.Now.ToLocalTime().ToString("HH:mm");
            md.idCustomer = Session["idCustomer"].ToString();
            md.valueTotal = cart.valueTotal;

            queryCart.insertCart(md);

            queryCart.searchIdSale(x);

            for (int i = 0; i < cart.ItemsCart.Count; i++)
            {

                mdV.idCart = x.idSale;
                mdV.idProduct = cart.ItemsCart[i].idCart;
                mdV.quantity = cart.ItemsCart[i].quantity;
                mdV.valuePartial = cart.ItemsCart[i].valuePartial;
                queryItemsCart.insertItem(mdV);
            }

            cart.valueTotal = 0;
            cart.ItemsCart.Clear();

            return RedirectToAction("confCarrinho");
        }

    }
}