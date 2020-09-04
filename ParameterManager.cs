using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;

namespace Konvolucio.MI2C191223
{
    public class ParameterManager
    {
        const string XmlRootElement = "mcanxProject";
        const string XmlNamespace = @"http://www.konvolucio.hu/mcanx/2016/project/content";

        private static Type[] SupportedTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(string),
                    typeof(ParameterStorage),
                    typeof(ParameterCollection),
                    typeof(ParameterItem),
                };
            }
        }

        static ParameterStorage _instance = new ParameterStorage();
        public static ParameterStorage Instance { get => _instance; }


        public static void SaveFile(string path)
        {
            var xmlFormat = new XmlSerializer(typeof(ParameterStorage), null, SupportedTypes, new XmlRootAttribute(XmlRootElement), XmlNamespace);
            using (Stream fStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xmlFormat.Serialize(fStream, Instance);
            }
        }

        public static void LoadFromFile(string path)
        {
            var xmlFormat = new XmlSerializer(typeof(ParameterStorage), null, SupportedTypes, new XmlRootAttribute(XmlRootElement), XmlNamespace);
            ParameterStorage instance;
            using (Stream fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                instance = (ParameterStorage)xmlFormat.Deserialize(fStream);
            Instance.Parameters.Clear();

            Instance.DeviceName = instance.DeviceName;
            Instance.SMBusAdapter = instance.SMBusAdapter;
            Instance.SMBusAddressSize = instance.SMBusAddressSize;
            foreach (ParameterItem item in instance.Parameters)
                Instance.Parameters.Add(item);
        }
    }



    public class ParameterStorage
    {
        public string DeviceName { get; set; }
        public int SMBusAddressSize { get; set; }
        public int SMBusSpeed { get; set; }
        public string SMBusAdapter { get; set; }

        public ParameterStorage()
        {
            Parameters = new ParameterCollection();
        }

        public  ParameterCollection Parameters { get; set; }

    }

    public class ParameterCollection : List<ParameterItem>
    {


    }

    public class ParameterItem
    {
        public byte Commmand { get; set; }
        public string Mode { get; set; } /*R,R/W*/
        public string Name { get; set; }
        public string Format { get; set; } 
        public byte Size { get; set; }
        public string DefaultValue { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public List<string> WriteHistory { get; set; }
     
        public ParameterItem()
        {

        }

        public ParameterItem(string name)
        {
            Name = name;
        }
    }
}
