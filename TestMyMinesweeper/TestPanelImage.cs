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
        public void TestInitialize()
        {
            PanelImage pi = new PanelImage(20);

            Image closing = pi.CreateImage(MyMinesweeper.Panel.PanelStatus.Closing, true);
            Assert.AreEqual(expected: "closing", actual: closing.Name.ToLower());
            Image opened = pi.CreateImage(MyMinesweeper.Panel.PanelStatus.Opened, false);
            Assert.AreEqual(expected: "opened", actual: opened.Name.ToLower());
            Image openedMine = pi.CreateImage(MyMinesweeper.Panel.PanelStatus.Opened, true);
            Assert.AreEqual(expected: "openedmine", actual: openedMine.Name.ToLower());
        }
    }
}
