using System.Reflection;
using CsvConverter.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Tests.Shared.Reflection
{
    [TestClass]
    public class ReflectionHelperTest
    {
        [TestMethod]
        public void CanFindPropertyInfoByName()
        {
            // Act
            PropertyInfo propInfo = ReflectionHelper.FindPropertyInfoByName<ReflectionHelperTester>(nameof(ReflectionHelperTester.Age));

            // Assert
            Assert.IsNotNull(propInfo, "Property not found!");
            Assert.AreEqual(nameof(ReflectionHelperTester.Age), propInfo.Name);
            Assert.AreEqual(typeof(int), propInfo.PropertyType);
        }
    }

    internal class ReflectionHelperTester
    {
        public int Age { get; set; }
    }
}
