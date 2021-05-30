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
            return GetDisplayNum(PanelNameClosing);
        }

        public int GetNumPanelOpened()
        {
            int num = GetDisplayNum(PanelNameOpened);
            return num + GetDisplayNum(PanelNameOpenedMine);
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

        public void MouseDown(System.Windows.Point p)
        {
            MainWindow.GameAreaMouseDown(p);
        }
    }
}