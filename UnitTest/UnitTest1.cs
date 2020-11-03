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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class UnitTestUnitOfWork
    {
        private ServiceProvider serviceProvider { get; set; }
        //[TestMethod]
        //public async Task TestMethodFindByIdAsync()
        //{
        //    // 1) Arrange làm các công việc cài đặt hay chuẩn bị dữ liệu cần thiết;
        //    var options = new DbContextOptionsBuilder<DataContext>()
        //.UseInMemoryDatabase(databaseName: "Products Test")
        //.Options;
        //    var roles = new List<Role>{
        //     new Role {ID = 1, Name = "Admin"},
        //     new Role {ID = 2, Name = "Supervisor"},
        //     new Role {ID = 3, Name = "Staff"},
        //     new Role {ID = 4, Name = "Worker"},
        //     new Role {ID = 5, Name = "Admin Costing"},
        //     new Role {ID = 6, Name = "Staff in mixing room"},

        //    }.AsQueryable();


        //    var mockRepo = new Mock<IUnitOfWork<DataContext>>();
        //    mockRepo
        //            .Setup(uow => uow.GetRepository<Role>().FindAll())
        //            .Returns(roles);
        //    // 2) Act là thực thi function hay method cần được test và lấy kết quả.

        //    var sut = new RoleService(mockRepo.Object);
        //    var res = await sut.GetAllDto();
        //    // 3) Assert là verify kết quả trả (actual result) ra có đúng như mong muốn không (expected result)
        //    Assert.IsNotNull(res);
        //}
        [TestMethod]
        public void Get_RoleObjectPassed_ProperMethodCalled()
        {
            // Arrange

            var testObject = new Role();

            var context = new Mock<DbContext>();
            var dbSetMock = new Mock<DbSet<Role>>();

            context.Setup(x => x.Set<Role>()).Returns(dbSetMock.Object);
            dbSetMock.Setup(x => x.Find(It.IsAny<int>())).Returns(testObject);

            // Act
            var repository = new ECRepository<Role>(context.Object);
            repository.FindById(1);

            // Assert
            context.Verify(x => x.Set<Role>());
            dbSetMock.Verify(x => x.Find(It.IsAny<int>()));
        }
        [TestMethod]
        public void GetAll_RoleObjectPassed_ProperMethodCalled()
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

            var dbSetMock = new Mock<DbSet<Role>>();
            dbSetMock.As<IQueryable<Role>>().Setup(x => x.Provider).Returns(testList.Provider);
            dbSetMock.As<IQueryable<Role>>().Setup(x => x.Expression).Returns(testList.Expression);
            dbSetMock.As<IQueryable<Role>>().Setup(x => x.ElementType).Returns(testList.ElementType);
            dbSetMock.As<IQueryable<Role>>().Setup(x => x.GetEnumerator()).Returns(testList.GetEnumerator());

            var context = new Mock<DbContext>();
            context.Setup(x => x.Set<Role>()).Returns(dbSetMock.Object);

            // Act
            var repository = new ECRepository<Role>(context.Object);
            var result = repository.GetAll();

            // Assert
            Assert.Equals(testList, result.ToList());
        }
        [TestMethod]
        public void Find_TestClassObjectPassed_ProperMethodCalled()
        {
            var testObject = new Role() { ID = 1, Name = "Admin" };
            var testList = new List<Role>() { testObject };

            var dbSetMock = new Mock<DbSet<Role>>();
            dbSetMock.As<IQueryable<Role>>().Setup(x => x.Provider).Returns(testList.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Role>>().Setup(x => x.Expression).Returns(testList.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Role>>().Setup(x => x.ElementType).Returns(testList.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Role>>().Setup(x => x.GetEnumerator()).Returns(testList.AsQueryable().GetEnumerator());

            var context = new Mock<DbContext>();
            context.Setup(x => x.Set<Role>()).Returns(dbSetMock.Object);

            var repository = new ECRepository<Role>(context.Object);

            var result = repository.FindAll(x => x.ID == 1);

            Assert.Equals(testList, result.ToList());
        }
        [TestMethod]
        public async Task Setup()
        {
            // arrange
            var testList = new List<RoleDto>{
             new RoleDto {ID = 1, Name = "Admin"},
             new RoleDto {ID = 2, Name = "Supervisor"},
             new RoleDto {ID = 3, Name = "Staff"},
             new RoleDto {ID = 4, Name = "Worker"},
             new RoleDto {ID = 5, Name = "Admin Costing"},
             new RoleDto {ID = 6, Name = "Staff in mixing room"},

            }.ToList();

            var mockRoleRepository = new Mock<IRoleService>();
            mockRoleRepository.Setup( x =>  x.GetAllAsync()).Returns(Task.FromResult(testList));
            // Act
            var service = new RoleController(mockRoleRepository.Object);
            IActionResult list = await service.GetAll();
            var okResult = list as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(200, okResult.StatusCode);

        }
        [TestMethod]
        public void Get_Role_By_Id()
        {
            var role = new Role { ID = 3 };
            var mockRoleRepository = new Mock<IRoleRepository>();

            mockRoleRepository.Setup(x => x.FindById(3)).Returns(role); //return Role
            mockRoleRepository.Object.FindById(3).Equals(role); //Assert expected value equal to actual value
            mockRoleRepository.Verify(x => x.FindById(role.ID), Times.Once()); //Assert that the Get method was called once
        }
        [TestMethod]
        public void Delete_Role_ById()
        {
            var role = new Role { ID = 3 };

            var mockRoleRepository = new Mock<IRoleRepository>();
            mockRoleRepository.Setup(x => x.Remove(role));
            mockRoleRepository.Object.Remove(role); //no return as it's a void method

            mockRoleRepository.Setup(x => x.FindById(3)).Returns(role);
            mockRoleRepository.Object.FindById(3).Equals(null); //Assert expected value to be null
            mockRoleRepository.Verify(x => x.Remove(role), Times.Once); //Assert that the Delete method was called once

        }

        [TestMethod]
        public void Create_Role_Calls_RoleRepositoryAdd()
        {
            var role = new Role { ID = 3, Name = "Admin" };
            var mockRoleRepository = new Mock<IRoleRepository>();
            mockRoleRepository.Setup(x => x.Add(role)); //no return as it's a void method
            mockRoleRepository.Object.Add(role);

            mockRoleRepository.Setup(x => x.FindById(3)).Returns(role);
            mockRoleRepository.Object.FindById(3).Equals(role);
            mockRoleRepository.Verify(x => x.Add(role), Times.Once()); //Assert that the Add method was called once

        }
        [TestMethod]
        public void Update_Role_Calls_RoleRepositoryAdd()
        {
            var role = new Role { ID = 3, Name = "Admin" };
            var mockRoleRepository = new Mock<IRoleRepository>();
            mockRoleRepository.Setup(x => x.Add(role)); //no return as it's a void method
            mockRoleRepository.Object.Add(role);

            role.Name = "User";
            mockRoleRepository.Setup(x => x.Update(role)); //no return as it's a void method
            mockRoleRepository.Object.Update(role);

            mockRoleRepository.Verify(x=> x.FindById(3).Name.Equals("User"));
           // mockRoleRepository.Verify(x => x.Update(role), Times.Once()); //Assert that the Add method was called once

        }
    }
}
