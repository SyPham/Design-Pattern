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

namespace UnitTest
{
    [TestClass]
    public class UnitTestUnitOfWork
    {
        private ServiceProvider serviceProvider { get; set; }
        [SetUp]
        public void SetUp()
        {
            var services = new ServiceCollection();
            services.AddDbContext<DataContext>(options => options.UseSqlServer("Server=10.4.0.9;Database=ECSTest;MultipleActiveResultSets=true;User Id=sa;Password=shc@1234"));
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IRepositoryFactory, UnitOfWork<DataContext>>();
            services.AddScoped<IUnitOfWork, UnitOfWork<DataContext>>();
            services.AddScoped<IUnitOfWork<DataContext>, UnitOfWork<DataContext>>();
            serviceProvider = services.BuildServiceProvider();
        }
        [TestMethod]
        public async Task TestMethodFindByIdAsync()
        {
            // 1) Arrange làm các công việc cài đặt hay chuẩn bị dữ liệu cần thiết;
            var options = new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase(databaseName: "Products Test")
        .Options;
            var roles = new List<Role>{
             new Role {ID = 1, Name = "Admin"},
             new Role {ID = 2, Name = "Supervisor"},
             new Role {ID = 3, Name = "Staff"},
             new Role {ID = 4, Name = "Worker"},
             new Role {ID = 5, Name = "Admin Costing"},
             new Role {ID = 6, Name = "Staff in mixing room"},

            }.AsQueryable();


            var mockRepo = new Mock<IUnitOfWork<DataContext>>();
            mockRepo
                    .Setup(uow => uow.GetRepository<Role>().FindAll())
                    .Returns(roles);
            // 2) Act là thực thi function hay method cần được test và lấy kết quả.

            var sut = new RoleService(mockRepo.Object);
            var res = await sut.GetAllDto();
            // 3) Assert là verify kết quả trả (actual result) ra có đúng như mong muốn không (expected result)
            NUnit.Framework.Assert.IsNotNull(res);
        }
    }
}
