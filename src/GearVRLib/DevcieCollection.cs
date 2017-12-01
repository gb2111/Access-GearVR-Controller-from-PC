using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver4VR.GearVR
{
    internal class DevcieCollection
    {
		//internal List<OculusDevice> oculusDevices = new List<OculusDevice>();
		//internal List<DaydreamDevice> daydreamDevices = new List<DaydreamDevice>();
		internal List<GearVrDevice> gearVrDevices = new List<GearVrDevice>();



        internal async Task<bool> Start()
        {
            foreach (var dev in gearVrDevices)
            {
                await dev.Start();
            }
            return true;
        }


        internal async Task<bool> Enumerate()
        {
            //Clear();
			//bool psmove = EnumeratePsMove();
			//bool dayd = await EnumerateDaydream();
			//bool oculusok = await EnumerateOculus();
			bool gearvr = await EnumerateGearVR();
            return gearvr;
        }

		async private Task<bool> EnumerateGearVR()
		{
			String findStuff = "System.DeviceInterface.Bluetooth.ServiceGuid:= \"{00001800-0000-1000-8000-00805f9b34fb}\"";

			var gearVrFound = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(findStuff);

			// this is the right way, stuff above is for debugging untill things works
			//  var devices = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(GattServiceUuids.HeartRate));



			if (gearVrFound != null && gearVrFound.Count > 0)
			{
				System.Diagnostics.Debug.WriteLine("FindAllAsync GearVR devices.Count : " + gearVrFound.Count);
				foreach (Windows.Devices.Enumeration.DeviceInformation device in gearVrFound)
				{
					if (device != null && device.Kind == Windows.Devices.Enumeration.DeviceInformationKind.DeviceInterface)
					{
						var d = new GearVrDevice(device);
						bool init = await d.Init();
						if (init)
						{
							this.gearVrDevices.Add(d);
						}
					}
				}
			}

			return true;
		}

	

    }
}
