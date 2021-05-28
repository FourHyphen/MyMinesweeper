using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using System;
using System.Windows;

namespace TestMyMinesweeper
{
    internal class GameAreaDriver
    {
        private static int MaxNumDisplayPanel = 100 * 100;    // 10000マスもあればテストには十分

        private static string PanelNameClosing = "Closing";

        private static string PanelNameOpened = "Opened";

        private dynamic MainWindow { get; }

        private IWPFDependencyObjectCollection<System.Windows.DependencyObject> Tree { get; set; }

        public GameAreaDriver(dynamic mainWindow)
        {
            MainWindow = mainWindow;
            Tree = new WindowControl(mainWindow).LogicalTree();
        }

        public int GetNumPanelClosing()
        {
            return GetDisplayNum(PanelNameClosing);
        }

        public int GetNumPanelOpened()
        {
            return GetDisplayNum(PanelNameOpened);
        }

        private int GetDisplayNum(string panelName)
        {
            UpdateNowMainWindowStatus();

            int num = 0;
            for (int i = 1; i <= MaxNumDisplayPanel; i++)
            {
                if (ExistPanel(panelName, i))
                {
                    num++;
                }
                else
                {
                    break;
                }
            }

            return num;
        }

        private bool ExistPanel(string panelName, int index)
        {
            string panelImageName = panelName + index.ToString();
            var panel = Tree.ByType<System.Windows.Controls.Image>().ByName(panelImageName);
            return (panel.Count != 0);
        }

        private void UpdateNowMainWindowStatus()
        {
            Tree = new WindowControl(MainWindow).LogicalTree();    // 現在の画面状況を取得
        }

        public bool IsShowingGameOver()
        {
            return false;
        }

        public void MouseDown(System.Windows.Point p)
        {
            MainWindow.GameAreaMouseDown(p);
        }
    }
}