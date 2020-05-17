using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using api.Modelos;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkshopController : ControllerBase
    {
        protected readonly ApiContext db;

        public WorkshopController(ApiContext context)
        {
            db = context;
            db.Seed();
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Workshop>> Get()
        {
            var lista = db.Workshops.ToList();
            return lista;
            // return new string[] { "value1", "value2" };
        }
    }
}