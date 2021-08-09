using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthDemo.Context;
using AuthDemo.Model;
using Microsoft.AspNetCore.Authorization;

namespace AuthDemo.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleClientsController : ControllerBase
    {
        private readonly UserDbContext _context;

        public RoleClientsController(UserDbContext context)
        {
            _context = context;
        }

        // GET: api/RoleClients
        [Authorize]
        [HttpGet]
        public IEnumerable<RoleClient> GetRoleClients()
        {
            return new List<RoleClient>() { new RoleClient() { Name ="testRole"} };

            //return await _context.RoleClients.ToListAsync();
        }

        // GET: api/RoleClients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleClient>> GetRoleClient(int id)
        {
            var roleClient = await _context.RoleClients.FindAsync(id);

            if (roleClient == null)
            {
                return NotFound();
            }

            return roleClient;
        }

        // PUT: api/RoleClients/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoleClient(int id, RoleClient roleClient)
        {
            if (id != roleClient.Id)
            {
                return BadRequest();
            }

            _context.Entry(roleClient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleClientExists(id))
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

        // POST: api/RoleClients
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<RoleClient>> PostRoleClient(RoleClient roleClient)
        {
            _context.RoleClients.Add(roleClient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoleClient", new { id = roleClient.Id }, roleClient);
        }

        // DELETE: api/RoleClients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoleClient>> DeleteRoleClient(int id)
        {
            var roleClient = await _context.RoleClients.FindAsync(id);
            if (roleClient == null)
            {
                return NotFound();
            }

            _context.RoleClients.Remove(roleClient);
            await _context.SaveChangesAsync();

            return roleClient;
        }

        private bool RoleClientExists(int id)
        {
            return _context.RoleClients.Any(e => e.Id == id);
        }
    }
}
