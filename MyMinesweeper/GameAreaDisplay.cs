using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MyMinesweeper
{
    internal class GameAreaDisplay
    {
        private MainWindow Main { get; set; }

        private int PanelSize { get; }

        private BitmapSource ImageClosing { get; set; }

        private BitmapSource ImageOpenedMine { get; set; }

        private BitmapSource ImageOpened { get; set; }

        public GameAreaDisplay(MainWindow main, int panelSize)
        {
            Main = main;
            PanelSize = panelSize;
            ImageClosing = ImageProcess.GetShowImage("./Resource/Image/Closing.png", PanelSize, PanelSize);
            ImageOpenedMine = ImageProcess.GetShowImage("./Resource/Image/OpenedMine.png", PanelSize, PanelSize);
            ImageOpened = ImageProcess.GetShowImage("./Resource/Image/Opened.png", PanelSize, PanelSize);
            Main.PlayResultArea.Visibility = System.Windows.Visibility.Hidden;
        }

        public void Update(Panels panels)
        {
            UpdateGameArea(panels);
            UpdateInformationArea(panels);
        }

        private void UpdateGameArea(Panels panels)
        {
            // TODO: 実装
            StackPanel all = new StackPanel();

            // TODO: Iteratorパターン使える
            for (int y = 0; y < panels.Height; y++)
            {
                StackPanel stackPanel = CreateStackPanel();
                for (int x = 0; x < panels.Width; x++)
                {
                    Image image = CreatePanelImage(panels, x, y);
                    stackPanel.Children.Add(image);
                }
                all.Children.Add(stackPanel);
            }

            Main.GameArea.Children.Add(all);
        }

        private StackPanel CreateStackPanel()
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            return stackPanel;
        }

        private Image CreatePanelImage(Panels panels, int x, int y)
        {
            Image image = new Image();
            Panel.PanelStatus status = panels.GetStatus(x, y);
            bool isMine = panels.IsMine(x, y);
            if (status == Panel.PanelStatus.Closing)
            {
                image.Source = ImageClosing;
                image.Name = "Closing";
            }
            else if (status == Panel.PanelStatus.Opened)
            {
                if (isMine)
                {
                    image.Source = ImageOpenedMine;
                    image.Name = "OpenedMine";
                }
                else
                {
                    image.Source = ImageOpened;
                    image.Name = "Opened";
                }
            }

            return image;
        }

        private void UpdateInformationArea(Panels panels)
        {
            Main.NumPanelClosing.Content = panels.GetNumClosing().ToString();
            Main.NumPanelOpened.Content = panels.GetNumOpened().ToString();
            Main.NumMine.Content = panels.GetNumMine().ToString();
        }
    }
}
