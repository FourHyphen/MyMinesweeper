﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyMinesweeper
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private Panels Panels { get; set; }

        private GameStatus GameStatus { get; set; }

        private GameAreaDisplay GameAreaDisplay { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            SetEnvironmentCurrentDirectory(Environment.CurrentDirectory + "../../../");  // F5開始を想定
        }

        private void SetEnvironmentCurrentDirectory(string environmentDirPath)
        {
            // TODO: 配布を考えるなら、exeと同階層にGameDataディレクトリがある場合/ない場合で分岐すべき
            Environment.CurrentDirectory = environmentDirPath;
        }

        private void MenuGameStartEasyClick(object sender, RoutedEventArgs e)
        {
            StartGame("Easy", 20);
        }

        private void MenuGameStartNormalClick(object sender, RoutedEventArgs e)
        {
            StartGame("Normal", 20);
        }

        private void StartGame(string gameMode, int panelSize)
        {
            Panels = PanelsFactory.Create(gameMode);
            GameStatus = new GameStatus();
            CreateGameAreaDisplay(panelSize);
            GameAreaDisplay.Update(Panels, GameStatus);
        }

        private void CreateGameAreaDisplay(int panelSize)
        {
            if (!(GameAreaDisplay is null))
            {
                GameAreaDisplay.Dispose();
            }
            GameAreaDisplay = new GameAreaDisplay(this, panelSize);
        }

        private void GameAreaMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameAreaMouseDown(e.GetPosition(GameArea));
        }

        private void GameAreaMouseDown(System.Windows.Point p)
        {
            if (!GameStatus.IsGameFinished(Panels))
            {
                GameAreaMouseDownCore(p);
            }
        }

        private void GameAreaMouseDownCore(System.Windows.Point p)
        {
            int x = (int)(p.X / 20.0);
            int y = (int)(p.Y / 20.0);
            if (GameStatus.GetMode() == GameStatus.GameMode.Open)
            {
                Panels.Open(x, y);
            }
            else if(GameStatus.GetMode() == GameStatus.GameMode.Flag)
            {
                Panels.AddFlag(x, y);
            }

            GameAreaDisplay.Update(Panels, GameStatus);
        }

        private void GameAreaMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            SwitchMode();
        }

        private void SwitchMode()
        {
            GameStatus.SwitchMode();
            GameAreaDisplay.Update(Panels, GameStatus);
        }
    }
}
