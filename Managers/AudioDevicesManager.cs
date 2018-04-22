using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioSwitch.CoreAudioApi;

namespace WindowsDisplayAudioProfile.Managers
{
    class AudioDevicesManager
    {
        private readonly List<string> DeviceNames = new List<string>();
        private int DefaultDeviceID;

        internal readonly MMDeviceEnumerator DeviceEnumerator = new MMDeviceEnumerator();
        private readonly Dictionary<int, string> DeviceIDs = new Dictionary<int, string>();

        internal AudioDevicesManager()
        {

        }

        internal List<string> GetAllAudioDeviceNames()
        {
            RefreshDeviceList(EDataFlow.eRender);
            return DeviceNames;
        }

        internal void RefreshDeviceList(EDataFlow renderType)
        {
            DeviceNames.Clear();
            DeviceIDs.Clear();

            var pDevices = DeviceEnumerator.EnumerateAudioEndPoints(renderType, EDeviceState.Active);
            var defDeviceID = DeviceEnumerator.GetDefaultAudioEndpoint(renderType, ERole.eMultimedia).ID;
            var devCount = pDevices.Count;

            for (var i = 0; i < devCount; i++)
            {
                var device = pDevices[i];
                var devID = device.ID;
                DeviceNames.Add(device.FriendlyName);
                DeviceIDs.Add(i, devID);

                if (devID == defDeviceID)
                    DefaultDeviceID = i;
            }
        }

        internal string GetDefaultDeviceName()
        {
            return DeviceNames.ElementAt(DefaultDeviceID);
        }
    }
}
