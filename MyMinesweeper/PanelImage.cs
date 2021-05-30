using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MyMinesweeper
{
    public class PanelImage
    {
        public BitmapSource ImageClosing { get; private set; }

        public BitmapSource ImageOpenedMine { get; private set; }

        public BitmapSource ImageOpened { get; private set; }

        public PanelImage(int panelSize)
        {
            ImageClosing = ImageProcess.GetShowImage("./Resource/Image/Closing.png", panelSize, panelSize);
            ImageOpenedMine = ImageProcess.GetShowImage("./Resource/Image/OpenedMine.png", panelSize, panelSize);
            ImageOpened = ImageProcess.GetShowImage("./Resource/Image/Opened.png", panelSize, panelSize);
        }

        public Image CreateImage(Panel.PanelStatus status, bool isMine)
        {
            Image image = new Image();
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
    }
}
