using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Konvolucio.MI2C191223
{
    [TestFixture]
    public class UnitTests
    {

        string ParamFilePath = @"D:\BQ20Z45.xml";

        [Test]
        public void SaveFile()
        {
            ParameterManager.Instance.DeviceName = "BQ20Z45-R";
            ParameterManager.Instance.SMBusAddressSize = 1;
            ParameterManager.Instance.SMBusSpeed = 10000;
            ParameterManager.Instance.SMBusAdapter = "SIL CP2112";

            ParameterItem pi1 = new ParameterItem
            {
                Commmand = 0,
                Name = "ManufacturerAccess",
                Mode = "R/W",
                Format = "X4",
                Size = 2,
                DefaultValue = "-",
                Unit = "-",
            };

            ParameterItem pi2 = new ParameterItem
            {
                Commmand = 0,
                Name = "RemainingCapacityAlarm",
                Mode = "R/W",
                Format = "D5",
                Size = 2,
                DefaultValue = "300",
                Unit = "mAh",
            }
;
            ParameterManager.Instance.Parameters.Add(pi1);
            ParameterManager.Instance.Parameters.Add(pi2);

            ParameterManager.SaveFile(ParamFilePath);
        }
        [Test]
        public void LoadFile()
        {
            ParameterManager.LoadFromFile(ParamFilePath);
            Assert.AreEqual(2, ParameterManager.Instance.Parameters.Count);
        }
    }
}
