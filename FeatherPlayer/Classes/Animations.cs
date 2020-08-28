using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace FeatherPlayer
{
    public class CustomAnimations
    {
        /// <summary>
        /// 滑条缓变动画
        /// </summary>
        /// <param name="sliname">指定控件</param>
        /// <param name="to">目标值</param>
        /// <param name="time">持续时间(ms)</param>
        public static void FloatSlider(Slider sliname, double to, int time)
        {
            lock (sliname)
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
                    Duration = TimeSpan.FromMilliseconds(time)  //动画播放时间
                };
                sliname.BeginAnimation(Slider.ValueProperty, slianimation);

            }
        }

        /// <summary>
        /// 滑条缓变动画
        /// </summary>
        /// <param name="sliname">指定控件</param>
        /// <param name="from">起始值</param>
        /// <param name="to">目标值</param>
        /// <param name="time">持续时间(ms)</param>
        public static void FloatSlider(Slider sliname, double from, double to, int time)
        {
            lock (sliname)
            {
                EasingFunctionBase easeFunction = new PowerEase()
                {
                    EasingMode = EasingMode.EaseOut,
                    Power = 5
                };
                DoubleAnimation slianimation = new DoubleAnimation()
                {
                    From = from,
                    To = to,
                    EasingFunction = easeFunction,             //缓动函数
                    Duration = TimeSpan.FromMilliseconds(time)  //动画播放时间
                };
                EventHandler handler = null;
                slianimation.Completed += handler = (s, e) =>
                {
                    sliname.Value = to;
                    slianimation.Completed -= handler;
                    slianimation = null;
                };
                sliname.BeginAnimation(Slider.ValueProperty, slianimation);
            }
        }

        /// <summary>
        /// 缓动缩放动画
        /// </summary>
        /// <param name="element">控件名</param>
        /// <param name="from">元素开始的大小</param>
        /// <param name="to">元素到达的大小</param>
        /// <param name="time">持续时间(毫秒)</param>
        public static void ScaleEasingAnimationShow(FrameworkElement element, double from, double to, int time)
        {
            lock (element)
            {
                ScaleTransform scale = new ScaleTransform();
                element.RenderTransform = scale;
                element.RenderTransformOrigin = new Point(0.5, 0.5);//定义圆心位置        
                EasingFunctionBase easeFunction = new PowerEase()
                {
                    EasingMode = EasingMode.EaseOut,
                    Power = 5
                };
                DoubleAnimation scaleAnimation = new DoubleAnimation()
                {
                    From = from,                                   //起始值
                    To = to,                                     //结束值
                    EasingFunction = easeFunction,                    //缓动函数
                    Duration = new TimeSpan(0, 0, 0, 0, time)  //动画播放时间
                };
                AnimationClock clock = scaleAnimation.CreateClock();
                scale.ApplyAnimationClock(ScaleTransform.ScaleXProperty, clock);
                scale.ApplyAnimationClock(ScaleTransform.ScaleYProperty, clock);
            }
        }

        /// <summary>
        /// 透明度动画
        /// </summary>
        /// <param name="elem">控件名</param>
        /// <param name="to">目标透明度</param>
        /// <param name="time">动画时间</param>
        public static void OpacityAnimation(UIElement elem, double to, int time)
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

        /// <summary>
        /// 透明度动画
        /// </summary>
        /// <param name="elem">控件名</param>
        /// <param name="from">起始透明度</param>
        /// <param name="to">目标透明度</param>
        /// <param name="time">动画时间</param>
        public static void OpacityAnimation(UIElement elem, double from, double to, int time)
        {
            lock (elem)
            {
                if (to == 1)
                {
                    elem.Visibility = Visibility.Visible;
                }
                DoubleAnimation opacity = new DoubleAnimation()
                {
                    From = from,
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

        public static void DelayTime(int mm)
        {
            DateTime current = DateTime.Now;
            while (current.AddMilliseconds(mm) > DateTime.Now)
            {
                DispatcherHelper.DoEvents();
            }
            return;
        }
        public static class DispatcherHelper
        {
            [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            public static void DoEvents()
            {
                DispatcherFrame frame = new DispatcherFrame();
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrames), frame);
                try { Dispatcher.PushFrame(frame); }
                catch (InvalidOperationException) { }
            }
            private static object ExitFrames(object frame)
            {
                ((DispatcherFrame)frame).Continue = false;
                return null;
            }
        }

    }
}
