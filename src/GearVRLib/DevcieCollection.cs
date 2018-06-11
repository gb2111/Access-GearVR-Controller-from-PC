using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;

namespace Driver4VR.GearVR
{
    internal class DevcieCollection
    {
		//internal List<OculusDevice> oculusDevices = new List<OculusDevice>();
		//internal List<DaydreamDevice> daydreamDevices = new List<DaydreamDevice>();
		internal List<GearVrDevice> gearVrDevices = new List<GearVrDevice>();
		private DeviceWatcher deviceWatcher;


		internal async Task<bool> Start()
        {
            foreach (var dev in gearVrDevices)
            {
                await dev.Start();
            }
            return true;
        }



		async internal Task<bool> StartScan()
		{

			// Additional properties we would like about the device.
			// Property strings are documented here https://msdn.microsoft.com/en-us/library/windows/desktop/ff521659(v=vs.85).aspx
			string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected", "System.Devices.Aep.Bluetooth.Le.IsConnectable" };

			// BT_Code: Example showing paired and non-paired in a single query.
			string aqsAllBluetoothLEDevices = "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")";

			deviceWatcher =
					DeviceInformation.CreateWatcher(
						aqsAllBluetoothLEDevices,
						requestedProperties,
						DeviceInformationKind.AssociationEndpoint);


			// Register event handlers before starting the watcher.
			deviceWatcher.Added += DeviceWatcher_Added;
			deviceWatcher.Updated += DeviceWatcher_Updated;
			deviceWatcher.Removed += DeviceWatcher_Removed;
			deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
			deviceWatcher.Stopped += DeviceWatcher_Stopped;


			// Start the watcher.
			deviceWatcher.Start();
			//String findStuff = "";// "System.DeviceInterface.Bluetooth.ServiceGuid:= \"{00001800-0000-1000-8000-00805f9b34fb}\"";

			//var gearVrFound = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(findStuff);

			//// this is the right way, stuff above is for debugging untill things works
			////  var devices = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(GattServiceUuids.HeartRate));



			//if (gearVrFound != null && gearVrFound.Count > 0)
			//{
			//	System.Diagnostics.Debug.WriteLine("FindAllAsync GearVR devices.Count : " + gearVrFound.Count);
			//	foreach (Windows.Devices.Enumeration.DeviceInformation device in gearVrFound)
			//	{
			//		if (device != null && device.Name.ToUpper().Contains("GEAR") && device.Kind == Windows.Devices.Enumeration.DeviceInformationKind.DeviceInterface)
			//		{
			//			var d = new GearVrDevice(device);
			//			bool init = await d.Init();
			//			if (init)
			//			{
			//				this.gearVrDevices.Add(d);
			//			}
			//		}
			//	}
			//}

			return true;
		}

		private void DeviceWatcher_Stopped(DeviceWatcher sender, object args)
		{
			Console.WriteLine(String.Format("Enumeration stopped!"));
		}

		private void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
		{
			Console.WriteLine(String.Format("Enumeration completed!"));
		}

		private void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
		{
			if (FindBluetoothLEDeviceDisplay(deviceInfoUpdate.Id) != null)
			{
				Console.WriteLine(String.Format("Updated {0}{1}", deviceInfoUpdate.Id, ""));
			}
		}

		private void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
		{
			GearVrDevice dev = FindBluetoothLEDeviceDisplay(deviceInfoUpdate.Id);
			if (dev != null)
			{
				dev.Update(deviceInfoUpdate);
			}
		}

		private void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation deviceInfo)
		{
			if (deviceInfo.Name.ToUpper().Contains("GEAR"))
			{
				if (FindBluetoothLEDeviceDisplay(deviceInfo.Id) == null)
				{
					GearVrDevice dev = new GearVrDevice(deviceInfo);
					gearVrDevices.Add(dev);
				}
			}
		}

		private GearVrDevice FindBluetoothLEDeviceDisplay(string id)
		{
			foreach (var dev in gearVrDevices)
			{
				string id0 = dev.Id;
				if (dev.Id == id)
					return dev;
			}

			return null;
		}
	}
}
