using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using WindowsDisplayAudioProfile.Entities;

namespace WindowsDisplayAudioProfile.Managers
{
    class ProfileManager
    {
        private const string APP_FOLDER = "WindowsDisplayAudioProfile";
        private const string DATA_FILE = "data.xml";
        private readonly DisplayDevicesManager displayDevicesManager;
        private ProfileCollection profileCollection;

        internal ProfileManager()
        {
            displayDevicesManager = new DisplayDevicesManager();
            GetAppDataFolder(); //to create app folder if does not exist
            RefreshProfileList();
        }

        internal void CreateProfile(string profileName, string displayDeviceName, string displaySettings, string audioDeviceName)
        {
            validateProfile(profileName, displayDeviceName, displaySettings, audioDeviceName);
            DisplayDevicesManager.DEVMODE devMode = displayDevicesManager.GetDevModeForDisplaySettings(displaySettings);
            Profile newProfile = CreateProfileFromInput(profileName, displayDeviceName, devMode, audioDeviceName);
            profileCollection.AddProfile(newProfile);
            WriteProfileListToDataFile();
        }

        internal List<Profile> GetAllProfiles()
        {
            RefreshProfileList();
            return profileCollection.ProfileList;
        }

        private void validateProfile(string profileName, string displayDeviceName, string displaySettings, string audioDeviceName)
        {
            if(profileName == "" || profileName == "Enter profile name")
            {
                throw new InvalidDataException("Please enter a profile name");
            }
        }

        private List<Profile> RefreshProfileList()
        {
            ReadProfileListFromDataFile();
            return profileCollection.ProfileList;
        }

        private Profile CreateProfileFromInput(string profileName, string displayDeviceName, DisplayDevicesManager.DEVMODE displaySettings, string audioDeviceName)
        {
            Profile profile = new Profile
            {
                ProfileName = profileName,
                DisplayDeviceName = displayDeviceName,
                DisplayWidth = displaySettings.dmPelsWidth,
                DisplayHeight = displaySettings.dmPelsHeight,
                DisplayBits = displaySettings.dmBitsPerPel,
                DisplayOrientation = displaySettings.dmDisplayOrientation,
                DisplayFrequency = displaySettings.dmDisplayFrequency,
                AudioDeviceName = audioDeviceName
            };
            return profile;
        }

        private void WriteProfileListToDataFile()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(profileCollection.GetType());
            string dataFilePath = Path.Combine(GetAppDataFolder(), DATA_FILE);
            using (TextWriter writer = new StreamWriter(dataFilePath))
            {
                xmlSerializer.Serialize(writer, profileCollection);
            }
        }

        private void ReadProfileListFromDataFile()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProfileCollection));
            XDocument doc = GetDataFileXDocument();
            using (var reader = doc.Root.CreateReader())
            {
                profileCollection = (ProfileCollection)xmlSerializer.Deserialize(reader);
            }
        }

        private XDocument GetDataFileXDocument()
        {
            string dataFilePath = Path.Combine(GetAppDataFolder(), DATA_FILE);
            if (!File.Exists(dataFilePath))
            {
                InitDataFile(dataFilePath);
            }
            return XDocument.Load(dataFilePath);
        }

        private void InitDataFile(string dataFilePath)
        {
            profileCollection = new ProfileCollection();
            XmlSerializer xmlSerializer = new XmlSerializer(profileCollection.GetType());
            using (TextWriter writer = new StreamWriter(dataFilePath))
            {
                xmlSerializer.Serialize(writer, profileCollection);
            }
        }

        private string GetAppDataFolder()
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string applicationFolder = Path.Combine(appDataFolder, APP_FOLDER);
            Directory.CreateDirectory(applicationFolder);
            return applicationFolder;
        }
    }
}
