using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Text.Json;

namespace GameLauncher
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Game> games = new List<Game>(); // 게임 리스트
        public MainWindow()
        {
            InitializeComponent();
            LoadGames();
            GameList.ItemsSource = games;
        }
        private void LoadGames()// 프로그램실행하면 게임로드
        {
            if (File.Exists("games.json"))
            {
                string json = File.ReadAllText("games.json");
                games = JsonSerializer.Deserialize<List<Game>>(json);
                GameList.ItemsSource = games;
            }
        }
        private void AddGame_Click(object sender, RoutedEventArgs e) // 게임 추가 버튼 기능
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Executable Files (*.exe)|*.exe";

            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;

                Game game = new Game
                {
                    Name = Path.GetFileNameWithoutExtension(filePath),
                    Path = filePath
                };

                games.Add(game);
                GameList.Items.Refresh();
            }
            SaveGames(); //게임 추가하면 json에다가 저장
        }
        private void SaveGames()// 게임 추가하면 저장기능
        {
            string json = JsonSerializer.Serialize(games);
            File.WriteAllText("games.json", json);
        }
        private void SortByName_Click(object sender, RoutedEventArgs e)//이름순 정렬 버튼 기능
        {
            games = games.OrderBy(g => g.Name).ToList();
            GameList.ItemsSource = games;
        }
        private void RunGame_Click(object sender, RoutedEventArgs e) // 게임 실행 버튼 기능
        {
            if (GameList.SelectedItem is Game selectedGame)
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = selectedGame.Path,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("실행 실패: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("게임을 선택하세요!");
            }
        }
    }
}
