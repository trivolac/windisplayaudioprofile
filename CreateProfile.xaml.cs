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
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.IO;
using System.Media;
using WindowsDisplayAudioProfile.Managers;

namespace WindowsDisplayAudioProfile
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CreateProfileWindow : Window
    {
        private readonly DisplayDevicesManager displayDevicesManager;
        private readonly AudioDevicesManager audioDevicesManager;
        private readonly ProfileManager profileManager;

        public CreateProfileWindow()
        {
            displayDevicesManager = new DisplayDevicesManager();
            audioDevicesManager = new AudioDevicesManager();
            profileManager = new ProfileManager();
            InitializeComponent();
            InitDisplaySettings();
            initAudioOutputDevices();
        }

        private void InitDisplaySettings()
        {
            LoadCbDisplayDeviceNames();
            SelectCurrentDisplayDevice();
        }

        private void LoadCbDisplayDeviceNames()
        {
            List<String> displayDeviceNames = displayDevicesManager.GetAllDisplayDeviceNames();
            ObservableCollection<string> cbDisplayDeviceList = new ObservableCollection<string>();

            foreach (string deviceName in displayDeviceNames)
            {
                cbDisplayDeviceList.Add(deviceName);
            }

            cbDisplayDeviceName.ItemsSource = cbDisplayDeviceList;
        }

        private void SelectCurrentDisplayDevice()
        {
            string currentDisplayDeviceName = displayDevicesManager.GetCurrentDisplayDeviceName();

            if(currentDisplayDeviceName == null)
            {
                cbDisplayDeviceName.SelectedIndex = 0;
                return;
            }

            for (int i = 0; i < cbDisplayDeviceName.Items.Count; i++)
            {
                string displayDeviceName = cbDisplayDeviceName.Items[i].ToString();
                if(displayDeviceName == currentDisplayDeviceName)
                {
                    cbDisplayDeviceName.SelectedIndex = i;
                    break;
                }
            }
        }

        private void LoadCbDisplaySettings(object sender, SelectionChangedEventArgs e)
        {
            ClearItemsInCbDisplaySettings();
            string selectedDisplayDeviceName = cbDisplayDeviceName.Items[cbDisplayDeviceName.SelectedIndex].ToString();
            List<string> displaySettings = displayDevicesManager.GetDisplaySettingsForSelectedDisplay(selectedDisplayDeviceName);
            cbDisplaySettings.ItemsSource = displaySettings;
            SelectCurrentDisplaySettings(selectedDisplayDeviceName);
        }

        private void SelectCurrentDisplaySettings(string selectedDisplayDeviceName)
        {
            string currentDisplayDeviceName = displayDevicesManager.GetCurrentDisplayDeviceName();

            if(currentDisplayDeviceName == selectedDisplayDeviceName)
            {
                string currentDisplaySettings = displayDevicesManager.GetCurrentDisplaySettings();
                for (int i = 0; i < cbDisplaySettings.Items.Count; i++)
                {
                    string displaySettings = cbDisplaySettings.Items[i].ToString();
                    if(displaySettings == currentDisplaySettings)
                    {
                        cbDisplaySettings.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                cbDisplaySettings.SelectedIndex = 0;
            }
        }

        private void ClearItemsInCbDisplaySettings()
        {
            cbDisplaySettings.ItemsSource = null;
        }

        private void initAudioOutputDevices()
        {
            LoadCbAudioOutputDevice();
            SelectCurrentAudioOutputDevice();
        }

        private void LoadCbAudioOutputDevice()
        {
            List<string> audioOutputDevices = audioDevicesManager.GetAllAudioDeviceNames();
            cbAudioOutputDevice.ItemsSource = audioOutputDevices;
        }

        private void SelectCurrentAudioOutputDevice()
        {
            string currentAudioOutputDevice = audioDevicesManager.GetDefaultDeviceName();
            for(int i = 0; i < cbAudioOutputDevice.Items.Count; i++)
            {
                if(currentAudioOutputDevice == cbAudioOutputDevice.Items[i].ToString())
                {
                    cbAudioOutputDevice.SelectedIndex = i;
                    break;
                }
            }
        }

        private void TbProfileName_GotFocus(object sender, RoutedEventArgs e)
        {
            tbProfileName.Text = "";
        }

        private void TbProfileName_LostFocus(object sender, RoutedEventArgs e)
        {
            if(tbProfileName.Text.Trim() == "")
            {
                tbProfileName.Text = createProfilePanel.FindResource("enterProfileName").ToString();
            }
        }

        private void BtnCreatProfile_Click(object sender, RoutedEventArgs e)
        {
            string profileName = tbProfileName.Text.Trim();
            string displayDeviceName = cbDisplayDeviceName.Text;
            string displaySettings = cbDisplaySettings.Text;
            string audioDevice = cbAudioOutputDevice.Text;
            try { 
                profileManager.CreateProfile(profileName, displayDeviceName, displaySettings, audioDevice);
                this.Visibility = Visibility.Hidden;
                ResetWindow();
            }
            catch(InvalidDataException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }catch(KeyNotFoundException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            ResetWindow();
        }

        private void ResetWindow()
        {
            tbProfileName.Text = createProfilePanel.FindResource("enterProfileName").ToString();
            SelectCurrentDisplayDevice();
            SelectCurrentDisplaySettings(displayDevicesManager.GetCurrentDisplayDeviceName());
            SelectCurrentAudioOutputDevice();
        }
    }
}
