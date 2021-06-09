using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using System;

namespace TestMyMinesweeper
{
    internal class InformationAreaDriver
    {
        private dynamic MainWindow { get; }

        private IWPFDependencyObjectCollection<System.Windows.DependencyObject> Tree { get; set; }

        private LabelAdapter NumPanelClosing { get; }

        private LabelAdapter NumPanelOpened { get; }

        private LabelAdapter NumMine { get; }

        private LabelAdapter NumFlag { get; }

        private LabelAdapter NumQuestion { get; }

        public InformationAreaDriver(dynamic mainWindow)
        {
            MainWindow = mainWindow;
            Tree = new WindowControl(mainWindow).LogicalTree();
            NumPanelClosing = new LabelAdapter("NumPanelClosing");
            NumPanelOpened = new LabelAdapter("NumPanelOpened");
            NumMine = new LabelAdapter("NumMine");
            NumFlag = new LabelAdapter("NumFlag");
            NumQuestion = new LabelAdapter("NumQuestion");
        }

        public int GetNumPanelClosing()
        {
            UpdateNowMainWindowStatus();
            return (int)NumPanelClosing.ContentNum(Tree);
        }

        public int GetNumPanelOpened()
        {
            UpdateNowMainWindowStatus();
            return (int)NumPanelOpened.ContentNum(Tree);
        }

        public int GetNumMine()
        {
            UpdateNowMainWindowStatus();
            return (int)NumMine.ContentNum(Tree);
        }

        public int GetNumPanelFlag()
        {
            UpdateNowMainWindowStatus();
            return (int)NumFlag.ContentNum(Tree);
        }

        public int GetNumPanelQuestion()
        {
            UpdateNowMainWindowStatus();
            return (int)NumQuestion.ContentNum(Tree);
        }

        private void UpdateNowMainWindowStatus()
        {
            Tree = new WindowControl(MainWindow).LogicalTree();    // 現在の画面状況を取得
        }
    }
}