using System;
using System.Diagnostics;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMyMinesweeper
{
    [TestClass]
    public class TestFeature
    {
        // 必要なパッケージ
        //  -> Codeer.Friendly
        //  -> Codeer.Friendly.Windows         -> WindowsAppFriend()
        //  -> Codeer.Friendly.Windows.Grasp   -> WindowControl()
        //  -> RM.Friendly.WPFStandardControls -> 各種WPFコントロールを取得するために必要
        // 必要な作業
        //  -> MyMinesweeperプロジェクトを参照に追加

        private string AttachExeName = "MyMinesweeper.exe";
        private WindowsAppFriend TestApp;
        private Process TestProcess;
        private dynamic MainWindow;
        private MainWindowDriver Driver;

        private string BeforeEnvironment { get; set; }

        [TestInitialize]
        public void Init()
        {
            // MainWindowプロセスにattach
            string exePath = System.IO.Path.GetFullPath(AttachExeName);
            TestApp = new WindowsAppFriend(Process.Start(exePath));
            TestProcess = Process.GetProcessById(TestApp.ProcessId);
            MainWindow = TestApp.Type("System.Windows.Application").Current.MainWindow;
            Driver = new MainWindowDriver(MainWindow);
            BeforeEnvironment = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Common.GetEnvironmentDirPath();
        }

        [TestCleanup]
        public void Cleanup()
        {
            TestApp.Dispose();
            TestProcess.CloseMainWindow();
            TestProcess.Dispose();

            Environment.CurrentDirectory = BeforeEnvironment;
        }

        [TestMethod]
        public void TestGameOverWhenStartGameAndPanelOpeningIsMine()
        {
            // ゲーム開始時の表示内容確認
            //           0       >= 80
            //   0 ～ 20■■■■■
            //          ■■■■■
            //          ■■■■■
            //  61 ～ 80■■■★■
            //          ★■■■■
            Driver.StartGame("Debug", 20);    // 20 = 1マスのサイズ(pixel)

            Assert.IsFalse(Driver.IsShowingGameOver());
            Assert.AreEqual(expected: 2, actual: Driver.GetNumPanelMine());
            Assert.AreEqual(expected: 25, actual: Driver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: Driver.GetNumPanelOpened());

            // ゲームオーバー時の表示内容確認
            Driver.GameAreaMouseDown(new System.Windows.Point(70, 70));
            Assert.IsTrue(Driver.IsShowingGameOver());
            Assert.AreEqual(expected: 2, actual: Driver.GetNumPanelMine());
            Assert.AreEqual(expected: 24, actual: Driver.GetNumPanelClosing());
            Assert.AreEqual(expected: 1, actual: Driver.GetNumPanelOpened());

            // ゲームオーバー後にそのゲームをプレイできないことの確認
            Driver.GameAreaMouseDown(new System.Windows.Point(10, 10));
            Assert.AreEqual(expected: 24, actual: Driver.GetNumPanelClosing());
        }
    }
}
