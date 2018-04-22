using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsDisplayAudioProfile.Entities
{
    [Serializable]
    public class Profile
    {
        private string profileName;
        private string displayDeviceName;
        private uint displayWidth;
        private uint displayHeight;
        private uint displayBits;
        private uint displayOrientation;
        private uint displayFrequency;
        private string audioDeviceName;

        public string ProfileName { get => profileName; set => profileName = value; }
        public string DisplayDeviceName { get => displayDeviceName; set => displayDeviceName = value; }
        public uint DisplayWidth { get => displayWidth; set => displayWidth = value; }
        public uint DisplayHeight { get => displayHeight; set => displayHeight = value; }
        public uint DisplayBits { get => displayBits; set => displayBits = value; }
        public uint DisplayOrientation { get => displayOrientation; set => displayOrientation = value; }
        public uint DisplayFrequency { get => displayFrequency; set => displayFrequency = value; }
        public string AudioDeviceName { get => audioDeviceName; set => audioDeviceName = value; }
    }
}
