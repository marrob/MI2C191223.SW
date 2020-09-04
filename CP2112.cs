using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SLAB_HID_DEVICE;

namespace Konvolucio.MI2C191223
{
    [TestFixture]
    public class CP2112_BQ20Z45
    {
        ushort vid = 0x10C4;
        ushort pid = 0xEA90;
        IntPtr connectedDevice;
        /*|Start| 0xB |WR| BYTE1|BYTE2*/
        const byte SlaveAddress = (0x0B << 1);
        int status = 0;
        [Test]
        public void BQ20Z45_first()
        {
            if (SLABHIDDevice_DLL.HidDevice_GetNumHidDevices(vid, pid)==0)
                throw new Exception("CP2112 not present");
            status = SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_Open(ref connectedDevice, 0, vid, pid);
            status = SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_SetSmbusConfig(connectedDevice, 20000, SlaveAddress, 0, 10, 10, 0, 2);

            string str = "DesignCapacity:" + BitConverter.ToUInt16(ParameterRead(0x18, 2), 0).ToString() + "mAh";
            Assert.AreEqual("DesignCapacity:5100mAh", str);
        
        }
        byte[] ParameterRead(byte command, ushort length)
        {
            byte iostatus = 0;
            byte[] readbuff = new byte[61];
            byte[] response = new byte[length];
            byte index = 0;

            SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_AddressReadRequest(connectedDevice, SlaveAddress, length, 1, new byte[] { command });
            System.Threading.Thread.Sleep(10);
            SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_ForceReadResponse(connectedDevice, length);

            do
            {
                byte bytesRead = 0;
                var stat = SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_GetReadResponse(
                              connectedDevice, ref iostatus, readbuff, 61, ref bytesRead);
                Console.WriteLine("stat:" + stat.ToString() + " ios status: " + iostatus.ToString() + " bytes: " + bytesRead.ToString());

                if (bytesRead != 0)
                {
                    Buffer.BlockCopy(readbuff, 0, response, index, bytesRead);
                    index += bytesRead;
                }

            } while (index != length);
            return response;
        }
    }
}
