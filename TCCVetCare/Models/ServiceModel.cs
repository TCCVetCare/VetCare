using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TCCVetCare.Models
{
    public class ServiceModel
    {
        public string idService { get; set; }

        [Display(Name = "Nome do serviço")]
        [Required(ErrorMessage = "Informe o nome do serviço")]
        [MaxLength(80, ErrorMessage = "O nome do serviço deve conter no maximo 255 caracteres")]
        public string nameService { get; set; }

        [Display(Name = "Descrição do serviço")]
        [Required(ErrorMessage = "Informe a descrição do serviço")]
        [MaxLength(80, ErrorMessage = "O nome do serviço deve conter no maximo 255 caracteres")]
        public string descriptionService { get; set; }


        [Display(Name = "Preço unitário do serviço")]
        [Required(ErrorMessage = "Informe o preço unitário do serviço")]
        [RegularExpression(@"^[0-9]+${11,11}", ErrorMessage = "Somente numeros")]
        public double priceService { get; set; }

    }
}