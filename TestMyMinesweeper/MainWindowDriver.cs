namespace TestMyMinesweeper
{
    class MainWindowDriver
    {
        private dynamic MainWindow { get; }

        public MainWindowDriver(dynamic mainWindow)
        {
            MainWindow = mainWindow;
        }

        internal void StartGame(string gameMode)
        {
            MainWindow.StartGame(gameMode);
        }

        internal void SetPanelSize(int panelSize)
        {
        }
    }
}
