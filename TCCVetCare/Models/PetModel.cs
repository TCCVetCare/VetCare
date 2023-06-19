using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TCCVetCare.Models
{
    public class PetModel
    {
        public string idPet { get; set; }

        public string idCustomer { get; set; }

        public string idPlan { get; set; }

        [Display(Name = "Nome do pet")]
        [Required(ErrorMessage = "Informe o nome do pet")]
        [MaxLength(80, ErrorMessage = "O nome deve conter no maximo 255 caracteres")]
        public string namePet { get; set; }


        [Display(Name = "Raça do pet")]
        [Required(ErrorMessage = "Escolha raça do pet")]
        public string idBreedPet { get; set; }

        [Display(Name = "Espécie do Pet")]
        [Required(ErrorMessage = "Escolha a espécie do pet")]
        public string idSpeciesPet { get; set; }

        [Display(Name = "Idade do pet")]
        [Required(ErrorMessage = "Digite a idade do pet")]
        [RegularExpression(@"^[0-9]+${11,11}", ErrorMessage = "Somente números")]
        public string agePet { get; set; }

        [Display(Name = "Genêro do pet")]
        [Required(ErrorMessage = "selecione o genêro do pet")]
        [MaxLength(11, ErrorMessage = "O genêro do pet deve possuir no máximo 1 caracther")]
        public string idGenderPet { get; set; }

        [Display(Name = "Imagem do pet")]
        [Required(ErrorMessage = "Escolha uma imagem para o pet")]
        public string imagePet { get; set; }

    }
}