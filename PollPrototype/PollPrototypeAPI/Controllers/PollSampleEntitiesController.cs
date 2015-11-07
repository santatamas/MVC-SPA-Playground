using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using PollPrototypeAPI.Models;

namespace PollPrototypeAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PollSampleEntitiesController : ApiController
    {
        private PollSampleContext db = new PollSampleContext();

        // GET: api/PollSampleEntities
        public IQueryable<PollSampleEntity> GetPollSamples()
        {
            if (!db.PollSamples.Any())
            {
                GenerateRandomPollSamples(100);
                db.SaveChanges();
            }
            return db.PollSamples;
        }

        private void GenerateRandomPollSamples(int count)
        {
            Random rnd = new Random();
            string[] countryIds = new[] {"HU", "US", "DE", "ES", "NO"};
            string[] firstNames = new[] { "Thomas", "George", "Judy", "Clair", "Richard", "Gillian", "Frank", "Anthony" };
            string[] lastNames = new[] { "Blair", "Bush", "Z", "Dawkins", "Hawkins", "Nothammer" };
            for (int i = 0; i < count; i++)
            {
                db.PollSamples.Add(new PollSampleEntity
                {
                    Name = firstNames[rnd.Next(0, firstNames.Length - 1)] + " " + lastNames[rnd.Next(0, lastNames.Length - 1)],
                    Age = rnd.Next(80),
                    CountryId = countryIds[rnd.Next(0,countryIds.Length - 1)],
                    NumericAnswer1 = rnd.Next(0,10),
                    NumericAnswer2 = rnd.Next(),
                    TrueFalseAnswer1 = rnd.Next(0,3) == 0,
                    TrueFalseAnswer2 = rnd.Next(0,3) == 0
                });
            }

        }

        // GET: api/PollSampleEntities/5
        [ResponseType(typeof(PollSampleEntity))]
        public async Task<IHttpActionResult> GetPollSampleEntity(int id)
        {
            PollSampleEntity pollSampleEntity = await db.PollSamples.FindAsync(id);
            if (pollSampleEntity == null)
            {
                return NotFound();
            }

            return Ok(pollSampleEntity);
        }

        // PUT: api/PollSampleEntities/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPollSampleEntity(int id, PollSampleEntity pollSampleEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pollSampleEntity.PollSampleEntityId)
            {
                return BadRequest();
            }

            db.Entry(pollSampleEntity).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollSampleEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PollSampleEntities
        [ResponseType(typeof(PollSampleEntity))]
        public async Task<IHttpActionResult> PostPollSampleEntity(PollSampleEntity pollSampleEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PollSamples.Add(pollSampleEntity);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = pollSampleEntity.PollSampleEntityId }, pollSampleEntity);
        }

        // DELETE: api/PollSampleEntities/5
        [ResponseType(typeof(PollSampleEntity))]
        public async Task<IHttpActionResult> DeletePollSampleEntity(int id)
        {
            PollSampleEntity pollSampleEntity = await db.PollSamples.FindAsync(id);
            if (pollSampleEntity == null)
            {
                return NotFound();
            }

            db.PollSamples.Remove(pollSampleEntity);
            await db.SaveChangesAsync();

            return Ok(pollSampleEntity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PollSampleEntityExists(int id)
        {
            return db.PollSamples.Count(e => e.PollSampleEntityId == id) > 0;
        }
    }
}