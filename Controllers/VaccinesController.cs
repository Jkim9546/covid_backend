using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using covid19_api.Models;

namespace covid19_api.Controllers
{
    [EnableCors("*", "*", "*")]
    public class VaccinesController : BaseController
    {
        // GET: api/Vaccines
        [ResponseType(typeof(Vaccine))]
        public IHttpActionResult GetVaccines()
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Vaccines);
            return Ok(response);
        }

        // GET: api/Vaccines/5
        [ResponseType(typeof(Vaccine))]
        public IHttpActionResult GetVaccine(Guid id)
        {
            Vaccine vaccine = new Vaccine();

            try
            {
                vaccine = db.Vaccines.Where(s => s.ControlID == id).SingleOrDefault();
            }
            catch
            {
                vaccine = null;
            }
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(vaccine);
            return Ok(response);
        }



        [Route("api/Vaccines/{proID}/All")]
        public IHttpActionResult GetProvince(int proID)
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Vaccines.Where(s => s.PrUid == proID).OrderBy(s => s.Week_End));
            return Ok(response);
        }

        [Route("api/Vaccines/{proID}/{date}/All")]
        public IHttpActionResult GetProvince(int proID, DateTime date)
        {
            DateTime latest = db.Vaccines.OrderByDescending(s=>s.Week_End).FirstOrDefault().Week_End;
            date = GetDate(date, latest);
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Vaccines.Where(s => s.PrUid == proID && s.Week_End == date).OrderBy(s => s.Week_End));
            return Ok(response);
        }

        [Route("api/Vaccines/alpha/{code}/All")]
        public IHttpActionResult GetProvince(string code)
        {
            int proID = GetProID(code);
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Vaccines.Where(s => s.PrUid == proID).OrderBy(s => s.Week_End));
            return Ok(response);
        }

        [Route("api/Vaccines/alpha/{code}/{date}/All")]
        public IHttpActionResult GetProvince(string code, DateTime date)
        {
            int proID = GetProID(code);
            DateTime latest = db.Vaccines.OrderByDescending(s => s.Week_End).FirstOrDefault().Week_End;
            date = GetDate(date, latest);
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Vaccines.Where(s => s.PrUid == proID && s.Week_End == date).OrderBy(s => s.Week_End));
            return Ok(response);
        }


        // PUT: api/Vaccines/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVaccine(Vaccine vaccine)
        {
            Guid id = vaccine.ControlID;
            string response = "";
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (!ModelState.IsValid)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("BadRequest - Missing Required Information");
                return Ok(response);
            }

            if (id != vaccine.ControlID)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("BadRequest-Mismatching Required Information");
                return Ok(response);
            }

            if (db.Vaccines.Find(id) != null)
            {
                db.Entry(db.Vaccines.Find(id)).State = EntityState.Detached;
            }

            db.Entry(vaccine).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VaccineExists(id))
                {
                    response = Newtonsoft.Json.JsonConvert.SerializeObject("NotFound");
                    return Ok(response);
                }
                else
                {
                    response = Newtonsoft.Json.JsonConvert.SerializeObject("Saving Error");
                    return Ok(response);
                }
            }

            response = Newtonsoft.Json.JsonConvert.SerializeObject("OK");
            return Ok(response);
        }

        // POST: api/Vaccines
        [ResponseType(typeof(Vaccine))]
        public IHttpActionResult PostVaccine(Vaccine vaccine)
        {
            string response = "";
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (!ModelState.IsValid)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("BadRequest-Mismatching Required Information");
                return Ok(response);
            }

            db.Vaccines.Add(vaccine);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (VaccineExists(vaccine.ControlID))
                {
                    response = Newtonsoft.Json.JsonConvert.SerializeObject("BadRequest-Already Existed");
                    return Ok(response);
                }
                else
                {
                    response = Newtonsoft.Json.JsonConvert.SerializeObject("Unexpected Error");
                    return Ok(response);
                }
            }

            response = Newtonsoft.Json.JsonConvert.SerializeObject("OK");
            return Ok(response);
        }

        // DELETE: api/Vaccines/5
        [ResponseType(typeof(Vaccine))]
        public IHttpActionResult DeleteVaccine(Guid id)
        {
            string response = "";
            Vaccine vaccine = db.Vaccines.Where(s => s.ControlID == id).SingleOrDefault();

            if (vaccine == null)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("Not Found");
                return Ok(response);
            }

            db.Vaccines.Remove(vaccine);
            db.SaveChanges();
            response = Newtonsoft.Json.JsonConvert.SerializeObject("OK");
            return Ok(response);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VaccineExists(Guid id)
        {
            return db.Vaccines.Count(e => e.ControlID == id) > 0;
        }

        public int GetProID(string code)
        {
            int pruID = 0;
            code = code.ToUpper();

            switch (code)
            {
                case "NL":
                    pruID = 10;
                    break;

                case "PE":
                    pruID = 11;
                    break;
                case "NS":
                    pruID = 12;
                    break;
                case "NB":
                    pruID = 13;
                    break;
                case "QC":
                    pruID = 24;
                    break;
                case "ON":
                    pruID = 35;
                    break;
                case "MB":
                    pruID = 46;
                    break;
                case "SK":
                    pruID = 47;
                    break;
                case "AB":
                    pruID = 48;
                    break;
                case "BC":
                    pruID = 59;
                    break;
                case "YT":
                    pruID = 60;
                    break;
                case "NT":
                    pruID = 61;
                    break;
                case "NU":
                    pruID = 62;
                    break;
            }
            return pruID;
        }

        public DateTime GetDate(DateTime date, DateTime latest)
        {
            switch (date.DayOfWeek.ToString().ToUpper())
            {
                case "SUNDAY":
                    date = date.AddDays(-1);
                    break;

                case "MONDAY":
                    date = date.AddDays(-2);
                    break;
                case "TUESDAY":
                    date = date.AddDays(-3);
                    break;
                case "WEDNESDAY":
                    date = date.AddDays(-4);
                    break;
                case "THURSDAY":
                    date = date.AddDays(-5);
                    break;
                case "FRIDAY":
                    date = date.AddDays(-6);
                    break;

                case "SATURDAY":
                    break;
            }

            // var diff = today.Subtract(date).TotalDays;
            //string dow = date.DayOfWeek.ToString();
            if (date < Convert.ToDateTime("2020-12-19"))
            {
                date = Convert.ToDateTime("2020-12-19");
            }
            else if (date >= latest)
            {
                date = latest;
            }

            return date;
        }
    }
}