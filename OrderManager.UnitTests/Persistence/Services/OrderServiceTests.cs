using FluentAssertions;
using Moq;
using NUnit.Framework;
using OrdersManager.Core;
using OrdersManager.Core.Models.Domain;
using OrdersManager.Core.Services;
using OrdersManager.Persistence.Services;
using System;

namespace OrdersManager.UnitTests.Persistence.Services
{
    class OrderServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IDiscountService> _mockDiscountService;
        private OrderService _orderService;
        private string _userId;
        private Product _product;
        private Order _order;

        private void Init()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockDiscountService = new Mock<IDiscountService>();
            _orderService = new OrderService(_mockUnitOfWork.Object, _mockDiscountService.Object);

            _userId = "1";
            _product = new Product { Id = 1, OrderId = 1 };
            _order = new Order();
            _mockUnitOfWork
                .Setup(x => x.Order.GetOrderWithProducts(_product.OrderId, _userId))
                .Returns(_order);
        }

        [Test]
        public void AddProduct_WhenOrderDoesntExists_ShouldThrowAnException()
        {
            Init();
            var basUserId = "2";

            Action action = () => _orderService.AddProduct(basUserId, _product);

            action.Should().ThrowExactly<Exception>().WithMessage("*Order doesn't exists.*");
        }

        [Test]
        public void AddProduct_WhenCalled_ShouldAddProductToDb()
        {
            Init();

            _orderService.AddProduct(_userId, _product);

            _mockUnitOfWork.Verify(x => x.Order.AddProduct(_product), Times.Once);
            _mockUnitOfWork.Verify(x => x.Complete(), Times.Once);
        }

        [Test]
        public void AddProduct_WhenCalled_ShouldUpdateTotalOrderPriceInDb()
        {
            Init();

            _orderService.AddProduct(_userId, _product);

            _mockUnitOfWork.Verify(x => 
                x.Order.UpdateTotalPrice(_product.OrderId, _userId, 0), 
                Times.Once);
            _mockUnitOfWork.Verify(x => x.Complete(), Times.Once);
        }

        [Test]
        public void AddProduct_WhenCalled_ShouldAplyDiscount()
        {
            Init();
            var discount = 1;
            _mockDiscountService.Setup(x => x.GetDiscount(_product.OrderId, _userId)).Returns(discount);

            _orderService.AddProduct(_userId, _product);

            _mockUnitOfWork.Verify(x => 
                x.Order.UpdateTotalPrice(_product.OrderId, _userId, discount),
                Times.Once);

        }

        [Test]
        public void AddProduct_WhenUpdateTotalPriceFails_ShouldNotSaveDataInDatabase()
        {
            Init();
            _mockUnitOfWork.Setup(x => 
                x.Order.UpdateTotalPrice(_product.OrderId, _userId, It.IsAny<decimal>()))
                .Throws(new Exception());

            Action action = () => _orderService.AddProduct(_userId, _product);

            action.Should().ThrowExactly<Exception>();
            _mockUnitOfWork.Verify(x => x.Complete(), Times.Never);
        }


    }
}
