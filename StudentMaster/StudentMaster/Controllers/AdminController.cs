using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentMaster.Models;
using StudentMaster.Services;
using StudentMaster.ViewModels;
using StudentMaster.ViewModels.AdminViewModels;

namespace StudentMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private AdminService adminService;

        public AdminController(AdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpPost]
        [Route("edituser")]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!!await adminService.EditUser(model))
                return BadRequest();
            return Ok();
        } 

        [HttpGet]
        [Route("all")]
        public async Task<UserListViewModel> Index(string name, int page = 1,
            SortState sortOrder = SortState.FirstNameAsc)
        {
            return await adminService.GetUsers(name, page, sortOrder);
        }
    }
}