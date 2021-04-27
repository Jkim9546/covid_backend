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
    public class WorldsController : BaseController
    {
        // private COVIDInfo db = new COVIDInfo();

        // GET: api/Worlds
        [ResponseType(typeof(World))]
        public IHttpActionResult GetWorlds()
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Worlds);
            return Ok(response);
        }

        // GET: api/Worlds/5
        [ResponseType(typeof(World))]
        public IHttpActionResult GetWorld(Guid id)
        {
            World world = new World();

            try
            {
                world = db.Worlds.Where(s => s.ControlID == id).SingleOrDefault();
            }
            catch
            {
                world = null;
            }
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(world);
            return Ok(response);
        }

        [Route("api/Worlds/{Country}/All")]
        public IHttpActionResult GetWorldCountryAll(string country)
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Worlds.Where(s => s.Country_Region == country).OrderBy(s => s.ReportedDate));
            return Ok(response);
        }

        [Route("api/Worlds/{Country}/{reportedDate}/All")]
        public IHttpActionResult GetWorldCountryAll(string country, DateTime reportedDate)
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Worlds.Where(s => s.Country_Region == country && s.ReportedDate == reportedDate).OrderBy(s => s.ReportedDate));
            return Ok(response);
        }

        [Route("api/Worlds/{Country}/Confirmed")]
        public IHttpActionResult GetWorldCountryConfirmed(string country)
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Worlds.Where(s => s.Country_Region == country && s.DataType == "Confirmed").OrderBy(s=>s.ReportedDate));
            return Ok(response);
        }

        [Route("api/Worlds/{Country}/{reportedDate}/Confirmed")]
        public IHttpActionResult GetWorldCountryConfirmed(string country, DateTime reportedDate)
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Worlds.Where(s => s.Country_Region == country && s.DataType == "Confirmed" && s.ReportedDate == reportedDate).OrderBy(s => s.ReportedDate));
            return Ok(response);
        }

        [Route("api/Worlds/{Country}/Recovered")]
        public IHttpActionResult GetWorldCountryRecovered(string country)
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Worlds.Where(s => s.Country_Region == country && s.DataType == "Recovered").OrderBy(s => s.ReportedDate));
            return Ok(response);
        }


        [Route("api/Worlds/{Country}/{reportedDate}/Recovered")]
        public IHttpActionResult GetWorldCountryRecovered(string country, DateTime reportedDate)
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Worlds.Where(s => s.Country_Region == country && s.DataType == "Recovered" && s.ReportedDate == reportedDate).OrderBy(s => s.ReportedDate));
            return Ok(response);
        }


        [Route("api/Worlds/{Country}/Deaths")]
        public IHttpActionResult GetWorldCountryDeaths(string country)
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Worlds.Where(s => s.Country_Region == country && s.DataType == "Deaths").OrderBy(s => s.ReportedDate));
            return Ok(response);
        }


        [Route("api/Worlds/{Country}/{reportedDate}/Deaths")]
        public IHttpActionResult GetWorldCountryDeaths(string country, DateTime reportedDate)
        {
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(db.Worlds.Where(s => s.Country_Region == country && s.DataType == "Deaths" && s.ReportedDate == reportedDate).OrderBy(s => s.ReportedDate));
            return Ok(response);
        }

        // PUT: api/Worlds/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWorld(World world)
        {
            Guid id = world.ControlID;
            string response = "";
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (!ModelState.IsValid)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("BadRequest - Missing Required Information");
                return Ok(response);
            }

            if (id != world.ControlID)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("BadRequest-Mismatching Required Information");
                return Ok(response);
            }

            if (db.Worlds.Find(id) != null)
            {
                db.Entry(db.Worlds.Find(id)).State = EntityState.Detached;
            }

            db.Entry(world).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorldExists(id))
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

        // POST: api/Worlds
        [ResponseType(typeof(World))]
        public IHttpActionResult PostWorld(World world)
        {
            string response = "";
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (!ModelState.IsValid)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("BadRequest-Mismatching Required Information");
                return Ok(response);
            }

            db.Worlds.Add(world);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (WorldExists(world.ControlID))
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

        // DELETE: api/Worlds/5
        [ResponseType(typeof(World))]
        public IHttpActionResult DeleteWorld(Guid id)
        {
            string response = "";
            World world = db.Worlds.Where(s => s.ControlID == id).SingleOrDefault();

            if (world == null)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject("Not Found");
                return Ok(response);
            }

            db.Worlds.Remove(world);
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

        private bool WorldExists(Guid id)
        {
            return db.Worlds.Count(e => e.ControlID == id) > 0;
        }
    }
}