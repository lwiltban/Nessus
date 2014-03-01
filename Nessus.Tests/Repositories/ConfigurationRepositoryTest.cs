using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nessus.Models;
using System.Collections.Generic;

namespace Nessus.Tests.Controllers
{
    [TestClass]
    public class ConfigurationRepositoryTest
    {
        [TestMethod]
        public void GetPaging()
        {
            // Arrange
            ConfigurationRepository repository = new ConfigurationRepository();

            // Act
            Configurations result = repository.GetAll(100000, 2);

            // Assert
            Assert.IsNotNull(result);

            Assert.IsNotNull(result.configurations);

            int count = 0;
            foreach (Configuration c in result.configurations)
            {
                count++;
            }
            Assert.AreEqual(count, 0);
        }

        [TestMethod]
        public void GetAll()
        {
            // Arrange
            ConfigurationRepository repository = new ConfigurationRepository();

            // Act
            Configurations result = repository.GetAll(100000, 0);

            // Assert
            Assert.IsNotNull(result);

            Assert.IsNotNull(result.configurations);
            
            int count = 0;
            foreach(Configuration c in result.configurations)
            {
                count++;
            }
            Assert.AreEqual(count, 2);
        }

        [TestMethod]
        public void Get()
        {
            // Arrange
            ConfigurationRepository repository = new ConfigurationRepository();

            // Act
            Configuration result = repository.Get("host1");

            // Assert
            Assert.IsNotNull(result);

            Assert.IsNotNull(result.hostname);

            Assert.AreEqual(result.name, "host1");
        }

        [TestMethod]
        public void GetBySort()
        {
            // Arrange 
            ConfigurationRepository repository = new ConfigurationRepository();

            // Act
            Configurations result = repository.GetBySort("name");

            // Assert
            Assert.IsNotNull(result);

            Assert.IsNotNull(result.configurations);

        }

        [TestMethod]
        public void Add()
        {
            // Arrange 
            ConfigurationRepository repository = new ConfigurationRepository();

            // Act
            Configuration add = new Configuration();
            add.name = "add";
            add.hostname = "test.com";
            add.port = 1;
            add.username = "test";

            Configuration result = repository.Add(add);

            // Assert
            Assert.IsNotNull(result);

            Assert.IsNotNull(result.name);

            Assert.AreEqual(result.name, add.name);

            Assert.AreEqual(result.hostname, add.hostname);
        }

        [TestMethod]
        public void Remove()
        {
            // Arrange 
            ConfigurationRepository repository = new ConfigurationRepository();

            // Act
            bool result = repository.Remove("host1");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Update()
        {
            // Arrange 
            ConfigurationRepository repository = new ConfigurationRepository();

            // Act
            Configuration update = new Configuration();
            update.name = "host1";
            update.hostname = "test.com";
            update.port = 1;
            update.username = "test";

            Configuration result = repository.Update(update);

            // Assert
            Assert.IsNotNull(result);

            Assert.IsNotNull(result.name);

            Assert.AreEqual(result.name, update.name);

            Assert.AreEqual(result.hostname, update.hostname);
        }

    }
}
