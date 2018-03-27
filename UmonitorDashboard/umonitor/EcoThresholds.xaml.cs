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
using System.Windows.Shapes;

namespace umonitor
{
    /// <summary>
    /// Interaction logic for EcoThresholds.xaml
    /// </summary>
    public partial class EcoThresholds : Window
    {
        public EcoThresholds()
        {
            InitializeComponent();
            tText.Text = MainWindow.tLTemp.ToString();
            sText.Text = MainWindow.tLSatu.ToString();
            bText.Text = MainWindow.tLBp.ToString();
            b3Text.Text = MainWindow.tBpD.ToString();
            pText.Text = MainWindow.tLPulse.ToString();
            t2Text.Text = MainWindow.tTemp.ToString();
            s2Text.Text = MainWindow.tSatu.ToString();
            b2Text.Text = MainWindow.tBp.ToString();
            b3LText.Text = MainWindow.tLBpD.ToString();
            p2Text.Text = MainWindow.tPulse.ToString();
        }

        private void btnSetL_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.tTemp = Double.Parse(t2Text.Text);
            MainWindow.tSatu = Double.Parse(s2Text.Text);
            MainWindow.tBp = Double.Parse(b2Text.Text);
            MainWindow.tPulse = Double.Parse(p2Text.Text);
            MainWindow.tLTemp = Double.Parse(tText.Text);
            MainWindow.tLSatu = Double.Parse(sText.Text);
            MainWindow.tLBp = Double.Parse(bText.Text);
            MainWindow.tLPulse = Double.Parse(pText.Text);

            this.Close();

        }
    }
}
