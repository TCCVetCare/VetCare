using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TCCVetCare.Models
{
    public class ScheduleModel
    {
        public string idSchedule { get; set; }

        [Display(Name = "Nome do Cliente")]
        [Required(ErrorMessage = "Informe o nome do cliente")]
        public string idCustomer { get; set; }

        [Display(Name = "Nome do pet")]
        [Required(ErrorMessage = "Informe o nome do pet")]
        public string idPet { get; set; }

        [Display(Name = "Serviço oferecido")]
        [Required(ErrorMessage = "Escolha o serviço deseja agendar")]
        public string idService { get; set; }

        [Display(Name = "Data do agendamento")]
        [Required(ErrorMessage = "Informe a data do agendamento")]
        public string dateSchedule { get; set; }

        [Display(Name = "Horário do agendamento")]
        [Required(ErrorMessage = "Informe o horário do agendamento")]
        public string timeSchedule { get; set; }

        [Display(Name = "Observações")]
        [Required(ErrorMessage = "Informe a observação")]
        public string observations { get; set; }
    }
}