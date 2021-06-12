namespace MyMinesweeper
{
    public class Panel
    {
        public enum PanelStatus
        {
            Closing,
            Opened,
            Flag,
            Question
        }

        public PanelStatus Status { get; private set; } = PanelStatus.Closing;

        public bool IsMine { get; }

        public Panel(bool isMine)
        {
            IsMine = isMine;
        }

        public Panel Clone()
        {
            return new Panel(IsMine);
        }

        public void Open()
        {
            Status = PanelStatus.Opened;
        }

        public void AddFlag()
        {
            if (Status == PanelStatus.Flag)
            {
                Status = PanelStatus.Question;
            }
            else if (Status == PanelStatus.Question)
            {
                Status = PanelStatus.Closing;
            }
            else
            {
                Status = PanelStatus.Flag;
            }
        }
    }
}