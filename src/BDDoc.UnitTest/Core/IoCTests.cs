using System;
using BDDoc.Core;
using System.Diagnostics.CodeAnalysis;
using NUnit.Core;
using NUnit.Framework;

namespace BDDoc.UnitTest.Core
{
    [ExcludeFromCodeCoverage]
    public class IoCTests
    {
        //Nested Types

        interface ITestObject
        {

            string Param1 { get; }

            string Param2 { get; }
        }

        public class TestObject : ITestObject
        {
            //Constructors

            public TestObject() { }

            public TestObject(string param1)
            {
                Param1 = param1;
            }

            public TestObject(string param1, string param2)
            {
                Param1 = param1;
                Param2 = param2;
            }

            //Properties

            public string Param1 { get; private set; }
            
            public string Param2 { get; private set; }
        }

        interface ITestObject2 { }

        public class TestObject2 : ITestObject2 { }

        //Tests

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Mapping_SameTypeTwice_AnExceptionIsThrow()
        {
            IoC.Map<ITestObject2, ITestObject2>();
            IoC.Map<ITestObject2, ITestObject2>();
        }

        [Test]
        public void Mapping_PassingInInputParameter_TheRightConstructorIsInvoked()
        {
            IoC.Map<ITestObject, TestObject>();

            const string str1 = "STR1";
            const string str2 = "STR2";
            const string str3 = "STR3";

            var testObj1 = IoC.Resolve<ITestObject>();
            Assert.IsNotNull(testObj1);
            Assert.IsNull(testObj1.Param1);
            Assert.IsNull(testObj1.Param2);

            var testObj2 = IoC.Resolve<ITestObject>(new object[] { str1});
            Assert.IsNotNull(testObj2);
            Assert.AreEqual(str1, testObj2.Param1);
            Assert.IsNull(testObj2.Param2);

            var testObj3 = IoC.Resolve<ITestObject>(new object[] { str2, str3 });
            Assert.IsNotNull(testObj3);
            Assert.AreEqual(str2, testObj3.Param1);
            Assert.AreEqual(str3, testObj3.Param2);

            var testObj4 = IoC.Resolve<ITestObject>(new object[] { str1, str2, str3 });
            Assert.IsNull(testObj4);
        }
    }
}
