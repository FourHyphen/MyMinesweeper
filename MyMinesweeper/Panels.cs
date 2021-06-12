using System;
using System.Collections.Generic;

namespace MyMinesweeper
{
    public class Panels
    {
        private List<Panel> PanelList { get; set; }
        private List<int> NumNearMineList { get; set; } = new List<int>();
        public int Width { get; }
        public int Height { get; }

        public Panels(List<Panel> panelList, int width, int height)
        {
            PanelList = panelList;
            Width = width;
            Height = height;
            InitNumNearMineList();
        }

        private void InitNumNearMineList()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (PanelList[CalcIndex(x, y)].IsMine)
                    {
                        NumNearMineList.Add(-1);
                        continue;
                    }

                    int nearMineNum = Filter3x3(CountMine, x, y);
                    NumNearMineList.Add(nearMineNum);
                }
            }
        }

        private int Filter3x3(Func<int, int, int> func, int x, int y)
        {
            int ret = 0;
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

                    ret += func(nowX, nowY);
                }
            }

            return ret;
        }

        private int CountMine(int x, int y)
        {
            int index = CalcIndex(x, y);
            if (PanelList[index].IsMine)
            {
                return 1;
            }
            return 0;
        }

        public int GetNumClosing()
        {
            int num = GetNumPanel(Panel.PanelStatus.Closing);
            num += GetNumPanel(Panel.PanelStatus.Question);
            return num + GetNumPanel(Panel.PanelStatus.Flag);
        }

        public int GetNumOpened()
        {
            return GetNumPanel(Panel.PanelStatus.Opened);
        }

        public int GetNumFlag()
        {
            return GetNumPanel(Panel.PanelStatus.Flag);
        }

        public int GetNumQuestion()
        {
            return GetNumPanel(Panel.PanelStatus.Question);
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
            Panel.PanelStatus status = PanelList[CalcIndex(x, y)].Status;
            if (status == Panel.PanelStatus.Flag || status == Panel.PanelStatus.Question)
            {
                return;
            }

            OpenAroundPanelsNearNotMine(x, y);
        }

        private void OpenAroundPanelsNearNotMine(int x, int y)
        {
            int index = CalcIndex(x, y);
            PanelList[index].Open();
            if (!IsMine(x, y) && NumNearMineList[index] == 0)
            {
                OpenAroundPanelsNearNotMineCore(x, y);
            }
        }

        private void OpenAroundPanelsNearNotMineCore(int x, int y)
        {
            Filter3x3(OpenRecursively, x, y);
        }

        private int OpenRecursively(int x, int y)
        {
            int index = CalcIndex(x, y);
            Panel.PanelStatus status = PanelList[index].Status;
            if (status == Panel.PanelStatus.Closing ||
                status == Panel.PanelStatus.Flag ||
                status == Panel.PanelStatus.Question)
            {
                PanelList[index].Open();
                if (NumNearMineList[index] == 0)
                {
                    OpenAroundPanelsNearNotMineCore(x, y);
                }
            }

            return 0;
        }

        private int CalcIndex(int x, int y)
        {
            return y * Width + x;
        }

        public void AddFlag(int x, int y)
        {
            int index = CalcIndex(x, y);
            if (PanelList[index].Status != Panel.PanelStatus.Opened)
            {
                PanelList[CalcIndex(x, y)].AddFlag();
            }
        }

        public bool IsOpenedPanelMine()
        {
            List<Panel> panelOpenedMine = PanelList.FindAll(x => (x.Status == Panel.PanelStatus.Opened && x.IsMine));
            return panelOpenedMine.Count > 0;
        }

        public bool IsAllOpenedPanelsWithoutMine()
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
    }
}