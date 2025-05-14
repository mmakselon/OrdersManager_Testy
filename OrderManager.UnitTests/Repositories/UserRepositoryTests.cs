using FluentAssertions;
using Moq;
using NUnit.Framework;
using OrdersManager.Core;
using OrdersManager.Models;
using OrdersManager.Persistence.Repositories;
using OrdersManager.UnitTests.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OrdersManager.UnitTests.Persistence.Repositories
{
    class UserRepositoryTests
    {
        private Mock<IApplicationDbContext> _mockContext;
        private UserRepository _userRepository;
        private Mock<DbSet<ApplicationUser>> _mockUsers;
        private List<ApplicationUser> _users;

        private void Init()
        {
            _users = new List<ApplicationUser>
                {
                    new ApplicationUser { Id = "1" },
                    new ApplicationUser { Id = "2" },
                    new ApplicationUser { Id = "3" }
                }.AsQueryable();

            _mockUsers = new Mock<DbSet<ApplicationUser>>();
            _mockUsers.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(_users.Provider);
            _mockUsers.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(_users.Expression);
            _mockUsers.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(_users.ElementType);
            _mockUsers.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(_users.GetEnumerator);

            _mockContext = new Mock<IApplicationDbContext>();
            _mockContext.Setup(x => x.Users).Returns(_mockUsers.Object);

            _userRepository = new UserRepository(_mockContext.Object);
        }

        [Test]
        public void GetUser_WhenUserDoesntExists_ShouldThrowNullReferenceException()
        {
            Init();
            var basdUserId = "4";

            Action action = () => _userRepository.GetUser(basdUserId);

            action.Should()
                .ThrowExactly<NullReferenceException>()
                .WithMessage("*User doesn't exists.*");
        }

        [Test]
        public void GetUser_WhenCalled_ShouldReturnUser()
        {
            Init();

            var result = _userRepository.GetUser(_users.First().Id);

            result.Should().Be(_users.First());
        }
    }
}
