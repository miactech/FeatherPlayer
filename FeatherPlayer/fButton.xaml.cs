using System;
using System.Collections.Generic;
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
    /// fButton.xaml 的交互逻辑
    /// </summary>
    public partial class fButton : UserControl
    {
        public fButton()
        {
            InitializeComponent();
        }
        private void b_MouseEnter(object sender, MouseEventArgs e)
        {
            OpacityAnimation(button, 0.9, 200);
            ScaleEasingAnimationShow(button, 1, 0.96, 300);
        }

        private void b_MouseLeave(object sender, MouseEventArgs e)
        {
            OpacityAnimation(button, 1, 200);
            ScaleEasingAnimationShow(button, 0.96, 1, 300);
        }


        private void b_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpacityAnimation(button, 0.8, 100);
            ScaleEasingAnimationShow(button, 0.96, 0.93, 150);//500
        }

        private void b_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OpacityAnimation(button, 0.9, 100);
            ScaleEasingAnimationShow(button, 0.93, 0.96, 150);
        }

        /// <summary>
        /// 透明度动画
        /// </summary>
        /// <param name="elem">控件名</param>
        /// <param name="to">目标透明度</param>
        /// <param name="time">时间</param>
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
        /// 缓动缩放动画
        /// </summary>
        /// <param name="element">控件名</param>
        /// <param name="from">元素开始的大小</param>
        /// <param name="to">元素到达的大小</param>
        /// <param name="time">持续时间(毫秒)</param>
        public static void ScaleEasingAnimationShow(UIElement element, double from, double to, int time)
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
