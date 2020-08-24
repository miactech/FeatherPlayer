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
        /// <param name="time">持续时间</param>
        public static void FloatSlider(Slider sliname,int time)
        {
            EasingFunctionBase EFAnimation = new PowerEase()
            {
                EasingMode = EasingMode.EaseOut,
                Power = 5
            };
            DoubleAnimation SliderAnimation = new DoubleAnimation()
            {
                From = sliname.Value,
                To = sliname.Maximum,
                EasingFunction = EFAnimation,
                Duration = new TimeSpan(0, 0, 0, 0, time)  //动画播放时间
            };
        }
    }
}
