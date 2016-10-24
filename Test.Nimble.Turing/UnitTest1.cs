using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nimble.Turing;

namespace Test.Nimble.Turing
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Smart smart = new Smart();
            var result = smart.Process("成都天气怎么样?");
            Assert.IsTrue(result.Contains("天气"));
        }
    }
}
