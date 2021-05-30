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
    }
}
