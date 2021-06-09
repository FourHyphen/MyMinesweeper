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
            GameStatus gs = new GameStatus();
            Panels panels = PanelsFactory.Create("Debug");
            Assert.IsFalse(gs.IsGameOver(panels));

            // 非地雷パネルのオープンチェック
            Assert.AreEqual(expected: Panel.PanelStatus.Closing, actual: panels.GetStatus(0, 0));
            panels.Open(0, 0);
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(0, 0));
            Assert.IsFalse(gs.IsGameOver(panels));

            // 地雷パネルのオープンチェック
            panels.Open(0, 4);
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(0, 4));
            Assert.IsTrue(gs.IsGameOver(panels));
        }

        [TestMethod]
        public void TestGameClearWhenAllPanelOpenWithoutMine()
        {
            GameStatus gs = new GameStatus();
            Panels panels = PanelsFactory.Create("Debug");
            Assert.IsFalse(gs.IsGameClear(panels));

            panels.Open(0, 0); panels.Open(1, 0); panels.Open(2, 0); panels.Open(3, 0); panels.Open(4, 0);
            panels.Open(0, 1); panels.Open(1, 1); panels.Open(2, 1); panels.Open(3, 1); panels.Open(4, 1);
            panels.Open(0, 2); panels.Open(1, 2); panels.Open(2, 2); panels.Open(3, 2); panels.Open(4, 2);
            panels.Open(0, 3); panels.Open(1, 3); panels.Open(2, 3); /*              */ panels.Open(4, 3);
            /*              */ panels.Open(1, 4); panels.Open(2, 4); panels.Open(3, 4);

            Assert.IsFalse(gs.IsGameOver(panels));
            Assert.IsFalse(gs.IsGameClear(panels));

            panels.Open(4, 4);
            Assert.IsFalse(gs.IsGameOver(panels));
            Assert.IsTrue(gs.IsGameClear(panels));
        }

        [TestMethod]
        public void TestGameFinishWhenGameOverOrGameClear()
        {
            // ゲームオーバー
            GameStatus gs = new GameStatus();
            Panels panels = PanelsFactory.Create("Debug");
            Assert.IsFalse(gs.IsGameFinished(panels));
            panels.Open(0, 4);
            Assert.IsTrue(gs.IsGameFinished(panels));

            // ゲームクリア
            panels = PanelsFactory.Create("Debug");
            panels.Open(0, 0); panels.Open(1, 0); panels.Open(2, 0); panels.Open(3, 0); panels.Open(4, 0);
            panels.Open(0, 1); panels.Open(1, 1); panels.Open(2, 1); panels.Open(3, 1); panels.Open(4, 1);
            panels.Open(0, 2); panels.Open(1, 2); panels.Open(2, 2); panels.Open(3, 2); panels.Open(4, 2);
            panels.Open(0, 3); panels.Open(1, 3); panels.Open(2, 3); /*              */ panels.Open(4, 3);
            /*              */ panels.Open(1, 4); panels.Open(2, 4); panels.Open(3, 4); panels.Open(4, 4);

            Assert.IsTrue(gs.IsGameFinished(panels));
        }

        [TestMethod]
        public void TestSwitchGameMode()
        {
            GameStatus gs = new GameStatus();
            Assert.AreEqual(expected: GameStatus.GameMode.Open, actual: gs.GetMode());
            gs.SwitchMode();
            Assert.AreEqual(expected: GameStatus.GameMode.Flag, actual: gs.GetMode());
            gs.SwitchMode();
            Assert.AreEqual(expected: GameStatus.GameMode.Open, actual: gs.GetMode());
        }
    }
}
