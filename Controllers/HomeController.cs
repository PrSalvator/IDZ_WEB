using IDZ.Models.Entities;
using IDZ.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDZ.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Doctor_SpecialityVM> doctors = new List<Doctor_SpecialityVM>();
            using (DBContext db = new DBContext())
            {
                foreach(DOCTOR_SPECIALITY ds in db.DOCTOR_SPECIALITY.Where(p => p.IS_VALID == true).ToList())
                {
                    doctors.Add(new Doctor_SpecialityVM {
                        ID = ds.ID,
                        Doctor = $"{ds.DOCTOR.LAST_NAME} {ds.DOCTOR.FIRST_NAME} {ds.DOCTOR.PATRONYMIC}",
                        Speciality = ds.SPECIALITY.NAME
                    });
                }
            }
            return View(doctors);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}