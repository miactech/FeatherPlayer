using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace FeatherPlayer
{
    class sliMove
    {
        /// <summary>
        /// 滑条缓变动画
        /// </summary>
        /// <param name="sliname">滑条名称</param>
        /// <param name="to">目标值</param>
        /// <param name="time">持续时间</param>
        public static void FloatSlider(Slider sliname,double to ,int time)
        {
            lock(sliname)
            {
                EasingFunctionBase easeFunction = new PowerEase()
                {
                    EasingMode = EasingMode.EaseOut,
                    Power = 5
                };
                DoubleAnimation slianimation = new DoubleAnimation()
                {
                    To = to,
                    EasingFunction = easeFunction,             //缓动函数
                    Duration = new TimeSpan(0, 0, 0, 0, time)  //动画播放时间
                };
                EventHandler handler = null;
                slianimation.Completed += handler = (s, e) =>
                {
                    slianimation.Completed -= handler;
                    slianimation = null;
                };
                sliname.BeginAnimation(Slider.ValueProperty,slianimation);
            }
        }
    }
}
