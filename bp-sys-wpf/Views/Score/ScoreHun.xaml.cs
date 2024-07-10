﻿using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace bp_sys_wpf
{
    /// <summary>
    /// ScoreHun.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreHun : Window
    {
        public static ScoreHun scoreHun;
        public ScoreHun()
        {
            InitializeComponent();
            scoreHun = this;
            try { this.Background = new ImageBrush(new BitmapImage(new Uri(MainWindow.mainWindow.GetAbsoluteFilePath("Resource/gui/score_bg_h.png")))); } catch { }
            TeamName.Foreground = Config.Score.Color.TeamName;
            S.Foreground = Config.Score.Color.S;
            Win.Foreground = Config.Score.Color.Score;
            Lose.Foreground = Config.Score.Color.Score;
            All.Foreground = Config.Score.Color.Score;
            WinWord.Foreground = Config.Score.Color.Word;
            LoseWord.Foreground = Config.Score.Color.Word;
            AllWord.Foreground = Config.Score.Color.Word;
        }
        private void ScoreHun1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
