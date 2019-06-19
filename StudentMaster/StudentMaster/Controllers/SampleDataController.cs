using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentMaster.Data;

namespace StudentMaster.Controllers
{
    [Route("api/[controller]")]

    public class SampleDataController : Controller
    {
        private ApplicationDbContext context;

        
        public SampleDataController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return context.Users.Select(u => u.UserName).ToArray();
        }

      
    }
}
