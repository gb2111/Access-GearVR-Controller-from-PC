using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;

namespace Driver4VR.GearVR
{
	internal class GearVrDevice
	{
		private DeviceInformation device;
	
		private Windows.Devices.Bluetooth.BluetoothLEDevice bleDevice;
		private Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService myService;
		private Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic notifyCharacteristic;
		private Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic writeCharacteristic;
		private byte[] eventData = new byte[60];
		private int[] eventAnalysis = new int[60 * 8];
		private int[] eventBits = new int[60 * 8];
		int eventAnalysisThr = 80;


		bool homeButton = false;
		bool backButton = false;
		bool volumeDownButton = false;
		bool volumeUpButton = false;
		bool triggerButton = false;
		bool touchpadButton = false;
		bool touchpadPressed = false;
		private int axisY = 0;
		private int axisX = 0;

		private float[] accel = new float[3];
		private float[] gyro = new float[3];
		private float[] mag = new float[3];

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

		private const int WM_SETREDRAW = 11;
		private int val1;
		private int val2;
		private int val3;
		private string deviceId;
		internal int bitStart;
		internal int bitEnd;
		internal bool startSuccess;

		private  int GetBitValue(int start, int end)
		{
			int val = 0;

			for (int i = start; i <= end; i++)
			{
				val += eventBits[i] * (1 << (i - start));
			}

			return val;
		}

		public int BitValue {
			get
			{
				return GetBitValue(bitStart, bitEnd);
			}
		}

		public string Id
		{
			get
			{
				return device.Id;
			}
		}

		public static void SuspendDrawing(Control parent)
		{
			SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
		}

		public static void ResumeDrawing(Control parent)
		{
			SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
			parent.Refresh();
		}

		internal bool IsConnected
		{
			get
			{
				return myService != null && notifyCharacteristic != null && writeCharacteristic != null;
			}
		}

		internal async Task<bool> Connect()
		{
			bool success = false;
			bleDevice = await Windows.Devices.Bluetooth.BluetoothLEDevice.FromIdAsync(deviceId);

			GattDeviceServicesResult servicesResult = await bleDevice.GetGattServicesAsync();

			if (servicesResult.Status == GattCommunicationStatus.Success) {
				IReadOnlyList<GattDeviceService>  services = servicesResult.Services;

				int servicesCount = services.Count;
				for (int i = 0; i < servicesCount; i++)
				{
					Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService service = services[i];
					string s = service.Uuid.ToString();

					if (s == "4f63756c-7573-2054-6872-65656d6f7465")
					{
						myService = service;

						//var CharResult = myService.GetAllCharacteristics();
						//Console.WriteLine("elo");
						break;
					}

				}
			}
			if (myService != null)
			{
				try
				{
					GattCharacteristicsResult charResult = await myService.GetCharacteristicsAsync();

					if(charResult.Status == GattCommunicationStatus.Success) {

						IReadOnlyList<GattCharacteristic> allCharacteristics = charResult.Characteristics;
					Guid notifyGuid = new Guid("{c8c51726-81bc-483b-a052-f7a14ea3d281}");
					Guid writeGuid = new Guid("{c8c51726-81bc-483b-a052-f7a14ea3d282}");


						foreach (Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic c in allCharacteristics)
						{
							if (
								c.CharacteristicProperties.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.Notify) &&
								c.Uuid == notifyGuid)
							{
								notifyCharacteristic = c;
							}
							else if (
								c.CharacteristicProperties.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.Write) &&
								c.Uuid == writeGuid)
							{
								writeCharacteristic = c;
							}

							if (notifyCharacteristic != null && writeCharacteristic != null)
								break;
						}

					}

					success = notifyCharacteristic != null && writeCharacteristic != null;

				}
				catch
				{
				}
			}

			return success;
		}

		internal bool kickingEvents = false;

		internal async void KickEvents()
		{
			kickingEvents = true;

			var writer = new Windows.Storage.Streams.DataWriter();
			short val = 0x0800;
			writer.WriteInt16(val);
			GattCommunicationStatus writeResult = await writeCharacteristic.WriteValueAsync(writer.DetachBuffer());

			if (writeResult == GattCommunicationStatus.Success)
			{

				writer = new Windows.Storage.Streams.DataWriter();
				val = 0x0100;
				writer.WriteInt16(val);
				writeResult = await writeCharacteristic.WriteValueAsync(writer.DetachBuffer());
			}

			kickingEvents = false;

			bool startSuccess = writeResult == GattCommunicationStatus.Success;

		}

		internal void Update(DeviceInformationUpdate deviceInfoUpdate)
		{
			
			Console.WriteLine(String.Format("Updated {0} - {1}", deviceInfoUpdate.Id, deviceInfoUpdate.Kind));


			this.device.Update(deviceInfoUpdate);
		}

		internal  void Draw(RichTextBox richText)
		{
			SuspendDrawing(richText);
			richText.Text = "";
			int curLen, nextLen;

			curLen = richText.TextLength;
			richText.Text += "00-0000    ";
			nextLen = richText.TextLength;
			richText.Select(curLen, nextLen);
			richText.SelectionColor = System.Drawing.Color.Black;

			if (eventData.Length > 4)
			{
				int byteNo = 0;
				int newLineStep = 2 * 8;
				for (int i = 0; i < eventAnalysis.Length; i++)
				{
					if (i != 0 && i % newLineStep == 0)
					{
						byteNo++;
						curLen = richText.TextLength;
						richText.AppendText(
							Environment.NewLine +
							(byteNo).ToString().PadLeft(2, '0') +
							"-"+
							(byteNo*8).ToString().PadLeft(4, '0') +
							":   ");
						
						nextLen = richText.TextLength;
						richText.Select(curLen, nextLen);
						richText.SelectionColor = System.Drawing.Color.Black;
					}
					else if (i != 0 && i % 8 == 0)
					{
						byteNo++;
						curLen = richText.TextLength;
						richText.AppendText(
							"          " +
							(byteNo).ToString().PadLeft(2, '0') +
							"-" +
							(byteNo * 8).ToString().PadLeft(4, '0') +
							":   ");
						richText.AppendText(":   ");
						nextLen = richText.TextLength;
						richText.Select(curLen, nextLen);
						richText.SelectionColor = System.Drawing.Color.Black;
					}


					curLen = richText.TextLength;
					richText.AppendText(eventBits[i].ToString());
					nextLen = richText.TextLength;
					richText.Select(curLen, nextLen);

					int col = 200 - ((int)(200 * Math.Abs(eventAnalysis[i]) / eventAnalysisThr));

					if (bitStart <= i && i <= bitEnd)
					{
						richText.SelectionColor = System.Drawing.Color.FromArgb(col, 255, 255);
					}
					else
					{
						richText.SelectionColor = System.Drawing.Color.FromArgb(col, col, col);
					}


					richText.AppendText(" ");
				}
				ResumeDrawing(richText);
			}
		}


		public override string ToString()
		{
			string str = "";

			str += "Trigger button: " + triggerButton + Environment.NewLine;
			str += "Home button: " + homeButton + Environment.NewLine;
			str += "Back button: " + backButton + Environment.NewLine;
			str += "Vol up button: " + volumeUpButton + Environment.NewLine;
			str += "Vol down button: " + volumeDownButton + Environment.NewLine;
			str += "Touchpad button: " + touchpadButton + Environment.NewLine;
			str += "Touchpad press: " + touchpadPressed + Environment.NewLine;
			str += "Axis: " + axisX + ", " + axisY + Environment.NewLine;

			str += "Val1: " + val1 + Environment.NewLine;
			str += "Val2: " + val2 + Environment.NewLine;
			str += "Val3: " + val3 + Environment.NewLine;

			str += string.Format("Accel: {0:N4}, {1:N4}, {2:N4}", accel[0], accel[1], accel[2]) + Environment.NewLine;
			str += string.Format("Gyro: {0:N4}, {1:N4}, {2:N4}", gyro[0], gyro[1], gyro[2]) + Environment.NewLine;
			str += string.Format("Mag: {0:N4}, {1:N4}, {2:N4}", mag[0], mag[1], mag[2]) + Environment.NewLine;

			return str;
		}

		public GearVrDevice(DeviceInformation device)
		{
			this.device = device;
			this.deviceId = device.Id;
		}




		internal async Task Initialize()
		{

			await InitializeEvents();
		}



		private async Task InitializeEvents()
		{
			Guid notifyGuid = new Guid("{c8c51726-81bc-483b-a052-f7a14ea3d281}");
			Guid writeGuid = new Guid("{c8c51726-81bc-483b-a052-f7a14ea3d282}");

			startSuccess = false;
			bool success = notifyCharacteristic != null && writeCharacteristic != null;

			if (success)
			{
				try
				{
					// Write the ClientCharacteristicConfigurationDescriptor in order for server to send notifications.               
					var result = await notifyCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
																Windows.Devices.Bluetooth.GenericAttributeProfile.GattClientCharacteristicConfigurationDescriptorValue.Notify);
					if (result == Windows.Devices.Bluetooth.GenericAttributeProfile.GattCommunicationStatus.Success)
					{
						notifyCharacteristic.ValueChanged += SelectedCharacteristic_ValueChanged;
						success = true;
					}

					if (success)
					{
					//	success = await InitialKickEvents();
					}

				}
				catch (Exception ex)
				{
					Console.WriteLine("error: " + ex.Message);
					// This usually happens when not all characteristics are found
					// or selected characteristic has no Notify.
				}
			}

			startSuccess = success;

		}

		private async Task<bool> InitialKickEvents()
		{
			var writer = new Windows.Storage.Streams.DataWriter();
			short val = 0x0800;
			writer.WriteInt16(val);
			GattCommunicationStatus writeResult = await writeCharacteristic.WriteValueAsync(writer.DetachBuffer());

			if (writeResult == GattCommunicationStatus.Success)
			{

				writer = new Windows.Storage.Streams.DataWriter();
				val = 0x0100;
				writer.WriteInt16(val);
				writeResult = await writeCharacteristic.WriteValueAsync(writer.DetachBuffer());
			}


			startSuccess = writeResult == GattCommunicationStatus.Success;
			return startSuccess;
		}

		private void SelectedCharacteristic_ValueChanged(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic sender, Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs args)
		{
			if(eventData.Length != args.CharacteristicValue.Length)
				eventData = new byte[args.CharacteristicValue.Length];

			Windows.Storage.Streams.DataReader.FromBuffer(args.CharacteristicValue).ReadBytes(eventData);

			Console.WriteLine("data event with length: " + eventData.Length);

			if (eventData.Length != 60)
				return;


			val1 = GetBitValue(388, 396);
			val2 = GetBitValue(404, 412);
			val3 = GetBitValue(418, 426);


			// Max observed value = 315
			// (corresponds to touchpad sensitive dimension in mm)
			axisX = (
				((eventData[54] & 0xF) << 6) +
				((eventData[55] & 0xFC) >> 2)
			) & 0x3FF;

			// Max observed value = 315
			axisY = (
				((eventData[55] & 0x3) << 8) +
				((eventData[56] & 0xFF) >> 0)
			) & 0x3FF;


			accel[0] = (eventData[4] + eventData[5] << 8) * 10000.0f * 9.80665f / 2048.0f;
			accel[1] = (eventData[6] + eventData[7] << 8) * 10000.0f * 9.80665f / 2048.0f;
			accel[2] = (eventData[8] + eventData[9] << 8) * 10000.0f * 9.80665f / 2048.0f;

			gyro[0] = (eventData[10] + eventData[11] << 8) * 10000.0f * 0.017453292f / 14.285f;
			gyro[1] = (eventData[12] + eventData[13] << 8) * 10000.0f * 0.017453292f / 14.285f;
			gyro[2] = (eventData[14] + eventData[15] << 8) * 10000.0f * 0.017453292f / 14.285f;

			mag[0] = (eventData[32] + eventData[33] << 8) * 0.06f;
			mag[0] = (eventData[34] + eventData[35] << 8) * 0.06f;
			mag[0] = (eventData[36] + eventData[37] << 8) * 0.06f;

			BitArray bits = new BitArray(eventData);

			for (int i = 0; i < bits.Count; i++)
			{
				eventBits[i] = bits[i] ? 1 : 0;
				if (bits[i])
				{
					if (eventAnalysis[i] < 0)
					{
						eventAnalysis[i] = 0;
					}
					else
					{
						eventAnalysis[i] = Math.Min(eventAnalysis[i] + 1, eventAnalysisThr);
					}
				}
				else
				{
					if (eventAnalysis[i] > 0)
					{
						eventAnalysis[i] = 0;
					}
					else
					{
						eventAnalysis[i] = Math.Max(eventAnalysis[i] - 1, -eventAnalysisThr);
					}
				}
			}

			triggerButton = 0 != (eventData[58] & (1 << 0));
			homeButton = 0 != (eventData[58] & (1 << 1));
			backButton = 0 != (eventData[58] & (1 << 2));
			touchpadButton = 0 != (eventData[58] & (1 << 3));
			volumeDownButton = 0 != (eventData[58] & (1 << 4));
			volumeUpButton = 0 != (eventData[58] & (1 << 5));
			touchpadPressed = axisX != 0 && axisY != 0;
		}
	
	}
}
