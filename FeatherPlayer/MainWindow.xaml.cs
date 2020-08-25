using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using System.Windows.Threading;
using ATL.AudioData;
using ATL;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace FeatherPlayer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Geometry pausedata, continuedata;//initialize the icons
        bool isSliderChanging = false;
        MusicPlayer player;
        public MainWindow()
        {
            string continuedatastr = "M10,16.5V7.5L16,12M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z";
            string pausestr = "M15,16H13V8H15M11,16H9V8H11M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z";
            continuedata = Geometry.Parse(continuedatastr);
            pausedata = Geometry.Parse(pausestr);

            player = new MusicPlayer();
            player.PlaybackStopped += Player_PlaybackStopped;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);

            timer.Tick += new EventHandler((object s1, EventArgs e1) => {
                if (!isSliderChanging) { sliSong.Value = player.Position.TotalMilliseconds; }
                lblPosition.Content = string.Format("{0:mm\\:ss} / {1:mm\\:ss}", player.Position, player.Length);
            });

            InitializeComponent();
            //playGrid.Visibility = Visibility.Hidden;
            //hhellonice i see you ok wait i will have dinner ok.
        }

        public enum playStatus
        {
            Playing,
            Paused,
            Unloaded
        }

        private void wndMain_Loaded(object sender, RoutedEventArgs e)
        {
            Blur.EnableBlur(this);
            //AudioInfo.wav.WavInfo wi = AudioInfo.wav.GetWavInfo("test.wav");
            //MessageBox.Show("Test");
        }

        private void btnExit_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation daToRed = new DoubleAnimation() {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            btnExitBackground.BeginAnimation(OpacityProperty, daToRed);
        }

        private void btnExit_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation daToTrans = new DoubleAnimation()
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            btnExitBackground.BeginAnimation(OpacityProperty, daToTrans);
        }

        private void btnExit_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btnExitBackground.Background = System.Windows.Media.Brushes.OrangeRed;
            player.Stop();
            Application.Current.Shutdown();
        }      

        private void PlayStop_MouseEnter(object sender, MouseEventArgs e)
        {
            fButton.OpacityAnimation(btnPlayStop, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(btnPlayStop, 1, 0.9, 500);
        }

        private void PlayStop_MouseLeave(object sender, MouseEventArgs e)
        {
            fButton.OpacityAnimation(btnPlayStop, 1, 200);
            btnMove.ScaleEasingAnimationShow(btnPlayStop, 0.9, 1, 500);
        }

        private void Back_MouseEnter(object sender, MouseEventArgs e)
        {
            fButton.OpacityAnimation(Back, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(Back, 1, 0.9, 500);
        }

        private void Back_MouseLeave(object sender, MouseEventArgs e)
        {
            fButton.OpacityAnimation(Back, 1, 200);
            btnMove.ScaleEasingAnimationShow(Back, 0.9, 1, 500);
        }

        private void Next_MouseEnter(object sender, MouseEventArgs e)
        {
            fButton.OpacityAnimation(Next, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(Next, 1, 0.9, 500);
        }

        private void Next_MouseLeave(object sender, MouseEventArgs e)
        {
            fButton.OpacityAnimation(Next, 1, 200);
            btnMove.ScaleEasingAnimationShow(Next, 0.9, 1, 500);
        }

        private void SongPic_MouseEnter(object sender, MouseEventArgs e)
        {
            fButton.OpacityAnimation(gridCover, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(gridCover, 1, 0.9, 500);
        }

        private void SongPic_MouseLeave(object sender, MouseEventArgs e)
        {
            fButton.OpacityAnimation(gridCover, 1, 200);
            btnMove.ScaleEasingAnimationShow(gridCover, 0.9, 1, 500);
        }
        DispatcherTimer timer = null;
        private void PlayStop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //sliSong.IsEnabled = true;
            string fileName;          
            //int stream;
            switch (player.PlaybackState)
            {
                case PlaybackState.Stopped:
                    //nothing loaded

                    OpenFileDialog opFile = new OpenFileDialog()
                    {
                        Title = "No music loaded. Please select a valid audio file.",
                        Filter = CodecFactory.SupportedFilesFilterEn
                    };
                    
                    if (opFile.ShowDialog() == true)
                    {
                        string strFileName = opFile.FileName;
                        string dirName = Path.GetDirectoryName(strFileName);
                        //string SongName = Path.GetFileName(strFileName);//获得歌曲名称
                        FileInfo fInfo = new FileInfo(strFileName);

                        //获取歌曲信息并显示
                        Track track = new Track(fInfo.FullName,true);
                        tbTitle.Text = track.Title;
                        tbArtist.Text = track.Artist;
                        lblSongInformation.Content = string.Format("{0}kHz / {1}Bit",track.SampleRate / 1000,"16");
                        gridCover.Background = System.Windows.Media.Brushes.White;
                        if (track.EmbeddedPictures.Count != 0)
                        {
                            MemoryStream ms = new MemoryStream(track.EmbeddedPictures[0].PictureData);
                            Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(() => { return false; });
                            Image cover = Image.FromStream(ms).GetThumbnailImage(47, 47, callb, IntPtr.Zero);
                            gridCover.Background = new System.Windows.Media.ImageBrush(ToImageSource(cover));
                        }


                        //获取默认音频输出设备                    
                        fileName = opFile.FileName;
                        //打开文件
                        player.Open(fileName, player.GetDefaultWasapiOutDevice());

                        sliSong.Maximum = player.Length.TotalMilliseconds;
                        player.Play();
                        player.Volume = 10;
                        PlayStop.Data = pausedata;
                        //改变播放进度
                        
                        timer.Start();
                    }
                    break;
                case PlaybackState.Playing: //点击暂停时
                    PlayStop.Data = continuedata;
                    player.Pause();
                    timer.Stop();
                    break;
                case PlaybackState.Paused: //点击继续时
                    PlayStop.Data = pausedata;
                    player.Play();
                    timer.Start();
                    break;
            }
        }

        private ImageSource ToImageSource(Image image )
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }


        /// <summary>
        /// move window
        /// </summary>
        private void gridTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try { DragMove(); }
            catch { }
        }

        private void frmPages_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void sliSong_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isSliderChanging = true; //isSliChanged为是否能改变滑条的判断bool
        }

        private void sliSong_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isSliderChanging = false;
            TimeSpan tsNewValue = TimeSpan.FromMilliseconds(sliSong.Value);
            //double perc = sliSong.Value / sliSong.Maximum;
            //TimeSpan position = TimeSpan.FromMilliseconds(player.Length.TotalMilliseconds * perc);
            player.Position = tsNewValue; //更改位置
            lblPosition.Content = string.Format("{0:mm\\:ss} / {1:mm\\:ss}", player.Position, player.Length);
        }

        private void sliSong_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) 
        {
            
        }

        private void sliSong_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            sliMove.FloatSlider(sliSong,0, 700);
            player.Stop();
            timer.Stop();
        }

        private void Next_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            sliMove.FloatSlider(sliSong,0, 700);
            player.Stop();
            timer.Stop();
        }
        /// <summary>
        /// 播放已停止
        /// </summary>
        private void Player_PlaybackStopped(object sender, PlaybackStoppedEventArgs e)
        {
            PlayStop.Data = continuedata;
            timer.Stop();
            //sliSong.Value = 0;
            //lblPosition.Content = "00:00 / 00:00";
        }
    }
}
