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
    [EnableCors("*","*","*")]
    public class OntariosController : BaseController
    {
        //private COVIDInfo db = new COVIDInfo();

        // GET: api/Ontarios
        [ResponseType(typeof(Ontario))]
        public IHttpActionResult GetOntatios()
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Ontarios);
            return Ok(response);
        }

        // GET: api/Ontarios/5
        [ResponseType(typeof(Ontario))]
        public IHttpActionResult GetOntario(Guid id)
        {
            Ontario ontario = new Ontario();

            try
            {
                ontario = db.Ontarios.Where(s => s.ControlID == id).SingleOrDefault();
            }
            catch
            {
                ontario = null;
            }
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(ontario);
            return Ok(response);

        }

        [Route("api/Ontarios/{PHU_NUM}/All")]
        public IHttpActionResult GetOntarioPHUByPHU_NUM(int PHU_NUM)
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Ontarios.Where(s => s.PHU_NUM == PHU_NUM).OrderBy(s => s.ReportedDate));
            return Ok(response);
        }

        [Route("api/Ontarios/{PHU_NUM}/{reportedDate}/All")]
        public IHttpActionResult GetOntarioPHUByPHU_NUM(int PHU_NUM, DateTime reportedDate)
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Ontarios.Where(s => s.PHU_NUM == PHU_NUM && s.ReportedDate == reportedDate).OrderBy(s => s.ReportedDate));
            return Ok(response);
        }

        [Route("api/Ontarios/Name/{PHU_NAME}/All")]
        public IHttpActionResult GetOntarioPHUByPHU_NAME(string PHU_NAME)
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Ontarios.Where(s => s.PHU_NAME.Contains(PHU_NAME)).OrderBy(s => s.ReportedDate));
            return Ok(response);
        }

        [Route("api/Ontarios/Name/{PHU_NAME}/{reportedDate}/All")]
        public IHttpActionResult GetOntarioPHUByPHU_NAME(string PHU_NAME, DateTime reportedDate)
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Ontarios.Where(s => s.PHU_NAME.Contains(PHU_NAME) && s.ReportedDate == reportedDate).OrderBy(s => s.ReportedDate));
            return Ok(response);
        }

        // PUT: api/Ontarios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOntario(Ontario ontario)
        {
            Guid id = ontario.ControlID;
            string response = "";
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (!ModelState.IsValid)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("BadRequest - Missing Required Information");
                return Ok(response);
            }

            if (id != ontario.ControlID)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("BadRequest-Mismatching Required Information");
                return Ok(response);
            }

            if (db.Ontarios.Find(id) != null)
            {
                db.Entry(db.Ontarios.Find(id)).State = EntityState.Detached;
            }

            db.Entry(ontario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OntarioExists(id))
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

        // POST: api/Ontarios
        [ResponseType(typeof(Ontario))]
        public IHttpActionResult PostOntario(Ontario ontario)
        {
            string response = "";
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (!ModelState.IsValid)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("BadRequest-Mismatching Required Information");
                return Ok(response);
            }

            db.Ontarios.Add(ontario);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OntarioExists(ontario.ControlID))
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

        // DELETE: api/Ontarios/5
        [ResponseType(typeof(Ontario))]
        public IHttpActionResult DeleteOntario(Guid id)
        {
            string response = "";
            Ontario ontario = db.Ontarios.Where(s => s.ControlID == id).SingleOrDefault();

            if (ontario == null)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("Not Found");
                return Ok(response);
            }

            db.Ontarios.Remove(ontario);
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

        private bool OntarioExists(Guid id)
        {
            return db.Ontarios.Count(e => e.ControlID == id) > 0;
        }
    }
}