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

        private void MenuGameStartClick(object sender, RoutedEventArgs e)
        {
            StartGame("Debug", 20);
        }

        private void StartGame(string gameMode, int panelSize)
        {
            Panels p = new Panels(gameMode);
            GameAreaDisplay gap = new GameAreaDisplay(this, panelSize);
            gap.Update(p);
        }

        private void GameAreaMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameAreaMouseDown(e.GetPosition(GameArea));
        }

        private void GameAreaMouseDown(System.Windows.Point p)
        {

        }
    }
}
