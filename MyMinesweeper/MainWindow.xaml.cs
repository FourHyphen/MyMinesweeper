﻿using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace MyMinesweeper
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private int PanelSize { get; } = 20;

        private Panels Panels { get; set; }

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
            StartGame("Easy", PanelSize);
        }

        private void MenuGameStartNormalClick(object sender, RoutedEventArgs e)
        {
            StartGame("Normal", PanelSize);
        }

        private void StartGame(string gameMode, int panelSize)
        {
            Panels = PanelsFactory.Create(gameMode);
            CreateGameAreaDisplay(panelSize);
            GameAreaDisplay.Update(Panels);
        }

        private void CreateGameAreaDisplay(int panelSize)
        {
            if (!(GameAreaDisplay is null))
            {
                GameAreaDisplay.Dispose();
            }
            GameAreaDisplay = new GameAreaDisplay(this, panelSize);
        }

        private void GameAreaMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GameAreaMouseLeftButtonDown(e.GetPosition(GameArea));
        }

        private void GameAreaMouseLeftButtonDown(System.Windows.Point p)
        {
            GameAreaMouseDown(p, "Open");
        }

        private void GameAreaMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            GameAreaMouseRightButtonDown(e.GetPosition(GameArea));
        }

        private void GameAreaMouseRightButtonDown(System.Windows.Point p)
        {
            GameAreaMouseDown(p, "AddFlag");
        }

        private void GameAreaMouseDown(System.Windows.Point p, string panelsMethodName)
        {
            if (!GameStatus.IsGameFinished(Panels))
            {
                GameAreaMouseDownCore(p, panelsMethodName);
            }
        }

        private void GameAreaMouseDownCore(System.Windows.Point p, string panelsMethodName)
        {
            int x = (int)(p.X / (double)PanelSize);
            int y = (int)(p.Y / (double)PanelSize);

            MethodInfo mi = Panels.GetType().GetMethod(panelsMethodName);
            mi.Invoke(Panels, new object[] { x, y });

            GameAreaDisplay.Update(Panels);
        }
    }
}
