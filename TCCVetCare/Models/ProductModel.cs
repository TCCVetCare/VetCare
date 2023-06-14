using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TCCVetCare.Models
{
    public class ProductModel
    {
        public string idProduct { get; set; }
        public string idSupplier { get; set; }

        [Display(Name = "Nome do produto")]
        [Required(ErrorMessage = "Informe o nome do produto")]
        [MaxLength(80, ErrorMessage = "O nome do produto deve conter no maximo 255 caracteres")]
        public string nameProduct { get; set; }

        [Display(Name = "Preço unitário do produto")]
        [Required(ErrorMessage = "Informe o preço unitário do produto")]
        [RegularExpression(@"^[0-9]+${11,11}", ErrorMessage = "Somente numeros")]
        public double unitPrice { get; set; }

        [Display(Name = "Descrição do produto")]
        [Required(ErrorMessage = "Informe a descrição do produto")]
        public string descriptionProduct { get; set; }

        [Display(Name = "Imagem do produto")]
        [Required(ErrorMessage = "Escolha uma imagem para o produto")]
        public string imageProduct { get; set; }
    }
}