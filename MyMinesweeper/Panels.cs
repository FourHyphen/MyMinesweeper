using System.Collections.Generic;

namespace MyMinesweeper
{
    public class Panels
    {
        private List<Panel> PanelList { get; set; } = new List<Panel>();
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Panels(string gameMode)
        {
            Init(gameMode);
        }

        private void Init(string gameMode)
        {
            if (gameMode.ToLower() == "debug")
            {
                InitDebug();
            }
        }

        private void InitDebug()
        {
            Width = 5;
            Height = 5;
            Panel plain = new Panel(false);
            Panel mine = new Panel(true);
            PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone());    // ■■■■■
            PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone());    // ■■■■■
            PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone());    // ■■■■■
            PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone()); PanelList.Add(mine.Clone()); PanelList.Add(plain.Clone());     // ■■■★■
            PanelList.Add(mine.Clone()); PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone()); PanelList.Add(plain.Clone());     // ★■■■■
        }

        public int GetNumClosing()
        {
            return GetNumPanel(Panel.PanelStatus.Closing);
        }

        public int GetNumOpened()
        {
            return GetNumPanel(Panel.PanelStatus.Opened);
        }

        private int GetNumPanel(Panel.PanelStatus status)
        {
            int maxIndex = Width * Height;
            int num = 0;
            for (int i = 0; i < maxIndex; i++)
            {
                if (PanelList[i].Status == status)
                {
                    num++;
                }
            }

            return num;
        }

        public int GetNumMine()
        {
            int maxIndex = Width * Height;
            int num = 0;
            for (int i = 0; i < maxIndex; i++)
            {
                if (PanelList[i].IsMine)
                {
                    num++;
                }
            }

            return num;
        }
    }
}