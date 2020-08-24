﻿using Microsoft.Win32;
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

namespace FeatherPlayer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Geometry pausedata, continuedata;//initialize the icons
        bool isSliderChanging = false; //进度条是否可以变化
        bool isLblPositionChanging = false; //位置文本是否可以改变
        bool isVoice = false; //是否为调节音量状态
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

            InitializeComponent();
            //playGrid.Visibility = Visibility.Hidden;
        }

        public enum playStatus
        {
            Playing,
            Paused,
            Unloaded
        }

        private void wndMain_Loaded(object sender, RoutedEventArgs e)
        {
            player.Volume = 50;
            sliVoice.Value = player.Volume;
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
        #region
        private void btnExit_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btnExitBackground.Background = Brushes.OrangeRed;
            player.Stop();
            Application.Current.Shutdown();
        }      

        private void PlayStop_MouseEnter(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(btnPlayStop, 1, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(btnPlayStop, 1, 0.9, 500);
        }

        private void PlayStop_MouseLeave(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(btnPlayStop, 0.9, 1, 200);
            btnMove.ScaleEasingAnimationShow(btnPlayStop, 0.9, 1, 500);
        }

        private void Back_MouseEnter(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(Back, 1, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(Back, 1, 0.9, 500);
        }

        private void Back_MouseLeave(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(Back, 0.9, 1, 200);
            btnMove.ScaleEasingAnimationShow(Back, 0.9, 1, 500);
        }

        private void Next_MouseEnter(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(Next, 1, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(Next, 1, 0.9, 500);
        }

        private void Next_MouseLeave(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(Next, 0.9, 1, 200);
            btnMove.ScaleEasingAnimationShow(Next, 0.9, 1, 500);
        }

        private void SongPic_MouseEnter(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongPic, 1, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(SongPic, 1, 0.9, 500);
        }

        private void SongPic_MouseLeave(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongPic, 0.9, 1, 200);
            btnMove.ScaleEasingAnimationShow(SongPic, 0.9, 1, 500);
        }
        #endregion
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

                        Track track = new Track(fInfo.FullName,true);
                        lblTitle.Content = track.Title;
                        lblArtist.Content = track.Artist;
                        
                        lblSongInformation.Content = string.Format("{0}kHz / {1}Bit",track.SampleRate / 1000,"16");

                        //获取默认音频输出设备
                        var mmdeviceEnumerator = new MMDeviceEnumerator();
                        MMDevice device = mmdeviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);                       
                        fileName = opFile.FileName;
                        //打开文件
                        player.Open(fileName, device);


                        sliSong.Maximum = player.Length.TotalMilliseconds;
                        player.Play();
                        player.Volume = 50;
                        PlayStop.Data = pausedata;
                        //改变播放进度


                        timer.Tick += new EventHandler((object s1 ,EventArgs e1) => {
                            if (!isSliderChanging) { sliSong.Value = player.Position.TotalMilliseconds; }
                            if (isLblPositionChanging) { lblPosition.Content = string.Format("{0:mm\\:ss} / {1:mm\\:ss}", player.Position, player.Length); }
                            if (isVoice)
                            {
                                player.Volume = (int)sliVoice.Value;
                                lblVoice.Content = player.Volume;
                            }
                        });

                        player.PlaybackStopped += new EventHandler<PlaybackStoppedEventArgs>((object s2,PlaybackStoppedEventArgs pse) => {
                            timer.Stop();
                        });
                        timer.Start();
                    }
                    break;
                case PlaybackState.Playing: //点击暂停时
                    PlayStop.Data = continuedata;
                    player.Pause();
                    break;
                case PlaybackState.Paused: //点击继续时
                    PlayStop.Data = pausedata;
                    player.Play();
                    break;
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
            //右键切换为音量条效果
            isVoice = true;
            sliVoice.Value = player.Volume; //同步音量
            isSliderChanging = false;
            isLblPositionChanging = false;
            sliMove.FloatSlider(sliSong, 0, 500);
            Delay.DelayTime(400);
            sliVoice.Visibility = Visibility.Visible;
            sliVoice.Value = 0;
            lblVoice.Content = player.Volume;
            lblPosition.Visibility = Visibility.Hidden;
            lblVoice.Visibility = Visibility.Visible;
            sliMove.FloatSlider(sliVoice, player.Volume, 500);
            sliSong.Visibility = Visibility.Hidden;


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
