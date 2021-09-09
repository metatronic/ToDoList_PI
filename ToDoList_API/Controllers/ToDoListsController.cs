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
using System.Web.Http.Description;
using ToDoList_API.Models;

namespace ToDoList_API.Controllers
{
    public class ToDoListsController : ApiController
    {
        private TRAININGEntities db = new TRAININGEntities();

        // GET: api/ToDoLists
        public IQueryable<ToDoList> GetToDoLists()
        {
            return db.ToDoLists;
        }

        // GET: api/ToDoLists/5
        [ResponseType(typeof(ToDoList))]
        public async Task<IHttpActionResult> GetToDoList(int id)
        {
            ToDoList toDoList = await db.ToDoLists.FindAsync(id);
            if (toDoList == null)
            {
                return NotFound();
            }

            return Ok(toDoList);
        }

        // PUT: api/ToDoLists/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutToDoList(int id, ToDoList toDoList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != toDoList.TaskId)
            {
                return BadRequest();
            }

            db.Entry(toDoList).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoListExists(id))
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

        // POST: api/ToDoLists
        [ResponseType(typeof(ToDoList))]
        public async Task<IHttpActionResult> PostToDoList(ToDoList toDoList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ToDoLists.Add(toDoList);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = toDoList.TaskId }, toDoList);
        }

        // DELETE: api/ToDoLists/5
        [ResponseType(typeof(ToDoList))]
        public async Task<IHttpActionResult> DeleteToDoList(int id)
        {
            ToDoList toDoList = await db.ToDoLists.FindAsync(id);
            if (toDoList == null)
            {
                return NotFound();
            }

            db.ToDoLists.Remove(toDoList);
            await db.SaveChangesAsync();

            return Ok(toDoList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ToDoListExists(int id)
        {
            return db.ToDoLists.Count(e => e.TaskId == id) > 0;
        }
    }
}