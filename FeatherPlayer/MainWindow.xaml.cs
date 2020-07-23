using CSCore;
using CSCore.Codecs.MP3;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FeatherPlayer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            string nextstr = "M10,16.5V7.5L16,12M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z";
            string pausestr = "M15,16H13V8H15M11,16H9V8H11M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z";
            nextdata = Geometry.Parse(nextstr);
            pausedata = Geometry.Parse(pausestr);
            soundOut = GetSoundOut();
            InitializeComponent();
        }
        public enum playStatus
        {
            Playing,
            Paused,
            Unloaded
        }

        playStatus playstatus = playStatus.Unloaded;
        ISoundOut soundOut;
        Geometry pausedata,nextdata;//initialize the icons
        private void wndMain_Loaded(object sender, RoutedEventArgs e)
        {

            Blur.EnableBlur(this);
            //AudioInfo.wav.WavInfo wi = AudioInfo.wav.GetWavInfo("test.wav");
            //MessageBox.Show("Test");
        }

        private void wndMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //this.DragMove();
        }

        private void btnExit_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation daToRed = new DoubleAnimation() {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            btnExitBackground.BeginAnimation(Canvas.OpacityProperty, daToRed);
        }

        private void btnExit_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation daToTrans = new DoubleAnimation()
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            btnExitBackground.BeginAnimation(Canvas.OpacityProperty, daToTrans);
        }

        private void btnExit_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btnExitBackground.Background = Brushes.OrangeRed;
            Application.Current.Shutdown();
        }
        MusicPlayer mp = new MusicPlayer();
        private void next_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            switch (playstatus)
            {
                case playStatus.Unloaded:
                    //nothing loaded
                    OpenFileDialog opfflac = new OpenFileDialog()
                    {
                        Title = "No music loaded. Please select a valid audio file.",
                        Filter = "FreeLosslessAudioCodec|*.flac|MPEG-3|*.mp3|Wave|*.wav"
                    };
                    if (opfflac.ShowDialog() == true)
                    {
                        FileStream fs = new FileStream(opfflac.FileName, FileMode.Open);
                        //mp.Open(opfflac.FileName,new MMDevice());
                    }
                    break;
                case playStatus.Playing:
                    soundOut.Pause();
                    playstatus = playStatus.Paused;
                    break;
                case playStatus.Paused:
                    soundOut.Resume();
                    playstatus = playStatus.Playing;
                    break;
                    

            }
        }

        private ISoundOut GetSoundOut()
        {
            if (WasapiOut.IsSupportedOnCurrentPlatform)
                return new WasapiOut();
            else
                return new DirectSoundOut();
        }

        private IWaveSource GetSoundSource(Stream stream)
        {
            // Instead of using the CodecFactory as helper, you specify the decoder directly:
            return new DmoMp3Decoder(stream);

        }
    }
}
