using DMR_API._Repositories.Interface;
using DMR_API._Repositories.Repositories;
using DMR_API._Services.Interface;
using DMR_API._Services.Services;
using DMR_API.Controllers;
using DMR_API.Data;
using DMR_API.DTO;
using DMR_API.Models;
using EC_API.Data;
using log4net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace UnitTest
{
    [TestClass]
    public class UnitTestRole
    {
        [Test]
        public async Task Setup()
        {

           var _roleRepository = new Mock<IECRepository<Role>>();
           var _unitOfWork = new Mock<IUnitOfWork<DataContext>>();

            // Arrange
            var fakeRoles = new List<Role>
                {
                    new Role {ID = 1, Name = "Role One"}
                }.AsQueryable();

            _unitOfWork.Setup(x => x.GetRepository<Role>()).Returns(_roleRepository.Object);
            _unitOfWork.Setup(x => x.GetRepository<Role>().GetAll()).Returns(fakeRoles);

            // Act
           var roleService = new RoleService(_unitOfWork.Object);
            var roles = await roleService.GetAllDto();

            // Assert
            Assert.NotNull(roles);
        }
    }
}
