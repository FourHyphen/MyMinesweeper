using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MyMinesweeper
{
    public class PanelImage
    {
        private BitmapSource ImageClosing { get; }

        private BitmapSource ImageOpenedMine { get; }

        private BitmapSource ImageOpenedNearMine0 { get; }

        private BitmapSource ImageOpenedNearMine1 { get; }

        private BitmapSource ImageOpenedNearMine2 { get; }

        private BitmapSource ImageOpenedNearMine3 { get; }

        private BitmapSource ImageOpenedNearMine4 { get; }

        private BitmapSource ImageOpenedNearMine5 { get; }

        private BitmapSource ImageOpenedNearMine6 { get; }

        private BitmapSource ImageOpenedNearMine7 { get; }

        private BitmapSource ImageOpenedNearMine8 { get; }

        private BitmapSource ImageFlag { get; }

        private BitmapSource ImageQuestion { get; }

        private BitmapSource ImageMineGameOver { get; }

        private BitmapSource ImageMineGameClear { get; }

        public PanelImage(int panelSize)
        {
            ImageClosing = ImageProcess.GetShowImage("./Resource/Image/Closing.png", panelSize, panelSize);
            ImageOpenedMine = ImageProcess.GetShowImage("./Resource/Image/OpenedMine.png", panelSize, panelSize);
            ImageOpenedNearMine0 = ImageProcess.GetShowImage("./Resource/Image/Opened0.png", panelSize, panelSize);
            ImageOpenedNearMine1 = ImageProcess.GetShowImage("./Resource/Image/Opened1.png", panelSize, panelSize);
            ImageOpenedNearMine2 = ImageProcess.GetShowImage("./Resource/Image/Opened2.png", panelSize, panelSize);
            ImageOpenedNearMine3 = ImageProcess.GetShowImage("./Resource/Image/Opened3.png", panelSize, panelSize);
            ImageOpenedNearMine4 = ImageProcess.GetShowImage("./Resource/Image/Opened4.png", panelSize, panelSize);
            ImageOpenedNearMine5 = ImageProcess.GetShowImage("./Resource/Image/Opened5.png", panelSize, panelSize);
            ImageOpenedNearMine6 = ImageProcess.GetShowImage("./Resource/Image/Opened6.png", panelSize, panelSize);
            ImageOpenedNearMine7 = ImageProcess.GetShowImage("./Resource/Image/Opened7.png", panelSize, panelSize);
            ImageOpenedNearMine8 = ImageProcess.GetShowImage("./Resource/Image/Opened8.png", panelSize, panelSize);
            ImageFlag = ImageProcess.GetShowImage("./Resource/Image/Flag.png", panelSize, panelSize);
            ImageQuestion = ImageProcess.GetShowImage("./Resource/Image/Question.png", panelSize, panelSize);
            ImageMineGameOver = ImageProcess.GetShowImage("./Resource/Image/MineGameOver.png", panelSize, panelSize);
            ImageMineGameClear = ImageProcess.GetShowImage("./Resource/Image/MineGameClear.png", panelSize, panelSize);
        }

        public Image CreateImage(Panel.PanelStatus status, bool isMine, int numNearMine, bool isGameOver, bool isGameClear)
        {
            if (isGameOver)
            {
                return CreateImageWhenGameOver(status, isMine, numNearMine);
            }
            else if (isGameClear)
            {
                return CreateImageWhenGameClear(status, isMine, numNearMine);
            }
            else
            {
                return CreateImage(status, isMine, numNearMine);
            }
        }

        private Image CreateImageWhenGameOver(Panel.PanelStatus status, bool isMine, int numNearMine)
        {
            Image image = new Image();
            if (status == Panel.PanelStatus.Opened)
            {
                SetImageOpened(isMine, numNearMine, ref image);
            }
            else
            {
                if (isMine)
                {
                    image.Source = ImageMineGameOver;
                    image.Name = "MineGameOver";
                }
                else
                {
                    SetImageClosing(ref image);
                }
            }

            return image;
        }

        private Image CreateImageWhenGameClear(Panel.PanelStatus status, bool isMine, int numNearMine)
        {
            Image image = new Image();
            if (status == Panel.PanelStatus.Opened)
            {
                SetImageOpened(isMine, numNearMine, ref image);
            }
            else
            {
                if (isMine)
                {
                    image.Source = ImageMineGameClear;
                    image.Name = "MineGameClear";
                }
                else
                {
                    SetImageClosing(ref image);
                }
            }

            return image;
        }

        private Image CreateImage(Panel.PanelStatus status, bool isMine, int numNearMine)
        {
            Image image = new Image();
            if (status == Panel.PanelStatus.Closing)
            {
                SetImageClosing(ref image);
            }
            else if (status == Panel.PanelStatus.Flag)
            {
                SetImageFlag(ref image);
            }
            else if (status == Panel.PanelStatus.Question)
            {
                SetImageQuestion(ref image);
            }
            else if (status == Panel.PanelStatus.Opened)
            {
                SetImageOpened(isMine, numNearMine, ref image);
            }

            return image;
        }

        private void SetImageClosing(ref Image image)
        {
            image.Source = ImageClosing;
            image.Name = "Closing";
        }

        private void SetImageFlag(ref Image image)
        {
            image.Source = ImageFlag;
            image.Name = "Flag";
        }

        private void SetImageQuestion(ref Image image)
        {
            image.Source = ImageQuestion;
            image.Name = "Question";
        }

        private void SetImageOpened(bool isMine, int numNearMine, ref Image image)
        {
            if (isMine)
            {
                image.Source = ImageOpenedMine;
                image.Name = "OpenedMine";
            }
            else
            {
                if (numNearMine == 0)
                {
                    image.Source = ImageOpenedNearMine0;
                    image.Name = "Opened0";
                }
                else if (numNearMine == 1)
                {
                    image.Source = ImageOpenedNearMine1;
                    image.Name = "Opened1";
                }
                else if (numNearMine == 2)
                {
                    image.Source = ImageOpenedNearMine2;
                    image.Name = "Opened2";
                }
                else if (numNearMine == 3)
                {
                    image.Source = ImageOpenedNearMine3;
                    image.Name = "Opened3";
                }
                else if (numNearMine == 4)
                {
                    image.Source = ImageOpenedNearMine4;
                    image.Name = "Opened4";
                }
                else if (numNearMine == 5)
                {
                    image.Source = ImageOpenedNearMine5;
                    image.Name = "Opened5";
                }
                else if (numNearMine == 6)
                {
                    image.Source = ImageOpenedNearMine6;
                    image.Name = "Opened6";
                }
                else if (numNearMine == 7)
                {
                    image.Source = ImageOpenedNearMine7;
                    image.Name = "Opened7";
                }
                else if (numNearMine == 8)
                {
                    image.Source = ImageOpenedNearMine8;
                    image.Name = "Opened8";
                }
            }
        }
    }
}
