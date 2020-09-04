using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SLAB_HID_DEVICE;

namespace Konvolucio.MI2C191223
{
    class Program
    {
        static void Main(string[] args)
        {
            new App();
        }
    }


    class App
    {
        internal const byte SBS_MANUFACTURE_ACCESS = 0x00;
        internal const byte SBS_CYCLE_COUNT = 0x17;
        internal const byte SBS_AVERAGE_VOLTAGE = 0x5d;
        internal const byte SBS_MANUFACTURER_NAME = 0x20; 

        internal const ushort MNFA_AVERAGE_VOLTAGE = 0x005d;
        internal const ushort MNFA_DEVICE_TYPE = 0x0001;


        ushort vid = 0x10C4;
        ushort pid = 0xEA90;
        IntPtr connectedDevice;
        /*|Start| 0xB |WR| BYTE1|BYTE2*/
        const byte SlaveAddress = (0x0B << 1);

        public App()
        {



            int status = 0;
            ushort bytes2read = 2;
            byte[] readbuff = new byte[61];
            byte[] valData = new byte[bytes2read];

            var i = SLABHIDDevice_DLL.HidDevice_GetNumHidDevices(0x10C4, 0xEA90);

            if (i == 0)
                throw new Exception("CP2112 not present");


            status = SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_Open(ref connectedDevice, 0, vid, pid);
            status = SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_SetSmbusConfig(connectedDevice, 20000, SlaveAddress, 0, 10, 10, 0, 2);
            status = SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_SetGpioConfig(connectedDevice, 0x20, 0x20, 0x13, 0xFF);

            //MakeReport();

            //Console.WriteLine("Mnf AverageVoltage:" + BitConverter.ToUInt16(ManufactureAcessRead(MNFA_AVERAGE_VOLTAGE, 2), 0).ToString("d") + "mV");
            //Console.WriteLine("SBS AverageVoltage:" + BitConverter.ToUInt16(SbsRead(SBS_AVERAGE_VOLTAGE, 2), 0).ToString("d") + "mV");
            //Console.WriteLine("Device Type:" + BitConverter.ToUInt16(ManufactureAcessRead(MNFA_DEVICE_TYPE, 2), 0).ToString("X4"));

            //SbsWrite(SBS_CYCLE_COUNT, new byte[] { 0x01, 0x00 });
            //System.Threading.Thread.Sleep(100);  /*Kritikus*/
            //Console.WriteLine("CycleCount:" + BitConverter.ToUInt16(SbsRead(SBS_CYCLE_COUNT, 2), 0).ToString("D5"));

            SbsStringWrite(SBS_MANUFACTURER_NAME, "PowerWorkshop");
            System.Threading.Thread.Sleep(100);
            Close();



        }



        void MakeReport()
        {
            string report = @"D:\Battery report.txt";
            ParameterManager.LoadFromFile(@"D:\BQ20Z45_V2.xml");
            System.IO.File.Delete(report);
            IoLog.Instance.FilePath = report;

            foreach (ParameterItem item in ParameterManager.Instance.Parameters)
            {
                string line = item.Name + ": ";

                try
                {
                    switch (item.Format)
                    {

                        case "D3": /*000 256*/
                            {
                                line += SbsRead(item.Commmand, item.Size)[0].ToString(item.Format);
                                break;
                            }

                        case "D5": /*00000 .. 65535*/
                            {
                                line += BitConverter.ToUInt16(SbsRead(item.Commmand, item.Size), 0).ToString(item.Format);
                                break;
                            }
                        case "X4": /*0000 .. FFFF*/
                            {
                                line += "0x" + BitConverter.ToUInt16(SbsRead(item.Commmand, item.Size), 0).ToString(item.Format);
                                break;
                            }
                        case "X8": /*00000000 .. FFFFFFFF*/
                            {
                                line += "0x" + BitConverter.ToUInt32(SbsRead(item.Commmand, item.Size), 0).ToString(item.Format);
                                break;
                            }
                        case "string":
                            {
                                line += System.Text.Encoding.UTF8.GetString(SbsBlockRead(item.Commmand, item.Size));
                                break;
                            }
                        case "yyyyMMdd":
                            {
                                ushort temp = BitConverter.ToUInt16(SbsRead(item.Commmand, item.Size), 0);
                                var years = ((temp & 0xFE00) >> 9) + 1980;
                                var months = (temp & 0x01E0) >> 5;
                                var days = (temp & 0x001F);
                                line += new DateTime(years, months, days).ToString(item.Format, System.Globalization.CultureInfo.InvariantCulture);                               
                                break;
                            }

                        default:
                            {
                                throw new Exception("Format " + item.Format + " not support.");
                            }
                    }
                }
                catch (Exception ex)
                {
                    line += "Error:" + ex.Message;
                }

                line += " " + item.Unit;
                IoLog.Instance.WriteLine(line);
            }

        }


        void SbsWrite(byte command, byte[] data)
        {
            byte[] buffer = new byte[data.Length + 1];
            buffer[0] = command;
            Buffer.BlockCopy(data, 0, buffer, 1, data.Length);
            SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_WriteRequest(connectedDevice, SlaveAddress, buffer, (byte)buffer.Length);

        }

        byte[] SbsBlockRead(byte command, ushort length)
        {
            byte[] buffer = SbsRead(command, length);
            byte[] result = new byte[buffer[0]];
            Buffer.BlockCopy(buffer, 1, result, 0, buffer[0]);
            return result;
        }

        void SbsStringWrite(byte command, string str)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(str);
            SbsBlockWrtie(command, bytes);
        }
  
        void SbsBlockWrtie(byte command, byte[] data)
        {
            byte[] buffer = new byte[data.Length + 1];
            Buffer.BlockCopy(data, 0, buffer, 1, data.Length);
            buffer[0] = (byte)data.Length;
            SbsWrite(command, buffer);
        }

        byte[] ManufactureAcessRead(ushort mnfaccess, ushort length)
        {
            byte[] response;
            SbsWrite(SBS_MANUFACTURE_ACCESS, BitConverter.GetBytes(mnfaccess));
            System.Threading.Thread.Sleep(10);  /*Kritikus*/
            response = SbsRead(SBS_MANUFACTURE_ACCESS, length);
            return response;
        }

        byte[] SbsRead(byte command, ushort length)
        {
            byte iostatus = 0;
            byte[] readbuff = new byte[61];
            byte[] response = new byte[length];
            byte index = 0;

            SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_AddressReadRequest(connectedDevice, SlaveAddress, length, 1, new byte[] { command });
            System.Threading.Thread.Sleep(10); /*Kritikus*/
            SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_ForceReadResponse(connectedDevice, length);

            do
            {
                byte bytesRead = 0;
                var stat = SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_GetReadResponse(
                              connectedDevice, ref iostatus, readbuff, 61, ref bytesRead);
                if (stat == SLAB_HID_TO_SMBUS.CP2112_DLL.HID_SMBUS_READ_TIMED_OUT)
                   throw new Exception("SLAB_HID_TO_SMBUS.CP2112_DLL.HID_SMBUS_READ_TIMED_OUT" + " Command: 0x" + command.ToString("X2"));

                if (bytesRead != 0)
                {
                    Buffer.BlockCopy(readbuff, 0, response, index, bytesRead);
                    index += bytesRead;
                }

            } while (index != length);
            return response;
        }

        private bool isOpen()
        {
            int opened = 0;
            SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_IsOpened(connectedDevice, ref opened);
            return (opened == 1);
        }

        public void Close()
        {
            if (connectedDevice != null)
            {
                SLAB_HID_TO_SMBUS.CP2112_DLL.HidSmbus_Close(connectedDevice);
            }
        }

        public void CheckHidPresent()
        {


        }

    }
}
