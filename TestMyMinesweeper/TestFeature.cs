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
            Assert.IsTrue(GameAreaDriver.IsShowingGameOver());
            Assert.AreEqual(expected: 24, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 1, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 2, actual: InformationAreaDriver.GetNumMine());
            Assert.AreEqual(expected: 24, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelOpened());

            // ゲームオーバー後にそのゲームをプレイできないことの確認
            GameAreaDriver.MouseDown(new System.Windows.Point(10, 10));
            Assert.AreEqual(expected: 24, actual: GameAreaDriver.GetNumPanelClosing());
        }

        [TestMethod]
        public void TestGameClearWhenAllPanelOpenWithoutMine()
        {
            int panelSize = 20;
            MainWindowDriver.StartGame("Debug", panelSize);
            OpenAllWithoutMine(panelSize);

            Assert.AreEqual(expected: 2, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 23, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 2, actual: InformationAreaDriver.GetNumMine());
            Assert.AreEqual(expected: 2, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 23, actual: GameAreaDriver.GetNumPanelOpened());

            Assert.IsFalse(GameAreaDriver.IsShowingGameOver());
            Assert.IsTrue(GameAreaDriver.IsShowingGameClear());
        }

        private void OpenAllWithoutMine(int panelSize)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    GameAreaDriver.MouseDown(new System.Windows.Point(10 + (i * panelSize), 10 + (j * panelSize)));
                }
            }
            Assert.IsFalse(GameAreaDriver.IsShowingGameOver());

            GameAreaDriver.MouseDown(new System.Windows.Point(10, 70));
            GameAreaDriver.MouseDown(new System.Windows.Point(30, 70));
            GameAreaDriver.MouseDown(new System.Windows.Point(50, 70));
            GameAreaDriver.MouseDown(new System.Windows.Point(90, 70));

            GameAreaDriver.MouseDown(new System.Windows.Point(30, 90));
            GameAreaDriver.MouseDown(new System.Windows.Point(50, 90));
            GameAreaDriver.MouseDown(new System.Windows.Point(70, 90));
            GameAreaDriver.MouseDown(new System.Windows.Point(90, 90));
        }
    }
}
