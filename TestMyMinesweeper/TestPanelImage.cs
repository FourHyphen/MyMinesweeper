using System;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMinesweeper;

namespace TestMyMinesweeper
{
    [TestClass]
    public class TestPanelImage
    {
        private string BeforeEnvironment { get; set; }

        [TestInitialize]
        public void Init()
        {
            BeforeEnvironment = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Common.GetEnvironmentDirPath();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Environment.CurrentDirectory = BeforeEnvironment;
        }

        [TestMethod]
        public void TestSuccessInitialize()
        {
            PanelImage pi = new PanelImage(20);

            Image closing = pi.CreateImage(MyMinesweeper.Panel.PanelStatus.Closing, true, 0);
            Assert.AreEqual(expected: "closing", actual: closing.Name.ToLower());

            AreEqualImageNearMine(pi, 0);
            AreEqualImageNearMine(pi, 1);
            AreEqualImageNearMine(pi, 2);
            AreEqualImageNearMine(pi, 3);
            AreEqualImageNearMine(pi, 4);
            AreEqualImageNearMine(pi, 5);
            AreEqualImageNearMine(pi, 6);
            AreEqualImageNearMine(pi, 7);
            AreEqualImageNearMine(pi, 8);

            Image openedMine = pi.CreateImage(MyMinesweeper.Panel.PanelStatus.Opened, true, 0);
            Assert.AreEqual(expected: "openedmine", actual: openedMine.Name.ToLower());

            Image flag = pi.CreateImage(MyMinesweeper.Panel.PanelStatus.Flag, true, 0);
            Assert.AreEqual(expected: "flag", actual: flag.Name.ToLower());

            Image question = pi.CreateImage(MyMinesweeper.Panel.PanelStatus.Question, true, 0);
            Assert.AreEqual(expected: "question", actual: question.Name.ToLower());
        }

        private void AreEqualImageNearMine(PanelImage pi, int nearMineNum)
        {
            Image opened = pi.CreateImage(MyMinesweeper.Panel.PanelStatus.Opened, false, nearMineNum);
            Assert.AreEqual(expected: "opened" + nearMineNum.ToString(), actual: opened.Name.ToLower());
        }
    }
}
