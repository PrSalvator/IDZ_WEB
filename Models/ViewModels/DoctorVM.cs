using IDZ.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IDZ.Models.ViewModels
{
    public class DoctorVM
    {
        public Guid DOCTOR_ID { get; set; }
        [Required]
        [DisplayName("Фамилия")]
        [StringLength(30)]
        public string LAST_NAME { get; set; }
        [DisplayName("Имя")]
        [StringLength(30)]
        [Required]
        public string FIRST_NAME { get; set; }
        [DisplayName("Отчество")]
        [StringLength(30)]
        public string PATRONYMIC { get; set; }

    }
}