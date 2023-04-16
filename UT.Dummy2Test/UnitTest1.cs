using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UT.Dummy2Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var v = new Dummy2.Multiply(6, 5).Result;
            Assert.AreEqual(6 * 5, v);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var v = new Dummy2.Multiply(10, 1).Result;
            Assert.AreEqual(10, v);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var v = new Dummy2.Multiply(101, 12).Result;
            Assert.AreEqual(101*12, v);
        }
    }
}
