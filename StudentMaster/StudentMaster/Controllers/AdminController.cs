using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentMaster.Services;
using StudentMaster.ViewModels;

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
            if (!await adminService.EditUser(model))
                return BadRequest(new { invalid = "User is not found" } );
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