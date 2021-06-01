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

            // パネルの状況の確認
            Assert.AreEqual(expected: 2, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 23, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 2, actual: InformationAreaDriver.GetNumMine());
            Assert.AreEqual(expected: 2, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 23, actual: GameAreaDriver.GetNumPanelOpened());

            // ゲームクリア表示の確認
            Assert.IsFalse(GameAreaDriver.IsShowingGameOver());
            Assert.IsTrue(GameAreaDriver.IsShowingGameClear());

            // ゲームクリア後にそのゲームをプレイできないことの確認
            GameAreaDriver.MouseDown(new System.Windows.Point(70, 70));
            Assert.AreEqual(expected: 23, actual: GameAreaDriver.GetNumPanelOpened());
        }

        [TestMethod]
        public void TestPanelOpenedWriteOfNearMineNum()
        {
            // ０１★★★１
            // １３５６５３
            // ２★★★★★
            // ３★７★８★
            // ２★４★★★
            int panelSize = 20;
            MainWindowDriver.StartGame("Debug2", panelSize);

            OpenAllWithoutMine2(panelSize);

            // パネルの状況の確認
            Assert.AreEqual(expected: 15, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 15, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 15, actual: InformationAreaDriver.GetNumMine());
            Assert.AreEqual(expected: 15, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 15, actual: GameAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelOpened(0));
            Assert.AreEqual(expected: 3, actual: GameAreaDriver.GetNumPanelOpened(1));
            Assert.AreEqual(expected: 2, actual: GameAreaDriver.GetNumPanelOpened(2));
            Assert.AreEqual(expected: 3, actual: GameAreaDriver.GetNumPanelOpened(3));
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelOpened(4));
            Assert.AreEqual(expected: 2, actual: GameAreaDriver.GetNumPanelOpened(5));
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelOpened(6));
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelOpened(7));
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelOpened(8));
        }

        [TestMethod]
        public void TestRestartGame()
        {
            int panelSize = 20;
            MainWindowDriver.StartGame("Debug", panelSize);
            Assert.IsFalse(GameAreaDriver.IsShowingGameOver());

            GameAreaDriver.MouseDown(new System.Windows.Point(70, 50));    // 非地雷パネル数のリセットチェックのためにオープン
            GameAreaDriver.MouseDown(new System.Windows.Point(70, 70));    // 地雷
            Assert.IsTrue(GameAreaDriver.IsShowingGameOver());
            Assert.AreEqual(expected: 23, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 2, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 2, actual: InformationAreaDriver.GetNumMine());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelOpened(1));

            // Restartチェック
            MainWindowDriver.StartGame("Debug2", panelSize);
            Assert.IsFalse(GameAreaDriver.IsShowingGameOver());
            Assert.AreEqual(expected: 30, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 15, actual: InformationAreaDriver.GetNumMine());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelOpened(1));
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

        private void OpenAllWithoutMine2(int panelSize)
        {
            // ex) panelSize = 20
            //  0  20   40   60   80   100   120
            //   ０   １   ★   ★   ★    １
            //   １   ３   ５   ６   ５    ３
            //   ２   ★   ★   ★   ★    ★
            //   ３   ★   ７   ★   ８    ★
            //   ２   ★   ４   ★   ★    ★
            GameAreaDriver.MouseDown(new System.Windows.Point( 10, 10));
            GameAreaDriver.MouseDown(new System.Windows.Point( 30, 10));
            GameAreaDriver.MouseDown(new System.Windows.Point(110, 10));

            GameAreaDriver.MouseDown(new System.Windows.Point( 10, 30));
            GameAreaDriver.MouseDown(new System.Windows.Point( 30, 30));
            GameAreaDriver.MouseDown(new System.Windows.Point( 50, 30));
            GameAreaDriver.MouseDown(new System.Windows.Point( 70, 30));
            GameAreaDriver.MouseDown(new System.Windows.Point( 90, 30));
            GameAreaDriver.MouseDown(new System.Windows.Point(110, 30));

            GameAreaDriver.MouseDown(new System.Windows.Point( 10, 50));

            GameAreaDriver.MouseDown(new System.Windows.Point( 10, 70));
            GameAreaDriver.MouseDown(new System.Windows.Point( 50, 70));
            GameAreaDriver.MouseDown(new System.Windows.Point( 90, 70));

            GameAreaDriver.MouseDown(new System.Windows.Point( 10, 90));
            GameAreaDriver.MouseDown(new System.Windows.Point( 50, 90));
        }
    }
}
