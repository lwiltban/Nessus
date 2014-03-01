using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nessus.Models;

namespace Nessus.Tests.Controllers
{
    [TestClass]
    public class AuthorizationRepositoryTest
    {

        [TestMethod]
        public void Get()
        {
            // Arrange
            AuthorizationRepository repository = new AuthorizationRepository();
            string basicToken = "test:password";

            // Act
            string nessusToken = repository.GetNessusToken(basicToken);

            // Assert
            Assert.IsNotNull(nessusToken);
          
        }

        [TestMethod]
        public void GetAgain()
        {
            // Arrange
            AuthorizationRepository repository = new AuthorizationRepository();
            string basicToken = "test:password";

            // Act
            string nessusToken = repository.GetNessusToken(basicToken);

            // Assert
            Assert.IsNotNull(nessusToken);

            // Act
            string nessusToken1 = repository.GetNessusToken(basicToken);

            // Assert
            Assert.IsNotNull(nessusToken1);

            Assert.AreNotEqual(nessusToken, nessusToken1);
        }

        [TestMethod]
        public void IsValid()
        {
            // Arrange
            AuthorizationRepository repository = new AuthorizationRepository();
            string basicToken = "test:password";

            // Act
            string nessusToken = repository.GetNessusToken(basicToken);

            // Assert
            Assert.IsNotNull(nessusToken);

            Assert.IsTrue(repository.IsValidNessusToken(nessusToken));
        }

        [TestMethod]
        public void Release()
        {
            // Arrange
            AuthorizationRepository repository = new AuthorizationRepository();
            string basicToken = "test:password";

            // Act
            string nessusToken = repository.GetNessusToken(basicToken);

            // Assert
            Assert.IsNotNull(nessusToken);

            Assert.IsTrue(repository.IsValidNessusToken(nessusToken));

            Assert.IsTrue(repository.ReleaseNessusToken(nessusToken));

            Assert.IsFalse(repository.IsValidNessusToken(nessusToken));
        }

    }
}
