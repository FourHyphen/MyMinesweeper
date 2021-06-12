using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using System;
using System.Windows;

namespace TestMyMinesweeper
{
    internal class GameAreaDriver
    {
        private static string PanelNameClosing = "Closing";

        private static string PanelNameOpened = "Opened";

        private static string PanelNameOpenedMine = "OpenedMine";

        private static string PanelNameOpenedNearMine = "OpenedNearMine";

        private static string PanelNameFlag = "Flag";

        private static string PanelNameQuestion = "Question";

        private static string PanelNameMineGameOver = "MineGameOver";

        private static string PanelNameMineGameClear = "MineGameClear";

        private dynamic MainWindow { get; }

        private IWPFDependencyObjectCollection<System.Windows.DependencyObject> Tree { get; set; }

        private LabelAdapter PlayResultLabel { get; set; }

        public GameAreaDriver(dynamic mainWindow)
        {
            MainWindow = mainWindow;
            Tree = new WindowControl(mainWindow).LogicalTree();
            PlayResultLabel = new LabelAdapter("PlayResultLabel");
        }

        public int GetNumPanelClosing()
        {
            int num = GetDisplayNum(PanelNameClosing);
            num += GetDisplayNum(PanelNameFlag);
            num += GetDisplayNum(PanelNameQuestion);
            num += GetDisplayNum(PanelNameMineGameOver);
            return num + GetDisplayNum(PanelNameMineGameClear);
        }

        public int GetNumPanelOpened()
        {
            int num = 0;
            for (int i = 0; i <=8; i++)
            {
                num += GetDisplayNum(PanelNameOpened + i.ToString());
            }
            return num + GetDisplayNum(PanelNameOpenedMine);
        }

        public int GetNumPanelOpened(int nearMineNum)
        {
            return GetDisplayNum(PanelNameOpened + nearMineNum.ToString());
        }

        public int GetNumPanelFlag()
        {
            return GetDisplayNum(PanelNameFlag);
        }

        public int GetNumPanelQuestion()
        {
            return GetDisplayNum(PanelNameQuestion);
        }

        public int GetNumPanelMineGameOver()
        {
            return GetDisplayNum(PanelNameMineGameOver);
        }

        public int GetNumPanelMineGameClear()
        {
            return GetDisplayNum(PanelNameMineGameClear);
        }

        private int GetDisplayNum(string panelName)
        {
            UpdateNowMainWindowStatus();
            return CountPanel(panelName);
        }

        private void UpdateNowMainWindowStatus()
        {
            Tree = new WindowControl(MainWindow).LogicalTree();    // 現在の画面状況を取得
        }

        private int CountPanel(string panelName)
        {
            var panel = Tree.ByType<System.Windows.Controls.Image>().ByName(panelName);
            return panel.Count;
        }

        public bool IsShowingGameOver()
        {
            UpdateNowMainWindowStatus();

            System.Windows.Visibility now = MainWindow.PlayResultArea.Visibility;
            if (now == Visibility.Hidden)
            {
                return false;
            }
            return PlayResultLabel.Content(Tree).ToLower().Contains("gameover");
        }

        public bool IsShowingGameClear()
        {
            UpdateNowMainWindowStatus();
            System.Windows.Visibility now = MainWindow.PlayResultArea.Visibility;
            if (now == Visibility.Hidden)
            {
                return false;
            }
            return PlayResultLabel.Content(Tree).ToLower().Contains("clear");
        }

        public void MouseLeftButtonDown(System.Windows.Point p)
        {
            MainWindow.GameAreaMouseLeftButtonDown(p);
        }

        internal void MouseRightButtonDown(System.Windows.Point p)
        {
            MainWindow.GameAreaMouseRightButtonDown(p);
        }

        internal int GetAreaWidth()
        {
            return 0;
        }

        internal int GetAreaHeight()
        {
            return 0;
        }
    }
}