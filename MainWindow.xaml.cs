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
using WindowsDisplayAudioProfile.Managers;
using WindowsDisplayAudioProfile.Entities;

namespace WindowsDisplayAudioProfile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CreateProfileWindow createProfileWindow;
        private readonly ProfileManager profileManager;

        public MainWindow()
        {
            InitializeComponent();
            createProfileWindow = new CreateProfileWindow();
            createProfileWindow.IsVisibleChanged += new DependencyPropertyChangedEventHandler(CreateProfileWindowVisibleChangedHandler);
            profileManager = new ProfileManager();
            RefreshLbProfileList();
        }

        private void RefreshLbProfileList()
        {
            List<Profile> profileList = profileManager.GetAllProfiles();
            if (profileList.Count == 0)
            {
                lbProfile.Items.Add(pnlMain.FindResource("noProfileCreated").ToString());
                lbProfile.IsEnabled = false;
            }
            else
            {
                lbProfile.Items.Clear();
                foreach (Profile profile in profileList)
                {
                    lbProfile.Items.Add(profile.ProfileName);
                    lbProfile.IsEnabled = true;
                }
            }
        }

        private void btnCreateProfile_Click(object sender, RoutedEventArgs e)
        {
            createProfileWindow.Show();
        }

        private void btnLoadProfile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Btn load profile clicked");
        }

        private void CreateProfileWindowVisibleChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            RefreshLbProfileList();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            createProfileWindow.Close();
            base.OnClosing(e);
        }
    }
}
