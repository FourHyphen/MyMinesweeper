using System;
using System.Collections.Generic;

namespace MyMinesweeper
{
    public class Panels
    {
        private List<Panel> PanelList { get; set; } = new List<Panel>();
        private List<int> NumNearMineList { get; set; } = new List<int>();
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
            else if (gameMode.ToLower() == "debug2")
            {
                InitDebug2();
            }

            InitNumNearMineList();
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

        private void InitDebug2()
        {
            Width = 6;
            Height = 5;
            Panel p = new Panel(false);
            Panel m = new Panel(true);
            PanelList.Add(p.Clone()); PanelList.Add(p.Clone()); PanelList.Add(m.Clone()); PanelList.Add(m.Clone()); PanelList.Add(m.Clone()); PanelList.Add(p.Clone());    // ０１★★★１
            PanelList.Add(p.Clone()); PanelList.Add(p.Clone()); PanelList.Add(p.Clone()); PanelList.Add(p.Clone()); PanelList.Add(p.Clone()); PanelList.Add(p.Clone());    // １３５６５３
            PanelList.Add(p.Clone()); PanelList.Add(m.Clone()); PanelList.Add(m.Clone()); PanelList.Add(m.Clone()); PanelList.Add(m.Clone()); PanelList.Add(m.Clone());    // ２★★★★★
            PanelList.Add(p.Clone()); PanelList.Add(m.Clone()); PanelList.Add(p.Clone()); PanelList.Add(m.Clone()); PanelList.Add(p.Clone()); PanelList.Add(m.Clone());    // ３★７★８★
            PanelList.Add(p.Clone()); PanelList.Add(m.Clone()); PanelList.Add(p.Clone()); PanelList.Add(m.Clone()); PanelList.Add(m.Clone()); PanelList.Add(m.Clone());    // ２★４★★★
        }

        private void InitNumNearMineList()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int index = y * Width + x;
                    if (PanelList[index].IsMine)
                    {
                        NumNearMineList.Add(-1);
                        continue;
                    }

                    int nearMineNum = 0;
                    for (int j = -1; j <= 1; j++)
                    {
                        int nowY = y + j;
                        if (nowY < 0 || nowY >= Height)
                        {
                            continue;
                        }

                        for (int i = -1; i <= 1; i++)
                        {
                            int nowX = x + i;
                            if (nowX < 0 || nowX >= Width)
                            {
                                continue;
                            }

                            int nowIndex = nowY * Width + nowX;
                            if (PanelList[nowIndex].IsMine)
                            {
                                nearMineNum++;
                            }
                        }
                    }

                    NumNearMineList.Add(nearMineNum);
                }
            }
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

        public int GetNumNearMine(int x, int y)
        {
            return NumNearMineList[CalcIndex(x, y)];
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

        public bool IsOpenedPanelMine()
        {
            List<Panel> gameOverPanel = PanelList.FindAll(x => (x.Status == Panel.PanelStatus.Opened && x.IsMine));
            return gameOverPanel.Count > 0;
        }

        public bool IsAllOpenedPanelsNotMine()
        {
            if (IsOpenedPanelMine())
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
            return (IsAllOpenedPanelsNotMine() || IsOpenedPanelMine());
        }
    }
}