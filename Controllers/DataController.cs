using IDZ.Models.Entities;
using IDZ.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace IDZ.Controllers
{
    public class DataController : Controller
    {
        // GET: Data
        private static Guid doctorId;
        private static Guid doctorSpecialityId;
        private static string patientId;
        public ActionResult Index()
        {

            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetDoctorReceptionHours(Guid doctorSpecialityId)
        {
            DataController.doctorSpecialityId = doctorSpecialityId;
            List<RECEPTION_HOURS> recHours = new List<RECEPTION_HOURS>();
            List<APPOINTMENT> appointments = new List<APPOINTMENT>();
            DateTime curDate1 = DateTime.Now.Date;
            DateTime curDate2 = DateTime.Now.AddDays(6).Date;
            using (DBContext db = new DBContext())
            {
                recHours = db.RECEPTION_HOURS.Where(d => d.DOCTOR_SPECIALITY_ID == doctorSpecialityId).
                    Include(p => p.DOCTOR_SPECIALITY).
                        Where(p => p.DOCTOR_SPECIALITY.IS_VALID == true).ToList();
                appointments = db.APPOINTMENT.Include(p => p.DOCTOR_SPECIALITY).
                    Where(d => d.DOCTOR_SPECIALITY_ID == doctorSpecialityId && d.DOCTOR_SPECIALITY.IS_VALID == true).ToList().
                    Where(d => d.DATETIME.Date >= curDate1 && d.DATETIME.Date <= curDate2).OrderBy(d => d.DATETIME).ToList();
            }
            ViewBag.ReceptionHours = recHours;
            return View(appointments);
        }
        [Authorize(Roles = "Patient")]
        public ActionResult CreateAppointment(DateTime date)
        {
            using (DBContext db = new DBContext())
            {
                APPOINTMENT a = new APPOINTMENT
                {
                    DOCTOR_SPECIALITY_ID = doctorSpecialityId,
                    DATETIME = date,
                    PATIENT_ID = patientId,
                    PARAMETR_ID = 2,
                    ID = Guid.NewGuid()
                };
                db.APPOINTMENT.Add(a);
                db.SaveChanges();
            }
            return RedirectToAction("GetDoctorReceptionHours", "Data", new { doctorSpecialityId });
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetAllReceptionHours()
        {
            List<DOCTOR_RECEPTION_HOURS> doctorsReceptionHours = new List<DOCTOR_RECEPTION_HOURS>();
            using (DBContext db = new DBContext())
            {
                doctorsReceptionHours = db.DOCTOR_RECEPTION_HOURS.ToList();
            }
            return View(doctorsReceptionHours);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GetDoctors()
        {
            List<DOCTOR> doctors;
            using (DBContext db = new DBContext())
            {
                doctors = db.DOCTOR.Where(p => p.IS_VALID == true).ToList();
            }
            return View(doctors);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditDoctor(Guid doctorId)
        {
            DoctorVM doctorVM;
            using (DBContext db = new DBContext())
            {
                DOCTOR doctor = db.DOCTOR.Where(p => p.ID == doctorId).First();
                doctorVM = new DoctorVM
                {
                    FIRST_NAME = doctor.FIRST_NAME,
                    LAST_NAME = doctor.LAST_NAME,
                    PATRONYMIC = doctor.PATRONYMIC,
                    DOCTOR_ID = doctor.ID
                };
            }
            return View(doctorVM);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditDoctor(DoctorVM doctor)
        {
            if (ModelState.IsValid)
            {
                using (DBContext db = new DBContext())
                {
                    DOCTOR d = new DOCTOR
                    {
                        LAST_NAME = doctor.LAST_NAME,
                        FIRST_NAME = doctor.FIRST_NAME,
                        PATRONYMIC = doctor.PATRONYMIC,
                        ID = doctor.DOCTOR_ID,
                        IS_VALID = true
                    };
                    db.Entry(d).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("GetDoctors");
            }
            return View(doctor);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditSpecialityOfDoctor(Guid doctorId)
        {
            DataController.doctorId = doctorId;
            List<SPECIALITY> accesSpecialities;
            List<SPECIALITY> docSpecialities;
            using (DBContext db = new DBContext())
            {
                List<SPECIALITY> specialities = db.SPECIALITY.ToList();
                docSpecialities = db.DOCTOR_SPECIALITY.Where(p => p.DOCTOR_ID == doctorId && p.IS_VALID == true)
                    .Include(p => p.SPECIALITY).Select(p => p.SPECIALITY).ToList();
                accesSpecialities = specialities.Except(docSpecialities).ToList();
            }
            ViewBag.SelectList = new SelectList(accesSpecialities, "ID", "NAME");
            return View(docSpecialities);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("EditSpecialityOfDoctor")]
        public ActionResult AddSpecialityToDoctor(Guid specialityId)
        {
            using (DBContext db = new DBContext())
            {
                if(db.DOCTOR_SPECIALITY.Any(p => p.DOCTOR_ID == doctorId && p.SPECIALITY_ID == specialityId))
                {
                    DOCTOR_SPECIALITY ds = db.DOCTOR_SPECIALITY.Where(p => p.DOCTOR_ID == doctorId && p.SPECIALITY_ID == specialityId).First();
                    ds.IS_VALID = true;
                    db.Entry(ds).State = EntityState.Modified;
                }
                else
                {
                    DOCTOR_SPECIALITY ds = new DOCTOR_SPECIALITY
                    {
                        ID = Guid.NewGuid(),
                        DOCTOR_ID = doctorId,
                        SPECIALITY_ID = specialityId,
                        IS_VALID = true
                    };
                    db.DOCTOR_SPECIALITY.Add(ds);
                }
                db.SaveChanges();
            }
            return RedirectToAction("EditSpecialityOfDoctor", new { doctorId });
        }
        [Authorize(Roles = "Admin")]
        public ActionResult RemoveSpecialityOfDoctor(Guid specialityId)
        {
            using (DBContext db = new DBContext())
            {
                DOCTOR_SPECIALITY ds = db.DOCTOR_SPECIALITY.Where(p => p.DOCTOR_ID == doctorId && p.SPECIALITY_ID == specialityId).First();
                ds.IS_VALID = false;
                db.Entry(ds).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("EditSpecialityOfDoctor", new { doctorId });
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteDoctor(Guid doctorId)
        {
            using (DBContext db = new DBContext())
            {
                DOCTOR d = db.DOCTOR.Where(p => p.ID == doctorId).First();
                d.IS_VALID = false;
                db.Entry(d).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("GetDoctors", "Data");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult CreateDoctor()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDoctor(DoctorVM doctorVM)
        {
            if (ModelState.IsValid)
            {
                using (DBContext db = new DBContext())
                {
                    DOCTOR d = new DOCTOR
                    {
                        LAST_NAME = doctorVM.LAST_NAME,
                        FIRST_NAME = doctorVM.FIRST_NAME,
                        PATRONYMIC = doctorVM.PATRONYMIC,
                        ID = Guid.NewGuid(),
                        IS_VALID = true
                    };
                    db.DOCTOR.Add(d);
                    db.SaveChanges();
                }
                return RedirectToAction("GetDoctors", "Data");
            }
            return View(doctorVM);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AddReceptionHoursForDoctor(Guid doctorSpecialityId)
        {
            DataController.doctorSpecialityId = doctorSpecialityId;
            return View(GetListReceptionHours(doctorSpecialityId));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddReceptionHoursForDoctor(int dayOfWeek, TimeSpan? startHour, TimeSpan? endHour)
        {
            if(startHour != null || endHour != null)
            {
                using (DBContext db = new DBContext())
                {
                    try
                    {
                        if (db.RECEPTION_HOURS.Any(p => p.DOCTOR_SPECIALITY_ID == doctorSpecialityId && p.NUMBER_OF_DAY_WEEK == dayOfWeek))
                        {
                            RECEPTION_HOURS rh = db.RECEPTION_HOURS.Where(p => p.DOCTOR_SPECIALITY_ID == doctorSpecialityId && p.NUMBER_OF_DAY_WEEK == dayOfWeek).First();
                            rh.NUMBER_OF_DAY_WEEK = dayOfWeek;
                            rh.HOUR_START = TimeSpan.Parse(startHour.ToString());
                            rh.HOUR_END = TimeSpan.Parse(endHour.ToString());
                            db.Entry(rh).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            RECEPTION_HOURS rh = new RECEPTION_HOURS
                            {
                                DOCTOR_SPECIALITY_ID = doctorSpecialityId,
                                NUMBER_OF_DAY_WEEK = dayOfWeek,
                                HOUR_END = TimeSpan.Parse(endHour.ToString()),
                                HOUR_START = TimeSpan.Parse(startHour.ToString())
                            };
                            db.RECEPTION_HOURS.Add(rh);
                            db.SaveChanges();
                        }
                    }
                    catch
                    {
                        View(GetListReceptionHours(doctorSpecialityId));
                    }
                }
            }
            return View(GetListReceptionHours(doctorSpecialityId));
        }
        private List<Doctor_Reception_HoursVM> GetListReceptionHours(Guid doctorSpecialityId)
        {
            DataController.doctorSpecialityId = doctorSpecialityId;
            List<Doctor_Reception_HoursVM> list = new List<Doctor_Reception_HoursVM>();
            using (DBContext db = new DBContext())
            {
                foreach (RECEPTION_HOURS rh in db.RECEPTION_HOURS.Where(p => p.DOCTOR_SPECIALITY_ID == doctorSpecialityId).ToList())
                {
                    list.Add(new Doctor_Reception_HoursVM
                    {
                        Day_Of_Week = rh.NUMBER_OF_DAY_WEEK,
                        Start_Hour = rh.HOUR_START,
                        End_Hour = rh.HOUR_END
                    });
                }
            }
            return list;
        }
        public static void getPatientId(Guid userId)
        {
            using (DBContext db = new DBContext())
            {
                USER u = db.USER.Include(p => p.PATIENT).Where(p => p.ID == userId).First();
                patientId = u.PATIENT.First().POLICE_ID;
            }
        }
    }
}