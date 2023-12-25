using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace IDZ.Models.ViewModels
{
    public class Doctor_SpecialityVM
    {
        public Guid ID { get; set; }
        [DisplayName("ФИО доктора")]
        public string Doctor { get; set; }
        [DisplayName("Специальность")]
        public string Speciality { get; set; }
    }
}