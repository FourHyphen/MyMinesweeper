using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMinesweeper
{
    public class GameStatus
    {
        public enum GameMode
        {
            Open,
            Flag
        }

        private GameMode _Mode { get; set; }

        public GameStatus() { }

        public bool IsGameOver(Panels panels)
        {
            return panels.IsOpenedPanelMine();
        }

        public bool IsGameClear(Panels panels)
        {
            return panels.IsAllOpenedPanelsWithoutMine();
        }

        public bool IsGameFinished(Panels panels)
        {
            return (panels.IsAllOpenedPanelsWithoutMine() || panels.IsOpenedPanelMine());
        }

        public GameMode GetMode()
        {
            return _Mode;
        }

        public void SwitchMode()
        {
            if (_Mode == GameMode.Flag)
            {
                _Mode = GameMode.Open;
            }
            else
            {
                _Mode = GameMode.Flag;
            }
        }
    }
}
