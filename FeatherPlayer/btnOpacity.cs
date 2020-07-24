using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace FeatherPlayer
{
    class btnOpacity
    {
        /// <summary>
        /// 透明度动画
        /// </summary>
        /// <param name="elem">控件名</param>
        /// <param name="to">目标透明度</param>
        /// <param name="time">时间</param>
        public static void FloatElement(UIElement elem, double to,int time)
        {
            lock (elem)
            {
                if (to == 1)
                {
                    elem.Visibility = Visibility.Visible;
                }
                DoubleAnimation opacity = new DoubleAnimation()
                {
                    To = to,
                    Duration = new TimeSpan(0, 0, 0, 0, time)
                };
                EventHandler handler = null;
                opacity.Completed += handler = (s, e) =>
                {
                    opacity.Completed -= handler;
                    if (to == 0)
                    {
                        elem.Visibility = Visibility.Collapsed;
                    }
                    opacity = null;
                };
                
                elem.BeginAnimation(UIElement.OpacityProperty, opacity);
            }
        }
    }
}
