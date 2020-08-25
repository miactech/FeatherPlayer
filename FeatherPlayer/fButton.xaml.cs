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

        private double _mouseEnterOpacity = 0.9;
        private double _mouseDownOpacity = 0.8;
        private double _mouseEnterScale = 0.96;
        private double _mouseDownScale = 0.93;

        /// <summary>
        /// 在鼠标移动至按钮上时 透明度的变化值
        /// <para>默认值：0.9</para>
        /// </summary>
        public double fMouseEnterOpacity
        {
            get { return _mouseEnterOpacity; }
            set { _mouseEnterOpacity = value; }
        }

        /// <summary>
        /// 在鼠标左键按在按钮上时 透明度的变化值
        /// <para>默认值：0.8</para>
        /// </summary>
        public double fMouseDownOpacity
        {
            get { return _mouseDownOpacity; }
            set { _mouseDownOpacity = value; }
        }
        /// <summary>
        /// 在鼠标移动至按钮上时的缩放程度
        /// <para>默认值：0.96</para>
        /// </summary>
        public double fMouseEnterScale
        {
            get { return _mouseEnterScale; }
            set { _mouseEnterScale = value; }
        }
        /// <summary>
        /// 在鼠标左键按在按钮上时的缩放程度
        /// <para>默认值：0.93</para>
        /// </summary>
        public double fMouseDownScale
        {
            get { return _mouseDownScale; }
            set { _mouseDownScale = value; }
        }
        /// <summary>
        /// 设置该控件的背景
        /// </summary>
        public Brush fBackground
        {
            //get { return button.Background; }
            set { button.Background = value; }
        }


        private void b_MouseEnter(object sender, MouseEventArgs e)
        {
            CustomAnimations.OpacityAnimation(button,1 , _mouseEnterOpacity, 200);
            CustomAnimations.ScaleEasingAnimationShow(button, 1, _mouseEnterScale, 300);
        }

        private void b_MouseLeave(object sender, MouseEventArgs e)
        {
            CustomAnimations.OpacityAnimation(button, _mouseEnterOpacity, 1, 200);
            CustomAnimations.ScaleEasingAnimationShow(button, _mouseEnterScale, 1, 300);
        }

        private void b_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CustomAnimations.OpacityAnimation(button, _mouseEnterOpacity, _mouseDownOpacity, 100);
            CustomAnimations.ScaleEasingAnimationShow(button, _mouseEnterScale, _mouseDownScale, 150);//500
        }

        private void b_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CustomAnimations.OpacityAnimation(button, _mouseDownOpacity, _mouseEnterOpacity, 100);
            CustomAnimations.ScaleEasingAnimationShow(button, _mouseDownScale, _mouseEnterScale, 150);
        }
    }
}
