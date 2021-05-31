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
        public void TestGameOverWhenPanelOpenedIsMine()
        {
            Panels panels = new Panels("Debug");
            Assert.IsFalse(panels.IsGameOver());

            // 非地雷パネルのオープンチェック
            Assert.AreEqual(expected: Panel.PanelStatus.Closing, actual: panels.GetStatus(0, 0));
            panels.Open(0, 0);
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(0, 0));
            Assert.IsFalse(panels.IsGameOver());

            // 地雷パネルのオープンチェック
            panels.Open(0, 4);
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(0, 4));
            Assert.IsTrue(panels.IsGameOver());
        }

        [TestMethod]
        public void TestGameClearWhenAllPanelOpenWithoutMine()
        {
            Panels panels = new Panels("Debug");
            Assert.IsFalse(panels.IsGameClear());

            panels.Open(0, 0); panels.Open(1, 0); panels.Open(2, 0); panels.Open(3, 0); panels.Open(4, 0);
            panels.Open(0, 1); panels.Open(1, 1); panels.Open(2, 1); panels.Open(3, 1); panels.Open(4, 1);
            panels.Open(0, 2); panels.Open(1, 2); panels.Open(2, 2); panels.Open(3, 2); panels.Open(4, 2);
            panels.Open(0, 3); panels.Open(1, 3); panels.Open(2, 3); /*              */ panels.Open(4, 3);
            /*              */ panels.Open(1, 4); panels.Open(2, 4); panels.Open(3, 4);

            Assert.IsFalse(panels.IsGameOver());
            Assert.IsFalse(panels.IsGameClear());

            panels.Open(4, 4);
            Assert.IsFalse(panels.IsGameOver());
            Assert.IsTrue(panels.IsGameClear());
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
