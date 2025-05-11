using FluentAssertions;
using Moq;
using NUnit.Framework;
using OrdersManager.Core;
using OrdersManager.Core.Models.Domain;
using OrdersManager.Persistence.Repositories;
using OrdersManager.UnitTests.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OrdersManager.UnitTests.Persistence.Repositories
{
    class OrderRepositoryTests
    {
        private Mock<IApplicationDbContext> _mockContext;
        private Mock<DbSet<Product>> _mockProducts;
        private OrderRepository _orderRepository;
        private Product _product;

        private void Init()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _mockProducts = new Mock<DbSet<Product>>();
            _product = new Product();

            _mockContext.Setup(x => x.Products).Returns(_mockProducts.Object);

            _orderRepository = new OrderRepository(_mockContext.Object);
        }

        [Test]
        public void AddProduct_WhenCalled_ShouldAddProductToDb()
        {
            Init();

            _orderRepository.AddProduct(_product);

            _mockContext.Verify(x => x.Products.Add(_product), Times.Once);
        }

    }
}
