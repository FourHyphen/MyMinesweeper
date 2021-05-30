using System;
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
            return PanelList.FindAll(x => x.Status == status).Count;
        }

        public int GetNumMine()
        {
            return PanelList.FindAll(x => x.IsMine).Count;
        }

        public Panel.PanelStatus GetStatus(int x, int y)
        {
            int index = y * Width + x;
            return PanelList[index].Status;
        }

        public bool IsMine(int x, int y)
        {
            int index = y * Width + x;
            return PanelList[index].IsMine;
        }

        public void Open(int x, int y)
        {
            int index = y * Width + x;
            PanelList[index].Open();
        }

        public bool IsGameOver()
        {
            List<Panel> gameOverPanel = PanelList.FindAll(x => (x.Status == Panel.PanelStatus.Opened && x.IsMine));
            return gameOverPanel.Count > 0;
        }
    }
}