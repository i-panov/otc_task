using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using otc_task.Models;

namespace otc_task.Controllers
{
    [Route("api/person")]
    public class PersonController : Controller
    {
        [HttpGet("list")]
        public async Task<IEnumerable<Person>> GetList()
        {
            using (var db = new ApplicationContext())
            {
                return await db.Persons.ToArrayAsync();
            }
        }

        [HttpGet("view")]
        public async Task<Person> GetOne(int id)
        {
            using (var db = new ApplicationContext())
            {
                return await db.Persons.Where(p => p.Id == id).FirstAsync();
            }
        }

        [HttpPost("save")]
        public async Task<object> Save([FromBody] Person newModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = ModelState.ToDictionary(p => p.Key, p => p.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                return new {Errors = errors};
            }

            using (var db = new ApplicationContext())
            {
                if (newModel.Id <= 0)
                {
                    db.Persons.Add(newModel);
                    Response.StatusCode = 201;
                }
                else
                {
                    db.Update(newModel);
                }

                await db.SaveChangesAsync();
            }

            return new {Id = newModel.Id};
        }

        [HttpDelete("delete")]
        public async Task<object> Delete(int id)
        {
            using (var db = new ApplicationContext())
            {
                try
                {
                    db.Persons.Remove(new Person() {Id = id});
                    await db.SaveChangesAsync();
                    return null;
                }
                catch (Exception e)
                {
                    Response.StatusCode = 400;
                    return new {Error = e.Message};
                }
            }
        }

        [HttpGet("averageAge")]
        public async Task<object> GetAverageAge()
        {
            using (var db = new ApplicationContext())
            {
                try
                {
                    var result = (int)Math.Round(await db.Persons.Select(p => p.Age).AverageAsync());
                    return new {Value = result};
                }
                catch (Exception e)
                {
                    Response.StatusCode = 400;
                    return new {Error = "Список пуст"};
                }
            }
        }

        [HttpGet("count")]
        public async Task<object> GetCount()
        {
            using (var db = new ApplicationContext())
            {
                var result = await db.Persons.CountAsync();
                return new {Value = result};
            }
        }
    }
}