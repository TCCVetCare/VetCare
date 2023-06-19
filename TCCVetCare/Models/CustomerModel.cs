using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TCCVetCare.Models
{
    public class CustomerModel
    {

        public string idCustomer { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Informe seu nome")]
        [MaxLength(80, ErrorMessage = "O nome deve conter no maximo 255 caracteres")]
        public string nameCustomer { get; set; }

        [Display(Name = "Número do CPF")]
        [Required(ErrorMessage = "Informe seu CPF")]
        [MaxLength(11, ErrorMessage = "O CPF deve conter no maximo 11 caracteres")]
        [MinLength(11, ErrorMessage = "O CPF deve conter no minimo 11 caracters")]

        public string cpfCustomer { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Informe seu email")]
        [MaxLength(50, ErrorMessage = "O email deve conter no maximo 15 caracteres")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\-][a-zA-Z ])?[a-zA-Z]*)*\s+<(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})>$|^(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})$", ErrorMessage = "Digite um Email válido")]
        public string emailCustomer { get; set; }


        [MaxLength(9, ErrorMessage = "Senha deve conter no maximo 8 caracteres")]
        [Required(ErrorMessage = "Digite a senha")]
        [Display(Name = "Password")]
        public string passwordCustomer { get; set; }


        [Display(Name = "Número do Telefone")]
        [Required(ErrorMessage = "Digite o número de telefone")]
        [MaxLength(11, ErrorMessage = "O telefone deve conter 11 caracteres")]
        [MinLength(11, ErrorMessage = "O telefone deve conter 11 caracteres")]
        [RegularExpression(@"^[0-9]+${11,11}", ErrorMessage = "Somente números")]
        public string phoneCustomer { get; set; }


        public string idAddress { get; set; }

        [Display(Name = "CEP")]
        [Required(ErrorMessage = "Digite o CEP")]
        public string zipCode { get; set; }

        [Display(Name = "Logradouro")]
        [Required(ErrorMessage = "Digite o logradouro")]
        public string streetName { get; set; }


        [Display(Name = "Número do logradouro")]
        [Required(ErrorMessage = "Digite o número do logradouro")]
        public string streetNumber { get; set; }

        [Display(Name = "Complemento")]
        public string addressComplement { get; set; }


        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "Digite o bairro")]
        public string neighborhood { get; set; }

        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "Digite a cidade")]
        public string city { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Digite o estado")]
        public string state { get; set; }

    }
}