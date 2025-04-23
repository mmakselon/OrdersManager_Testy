
using FluentAssertions;
using Moq;
using NLog;
using NUnit.Framework;
using OrdersManager.Controllers;
using OrdersManager.Core.Models.Domain;
using OrdersManager.Core.Services;
using OrdersManager.UnitTests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace OrderManager.UnitTests.Controllers
{
    internal class OrderControllerTests
    {
        private Mock<ILogger> _mockLogger;
        private Mock<IOrderService> _mockOrderService;
        private OrderController _orderController;
        private string _userId;
        private Product _product;

        private void Init()
        {
            _mockLogger = new Mock<ILogger>();
            _mockOrderService = new Mock<IOrderService>();
            _orderController = new OrderController(_mockOrderService.Object, _mockLogger.Object);

            _userId = "1";
            _orderController.MockCurrentUser(_userId, "1@2.pl");

            _product = new Product { OrderId = 1 };
        }

        [Test]
        public void AddProduct_WhenProductIsNull_ShouldReturnBadRequest()
        {
            Init();

            var result = _orderController.AddProduct(null);
            result.Should().BeOfType<BadRequestResult>();

        }

        [Test]
        public void AddProduct_WhenOrderIsLowerThan1_ShouldReturnBadRequest()
        {
            Init();
            _product.OrderId = 0;

            var result = _orderController.AddProduct(_product);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public void AddProduct_WhenCalled_ShouldAddProduct()
        {
            Init();

            var result = _orderController.AddProduct(_product);

            _mockOrderService.Verify(x=>x.AddProduct(_userId, _product), Times.Once());
        }

        [Test]
        public void AddProduct_WhenCalled_ShouldReturnOkReasult()
        {
            Init();
        }

        [Test]
        public void AddProduct_WhenOrderServiceFails_ShouldLogError()
        {
            Init();
        }

        [Test]
        public void AddProduct_WhenOrderServiceFails_ShouldReturnBadRequest()
        {
            Init();
        }
    }
}
