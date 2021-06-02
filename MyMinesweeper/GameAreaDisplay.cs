﻿using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MyMinesweeper
{
    internal class GameAreaDisplay : IDisposable
    {
        private MainWindow Main { get; set; }

        private StackPanel PanelsArea { get; set; }

        private PanelImage PanelImage { get; set; }

        public GameAreaDisplay(MainWindow main, int panelSize)
        {
            Main = main;
            PanelImage = new PanelImage(panelSize);
            Main.PlayResultArea.Visibility = System.Windows.Visibility.Hidden;
        }

        public void Dispose()
        {
            Clear();
        }

        private void Clear()
        {
            PanelsArea.Children.Clear();
        }

        public void Update(Panels panels)
        {
            UpdateGameArea(panels);
            UpdateInformationArea(panels);
            DisplayGameFinish(panels);
        }

        private void UpdateGameArea(Panels panels)
        {
            InitPanelsArea();

            for (int y = 0; y < panels.Height; y++)
            {
                StackPanel stackPanel = CreateStackPanel();
                for (int x = 0; x < panels.Width; x++)
                {
                    Image image = CreateImage(panels, x, y);
                    stackPanel.Children.Add(image);
                }

                PanelsArea.Children.Add(stackPanel);
            }
        }

        private void InitPanelsArea()
        {
            if (PanelsArea is null)
            {
                PanelsArea = new StackPanel();
                Main.GameArea.Children.Add(PanelsArea);
            }
            else
            {
                Clear();
            }
        }

        private StackPanel CreateStackPanel()
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            return stackPanel;
        }

        private Image CreateImage(Panels panels, int x, int y)
        {
            Panel.PanelStatus status = panels.GetStatus(x, y);
            bool isMine = panels.IsMine(x, y);
            int numNearMine = panels.GetNumNearMine(x, y);
            return PanelImage.CreateImage(status, isMine, numNearMine);
        }

        private void UpdateInformationArea(Panels panels)
        {
            Main.NumPanelClosing.Content = panels.GetNumClosing().ToString();
            Main.NumPanelOpened.Content = panels.GetNumOpened().ToString();
            Main.NumMine.Content = panels.GetNumMine().ToString();
        }

        private void DisplayGameFinish(Panels panels)
        {
            if (panels.IsOpenedPanelMine())
            {
                Main.PlayResultArea.Visibility = System.Windows.Visibility.Visible;
                Main.PlayResultLabel.Content = "GameOver....";
            }
            else if (panels.IsAllOpenedPanelsNotMine())
            {
                Main.PlayResultArea.Visibility = System.Windows.Visibility.Visible;
                Main.PlayResultLabel.Content = "GameClear!";
                Main.PlayResultArea.Background = System.Windows.Media.Brushes.Blue;
            }
        }
    }
}
