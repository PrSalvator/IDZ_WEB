using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace IDZ.Models.ViewModels
{
    public class Doctor_Reception_HoursVM
    {
        [DisplayName("Фио доктора")]
        public string Doctor { get; set; }
        [DisplayName("Специальность")]
        public string Speciality { get; set; }
        [DisplayName("Час начала приема")]
        public Nullable<System.TimeSpan> Start_Hour { get; set; }
        [DisplayName("Час конца приема")]
        public Nullable<System.TimeSpan> End_Hour { get; set; }
        [DisplayName("День недели")]
        public int Day_Of_Week { get; set; }
    }
}