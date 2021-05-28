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
        private MainWindowDriver MainWindowDriver;
        private GameAreaDriver GameAreaDriver;
        private InformationAreaDriver InformationAreaDriver;

        private string BeforeEnvironment { get; set; }

        [TestInitialize]
        public void Init()
        {
            // MainWindowプロセスにattach
            string exePath = System.IO.Path.GetFullPath(AttachExeName);
            TestApp = new WindowsAppFriend(Process.Start(exePath));
            TestProcess = Process.GetProcessById(TestApp.ProcessId);
            MainWindow = TestApp.Type("System.Windows.Application").Current.MainWindow;

            MainWindowDriver = new MainWindowDriver(MainWindow);
            GameAreaDriver = new GameAreaDriver(MainWindow);
            InformationAreaDriver = new InformationAreaDriver(MainWindow);

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
            MainWindowDriver.StartGame("Debug", 20);    // 20 = 1マスのサイズ(pixel)

            Assert.IsFalse(GameAreaDriver.IsShowingGameOver());
            Assert.AreEqual(expected: 25, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 2, actual: InformationAreaDriver.GetNumMine());
            Assert.AreEqual(expected: 25, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelOpened());

            // ゲームオーバー時の表示内容確認
            GameAreaDriver.MouseDown(new System.Windows.Point(70, 70));
            Assert.IsFalse(GameAreaDriver.IsShowingGameOver());
            Assert.AreEqual(expected: 24, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 1, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 2, actual: InformationAreaDriver.GetNumMine());
            Assert.AreEqual(expected: 24, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelOpened());

            // ゲームオーバー後にそのゲームをプレイできないことの確認
            GameAreaDriver.MouseDown(new System.Windows.Point(10, 10));
            Assert.AreEqual(expected: 24, actual: GameAreaDriver.GetNumPanelClosing());
        }
    }
}
