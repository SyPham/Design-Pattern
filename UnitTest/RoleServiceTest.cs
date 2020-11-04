using AutoMapper;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Newtonsoft.Json;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;
using CollectionAssert = NUnit.Framework.CollectionAssert;

namespace UnitTest
{
    [TestFixture]
    public class RoleServiceTest
    {
        private IRoleService _roleService;
        private IECRepository<Role> _roleRepository;
        private IUnitOfWork _unitOfWork;
        private List<Role> _roles;
        private DataContext dataContext;
        public RoleServiceTest()
        {
        }
        /// <summary>
        /// Setup dummy roles data
        /// </summary>
        /// <returns></returns>
        private static List<Role> SetUpRoles()
        {
            var testList = new List<Role>{
             new Role {ID = 1, Name = "Admin"},
             new Role {ID = 2, Name = "Supervisor"},
             new Role {ID = 3, Name = "Staff"},
             new Role {ID = 4, Name = "Worker"},
             new Role {ID = 5, Name = "Admin Costing"},
             new Role {ID = 6, Name = "Staff in mixing room"},

            };
        
            return testList;

        }

        /// <summary>
        /// Initial setup for tests
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _roles = SetUpRoles();
            dataContext = new Mock<DataContext>().Object;
            var unitOfWork = new Mock<IUnitOfWork>();
            _roleRepository = SetUpRoleRepository();
            unitOfWork.SetupGet(s => s.GetRepository<Role>()).Returns(_roleRepository);
            _unitOfWork = unitOfWork.Object;
            _roleService = new RoleService(_unitOfWork);
        }
        /// <summary>
        /// Service should return all the roles
        /// </summary>
        [Test]
        public async Task GetAllRolesTest()
        {
            var roles = await  _roleService.GetAllDto();
            if (roles != null)
            {
                var roleList =
                    roles.Select(
                        roleEntity =>
                        new Role { ID = roleEntity.ID, Name  = roleEntity.Name }).
                        ToList();
                CollectionAssert.AreEqual(
                    roles,
                    _roles);
            }
        }
        [Test]
        public async Task GetAllAsync()
        {
            // Arrange
            var testList = new List<Role>{
             new Role {ID = 1, Name = "Admin"},
             new Role {ID = 2, Name = "Supervisor"},
             new Role {ID = 3, Name = "Staff"},
             new Role {ID = 4, Name = "Worker"},
             new Role {ID = 5, Name = "Admin Costing"},
             new Role {ID = 6, Name = "Staff in mixing room"},

            }.AsQueryable();
            Mock<IRoleRepository> mockRoleRepo = new Mock<IRoleRepository>();
            mockRoleRepo.Setup(x => x.FindAll()).Returns(testList);

            // Act
            IRoleService service = new RoleService(mockRoleRepo.Object);
            var result = await service.GetAllDto();

            // Assert
            Assert.AreEqual(testList.Count(), result.Count);
        }
        /// <summary>
        /// Setup dummy repository
        /// </summary>
        /// <returns></returns>
        private ECRepository<Role> SetUpRoleRepository()
        {
            // Initialise repository
            var mockRepo = new Mock<ECRepository<Role>>(MockBehavior.Default, dataContext);

            // Setup mocking behavior
            mockRepo.Setup(p => p.FindAll()).Returns(_roles.AsQueryable());

            mockRepo.Setup(p => p.FindById(It.IsAny<int>()))
                .Returns(new Func<int, Role>(
                             id => _roles.AsQueryable().First(p => p.ID.Equals(id))));

            mockRepo.Setup(p => p.Add((It.IsAny<Role>())))
                .Callback(new Action<Role>(newRole =>
                {
                    dynamic maxRoleID = _roles.Last().ID;
                    dynamic nextRoleID = maxRoleID + 1;
                    newRole.ID = nextRoleID;
                    _roles.Add(newRole);
                }));

            mockRepo.Setup(p => p.Update(It.IsAny<Role>()))
                .Callback(new Action<Role>(prod =>
                {
                    var oldRole = _roles.Find(a => a.ID == prod.ID);
                    oldRole = prod;
                }));

            mockRepo.Setup(p => p.FindById(It.IsAny<int>()))
                .Callback(new Action<Role>(prod =>
                {
                    var roleToRemove =
                        _roles.Find(a => a.ID == prod.ID);

                    if (roleToRemove != null)
                        _roles.Remove(roleToRemove);
                }));

            // Return mock implementation object
            return mockRepo.Object;
        }

    }
}
