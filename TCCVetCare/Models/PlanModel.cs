using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TCCVetCare.Models
{
    public class PlanModel
    {
        public string idPlan { get; set; }

        [Display(Name = "Nome do plano")]
        [Required(ErrorMessage = "Informe o nome do plano")]
        [MaxLength(80, ErrorMessage = "O nome do plano deve conter no maximo 255 caracteres")]
        public string namePlan { get; set; }

        [Display(Name = "Descrição do plano")]
        [Required(ErrorMessage = "Informe a descrição do plano")]
        [MaxLength(80, ErrorMessage = "a descrição do plano deve conter no maximo 255 caracteres")]
        public string descriptionPlan { get; set; }

        [Display(Name = "Preço do plano")]
        [Required(ErrorMessage = "Informe o preço do plano")]
        [RegularExpression(@"^[0-9]+${11,11}", ErrorMessage = "Somente números")]
        public double pricePlan { get; set; }

        [Display(Name = "Imagem do plano")]
        public string imagePlan { get; set; }

    }
}