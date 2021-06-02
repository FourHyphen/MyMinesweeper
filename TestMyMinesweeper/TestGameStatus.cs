using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMinesweeper;

namespace TestMyMinesweeper
{
    [TestClass]
    public class TestGameStatus
    {
        [TestMethod]
        public void TestGameOverWhenPanelOpenedIsMine()
        {
            Panels panels = new Panels("Debug");
            Assert.IsFalse(GameStatus.IsGameOver(panels));

            // 非地雷パネルのオープンチェック
            Assert.AreEqual(expected: Panel.PanelStatus.Closing, actual: panels.GetStatus(0, 0));
            panels.Open(0, 0);
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(0, 0));
            Assert.IsFalse(GameStatus.IsGameOver(panels));

            // 地雷パネルのオープンチェック
            panels.Open(0, 4);
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(0, 4));
            Assert.IsTrue(GameStatus.IsGameOver(panels));
        }
    }
}
