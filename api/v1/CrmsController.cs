using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.Data;
using Training.Models;
using Training.crm;

namespace Training.api.v1
{
    [Produces("application/json")]
    [Route("api/Crms")]
    public class CrmsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CrmsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Crms
        [HttpGet]
        public IEnumerable<Crm> GetCrm()
        {

            return _context.Crm;
        }

        [HttpGet("crmtest")]
        public string TestCrm()
        {
            CrmServiceProvider serviceProvider = new CrmServiceProvider();
            var service = serviceProvider.GetService();

            return serviceProvider.GetTestUser();
        }

        // GET: api/Crms/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCrm([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var crm = await _context.Crm.SingleOrDefaultAsync(m => m.CrmId == id);

            if (crm == null)
            {
                return NotFound();
            }

            return Ok(crm);
        }

        // PUT: api/Crms/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCrm([FromRoute] Guid id, [FromBody] Crm crm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != crm.CrmId)
            {
                return BadRequest();
            }

            _context.Entry(crm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CrmExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Crms
        [HttpPost]
        public async Task<IActionResult> PostCrm([FromBody] Crm crm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Crm.Add(crm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCrm", new { id = crm.CrmId }, crm);
        }

        // DELETE: api/Crms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCrm([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var crm = await _context.Crm.SingleOrDefaultAsync(m => m.CrmId == id);
            if (crm == null)
            {
                return NotFound();
            }

            _context.Crm.Remove(crm);
            await _context.SaveChangesAsync();

            return Ok(crm);
        }

        private bool CrmExists(Guid id)
        {
            return _context.Crm.Any(e => e.CrmId == id);
        }
    }
}