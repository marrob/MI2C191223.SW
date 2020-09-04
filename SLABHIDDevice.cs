///////////////////////////////////////////////////////////////////////////// 
// SLABHIDDevice.cs 
// SLABHIDDevice.dll imports and wrapper class Version 1.5 
///////////////////////////////////////////////////////////////////////////// 

///////////////////////////////////////////////////////////////////////////// 
// Namespaces 
///////////////////////////////////////////////////////////////////////////// 

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SLAB_HID_DEVICE
{
    ///////////////////////////////////////////////////////////////////////////// 
    // SLABHIDDevice.dll Imports 
    ///////////////////////////////////////////////////////////////////////////// 

    public class SLABHIDDevice_DLL
    {
        #region SLABHIDDevice.dll Import Functions 

        [DllImport("SLABHIDDevice.dll")]
        public static extern uint HidDevice_GetNumHidDevices(ushort vid, ushort pid);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetHidString(uint deviceIndex, ushort vid, ushort pid, byte hidStringType, StringBuilder deviceString, uint deviceStringLength);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetHidIndexedString(uint deviceIndex, ushort vid, ushort pid, uint stringIndex, StringBuilder deviceString, uint deviceStringLength);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetHidAttributes(uint deviceIndex, ushort vid, ushort pid, ref ushort deviceVid, ref ushort devicePid, ref ushort deviceReleaseNumber);

        [DllImport("SLABHIDDevice.dll")]
        public static extern void HidDevice_GetHidGuid(ref Guid hidGuid);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetHidLibraryVersion(ref byte major, ref byte minor, ref int release);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_Open(ref IntPtr device, uint deviceIndex, ushort vid, ushort pid, uint numInputBuffers);

        [DllImport("SLABHIDDevice.dll")]
        public static extern int HidDevice_IsOpened(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern uint HidDevice_GetHandle(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetString(IntPtr device, byte hidStringType, StringBuilder deviceString, uint deviceStringLength);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetIndexedString(IntPtr device, uint stringIndex, StringBuilder deviceString, uint deviceStringLength);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetAttributes(IntPtr device, ref ushort deviceVid, ref ushort devicePid, ref ushort deviceReleaseNumber);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_SetFeatureReport_Control(IntPtr device, byte[] buffer, uint bufferSize);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetFeatureReport_Control(IntPtr device, byte[] buffer, uint bufferSize);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_SetOutputReport_Interrupt(IntPtr device, byte[] buffer, uint bufferSize);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetInputReport_Interrupt(IntPtr device, byte[] buffer, uint bufferSize, uint numReports, ref uint bytesReturned);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_SetOutputReport_Control(IntPtr device, byte[] buffer, uint bufferSize);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetInputReport_Control(IntPtr device, byte[] buffer, uint bufferSize);

        [DllImport("SLABHIDDevice.dll")]
        public static extern ushort HidDevice_GetInputReportBufferLength(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern ushort HidDevice_GetOutputReportBufferLength(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern ushort HidDevice_GetFeatureReportBufferLength(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern uint HidDevice_GetMaxReportRequest(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern int HidDevice_FlushBuffers(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern int HidDevice_CancelIo(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern void HidDevice_GetTimeouts(IntPtr device, ref uint getReportTimeout, ref uint setReportTimeout);

        [DllImport("SLABHIDDevice.dll")]
        public static extern void HidDevice_SetTimeouts(IntPtr device, uint getReportTimeout, uint setReportTimeout);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_Close(IntPtr device);

        #endregion
    }

    ///////////////////////////////////////////////////////////////////////////// 
    // SLABHIDDevice.dll Wrapper Class 
    ///////////////////////////////////////////////////////////////////////////// 

    public class Hid
    {
        // Protected members 
        protected IntPtr m_hid;

        // Definitions 
        #region SLABHIDDevice.dll Definitions 

        // Return Codes 
        public const byte HID_DEVICE_SUCCESS = 0x00;
        public const byte HID_DEVICE_NOT_FOUND = 0x01;
        public const byte HID_DEVICE_NOT_OPENED = 0x02;
        public const byte HID_DEVICE_ALREADY_OPENED = 0x03;
        public const byte HID_DEVICE_TRANSFER_TIMEOUT = 0x04;
        public const byte HID_DEVICE_TRANSFER_FAILED = 0x05;
        public const byte HID_DEVICE_CANNOT_GET_HID_INFO = 0x06;
        public const byte HID_DEVICE_HANDLE_ERROR = 0x07;
        public const byte HID_DEVICE_INVALID_BUFFER_SIZE = 0x08;
        public const byte HID_DEVICE_SYSTEM_CODE = 0x09;
        public const byte HID_DEVICE_UNSUPPORTED_FUNCTION = 0x0A;
        public const byte HID_DEVICE_UNKNOWN_ERROR = 0xFF;

        // Max number of USB Devices allowed 
        public const byte MAX_USB_DEVICES = 64;

        // Max number of reports that can be requested at time 
        public const uint MAX_REPORT_REQUEST_XP = 512;
        public const uint MAX_REPORT_REQUEST_2K = 200;
        public const uint DEFAULT_REPORT_INPUT_BUFFERS = 0;

        // String Types 
        public const byte HID_VID_STRING = 0x01;
        public const byte HID_PID_STRING = 0x02;
        public const byte HID_PATH_STRING = 0x03;
        public const byte HID_SERIAL_STRING = 0x04;
        public const byte HID_MANUFACTURER_STRING = 0x05;
        public const byte HID_PRODUCT_STRING = 0x06;

        // String Lengths 
        public const uint MAX_VID_LENGTH = 5;
        public const uint MAX_PID_LENGTH = 5;
        public const uint MAX_PATH_LENGTH = 260;
        public const uint MAX_SERIAL_STRING_LENGTH = 256;
        public const uint MAX_MANUFACTURER_STRING_LENGTH = 256;
        public const uint MAX_PRODUCT_STRING_LENGTH = 256;
        public const uint MAX_INDEXED_STRING_LENGTH = 256;
        public const uint MAX_STRING_LENGTH = 260;

        #endregion

        // Constructor/Destructor 
        public Hid() { }
        ~Hid() { }

        // Public Methods 
        public static uint GetNumHidDevices(ushort vid, ushort pid)
        {
            return SLABHIDDevice_DLL.HidDevice_GetNumHidDevices(vid, pid);
        }

        public static byte GetHidString(uint deviceIndex, ushort vid, ushort pid, byte hidStringType, StringBuilder deviceString, uint deviceStringLength)
        {
            return SLABHIDDevice_DLL.HidDevice_GetHidString(deviceIndex, vid, pid, hidStringType, deviceString, deviceStringLength);
        }

        public static byte GetHidIndexedString(uint deviceIndex, ushort vid, ushort pid, uint stringIndex, StringBuilder deviceString, uint deviceStringLength)
        {
            return SLABHIDDevice_DLL.HidDevice_GetHidIndexedString(deviceIndex, vid, pid, stringIndex, deviceString, deviceStringLength);
        }

        public static byte GetHidAttributes(uint deviceIndex, ushort vid, ushort pid, ref ushort deviceVid, ref ushort devicePid, ref ushort deviceReleaseNumber)
        {
            return SLABHIDDevice_DLL.HidDevice_GetHidAttributes(deviceIndex, vid, pid, ref deviceVid, ref devicePid, ref deviceReleaseNumber);
        }

        public static void GetHidGuid(ref Guid hidGuid)
        {
            SLABHIDDevice_DLL.HidDevice_GetHidGuid(ref hidGuid);
        }

        public static byte GetHidLibraryVersion(ref byte major, ref byte minor, ref int release)
        {
            return SLABHIDDevice_DLL.HidDevice_GetHidLibraryVersion(ref major, ref minor, ref release);
        }

        public static bool GetDeviceIndex(ushort vid, ushort pid, string serial, ref uint deviceIndex)
        {
            uint index = 0;
            bool found = false;

            // Iterate through each connected device and search for a device 
            // with a serial string matching the user selected device in the 
            // device list 
            for (uint i = 0; i < Hid.GetNumHidDevices(vid, pid); i++)
            {
                StringBuilder str = new StringBuilder((int)Hid.MAX_SERIAL_STRING_LENGTH);

                if (Hid.GetHidString(i, vid, pid, Hid.HID_SERIAL_STRING, str, Hid.MAX_SERIAL_STRING_LENGTH) == Hid.HID_DEVICE_SUCCESS)
                {
                    // Device serial strings match 
                    if (serial == str.ToString())
                    {
                        index = i;
                        found = true;
                        break;
                    }
                }
            }

            deviceIndex = index;

            return found;
        }

        // Connect to HID device with the VID/PID and serial string 
        // and set timeouts 
        public bool Connect(ushort vid, ushort pid, string serial, uint getReportTimeout, uint setReportTimeout)
        {
            bool connected = false;
            uint index = 0;
            bool found = false;

            // Find specified device 
            found = GetDeviceIndex(vid, pid, serial, ref index);

            // Open device with matching serial string 
            if (found)
            {
                if (Open(index, vid, pid, MAX_REPORT_REQUEST_XP) == Hid.HID_DEVICE_SUCCESS)
                {
                    // Set read/write timeouts 
                    // 
                    // Read timeouts are temporarily set to 0 ms 
                    // in the read timer and then restored 
                    SetTimeouts(getReportTimeout, setReportTimeout);

                    connected = true;
                }
            }

            return connected;
        }

        public byte Open(uint deviceIndex, ushort vid, ushort pid, uint numInputBuffers)
        {
            return SLABHIDDevice_DLL.HidDevice_Open(ref m_hid, deviceIndex, vid, pid, numInputBuffers);
        }

        public int IsOpened()
        {
            return SLABHIDDevice_DLL.HidDevice_IsOpened(m_hid);
        }

        public uint GetHandle()
        {
            return SLABHIDDevice_DLL.HidDevice_GetHandle(m_hid);
        }

        public byte GetString(byte hidStringType, StringBuilder deviceString, uint deviceStringLength)
        {
            return SLABHIDDevice_DLL.HidDevice_GetString(m_hid, hidStringType, deviceString, deviceStringLength);
        }

        public byte GetIndexedString(uint stringIndex, StringBuilder deviceString, uint deviceStringLength)
        {
            return SLABHIDDevice_DLL.HidDevice_GetIndexedString(m_hid, stringIndex, deviceString, deviceStringLength);
        }

        public byte GetAttributes(ref ushort deviceVid, ref ushort devicePid, ref ushort deviceReleaseNumber)
        {
            return SLABHIDDevice_DLL.HidDevice_GetAttributes(m_hid, ref deviceVid, ref devicePid, ref deviceReleaseNumber);
        }

        public byte SetFeatureReport_Control(byte[] buffer, uint bufferSize)
        {
            return SLABHIDDevice_DLL.HidDevice_SetFeatureReport_Control(m_hid, buffer, bufferSize);
        }

        public byte GetFeatureReport_Control(byte[] buffer, uint bufferSize)
        {
            return SLABHIDDevice_DLL.HidDevice_GetFeatureReport_Control(m_hid, buffer, bufferSize);
        }

        public byte SetOutputReport_Interrupt(byte[] buffer, uint bufferSize)
        {
            return SLABHIDDevice_DLL.HidDevice_SetOutputReport_Interrupt(m_hid, buffer, bufferSize);
        }

        public byte GetInputReport_Interrupt(byte[] buffer, uint bufferSize, uint numReports, ref uint bytesReturned)
        {
            return SLABHIDDevice_DLL.HidDevice_GetInputReport_Interrupt(m_hid, buffer, bufferSize, numReports, ref bytesReturned);
        }

        public byte SetOutputReport_Control(byte[] buffer, uint bufferSize)
        {
            return SLABHIDDevice_DLL.HidDevice_SetOutputReport_Control(m_hid, buffer, bufferSize);
        }

        public byte GetInputReport_Control(byte[] buffer, uint bufferSize)
        {
            return SLABHIDDevice_DLL.HidDevice_GetInputReport_Control(m_hid, buffer, bufferSize);
        }

        public ushort GetInputReportBufferLength()
        {
            return SLABHIDDevice_DLL.HidDevice_GetInputReportBufferLength(m_hid);
        }

        public ushort GetOutputReportBufferLength()
        {
            return SLABHIDDevice_DLL.HidDevice_GetOutputReportBufferLength(m_hid);
        }

        public ushort GetFeatureReportBufferLength()
        {
            return SLABHIDDevice_DLL.HidDevice_GetFeatureReportBufferLength(m_hid);
        }

        public uint GetMaxReportRequest()
        {
            return SLABHIDDevice_DLL.HidDevice_GetMaxReportRequest(m_hid);
        }

        public int FlushBuffers()
        {
            return SLABHIDDevice_DLL.HidDevice_FlushBuffers(m_hid);
        }

        public int CancelIo()
        {
            return SLABHIDDevice_DLL.HidDevice_CancelIo(m_hid);
        }

        public void GetTimeouts(ref uint getReportTimeout, ref uint setReportTimeout)
        {
            SLABHIDDevice_DLL.HidDevice_GetTimeouts(m_hid, ref getReportTimeout, ref setReportTimeout);
        }

        public void SetTimeouts(uint getReportTimeout, uint setReportTimeout)
        {
            SLABHIDDevice_DLL.HidDevice_SetTimeouts(m_hid, getReportTimeout, setReportTimeout);
        }

        public byte Close()
        {
            return SLABHIDDevice_DLL.HidDevice_Close(m_hid);
        }
    }
}

