using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using System;

namespace TestMyMinesweeper
{
    class MainWindowDriver
    {
        private dynamic MainWindow { get; }
        private GameAreaDriver GameAreaDriver { get; set; }
        private IWPFDependencyObjectCollection<System.Windows.DependencyObject> Tree { get; set; }

        public MainWindowDriver(dynamic mainWindow)
        {
            MainWindow = mainWindow;
            GameAreaDriver = new GameAreaDriver(mainWindow);
            Tree = new WindowControl(mainWindow).LogicalTree();
        }

        internal int GetNumPanelMine()
        {
            UpdateNowMainWindowStatus();
            return GameAreaDriver.GetNumPanelMine(Tree);
        }

        internal int GetNumPanelClosing()
        {
            UpdateNowMainWindowStatus();
            return GameAreaDriver.GetNumPanelClosing(Tree);
        }

        internal int GetNumPanelOpened()
        {
            UpdateNowMainWindowStatus();
            return GameAreaDriver.GetNumPanelOpened(Tree);
        }

        internal bool IsShowingGameOver()
        {
            UpdateNowMainWindowStatus();
            return GameAreaDriver.IsShowingGameOver(Tree);
        }

        private void UpdateNowMainWindowStatus()
        {
            Tree = new WindowControl(MainWindow).LogicalTree();    // 現在の画面状況を取得
        }

        internal void StartGame(string gameMode, int panelSize)
        {
            MainWindow.StartGame(gameMode, panelSize);
        }

        internal void GameAreaMouseDown(System.Windows.Point p)
        {
            MainWindow.GameAreaMouseDown(p);
        }
    }
}
