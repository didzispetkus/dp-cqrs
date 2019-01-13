using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DP.CQRS.Tests
{
    [TestClass]
    public partial class CommandDispatcherTests
    {
        private static ICommandDispatcher CreateCommandDispatcher(IServiceProvider serviceProvider) => new CommandDispatcher(serviceProvider);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullServiceProvider_ThrowsArgumentNullException()
        {
            CreateCommandDispatcher(null);
        }

        [TestMethod]
        public void Dispatch_ValidCommand_CommandHandled()
        {
            //Arrange
            var commandHandled = false;

            var commandHandlerMock = new Mock<ICommandHandler<TestCommand>>();
            commandHandlerMock.Setup(x => x.Handle(It.IsAny<TestCommand>())).Callback(() => commandHandled = true);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(ICommandHandler<TestCommand>))).Returns(commandHandlerMock.Object);

            var dispatcher = CreateCommandDispatcher(serviceProviderMock.Object);
            //Act
            dispatcher.Dispatch(new TestCommand());

            //Assert
            Assert.IsTrue(commandHandled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Dispatch_NullCommand_ThrowsArgumentNullException()
        {
            //Arrange
            var dispatcher = CreateCommandDispatcher(new Mock<IServiceProvider>().Object);
            //Act
            dispatcher.Dispatch((TestCommand)null);
        }

        [TestMethod]
        public void Dispatch_NonRegisteredHandler_ThrowsCommandHandlerNotFoundException()
        {
            //Arrange
            var expectedHandlerType = typeof(ICommandHandler<TestCommand>);
            bool exceptionThrown = false;

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(expectedHandlerType)).Returns(null);

            var dispatcher = CreateCommandDispatcher(serviceProviderMock.Object);

            //Act
            try
            {
                dispatcher.Dispatch(new TestCommand());
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
