using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return panels.IsAllOpenedPanelsNotMine();
        }

        public static bool IsGameFinished(Panels panels)
        {
            return (panels.IsAllOpenedPanelsNotMine() || panels.IsOpenedPanelMine());
        }
    }
}
