using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsDisplayAudioProfile.Managers
{
    class DisplayDevicesManager
    {
        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean EnumDisplaySettings(
            [param: MarshalAs(UnmanagedType.LPTStr)] string lpszDeviceName,
            [param: MarshalAs(UnmanagedType.U4)] int iModeNum,
            [In, Out] ref DEVMODE lpDevMode);

        private const int ENUM_CURRENT_SETTINGS = -1;
        private const int ENUM_REGISTRY_SETTINGS = -2;

        [StructLayout(LayoutKind.Sequential,
            CharSet = CharSet.Ansi)]
        internal struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;

            // After the 32-bytes array
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmSpecVersion;

            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmDriverVersion;

            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmSize;

            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmDriverExtra;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmFields;

            public POINTL dmPosition;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayOrientation;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayFixedOutput;

            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmColor;

            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmDuplex;

            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmYResolution;

            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmTTOption;

            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmCollate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;

            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmLogPixels;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmBitsPerPel;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPelsWidth;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPelsHeight;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayFlags;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayFrequency;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmICMMethod;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmICMIntent;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmMediaType;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDitherType;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmReserved1;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmReserved2;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPanningWidth;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPanningHeight;

        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINTL
        {
            [MarshalAs(UnmanagedType.I4)]
            public int x;
            [MarshalAs(UnmanagedType.I4)]
            public int y;
        }

        private DEVMODE devMode;

        internal DisplayDevicesManager()
        {
            devMode = new DEVMODE();
        }

        internal List<string> GetAllDisplayDeviceNames()
        {
            int index = 0;
            HashSet<string> deviceNameHashSet = new HashSet<string>();
            devMode.dmSize = (ushort)Marshal.SizeOf(devMode);

            while (EnumDisplaySettings(null,
                index,
                ref devMode) == true)
            {
                deviceNameHashSet.Add(devMode.dmDeviceName.ToUpper());
                index++;
            }

            return deviceNameHashSet.ToList();
        }

        internal string GetCurrentDisplayDeviceName()
        {
            devMode.dmSize = (ushort)Marshal.SizeOf(devMode);

            if (EnumDisplaySettings(null,
                ENUM_CURRENT_SETTINGS,
                ref devMode) == true) // Succeeded
            {
                return devMode.dmDeviceName.ToUpper();
            }

            return null;
        }

        internal List<string> GetAllDisplaySettings()
        {
            devMode.dmSize = (ushort)Marshal.SizeOf(devMode);
            int index = 0;
            List<string> displaySettingList = new List<string>();

            while (EnumDisplaySettings(null,
                index,
                ref devMode) == true)
            {
                String displaySettings = GenerateDisplaySettingString(devMode);
                displaySettingList.Add(displaySettings);
                index++;
            }

            return displaySettingList;
        }

        internal List<string> GetDisplaySettingsForSelectedDisplay(string deviceName)
        {
            devMode.dmSize = (ushort)Marshal.SizeOf(devMode);
            int index = 0;
            List<string> displaySettingList = new List<string>();

            while (EnumDisplaySettings(null,
                index,
                ref devMode) == true)
            {
                if(devMode.dmDeviceName.ToUpper() == deviceName)
                {
                    String displaySettings = GenerateDisplaySettingString(devMode);
                    displaySettingList.Add(displaySettings);
                }                
                index++;
            }

            return displaySettingList;
        }

        internal string GetCurrentDisplaySettings()
        {
            devMode.dmSize = (ushort)Marshal.SizeOf(devMode);

            if (EnumDisplaySettings(null,
                ENUM_CURRENT_SETTINGS,
                ref devMode) == true) // Succeeded
            {
                return GenerateDisplaySettingString(devMode);
            }

            return null;
        }

        internal DEVMODE GetDevModeForDisplaySettings(String displaySettings)
        {
            string[] displaySettingsBreakDown = displaySettings.Split(',');

            string[] displayResolutionBreakdown = displaySettingsBreakDown[0].Split(new string[] { " by " }, StringSplitOptions.None);
            uint width = Convert.ToUInt32(displayResolutionBreakdown[0]);
            uint height = Convert.ToUInt32(displayResolutionBreakdown[1]);

            string[] bitBreakdown = displaySettingsBreakDown[1].Trim().Split(' ');
            uint bits = Convert.ToUInt32(bitBreakdown[0]);

            string[] degreesBreakdown = displaySettingsBreakDown[2].Trim().Split(' ');
            uint degrees = Convert.ToUInt32(degreesBreakdown[0]);

            string[] frequencyBreakdown = displaySettingsBreakDown[3].Trim().Split(' ');
            uint frequency = Convert.ToUInt32(frequencyBreakdown[0]);

            devMode.dmSize = (ushort)Marshal.SizeOf(devMode);
            int index = 0;

            while (EnumDisplaySettings(null,
                index,
                ref devMode) == true)
            {
                if(devMode.dmPelsWidth == width &&
                    devMode.dmPelsHeight == height &&
                    devMode.dmBitsPerPel == bits &&
                    devMode.dmDisplayOrientation == degrees &&
                    devMode.dmDisplayFrequency == frequency)
                {
                    return devMode;
                }
                index++;
            }

            throw new KeyNotFoundException("Could not find specified display settings");
        }

        private string GenerateDisplaySettingString(DEVMODE devMode)
        {
            return devMode.dmPelsWidth + " by " + devMode.dmPelsHeight + ", " +
                    devMode.dmBitsPerPel + " bit, " +
                    devMode.dmDisplayOrientation + " degrees, " +
                    devMode.dmDisplayFrequency + " hertz";
        }
    }
}
