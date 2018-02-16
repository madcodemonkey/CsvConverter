using System;
using System.Collections.Generic;
using CsvConverter.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Tests.Shared
{
    [TestClass]
    public class ReflectionAttributeExtensionsTest
    {
        [DataTestMethod]
        [DataRow(true, 2)]
        [DataRow(false, 1)]
        public void Type_HelpFindAllClassAttributes_CanFindAllAttributesOnClass(bool findInheritedOnBaseClass, int expectedCount)
        {
            // Arrange
            // Act
            List<ReflectExtBaseAttribute> items = typeof(ReflectExtSuperClass)
                .HelpFindAllClassAttributes<ReflectExtBaseAttribute>(findInheritedOnBaseClass);

            // Assert
            Assert.AreEqual(expectedCount, items.Count);
            Assert.IsTrue(items.Exists(w => w.MyBaseNumber == 34));
            if (expectedCount > 1)
               Assert.IsTrue(items.Exists(w => w.MyBaseNumber == 12));
        }


        [TestMethod]
        public void PropertyInfo_HelpFindAllAttributes_CanFindAllAttributesOnPropertyOnAClass()
        {
            // Arrange
            var propInfo = ReflectionHelper.FindPropertyInfoByName<ReflectExtBaseClass>(nameof(ReflectExtBaseClass.MyBaseString));

            // Act
            List<ReflectExtBaseAttribute> items = propInfo.HelpFindAllAttributes<ReflectExtBaseAttribute>(true);

            // Assert
            Assert.AreEqual(2, items.Count);
            Assert.IsTrue(items.Exists(w => w.MyBaseNumber == 78));
            Assert.IsTrue(items.Exists(w => w.MyBaseNumber == 124));
        }
                
        [TestMethod]
        public void PropertyInfo_HelpFindAllAttributes_CanFindAllAttributesOnPropertyInBaseClass()
        {
            // Arrange
            var propInfo = ReflectionHelper.FindPropertyInfoByName<ReflectExtSuperClass>(nameof(ReflectExtSuperClass.MyBaseString));

            // Act
            List<ReflectExtBaseAttribute> items = propInfo.HelpFindAllAttributes<ReflectExtBaseAttribute>(true);

            // Assert
            Assert.AreEqual(2, items.Count);
            Assert.IsTrue(items.Exists(w => w.MyBaseNumber == 78));
            Assert.IsTrue(items.Exists(w => w.MyBaseNumber == 124));
        }

        [TestMethod]
        public void PropertyInfo_HelpFindAttribute_CanFindFirstOrDefaultAttribute()
        {
            // Arrange
            var propInfo = ReflectionHelper.FindPropertyInfoByName<ReflectExtBaseClass>(nameof(ReflectExtBaseClass.MyBaseString));

            // Act
            ReflectExtBaseAttribute item = propInfo.HelpFindAttribute<ReflectExtBaseAttribute>(true);

            // Assert
            Assert.IsNotNull(item);
            Assert.AreEqual(78, item.MyBaseNumber);
        }    
    }


    [ReflectExtBase(MyBaseNumber = 12)]
    internal class ReflectExtBaseClass
    {
        [ReflectExtBase(MyBaseNumber = 78)]
        [ReflectExtSuper(MyBaseNumber = 124, MyNumber = 23212)]
        public virtual string MyBaseString { get; set; }
    }


    [ReflectExtSuper(MyBaseNumber = 34, MyNumber = 56)]
    internal class ReflectExtSuperClass : ReflectExtBaseClass
    {
        [ReflectExtSuper(MyBaseNumber = 90, MyNumber = 100)]
        public string MyString { get; set; }
    }
    


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    internal class ReflectExtBaseAttribute : Attribute
    {
        public int MyBaseNumber { get; set; }
    }
        
    internal class ReflectExtSuperAttribute : ReflectExtBaseAttribute
    {
        public int MyNumber { get; set; }
    }
}
