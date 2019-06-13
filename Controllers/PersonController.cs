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
        private ApplicationContext Db;

        public PersonController(ApplicationContext db)
        {
            Db = db;
        }

        [HttpGet("list")]
        public async Task<IEnumerable<Person>> GetList() => await Db.Persons.ToArrayAsync();

        [HttpGet("view")]
        public async Task<Person> GetOne(int id) => await Db.Persons.Where(p => p.Id == id).FirstAsync();

        [HttpPost("save")]
        public async Task<object> Save([FromBody] Person newModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = ModelState.ToDictionary(p => p.Key, p => p.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                return new {Errors = errors};
            }
            
            if (newModel.Id <= 0)
            {
                Db.Persons.Add(newModel);
                Response.StatusCode = 201;
            }
            else
            {
                Db.Update(newModel);
            }

            await Db.SaveChangesAsync();
            return new {Id = newModel.Id};
        }

        [HttpDelete("delete")]
        public async Task<object> Delete(int id)
        {
            try
            {
                Db.Persons.Remove(new Person() {Id = id});
                await Db.SaveChangesAsync();
                return null;
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return new {Error = e.Message};
            }
        }

        [HttpGet("averageAge")]
        public async Task<object> GetAverageAge()
        {
            try
            {
                var result = (int)Math.Round(await Db.Persons.Select(p => p.Age).AverageAsync());
                return new {Value = result};
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return new {Error = "Список пуст"};
            }
        }

        [HttpGet("count")]
        public async Task<object> GetCount() => new {Value = await Db.Persons.CountAsync()};
    }
}