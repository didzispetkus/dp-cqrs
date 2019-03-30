using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DP.CQRS.Async.Tests
{
    [TestClass]
    public class AsyncQueryDispatcherTests
    {
        private static IAsyncQueryDispatcher CreateQueryDispatcher(IServiceProvider provider) => new AsyncQueryDispatcher(provider);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullServiceProvider_ThrowsArgumentNullException()
        {
            CreateQueryDispatcher(null);
        }

        [TestMethod]
        public async Task DispatchAsync_ValidQuery_ReturnsValue()
        {
            //Arrange
            const string expectedValue = "This is a unit test";

            var handlerMock = new Mock<IAsyncQueryHandler<AsyncTestQuery, string>>();
            handlerMock
                .Setup(x => x.HandleAsync(It.IsAny<AsyncTestQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(expectedValue));

            var serviceMock = new Mock<IServiceProvider>();
            serviceMock.Setup(x => x.GetService(It.IsAny<Type>())).Returns(handlerMock.Object);

            var dispatcher = CreateQueryDispatcher(serviceMock.Object);
            var query = new AsyncTestQuery();

            //Act
            var actualValue = await dispatcher.DispatchAsync(query);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task DispatchAsync_NullQuery_ThrowsArgumentNullException()
        {
            //Arrange
            var dispatcher = CreateQueryDispatcher(new Mock<IServiceProvider>().Object);
            //Act
            await dispatcher.DispatchAsync((AsyncTestQuery)null);
        }

        [TestMethod]
        public async Task DispatchAsync_NonRegisteredHandler_ThrowsQueryHandlerNotFoundException()
        {
            //Arrange
            var expectedHandlerType = typeof(IAsyncQueryHandler<AsyncTestQuery, string>);
            var mock = new Mock<IServiceProvider>();
            mock.Setup(x => x.GetService(expectedHandlerType)).Returns(null);

            var dispatcher = CreateQueryDispatcher(mock.Object);

            var exceptionThrown = false;
            //Act
            try
            {
                await dispatcher.DispatchAsync(new AsyncTestQuery());
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
