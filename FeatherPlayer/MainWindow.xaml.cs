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
using System.Windows.Controls;

namespace FeatherPlayer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Geometry pausedata, continuedata;//initialize the icons
        bool isSliderChanging = true; //滑条是否能改变，默认为true
        MusicPlayer player;
        public MainWindow()
        {
            string continuedatastr = "M10,16.5V7.5L16,12M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z";
            string pausestr = "M15,16H13V8H15M11,16H9V8H11M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z";
            continuedata = Geometry.Parse(continuedatastr);
            pausedata = Geometry.Parse(pausestr);

            player = new MusicPlayer(MusicPlayer.GetDefaultWasapiOutDevice());
            player.PlaybackStopped += Player_PlaybackStopped;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);

            timer.Tick += new EventHandler((object s1, EventArgs e1) =>
            {
            if (isSliderChanging)
                {
                    sliSong.Value = player.Position.TotalMilliseconds;
                    lblPosition.Content = string.Format("{0:mm\\:ss} / {1:mm\\:ss}", player.Position, player.Length); }
                }
            );

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
            CustomAnimations.OpacityAnimation(btnPlayStop, 0.9, 200);
            CustomAnimations.ScaleEasingAnimationShow(btnPlayStop, 1, 0.9, 500);
        }

        private void PlayStop_MouseLeave(object sender, MouseEventArgs e)
        {
            CustomAnimations.OpacityAnimation(btnPlayStop, 1, 200);
            CustomAnimations.ScaleEasingAnimationShow(btnPlayStop, 0.9, 1, 500);
        }

        private void Back_MouseEnter(object sender, MouseEventArgs e)
        {
            CustomAnimations.OpacityAnimation(Back, 0.9, 200);
            CustomAnimations.ScaleEasingAnimationShow(Back, 1, 0.9, 500);
        }

        private void Back_MouseLeave(object sender, MouseEventArgs e)
        {
            CustomAnimations.OpacityAnimation(Back, 1, 200);
            CustomAnimations.ScaleEasingAnimationShow(Back, 0.9, 1, 500);
        }

        private void Next_MouseEnter(object sender, MouseEventArgs e)
        {
            CustomAnimations.OpacityAnimation(Next, 0.9, 200);
            CustomAnimations.ScaleEasingAnimationShow(Next, 1, 0.9, 500);
        }

        private void Next_MouseLeave(object sender, MouseEventArgs e)
        {
            CustomAnimations.OpacityAnimation(Next, 1, 200);
            CustomAnimations.ScaleEasingAnimationShow(Next, 0.9, 1, 500);
        }

        private void SongPic_MouseEnter(object sender, MouseEventArgs e)
        {
            CustomAnimations.OpacityAnimation(gridCover, 0.9, 200);
            CustomAnimations.ScaleEasingAnimationShow(gridCover, 1, 0.9, 500);
        }

        private void SongPic_MouseLeave(object sender, MouseEventArgs e)
        {
            CustomAnimations.OpacityAnimation(gridCover, 1, 200);
            CustomAnimations.ScaleEasingAnimationShow(gridCover, 0.9, 1, 500);
        }
        DispatcherTimer timer = null;
        private void PlayStop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //isVoice = false;
            ///isSliderChanging = true;
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
                        
                        gridCover.Background = System.Windows.Media.Brushes.White;
                        if (track.EmbeddedPictures.Count != 0)
                        {
                            MemoryStream ms = new MemoryStream(track.EmbeddedPictures[0].PictureData);
                            System.Drawing.Image.GetThumbnailImageAbort callb = new System.Drawing.Image.GetThumbnailImageAbort(() => { return false; });
                            System.Drawing.Image cover = System.Drawing.Image.FromStream(ms).GetThumbnailImage(47, 47, callb, IntPtr.Zero);
                            gridCover.Background = new System.Windows.Media.ImageBrush(ToImageSource(cover));
                            
                        }                  
                        fileName = opFile.FileName;
                        //打开文件
                        player.Open(fileName);

                        lblSongInformation.Content = string.Format("{0}kHz / {1}Bit", player.SampleRate / 1000, player.BitDepth);
                        sliSong.Maximum = player.Length.TotalMilliseconds;
                        player.Play();
                        player.Volume = 50;
                        PlayStop.Data = pausedata;
                        //改变播放进度

                        player.PlaybackStopped += new EventHandler<PlaybackStoppedEventArgs>((object s2,PlaybackStoppedEventArgs pse) => {
                            timer.Stop();
                        });
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

        private ImageSource ToImageSource(System.Drawing.Image image)
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
            isSliderChanging = false;
        }

        private void sliSong_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //手动调整位置
            isSliderChanging = false;
            TimeSpan tsNewValue = TimeSpan.FromMilliseconds(sliSong.Value);
            player.Position = tsNewValue; //更改位置
            lblPosition.Content = string.Format("{0:mm\\:ss} / {1:mm\\:ss}", player.Position, player.Length);
            isSliderChanging = true;
        }

        private void sliSong_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) 
        {

        }

        private void sliSong_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            player.Stop();
            timer.Stop();
            CustomAnimations.FloatSlider(sliSong, 0, 200);
        }

        private void Next_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            sliSong.Value = 0;
            //CustomAnimations.FloatSlider(sliSong,0, 700);
            player.Stop();
            timer.Stop();
        }

        private void sliSong_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //右键切换为音量条效果
            //isSliderChanging = true;
            CustomAnimations.FloatSlider(sliSong, 0, 400);
            sliVolume.Opacity = 1;
            lblVolume.Content = player.Volume;
            lblPosition.Opacity = 0;
            lblVolume.Opacity = 1;
            CustomAnimations.FloatSlider(sliVolume, player.Volume, 400);
            sliSong.Opacity = 0;  
        }

        private void sliVolume_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //如果切换为进度条
            sliVolume.Opacity = 0;
            lblVolume.Opacity = 0;
            sliSong.Opacity = 1;
            CustomAnimations.FloatSlider(sliSong, player.Position.TotalMilliseconds, 400);
            //sliSong.Value = player.Position.TotalMilliseconds;
            lblPosition.Opacity = 1;
            lblPosition.Content = string.Format("{0:mm\\:ss} / {1:mm\\:ss}", player.Position, player.Length); //同步位置
        }

        private void sliVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Volume = Convert.ToInt32(sliVolume.Value);
            lblVolume.Content = Convert.ToInt32(sliVolume.Value);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("当前参数.\n\r当前isSliderChanging状态：" + isSliderChanging + "\n\r\n\r当前player音量：" + player.Volume.ToString() + ".\n\r当前player位置：" + player.Position.TotalMilliseconds.ToString() + "\n\r\n\r当前sliSong的Value：" + sliSong.Value.ToString() + "\n\r当前timer刻度：" + timer.Interval.TotalMilliseconds.ToString(), "调试信息");
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
