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

namespace BarCode
{
    /// <summary>
    /// TimerSet.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TimerSet : UserControl
    {
        public TimerSet()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double param = Convert.ToDouble(tbox.Text);
            if(param >= 1)
            {
                tbox.Text = (param - 1).ToString();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            double param = Convert.ToDouble(tbox.Text);
            tbox.Text = (param + 1).ToString();
        }
    }
}
