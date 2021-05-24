using RM.Friendly.WPFStandardControls;
using System;
using System.Windows;

namespace TestMyMinesweeper
{
    internal class GameAreaDriver
    {
        private dynamic MainWindow { get; }

        public GameAreaDriver(dynamic mainWindow)
        {
            MainWindow = mainWindow;
        }

        internal int GetNumPanelMine(IWPFDependencyObjectCollection<DependencyObject> tree)
        {
            return 0;
        }

        internal int GetNumPanelClosing(IWPFDependencyObjectCollection<DependencyObject> tree)
        {
            return 0;
        }

        internal int GetNumPanelOpened(IWPFDependencyObjectCollection<DependencyObject> tree)
        {
            return 0;
        }

        internal bool IsShowingGameOver(IWPFDependencyObjectCollection<DependencyObject> tree)
        {
            return false;
        }
    }
}