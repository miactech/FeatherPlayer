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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FeatherPlayer
{
    /// <summary>
    /// MySongList.xaml 的交互逻辑
    /// </summary>
    public partial class MySongList : Page
    {
        public MySongList()
        {
            InitializeComponent();
        }
        private void SongList1_MouseEnter(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList1, 1, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(SongList1, 1, 0.9, 500);
        }

        private void SongList1_MouseLeave(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList1, 0.9, 1, 200);
            btnMove.ScaleEasingAnimationShow(SongList1, 0.9, 1, 500);
        }

        private void SongList2_MouseEnter(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList2, 1, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(SongList2, 1, 0.9, 500);
        }

        private void SongList2_MouseLeave(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList2, 0.9, 1, 200);
            btnMove.ScaleEasingAnimationShow(SongList2, 0.9, 1, 500);
        }

        private void SongList3_MouseEnter(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList3, 1, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(SongList3, 1, 0.9, 500);
        }

        private void SongList3_MouseLeave(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList3, 0.9, 1, 200);
            btnMove.ScaleEasingAnimationShow(SongList3, 0.9, 1, 500);
        }

        private void SongList4_MouseEnter(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList4, 1, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(SongList4, 1, 0.9, 500);
        }

        private void SongList4_MouseLeave(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList4, 0.9, 1, 200);
            btnMove.ScaleEasingAnimationShow(SongList4, 0.9, 1, 500);
        }

        private void SongList5_MouseEnter(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList5, 1, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(SongList5, 1, 0.9, 500);
        }

        private void SongList5_MouseLeave(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList5, 0.9, 1, 200);
            btnMove.ScaleEasingAnimationShow(SongList5, 0.9, 1, 500);
        }

        private void SongList6_MouseEnter(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList6, 1, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(SongList6, 1, 0.9, 500);
        }

        private void SongList6_MouseLeave(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList6, 0.9, 1, 200);
            btnMove.ScaleEasingAnimationShow(SongList6, 0.9, 1, 500);
        }

        private void SongList7_MouseEnter(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList7, 1, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(SongList7, 1, 0.9, 500);
        }

        private void SongList7_MouseLeave(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList7, 0.9, 1, 200);
            btnMove.ScaleEasingAnimationShow(SongList7, 0.9, 1, 500);
        }

        private void SongList8_MouseEnter(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList8, 1, 0.9, 200);
            btnMove.ScaleEasingAnimationShow(SongList8, 1, 0.9, 500);
        }

        private void SongList8_MouseLeave(object sender, MouseEventArgs e)
        {
            btnOpacity.FloatElement(SongList8, 0.9, 1, 200);
            btnMove.ScaleEasingAnimationShow(SongList8, 0.9, 1, 500);
        }

        private void SongList1_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            btnOpacity.FloatElement(SongList1, 1, 0.8, 200);
            btnMove.ScaleEasingAnimationShow(SongList1, 0.9, 0.8, 500);
        }

        private void SongList1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            btnOpacity.FloatElement(SongList1, 0.8, 1, 200);
            btnMove.ScaleEasingAnimationShow(SongList1, 0.8, 0.9, 500);
        }
    }
}
