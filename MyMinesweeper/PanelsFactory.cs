using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMinesweeper
{
    public class PanelsFactory
    {
        public static Panels Create(string mode)
        {
            string modeLower = mode.ToLower();
            if (modeLower == "easy")
            {
                return CreateEasy();
            }
            else if (modeLower == "debug")
            {
                return CreateDebug();
            }
            else if (modeLower == "debug2")
            {
                return CreateDebug2();
            }

            return null;
        }

        private static Panels CreateEasy()
        {
            List<Panel> panelList = new List<Panel>();
            Panel plain = new Panel(false);
            Panel mine = new Panel(true);

            int width = 9;
            int height = 9;
            int mineNum = 15;
            for (int i = 0; i < width * height; i++)
            {
                if (i < mineNum)
                {
                    panelList.Add(mine);
                }
                else
                {
                    panelList.Add(plain);
                }
            }

            // シャッフル参考: https://dobon.net/vb/dotnet/programing/arrayshuffle.html
            List<Panel> panelListRandom = panelList.OrderBy(i => Guid.NewGuid()).ToList();

            return CreateCore(panelListRandom, 9, 9);
        }

        private static Panels CreateDebug()
        {
            List<Panel> panelList = new List<Panel>();
            Panel plain = new Panel(false);
            Panel mine = new Panel(true);
            panelList.Add(plain.Clone()); panelList.Add(plain.Clone()); panelList.Add(plain.Clone()); panelList.Add(plain.Clone()); panelList.Add(plain.Clone());    // ■■■■■
            panelList.Add(plain.Clone()); panelList.Add(plain.Clone()); panelList.Add(plain.Clone()); panelList.Add(plain.Clone()); panelList.Add(plain.Clone());    // ■■■■■
            panelList.Add(plain.Clone()); panelList.Add(plain.Clone()); panelList.Add(plain.Clone()); panelList.Add(plain.Clone()); panelList.Add(plain.Clone());    // ■■■■■
            panelList.Add(plain.Clone()); panelList.Add(plain.Clone()); panelList.Add(plain.Clone()); panelList.Add(mine.Clone()); panelList.Add(plain.Clone());     // ■■■★■
            panelList.Add(mine.Clone()); panelList.Add(plain.Clone()); panelList.Add(plain.Clone()); panelList.Add(plain.Clone()); panelList.Add(plain.Clone());     // ★■■■■

            return CreateCore(panelList, 5, 5);
        }

        private static Panels CreateDebug2()
        {
            List<Panel> panelList = new List<Panel>();
            Panel p = new Panel(false);
            Panel m = new Panel(true);
            panelList.Add(p.Clone()); panelList.Add(p.Clone()); panelList.Add(m.Clone()); panelList.Add(m.Clone()); panelList.Add(m.Clone()); panelList.Add(p.Clone());    // ０１★★★１
            panelList.Add(p.Clone()); panelList.Add(p.Clone()); panelList.Add(p.Clone()); panelList.Add(p.Clone()); panelList.Add(p.Clone()); panelList.Add(p.Clone());    // １３５６５３
            panelList.Add(p.Clone()); panelList.Add(m.Clone()); panelList.Add(m.Clone()); panelList.Add(m.Clone()); panelList.Add(m.Clone()); panelList.Add(m.Clone());    // ２★★★★★
            panelList.Add(p.Clone()); panelList.Add(m.Clone()); panelList.Add(p.Clone()); panelList.Add(m.Clone()); panelList.Add(p.Clone()); panelList.Add(m.Clone());    // ３★７★８★
            panelList.Add(p.Clone()); panelList.Add(m.Clone()); panelList.Add(p.Clone()); panelList.Add(m.Clone()); panelList.Add(m.Clone()); panelList.Add(m.Clone());    // ２★４★★★

            return CreateCore(panelList, 6, 5);
        }

        private static Panels CreateCore(List<Panel> panelList, int width, int height)
        {
            return new Panels(panelList, width, height);
        }
    }
}
