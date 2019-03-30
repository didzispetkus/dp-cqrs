using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DP.CQRS.Tests
{
    [TestClass]
    public class QueryDispatcherTests
    {
        private static IQueryDispatcher CreateQueryDispatcher(IServiceProvider provider) => new QueryDispatcher(provider);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullServiceProvider_ThrowsArgumentNullException()
        {
            CreateQueryDispatcher(null);
        }

        [TestMethod]
        public void Dispatch_ValidQuery_ReturnsValue()
        {
            //Arrange
            const string expectedValue = "This is a unit test";

            var handlerMock = new Mock<IQueryHandler<TestQuery, string>>();
            handlerMock
                .Setup(x => x.Handle(It.IsAny<TestQuery>()))
                .Returns(expectedValue);

            var serviceMock = new Mock<IServiceProvider>();
            serviceMock.Setup(x => x.GetService(It.IsAny<Type>())).Returns(handlerMock.Object);

            var dispatcher = CreateQueryDispatcher(serviceMock.Object);
            var query = new TestQuery();

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
            //Act
            dispatcher.Dispatch((TestQuery)null);
        }

        [TestMethod]
        public void Dispatch_NonRegisteredHandler_ThrowsQueryHandlerNotFoundException()
        {
            //Arrange
            var expectedHandlerType = typeof(IQueryHandler<TestQuery, string>);
            var mock = new Mock<IServiceProvider>();
            mock.Setup(x => x.GetService(expectedHandlerType)).Returns(null);

            var dispatcher = CreateQueryDispatcher(mock.Object);

            var exceptionThrown = false;
            //Act
            try
            {
                dispatcher.Dispatch(new TestQuery());
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
