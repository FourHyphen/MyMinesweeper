using RM.Friendly.WPFStandardControls;
using System;
using System.Windows;

namespace TestMyMinesweeper
{
    internal class GameAreaDriver
    {
        private static int MaxNumDisplayPanel = 100 * 100;    // 10000マスもあればテストには十分

        private static string PanelNameMine = "Mine";

        private static string PanelNameClosing = "Closing";

        private static string PanelNameOpened = "Opened";

        public GameAreaDriver() { }

        internal int GetNumPanelMine(IWPFDependencyObjectCollection<DependencyObject> tree)
        {
            return GetDisplayNum(tree, PanelNameMine);
        }

        internal int GetNumPanelClosing(IWPFDependencyObjectCollection<DependencyObject> tree)
        {
            return GetDisplayNum(tree, PanelNameClosing);
        }

        internal int GetNumPanelOpened(IWPFDependencyObjectCollection<DependencyObject> tree)
        {
            return GetDisplayNum(tree, PanelNameOpened);
        }

        public int GetDisplayNum(IWPFDependencyObjectCollection<DependencyObject> logicalTree, string panelName)
        {
            int num = 0;
            for (int i = 1; i <= MaxNumDisplayPanel; i++)
            {
                if (ExistPanel(logicalTree, panelName, i))
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

        private bool ExistPanel(IWPFDependencyObjectCollection<DependencyObject> logicalTree, string panelName, int index)
        {
            string panelImageName = panelName + index.ToString();
            var panel = logicalTree.ByType<System.Windows.Controls.Image>().ByName(panelImageName);
            return (panel.Count != 0);
        }

        internal bool IsShowingGameOver(IWPFDependencyObjectCollection<DependencyObject> tree)
        {
            return false;
        }
    }
}