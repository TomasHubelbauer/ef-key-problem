using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ef_key_problem.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private EfKeyContext _context;

        public ValuesController(EfKeyContext context) {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public string Get()
        {
            return string.Join(Environment.NewLine, _context.Todos.Select(todo => $"{todo.Id} {todo.Name}"));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
