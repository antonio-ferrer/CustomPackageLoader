using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UT.DummyContainerTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDummy1Reference()
        {
            int v = new DummyContainer.PrettyLittleClass().DoSomethingWithDummy1();
            Assert.AreNotEqual(0, v);
        }

        [TestMethod]
        public void TestDummy2Reference()
        {
            int v = new DummyContainer.PrettyLittleClass().DoSomehingWithDummy2AndReturnNine();
            Assert.AreEqual(9, v);
        }

        [TestMethod]
        public void TestDummy3Reference()
        {
            var v = new DummyContainer.PrettyLittleClass().DoSomethingCrazyWithDummy3AndReturnBoolean();
            Assert.IsTrue(v);
        }
    }
}
