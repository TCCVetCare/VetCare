using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TCCVetCare.Models
{
    public class SupplierModel
    {
        public string idSupplier { get; set; }

        [Display(Name = "Nome do fornecedor")]
        [Required(ErrorMessage = "Informe o nome do fornecedor")]
        [MaxLength(80, ErrorMessage = "O nome do fornecedor deve conter no maximo 255 caracteres")]
        public string nameSupplier { get; set; }

        [Display(Name = "CNPJ do fornecedor")]
        [Required(ErrorMessage = "Informe o CNPJ do fornecedor")]
        [MinLength(15, ErrorMessage = "O CNPJ do fornecedor deve conter no máximo 15 caracteres")]
        public string cnpjSupplier { get; set; }
    }
}