namespace TestMyMinesweeper
{
    class MainWindowDriver
    {
        private dynamic MainWindow { get; }

        public MainWindowDriver(dynamic mainWindow)
        {
            MainWindow = mainWindow;
        }

        internal void StartGame(string gameMode, int panelSize)
        {
            MainWindow.StartGame(gameMode, panelSize);
        }
    }
}
