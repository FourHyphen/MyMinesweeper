using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;

namespace TestMyMinesweeper
{
    class MainWindowDriver
    {
        private dynamic MainWindow { get; }

        private IWPFDependencyObjectCollection<System.Windows.DependencyObject> Tree { get; set; }

        private LabelAdapter MainWindowWidth { get; set; }

        private LabelAdapter MainWindowHeight { get; set; }

        public MainWindowDriver(dynamic mainWindow)
        {
            MainWindow = mainWindow;
            Tree = new WindowControl(mainWindow).LogicalTree();
            MainWindowWidth = new LabelAdapter("MainWindowWidth");
            MainWindowHeight = new LabelAdapter("MainWindowHeight");
        }

        internal void StartGame(string gameMode)
        {
            MainWindow.StartGame(gameMode);
        }

        internal void SetPanelSize(int panelSize)
        {
            MainWindow.SetPanelSize(panelSize);
        }

        internal int GetWindowWidth()
        {
            UpdateNowMainWindowStatus();
            return (int)MainWindowWidth.ContentNum(Tree);
        }

        internal int GetWindowHeight()
        {
            UpdateNowMainWindowStatus();
            return (int)MainWindowHeight.ContentNum(Tree);
        }

        private void UpdateNowMainWindowStatus()
        {
            Tree = new WindowControl(MainWindow).LogicalTree();    // 現在の画面状況を取得
        }
    }
}
