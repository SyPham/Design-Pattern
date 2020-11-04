﻿using DMR_API._Services.Interface;
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
        public RoleController(IRoleService RoleService)
        {
            _RoleService = RoleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var role = await _RoleService.GetAllAsync();
            return Ok(role);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromQuery]RoleDto roleDto)
        {
            var role = await _RoleService.Add(roleDto);
            return Ok(role);
        }
    }
}
