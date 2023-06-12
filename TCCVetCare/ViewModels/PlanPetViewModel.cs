using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TCCVetCare.ViewModels
{
    public class PlanPetViewModel
    {
        public string idPlanPet { get; set; }

        public string idPlan { get; set; }
        [Display(Name = "Selecione o Pet")]
        //[Required(ErrorMessage = "O campo pet é obrigatório.")]
        public string idPet { get; set; }

        [Display(Name = "Selecione a forma de pagamento")]
        //[Required(ErrorMessage = "O campo forma de pagamento é obrigatório")]

        public string idFormOfPayment { get; set; }

        public string CreditCardNumber { get; set; }


        public string CardNumber { get; set; }

        public string LimitDate { get; set; }


        public string nameBoleto { get; set; }

        public string cpfBoleto { get; set; }


        public string KeyPix { get; set; }
    }
}