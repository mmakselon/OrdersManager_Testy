
using Moq;
using NLog;
using NUnit.Framework;
using OrdersManager.Controllers;
using OrdersManager.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.UnitTests.Controllers
{
    internal class OrderControllerTests
    {
        private Mock<ILogger> _mockLogger;
        private Mock<IOrderService> _mockOrderService;
        private OrderController _orderController;

        private void Init()
        {
            _mockLogger = new Mock<ILogger>();
            _mockOrderService = new Mock<IOrderService>();
            _orderController = new OrderController(_mockOrderService.Object, _mockLogger.Object);
        }

        [Test]
        public void AddProduct_WhenProductIsNull_ShouldReturnBadRequest()
        {
            Init();

        }

        [Test]
        public void AddProduct_WhenOrderIsLowerThan1_ShouldReturnBadRequest()
        {
            Init();

        }

        [Test]
        public void AddProduct_WhenCalled_ShouldAddProduct()
        {
            Init();
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
