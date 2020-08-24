using System;
using System.ComponentModel;
using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;

namespace FeatherPlayer
{
        public class MusicPlayer : Component
        {
            private ISoundOut _soundOut;
            private IWaveSource _waveSource;
        /// <summary>
        /// 播放停止事件
        /// </summary>
            public event EventHandler<PlaybackStoppedEventArgs> PlaybackStopped;
        /// <summary>
        /// 当前播放状态。
        /// </summary>
            public PlaybackState PlaybackState
            {
                get
                {
                    if (_soundOut != null)
                        return _soundOut.PlaybackState;
                    return PlaybackState.Stopped;
                }
            }
        /// <summary>
        /// 获取当前播放位置。
        /// </summary>
            public TimeSpan Position
            {
                get
                {
                    if (_waveSource != null)
                        return _waveSource.GetPosition();
                    return TimeSpan.Zero;
                }
                set
                {
                    if (_waveSource != null)
                        _waveSource.SetPosition(value);
                }
            }
        /// <summary>
        /// 获取歌曲总长度。
        /// </summary>
            public TimeSpan Length
            {
                get
                {
                    if (_waveSource != null)
                        return _waveSource.GetLength();
                    return TimeSpan.Zero;
                }
            }
        /// <summary>
        /// 设置音量。 (0~100)
        /// </summary>
            public int Volume
            {
            
                get
                {
                    if (_soundOut != null)
                        return Math.Min(100, Math.Max((int)(_soundOut.Volume * 100), 0));
                    return 100;
                }
                set
                {
                    if (_soundOut != null)
                    {
                        _soundOut.Volume = Math.Min(1.0f, Math.Max(value / 100f, 0f));
                    }
                }
            }
        /// <summary>
        /// 打开一个音频文件。
        /// </summary>
        /// <param name="filename">音频文件名</param>
        /// <param name="device">要使用的音频设备</param>
            public void Open(string filename, MMDevice device)
            {
                CleanupPlayback();

                _waveSource =
                    CodecFactory.Instance.GetCodec(filename);
                _soundOut = new WasapiOut() { Latency = 100, Device = device };
                _soundOut.Initialize(_waveSource);
                if (PlaybackStopped != null) _soundOut.Stopped += PlaybackStopped;
            }
        /// <summary>
        /// 获取默认音频输出设备。
        /// </summary>
        /// <returns>默认音频输出设备</returns>
        public MMDevice GetDefaultWasapiOutDevice()
        {
            var mmdeviceEnumerator = new MMDeviceEnumerator();
            return mmdeviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        }


        /// <summary>
        /// 播放。
        /// </summary>
            public void Play()
            {
                if (_soundOut != null)
                    _soundOut.Play();
            }
        /// <summary>
        /// 暂停。
        /// </summary>
            public void Pause()
            {
                if (_soundOut != null)
                    _soundOut.Pause();
            }
        /// <summary>
        /// 停止播放。
        /// </summary>
            public void Stop()
            {
                if (_soundOut != null)
                    _soundOut.Stop();
            }

            private void CleanupPlayback()
            {
                if (_soundOut != null)
                {
                    _soundOut.Dispose();
                    _soundOut = null;
                }
                if (_waveSource != null)
                {
                    _waveSource.Dispose();
                    _waveSource = null;
                }
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                CleanupPlayback();
            }
        }
    }


