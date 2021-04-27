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
    public class CanadasController : BaseController
    {
        // GET: api/Canadas
        [ResponseType(typeof(Canada))]
        public IHttpActionResult GetCanadas()
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Canadas);
            return Ok(response);
        }

        // GET: api/Canadas/5
        [ResponseType(typeof(Canada))]
        public IHttpActionResult GetCanada(Guid id)
        {
            Canada canada = new Canada();

            try
            {
                canada = db.Canadas.Where(s => s.ControlID == id).SingleOrDefault();
            }
            catch
            {
                canada = null;
            }
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(canada);
            return Ok(response);

        }


        [Route("api/Canadas/{proID}/All")]
        public IHttpActionResult GetProvince(int proID)
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Canadas.Where(s => s.PrUid == proID).OrderBy(s => s.Date));
            return Ok(response);
        }

        [Route("api/Canadas/{proID}/{date}/All")]
        public IHttpActionResult GetProvince(int proID, DateTime date)
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Canadas.Where(s => s.PrUid == proID && s.Date == date).OrderBy(s => s.Date));
            return Ok(response);
        }

        [Route("api/Canadas/alpha/{code}/All")]
        public IHttpActionResult GetProvince(string code)
        {
            int proID = GetProID(code);
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Canadas.Where(s => s.PrUid == proID).OrderBy(s => s.Date));
            return Ok(response);
        }

        [Route("api/Canadas/alpha/{code}/{date}/All")]
        public IHttpActionResult GetProvince(string code, DateTime date)
        {
            int proID = GetProID(code);
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Canadas.Where(s => s.PrUid == proID && s.Date == date).OrderBy(s => s.Date));
            return Ok(response);
        }

        // PUT: api/Canadas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCanada(Canada canada)
        {
            Guid id = canada.ControlID;
            string response = "";
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (!ModelState.IsValid)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("BadRequest - Missing Required Information");
                return Ok(response);
            }

            if (id != canada.ControlID)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("BadRequest-Mismatching Required Information");
                return Ok(response);
            }

            if (db.Canadas.Find(id) != null)
            {
                db.Entry(db.Canadas.Find(id)).State = EntityState.Detached;
            }

            db.Entry(canada).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CanadaExists(id))
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

        // POST: api/Canadas
        [ResponseType(typeof(Canada))]
        public IHttpActionResult PostCanada(Canada canada)
        {
            string response = "";
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (!ModelState.IsValid)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("BadRequest-Mismatching Required Information");
                return Ok(response);
            }

            db.Canadas.Add(canada);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CanadaExists(canada.ControlID))
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

        // DELETE: api/Canadas/5
        [ResponseType(typeof(Canada))]
        public IHttpActionResult DeleteCanada(Guid id)
        {
            string response = "";
            Canada canada = db.Canadas.Where(s => s.ControlID == id).SingleOrDefault();

            if (canada == null)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("Not Found");
                return Ok(response);
            }

            db.Canadas.Remove(canada);
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

        private bool CanadaExists(Guid id)
        {
            return db.Canadas.Count(e => e.ControlID == id) > 0;
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
    }
}