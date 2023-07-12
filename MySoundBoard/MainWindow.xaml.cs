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
using NAudio.Wave;
using Microsoft.Win32;

namespace MySoundBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WaveOutEvent outputDevice;
        private MediaFoundationReader? audioFile;
        private List<string> soundFilePaths;
        private Button? currentlyPlayingButton = null;

        public MainWindow()
        {
            InitializeComponent();
            outputDevice = new WaveOutEvent();
            audioFile = null;
            soundFilePaths = new List<string>();
        }

        private void PlaySound(Button button, string filePath)
        {
            if (button == currentlyPlayingButton)
            {
                outputDevice.Stop();
                audioFile?.Dispose();
                audioFile = null;
                currentlyPlayingButton = null;
            }
            else
            {
                if (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    outputDevice.Stop();
                    audioFile?.Dispose();
                    audioFile = null;   
                }

                audioFile = new MediaFoundationReader(filePath);
                outputDevice.Init(audioFile);
                outputDevice.Play();
                currentlyPlayingButton = button;
            }
        }

        private void AddSoundButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio files (*.wav, *.mp3)|*.wav;*.mp3";

            if (openFileDialog.ShowDialog() == true)
            {
                string newSoundFilePath = openFileDialog.FileName;
                soundFilePaths.Add(newSoundFilePath);

                Button newSoundButton = new Button();
                newSoundButton.Content = $"Sound {soundFilePaths.Count}";
                newSoundButton.Height = 50;
                newSoundButton.Width = 100;
                newSoundButton.Click += (s, e) => PlaySound(newSoundButton, newSoundFilePath);

                SoundButtonPanel.Children.Add(newSoundButton);
            }
        }
    }
}
