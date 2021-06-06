﻿using System;
using System.Collections.Generic;
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
            Panels panels = PanelsFactory.Create("Debug");
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
            Panels panels = PanelsFactory.Create("Debug2");

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

        [TestMethod]
        public void TestOpenTouchedPanelsWhenPanelNotNearMine0Open()
        {
            // 隣接地雷数0のパネルを開いた際、周囲1マス範囲のパネルを開く
            Panels panels = PanelsFactory.Create("Debug");
            panels.Open(1, 1);
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(0, 0));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(1, 0));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(2, 0));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(0, 1));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(1, 1));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(2, 1));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(0, 2));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(1, 2));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(2, 2));
        }

        [TestMethod]
        public void TestAllPanelsNearNotMineOpenWhenPanelNearMine0Open()
        {
            // ■■■■■
            // ■■■■■
            // ■■■■■
            // ■■■★■
            // ★■■■■
            // 左上起点での探索
            Panels panels = PanelsFactory.Create("Debug");
            panels.Open(0, 0);
            AssertAreEqualClosingAndOpened(panels);

            // 右上起点での探索
            panels = PanelsFactory.Create("Debug");
            panels.Open(4, 0);
            AssertAreEqualClosingAndOpened(panels);

            // 非端起点での探索
            panels = PanelsFactory.Create("Debug");
            panels.Open(2, 1);
            AssertAreEqualClosingAndOpened(panels);

            // 隣接地雷数1以上のパネルを開いても周囲を開けない
            panels = PanelsFactory.Create("Debug");
            panels.Open(2, 2);
            Assert.AreEqual(expected: 1, actual: panels.GetNumOpened());
            Assert.AreEqual(expected: 24, actual: panels.GetNumClosing());

            // 地雷パネルを開いても周囲を開けない
            panels = PanelsFactory.Create("Debug");
            panels.Open(3, 3);
            Assert.AreEqual(expected: 1, actual: panels.GetNumOpened());
            Assert.AreEqual(expected: 24, actual: panels.GetNumClosing());
        }

        [TestMethod]
        public void TestEasyMode()
        {
            Panels panels = PanelsFactory.Create("Easy");
            Assert.AreEqual(expected: 81, actual: panels.GetNumClosing());
            Assert.AreEqual(expected: 15, actual: panels.GetNumMine());

            // パネルをランダムで生成していることの確認
            DoCreatePanelsAtRandom();
        }

        [TestMethod]
        public void TestNormalMode()
        {
            Panels panels = PanelsFactory.Create("Normal");
            Assert.AreEqual(expected: 225, actual: panels.GetNumClosing());
            Assert.AreEqual(expected: 57, actual: panels.GetNumMine());

            // パネルをランダムで生成していることの確認
            DoCreatePanelsAtRandom();
        }

        private void DoCreatePanelsAtRandom()
        {
            // Panels生成して同じ位置をOpenを繰り返したとき、閉じているパネル数が完全一致するなら固定配置を生成しているのでNGとする
            List<int> closings = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                Panels panels = PanelsFactory.Create("Easy");
                panels.Open(1, 1);
                closings.Add(panels.GetNumClosing());
            }
            closings.Sort();
            Assert.IsTrue(closings[0] != closings[99]);
        }

        private void AssertAreEqualClosingAndOpened(Panels panels)
        {
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(0, 0));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(1, 0));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(2, 0));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(3, 0));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(4, 0));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(0, 1));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(1, 1));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(2, 1));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(3, 1));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(4, 1));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(0, 2));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(1, 2));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(2, 2));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(3, 2));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(4, 2));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(0, 3));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(1, 3));
            Assert.AreEqual(expected: Panel.PanelStatus.Opened, actual: panels.GetStatus(2, 3));
            Assert.AreEqual(expected: Panel.PanelStatus.Closing, actual: panels.GetStatus(3, 3));
            Assert.AreEqual(expected: Panel.PanelStatus.Closing, actual: panels.GetStatus(4, 3));
            Assert.AreEqual(expected: Panel.PanelStatus.Closing, actual: panels.GetStatus(0, 4));
            Assert.AreEqual(expected: Panel.PanelStatus.Closing, actual: panels.GetStatus(1, 4));
            Assert.AreEqual(expected: Panel.PanelStatus.Closing, actual: panels.GetStatus(2, 4));
            Assert.AreEqual(expected: Panel.PanelStatus.Closing, actual: panels.GetStatus(3, 4));
            Assert.AreEqual(expected: Panel.PanelStatus.Closing, actual: panels.GetStatus(4, 4));
        }
    }
}
