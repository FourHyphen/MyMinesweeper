using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MyMinesweeper
{
    internal class GameAreaDisplay
    {
        private Grid GameArea { get; set; }

        private int PanelSize { get; }

        private BitmapSource ImageClosing { get; set; }

        public GameAreaDisplay(Grid gameArea, int panelSize)
        {
            GameArea = gameArea;
            PanelSize = panelSize;
            ImageClosing = ImageProcess.GetShowImage("./Resource/Image/Closing.png", PanelSize, PanelSize);
        }

        internal void Update(Panels panels)
        {
            // TODO: 実装
            StackPanel all = new StackPanel();

            // TODO: Iteratorパターン使える
            for (int y = 0; y < panels.Height; y++)
            {
                StackPanel stackPanel = CreateStackPanel();
                for (int x = 0; x < panels.Width; x++)
                {
                    Image image = CreatePanelImage();
                    stackPanel.Children.Add(image);
                }
                all.Children.Add(stackPanel);
            }

            GameArea.Children.Add(all);
        }

        private StackPanel CreateStackPanel()
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            return stackPanel;
        }

        private Image CreatePanelImage()
        {
            Image image = new Image();
            image.Source = ImageClosing;
            return image;
        }
    }
}
