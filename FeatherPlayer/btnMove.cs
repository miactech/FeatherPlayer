using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FeatherPlayer
{
    class btnMove
    {
        /// <summary>
        /// 缓动动画
        /// </summary>
        /// <param name="element">控件名</param>
        /// <param name="from">元素开始的大小</param>
        /// <param name="from">元素到达的大小</param>
        /// <param name="time">持续时间(毫秒)</param>
        public static void ScaleEasingAnimationShow(FrameworkElement element, double from, double to,int time)
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
    }
}
