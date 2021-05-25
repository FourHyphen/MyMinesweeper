using System;
using System.Windows.Controls;

namespace MyMinesweeper
{
    internal class GameAreaDisplay
    {
        private Grid GameArea { get; set; }

        private int PanelSize { get; }

        public GameAreaDisplay(Grid gameArea, int panelSize)
        {
            GameArea = gameArea;
            PanelSize = panelSize;
        }

        internal void Update(Panels panels)
        {
            // TODO: 実装
        }
    }
}