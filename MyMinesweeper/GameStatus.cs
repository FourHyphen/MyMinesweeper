namespace MyMinesweeper
{
    public class GameStatus
    {
        private GameStatus() { }

        public static bool IsGameOver(Panels panels)
        {
            return panels.IsOpenedPanelMine();
        }

        public static bool IsGameClear(Panels panels)
        {
            return panels.IsAllOpenedPanelsWithoutMine();
        }

        public static bool IsGameFinished(Panels panels)
        {
            return (panels.IsAllOpenedPanelsWithoutMine() || panels.IsOpenedPanelMine());
        }
    }
}
