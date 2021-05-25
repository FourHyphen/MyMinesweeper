﻿namespace MyMinesweeper
{
    internal class Panel
    {
        public enum PanelStatus
        {
            Closing,
            Opened
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
    }
}