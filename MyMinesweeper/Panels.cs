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
            return PanelList[CalcIndex(x, y)].Status;
        }

        public bool IsMine(int x, int y)
        {
            return PanelList[CalcIndex(x, y)].IsMine;
        }

        public void Open(int x, int y)
        {
            PanelList[CalcIndex(x, y)].Open();
        }

        private int CalcIndex(int x, int y)
        {
            return y * Width + x;
        }

        public bool IsGameOver()
        {
            List<Panel> gameOverPanel = PanelList.FindAll(x => (x.Status == Panel.PanelStatus.Opened && x.IsMine));
            return gameOverPanel.Count > 0;
        }

        public bool IsGameClear()
        {
            if (IsGameOver())
            {
                return false;
            }

            int opened = GetNumOpened();
            int mine = GetNumMine();
            int all = PanelList.Count;
            return (opened + mine) == all;
        }

        public bool IsGameFinished()
        {
            return (IsGameClear() || IsGameOver());
        }
    }
}