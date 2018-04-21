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

namespace Windows_Display_Audio_Profile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            checkLbProfileListOnInit();
        }

        private void checkLbProfileListOnInit()
        {
            if (lbProfile.Items.Count == 0)
            {
                lbProfile.Items.Add(pnlMain.FindResource("noProfileCreated").ToString());
                lbProfile.IsEnabled = false;
            }
        }

        private void btnCreateProfile_Click(object sender, RoutedEventArgs e)
        {
            CreateProfileWindow createProfileWindow = new CreateProfileWindow();
            createProfileWindow.Show();
        }

        private void btnLoadProfile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Btn load profile clicked");
        }
    }
}
