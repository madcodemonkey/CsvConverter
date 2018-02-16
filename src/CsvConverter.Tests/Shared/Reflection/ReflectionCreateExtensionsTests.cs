using System;
using CsvConverter.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Tests.Shared.Reflection
{
    [TestClass]
    public class ReflectionCreateExtensionsTests
    {
        [TestMethod]
        public void Type_HelpCreateAndCastToInterface_CanCreateClassAndCastToInterface()
        {
            // Act
            var stuff  = typeof(ReflectionCreateExtensionsTester).HelpCreateAndCastToInterface<IReflectionCreateExtensionsTester>();

            // Assert
            Assert.IsNotNull(stuff);
            Assert.AreEqual(45, stuff.Diameter);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Type_HelpCreateAndCastToInterface_IfTheClassDoesImplementTheInterfaceYouWillGetAnExcetion()
        {
            // Act
            var stuff = typeof(ReflectionCreateExtensionsTester2).HelpCreateAndCastToInterface<IReflectionCreateExtensionsTester>();

            // Assert
            Assert.Fail("Should have received an exception.");
        }

    }


    internal interface IReflectionCreateExtensionsTester
    {
        int Diameter { get; set; }
    }

    internal class ReflectionCreateExtensionsTester : IReflectionCreateExtensionsTester
    {
        public int Diameter { get; set; } = 45;
    }

    internal class ReflectionCreateExtensionsTester2
    {
        public int Age { get; set; } = 3;
    }
}
