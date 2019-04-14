using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UdpTool.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine($"Called RadioButton_Checked {e.ToString()}");
        }

        private async void FileOpenButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "ファイルを開く",
                Filter = "全てのファイル(*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                using (FileStream fs = new FileStream(dialog.FileName, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        try
                        {
                            // 処理する
                            this.SendDataBox.Text = await sr.ReadToEndAsync();
                        }
                        catch (Exception ex)
                        {
                            // 例外処理
                            Console.WriteLine($"{ex}");
                        }
                    }
                }
            }

        }

        private void FileSaveButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "ファイルを保存",
                Filter = "テキストファイル(*.txt)|*.txt;|JSONファイル(*.json)|*.json"
            };
            if (dialog.ShowDialog() == true)
            {
                
            }
            else
            {
            }
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // 0-9のみ
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }

        private void TextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // 貼り付けを許可しない
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
