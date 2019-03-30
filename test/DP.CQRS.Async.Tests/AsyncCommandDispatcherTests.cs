using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DP.CQRS.Async.Tests
{
    [TestClass]
    public class AsyncCommandDispatcherTests
    {
        private static IAsyncCommandDispatcher CreateCommandDispatcher(IServiceProvider serviceProvider) => new AsyncCommandDispatcher(serviceProvider);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullServiceProvider_ThrowsArgumentNullException()
        {
            CreateCommandDispatcher(null);
        }

        [TestMethod]
        public async Task DispatchAsync_ValidCommand_CommandHandled()
        {
            //Arrange
            var commandHandled = false;

            var commandHandlerMock = new Mock<IAsyncCommandHandler<AsyncTestCommand>>();
            commandHandlerMock.Setup(
                    x => x.HandleAsync(It.IsAny<AsyncTestCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Callback(() => commandHandled = true);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(IAsyncCommandHandler<AsyncTestCommand>))).Returns(commandHandlerMock.Object);

            var dispatcher = CreateCommandDispatcher(serviceProviderMock.Object);
            var source = new CancellationTokenSource();
            //Act
            await dispatcher.DispatchAsync(new AsyncTestCommand(), source.Token);

            //Assert
            Assert.IsTrue(commandHandled);
            Assert.IsFalse(source.Token.IsCancellationRequested);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task DispatchAsync_NullCommand_ThrowsArgumentNullException()
        {
            //Arrange
            var dispatcher = CreateCommandDispatcher(new Mock<IServiceProvider>().Object);
            //Act
            await dispatcher.DispatchAsync((AsyncTestCommand)null);
        }

        [TestMethod]
        public async Task DispatchAsync_NonRegisteredHandler_ThrowsCommandHandlerNotFoundException()
        {
            //Arrange
            var expectedHandlerType = typeof(IAsyncCommandHandler<AsyncTestCommand>);
            var exceptionThrown = false;

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(expectedHandlerType)).Returns(null);

            var dispatcher = CreateCommandDispatcher(serviceProviderMock.Object);

            //Act
            try
            {
                await dispatcher.DispatchAsync(new AsyncTestCommand());
            }
            catch (CommandHandlerNotFoundException ex)
            {
                exceptionThrown = true;
                Assert.AreEqual(expectedHandlerType, ex.HandlerType);
            }
            //Assert
            Assert.IsTrue(exceptionThrown);
        }

    }
}
