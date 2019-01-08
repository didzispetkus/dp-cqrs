using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DP.CQRS.Tests
{
    [TestClass]
    public partial class QueryDispatcherTests
    {
        private IQueryDispatcher CreateQueryDispatcher(IServiceProvider provider) => new QueryDispatcher(provider);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullServiceProvider_ThrowsArgumentNullException()
        {
            IServiceProvider serviceProvider = null;
            var dispatcher = CreateQueryDispatcher(serviceProvider);
        }

        [TestMethod]
        public void Dispatch_ValidQuery_ReturnsValue()
        {
            //Arrange
            string expectedValue = "This is a unit test";

            var handlerMock = new Mock<IQueryHandler<TestQuery, string>>();
            handlerMock
                .Setup(x => x.Handle(It.IsAny<TestQuery>()))
                .Returns(expectedValue);

            var serviceMock = new Mock<IServiceProvider>();
            serviceMock.Setup(x => x.GetService(It.IsAny<Type>())).Returns(handlerMock.Object);

            var dispatcher = CreateQueryDispatcher(serviceMock.Object);
            TestQuery query = new TestQuery();

            //Act
            var actualValue = dispatcher.Dispatch(query);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Dispatch_NullQuery_ThrowsArgumentNullException()
        {
            //Arrange
            var dispatcher = CreateQueryDispatcher(new Mock<IServiceProvider>().Object);
            TestQuery query = null;
            //Act
            var result = dispatcher.Dispatch(query);
        }

        [TestMethod]
        public void Dispatch_NonRegisteredHandler_ThrowsQueryHandlerNotFoundException()
        {
            //Arrange
            var expectedHandlerType = typeof(IQueryHandler<TestQuery, string>);
            var mock = new Mock<IServiceProvider>();
            mock.Setup(x => x.GetService(expectedHandlerType)).Returns(null);

            var dispatcher = CreateQueryDispatcher(mock.Object);

            bool exceptionThrown = false;
            //Act
            try
            {
                var result = dispatcher.Dispatch(new TestQuery());
            }
            catch (QueryHandlerNotFoundException ex)
            {
                exceptionThrown = true;
                Assert.AreEqual(expectedHandlerType, ex.HandlerType);
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }
    }
}
