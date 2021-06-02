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
            return panels.IsGameOver();
        }

        public static bool IsGameClear(Panels panels)
        {
            return panels.IsGameClear();
        }
    }
}
