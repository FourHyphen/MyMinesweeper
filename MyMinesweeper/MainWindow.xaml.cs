using System;
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
            if (!GameStatus.IsGameFinished(Panels))
            {
                GameAreaMouseLeftButtonDownCore(p);
            }
        }

        private void GameAreaMouseLeftButtonDownCore(System.Windows.Point p)
        {
            int x = (int)(p.X / 20.0);
            int y = (int)(p.Y / 20.0);
            Panels.Open(x, y);
            GameAreaDisplay.Update(Panels);
        }

        private void GameAreaMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            GameAreaMouseRightButtonDown(e.GetPosition(GameArea));
        }

        private void GameAreaMouseRightButtonDown(System.Windows.Point p)
        {
            if (!GameStatus.IsGameFinished(Panels))
            {
                GameAreaMouseRightButtonDownCore(p);
            }
        }

        private void GameAreaMouseRightButtonDownCore(System.Windows.Point p)
        {
            int x = (int)(p.X / 20.0);
            int y = (int)(p.Y / 20.0);
            Panels.AddFlag(x, y);
            GameAreaDisplay.Update(Panels);
        }
    }
}
