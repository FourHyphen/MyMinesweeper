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
            StartGame("Debug", 20);    // 20 = 1マスのサイズ(pixel)

            Assert.IsFalse(GameAreaDriver.IsShowingGameOver());
            Assert.AreEqual(expected: 25, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 2, actual: InformationAreaDriver.GetNumMine());
            Assert.AreEqual(expected: 25, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelOpened());

            // ゲームオーバー時の表示内容確認
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(70, 70));
            Assert.IsTrue(GameAreaDriver.IsShowingGameOver());
            Assert.AreEqual(expected: 24, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 1, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 2, actual: InformationAreaDriver.GetNumMine());
            Assert.AreEqual(expected: 24, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelOpened());

            // ゲームオーバー後にそのゲームをプレイできないことの確認
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(10, 10));
            Assert.AreEqual(expected: 24, actual: GameAreaDriver.GetNumPanelClosing());
        }

        [TestMethod]
        public void TestGameClearWhenAllPanelOpenWithoutMine()
        {
            int panelSize = 20;
            StartGame("Debug", panelSize);
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
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(70, 70));
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
            StartGame("Debug2", panelSize);

            OpenAllWithoutMine2();

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
            StartGame("Debug", panelSize);
            Assert.IsFalse(GameAreaDriver.IsShowingGameOver());

            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(70, 50));    // 非地雷パネル数のリセットチェックのためにオープン
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(70, 70));    // 地雷
            Assert.IsTrue(GameAreaDriver.IsShowingGameOver());
            Assert.AreEqual(expected: 23, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 2, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 2, actual: InformationAreaDriver.GetNumMine());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelOpened(1));

            // Restartチェック
            StartGame("Debug2", panelSize);
            Assert.IsFalse(GameAreaDriver.IsShowingGameOver());
            Assert.AreEqual(expected: 30, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 15, actual: InformationAreaDriver.GetNumMine());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelOpened(1));
        }

        [TestMethod]
        public void TestAllPanelNearNotMineOpenWhenPanelNumOfNearMine0Open()
        {
            // 隣接地雷数0の非地雷パネルを開いたら、周囲の非地雷パネルをまとめて開く機能
            int panelSize = 20;
            StartGame("Debug", panelSize);

            // 角から探索開始(1)
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(10, 10));
            Assert.AreEqual(expected: 7, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 18, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 6, actual: GameAreaDriver.GetNumPanelOpened(1));

            // 角から探索開始(2)
            StartGame("Debug", panelSize);
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(90, 10));
            Assert.AreEqual(expected: 7, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 18, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 6, actual: GameAreaDriver.GetNumPanelOpened(1));

            // 角以外から探索開始
            StartGame("Debug", panelSize);
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(30, 30));
            Assert.AreEqual(expected: 7, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 18, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 6, actual: GameAreaDriver.GetNumPanelOpened(1));

            // 地雷パネルに隣接しているパネルを開いても、他パネルは開かない
            StartGame("Debug", panelSize);
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(50, 50));
            Assert.AreEqual(expected: 24, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 1, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelOpened(1));

            // 地雷パネルを開いても、他パネルは開かない
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(70, 70));
            Assert.AreEqual(expected: 23, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 2, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelOpened(1));
        }

        [TestMethod]
        public void TestStartGameEasyMode()
        {
            int panelSize = 20;
            StartGame("Easy", panelSize);

            Assert.AreEqual(expected: 81, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 10, actual: InformationAreaDriver.GetNumMine());     // 地雷密度 = 0.123... = 10 / 81
            Assert.AreEqual(expected: 81, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelOpened(0));
        }

        [TestMethod]
        public void TestStartGameNormalMode()
        {
            int panelSize = 20;
            StartGame("Normal", panelSize);

            Assert.AreEqual(expected: 225, actual: InformationAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: InformationAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 33, actual: InformationAreaDriver.GetNumMine());    // 地雷密度 = 0.15弱 = 33 / 225
            Assert.AreEqual(expected: 225, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelOpened());
        }

        [TestMethod]
        public void TestAddFlagAndQuestionForPanel()
        {
            int panelSize = 20;
            StartGame("Debug", panelSize);

            // 旗を立てる際はパネルを開かない
            GameAreaDriver.MouseRightButtonDown(new System.Windows.Point(10, 10));
            Assert.AreEqual(expected: 25, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelFlag());
            Assert.AreEqual(expected: 1, actual: InformationAreaDriver.GetNumPanelFlag());

            // 旗が立っているパネルは開けない
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(10, 10));
            Assert.AreEqual(expected: 25, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelFlag());
            Assert.AreEqual(expected: 1, actual: InformationAreaDriver.GetNumPanelFlag());

            // 旗が立っているパネルに再度旗を立てることで疑問符を付けられる
            GameAreaDriver.MouseRightButtonDown(new System.Windows.Point(10, 10));
            Assert.AreEqual(expected: 25, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelFlag());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelQuestion());
            Assert.AreEqual(expected: 1, actual: InformationAreaDriver.GetNumPanelQuestion());

            // 疑問符パネルは開けない
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(10, 10));
            Assert.AreEqual(expected: 25, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelQuestion());

            // 疑問符パネルに再度旗を立てることで、通常の未開放パネルに戻る
            GameAreaDriver.MouseRightButtonDown(new System.Windows.Point(10, 10));
            Assert.AreEqual(expected: 25, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelFlag());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelQuestion());

            // 未開放パネルに戻れば、それを開くことができる
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(10, 10));
            Assert.AreEqual(expected: 7, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 18, actual: GameAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelFlag());

            // 地雷パネルにも旗を立てることができる
            GameAreaDriver.MouseRightButtonDown(new System.Windows.Point(70, 70));
            Assert.AreEqual(expected: 7, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 18, actual: GameAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelFlag());

            // 地雷パネルでも旗が立っている場合は開けない -> ゲームオーバーにならない
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(70, 70));
            Assert.IsFalse(GameAreaDriver.IsShowingGameOver());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelFlag());

            // 隣接地雷数0のパネルに旗および疑問符をつけている場合、まとめて開く処理に巻き込まれるのであれば開く
            StartGame("Debug", panelSize);
            GameAreaDriver.MouseRightButtonDown(new System.Windows.Point(30, 10));    // Flag
            GameAreaDriver.MouseRightButtonDown(new System.Windows.Point(50, 10));
            GameAreaDriver.MouseRightButtonDown(new System.Windows.Point(50, 10));    // Question
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelFlag());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelQuestion());

            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(30, 30));
            Assert.AreEqual(expected: 7, actual: GameAreaDriver.GetNumPanelClosing());
            Assert.AreEqual(expected: 18, actual: GameAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelFlag());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelQuestion());
        }

        [TestMethod]
        public void TestShowAllMineWhenGameOver()
        {
            int panelSize = 20;
            StartGame("Debug", panelSize);
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelMineGameOver());

            // 旗パネルや疑問符パネルはゲームオーバー時にその設定をリセットする
            GameAreaDriver.MouseRightButtonDown(new System.Windows.Point(10, 90));    // 旗
            GameAreaDriver.MouseRightButtonDown(new System.Windows.Point(10, 10));
            GameAreaDriver.MouseRightButtonDown(new System.Windows.Point(10, 10));    // 疑問符
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelFlag());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelQuestion());

            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(70, 70));
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelOpened());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelMineGameOver());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelFlag());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelQuestion());
        }

        [TestMethod]
        public void TestShowAllMineWhenGameClear()
        {
            int panelSize = 20;
            StartGame("Debug", panelSize);
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelMineGameClear());

            // 旗パネルや疑問符パネルはゲームクリア時にその設定をリセットする
            GameAreaDriver.MouseRightButtonDown(new System.Windows.Point(70, 70));    // 旗
            GameAreaDriver.MouseRightButtonDown(new System.Windows.Point(10, 90));
            GameAreaDriver.MouseRightButtonDown(new System.Windows.Point(10, 90));    // 疑問符
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelFlag());
            Assert.AreEqual(expected: 1, actual: GameAreaDriver.GetNumPanelQuestion());

            OpenAllWithoutMine(20);
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelMineGameOver());
            Assert.AreEqual(expected: 2, actual: GameAreaDriver.GetNumPanelMineGameClear());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelFlag());
            Assert.AreEqual(expected: 0, actual: GameAreaDriver.GetNumPanelQuestion());
        }

        [TestMethod]
        public void TestSetPanelSize()
        {
            int panelSize = 30;
            StartGame("Debug", panelSize);

            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(70, 70));      // 初期値だと地雷の位置
            Assert.IsFalse(GameAreaDriver.IsShowingGameOver());
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(115, 115));    // パネルサイズ変更後だと地雷の位置
            Assert.IsTrue(GameAreaDriver.IsShowingGameOver());

            // 画面拡大時の確認
            panelSize = 30;
            StartGame("Normal", panelSize);
            int gameAreaWidth = GameAreaDriver.GetAreaWidth();
            int gameAreaHeight = GameAreaDriver.GetAreaHeight();
            int mainWindowWidth = MainWindowDriver.GetWindowWidth();
            int mainWindowHeight = MainWindowDriver.GetWindowHeight();
            int areaSize = panelSize * 15;
            Assert.IsTrue(gameAreaWidth >= areaSize);
            Assert.IsTrue(gameAreaHeight >= areaSize);
            Assert.IsTrue(mainWindowWidth >= areaSize);
            Assert.IsTrue(mainWindowHeight >= areaSize);

            // 画面縮小時の確認
            panelSize = 20;
            StartGame("Easy", panelSize);
            gameAreaWidth = GameAreaDriver.GetAreaWidth();
            gameAreaHeight = GameAreaDriver.GetAreaHeight();
            mainWindowWidth = MainWindowDriver.GetWindowWidth();
            mainWindowHeight = MainWindowDriver.GetWindowHeight();
            areaSize = panelSize * 9;
            Assert.IsTrue(areaSize <= gameAreaWidth && gameAreaWidth <= (areaSize + panelSize));
            Assert.IsTrue(areaSize <= gameAreaHeight && gameAreaHeight <= (areaSize + panelSize));
            Assert.IsTrue(areaSize <= mainWindowWidth);
            Assert.IsTrue(areaSize <= mainWindowHeight && mainWindowHeight <= (areaSize + 100));    // 100 = メニューバー等の、ゲーム領域外の縦幅
        }

        private void StartGame(string mode, int panelSize)
        {
            MainWindowDriver.SetPanelSize(panelSize);
            MainWindowDriver.StartGame(mode);
        }

        private void OpenAllWithoutMine(int panelSize)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(10 + (i * panelSize), 10 + (j * panelSize)));
                }
            }
            Assert.IsFalse(GameAreaDriver.IsShowingGameOver());

            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(10, 70));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(30, 70));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(50, 70));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(90, 70));

            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(30, 90));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(50, 90));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(70, 90));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(90, 90));
        }

        private void OpenAllWithoutMine2()
        {
            // ex) panelSize = 20
            //  0  20   40   60   80   100   120
            //   ０   １   ★   ★   ★    １
            //   １   ３   ５   ６   ５    ３
            //   ２   ★   ★   ★   ★    ★
            //   ３   ★   ７   ★   ８    ★
            //   ２   ★   ４   ★   ★    ★
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point( 10, 10));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point( 30, 10));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(110, 10));

            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point( 10, 30));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point( 30, 30));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point( 50, 30));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point( 70, 30));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point( 90, 30));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point(110, 30));

            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point( 10, 50));

            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point( 10, 70));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point( 50, 70));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point( 90, 70));

            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point( 10, 90));
            GameAreaDriver.MouseLeftButtonDown(new System.Windows.Point( 50, 90));
        }
    }
}
