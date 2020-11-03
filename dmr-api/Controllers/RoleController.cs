using DMR_API._Services.Interface;
using DMR_API.DTO;
using DMR_API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMR_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _RoleService;
        private readonly ILogger<RoleController> _logger;
        public RoleController(IRoleService RoleService, ILogger<RoleController> logger)
        {
            _RoleService = RoleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var role = await _RoleService.GetAllAsync();
            return Ok(role);
        }
     
    }
}
