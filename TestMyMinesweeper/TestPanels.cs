using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMinesweeper;

namespace TestMyMinesweeper
{
    [TestClass]
    public class TestPanels
    {
        [TestMethod]
        public void TestInitDebugMode()
        {
            Panels panels = new Panels("Debug");
            Assert.AreEqual(expected: 25, actual: panels.GetNumClosing());
            Assert.AreEqual(expected: 2, actual: panels.GetNumMine());
            Assert.AreEqual(expected: 0, actual: panels.GetNumOpened());
        }

        [TestMethod]
        public void TestPanelHaveNumNearMine()
        {
            // ０１★★★１
            // １３５６５３
            // ２★★★★★
            // ３★７★８★
            // ２★４★★★
            Panels panels = new Panels("Debug2");

            Assert.AreEqual(expected: 0, actual: panels.GetNumNearMine(0, 0));
            Assert.AreEqual(expected: 1, actual: panels.GetNumNearMine(1, 0));
            Assert.AreEqual(expected: 1, actual: panels.GetNumNearMine(5, 0));
            Assert.AreEqual(expected: 1, actual: panels.GetNumNearMine(0, 1));
            Assert.AreEqual(expected: 3, actual: panels.GetNumNearMine(1, 1));
            Assert.AreEqual(expected: 5, actual: panels.GetNumNearMine(2, 1));
            Assert.AreEqual(expected: 6, actual: panels.GetNumNearMine(3, 1));
            Assert.AreEqual(expected: 5, actual: panels.GetNumNearMine(4, 1));
            Assert.AreEqual(expected: 3, actual: panels.GetNumNearMine(5, 1));
            Assert.AreEqual(expected: 2, actual: panels.GetNumNearMine(0, 2));
            Assert.AreEqual(expected: 3, actual: panels.GetNumNearMine(0, 3));
            Assert.AreEqual(expected: 7, actual: panels.GetNumNearMine(2, 3));
            Assert.AreEqual(expected: 8, actual: panels.GetNumNearMine(4, 3));
            Assert.AreEqual(expected: 2, actual: panels.GetNumNearMine(0, 4));
            Assert.AreEqual(expected: 4, actual: panels.GetNumNearMine(2, 4));
        }
    }
}
