using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Konvolucio.MI2C191223
{
    public class IoLog
    {
        public static IoLog Instance { get; } = new IoLog();
        public string FilePath;
        public bool Enabled;

        public double? GetFileSizeKB
        {
            get
            {
                if (File.Exists(FilePath))
                {
                    FileInfo fi = new FileInfo(FilePath);
                    return fi.Length / 1024;
                }
                else
                    return null;
            }
        }

        public IoLog()
        {
            Enabled = true;
            FilePath = "IoLog.txt";
            WriteLine("**** Appliction Start ****");
        }

        public void WriteLine(string message, byte[] data)
        {
            WriteLine(message + ":" + ByteArrayLogString(data));
        }

        public void WriteLine(string message, byte[] data, string messageafter)
        {
            WriteLine(message + ":" + ByteArrayLogString(data) + ":" + messageafter);
        }

        public void WriteLine(string message)
        {
            if (Enabled)
            {
                message = DateTime.Now.ToString(AppConstants.GenericTimestampFormat, System.Globalization.CultureInfo.InvariantCulture) + ";" + message + AppConstants.NewLine;
                var fileWrite = new StreamWriter(FilePath, true, Encoding.ASCII);
                fileWrite.NewLine = AppConstants.NewLine;
                fileWrite.Write(message);
                fileWrite.Flush();
                fileWrite.Close();
            }
        }

        static string ByteArrayLogString(byte[] byteArray)
        {
            string retval = string.Empty;

            for (int i = 0; i < +byteArray.Length; i++)
                retval += string.Format("{0:X2} ", byteArray[i]);

            if (byteArray.Length > 1)
                retval = retval.Remove(retval.Length - 1, 1);
            return (retval);
        }
    }
}
