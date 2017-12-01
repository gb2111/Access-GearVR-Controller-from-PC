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
		private System.Collections.Generic.IReadOnlyList<Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService> services;
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

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

		private const int WM_SETREDRAW = 11;
		private int val1;
		private int val2;
		private int val3;
		private string deviceId;
		private IReadOnlyList<GattCharacteristic> allCharacteristics;

		public static void SuspendDrawing(Control parent)
		{
			SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
		}

		public static void ResumeDrawing(Control parent)
		{
			SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
			parent.Refresh();
		}

		internal async Task<bool> Init()
		{
			bool success = false;
			bleDevice = await Windows.Devices.Bluetooth.BluetoothLEDevice.FromIdAsync(deviceId);

			services = bleDevice.GattServices;

			int ccc = services.Count;
			for (int i = 0; i < ccc; i++)
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
			if (myService != null)
			{
				try
				{

					allCharacteristics = myService.GetAllCharacteristics();
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

					success = notifyCharacteristic != null && writeCharacteristic != null;

				}
				catch
				{
				}
			}

			return success;
		}

		internal  void Draw(RichTextBox richText)
		{
			SuspendDrawing(richText);
			richText.Text = "";
			int curLen, nextLen;

			curLen = richText.TextLength;
			richText.Text += "00    ";
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
						richText.AppendText( Environment.NewLine + (byteNo).ToString().PadLeft(2, '0') + ":   ");
						nextLen = richText.TextLength;
						richText.Select(curLen, nextLen);
						richText.SelectionColor = System.Drawing.Color.Black;
					}
					else if (i != 0 && i % 8 == 0)
					{
						byteNo++;
						curLen = richText.TextLength;
						richText.AppendText("          " + (byteNo).ToString().PadLeft(2, '0') + ":   ");
						nextLen = richText.TextLength;
						richText.Select(curLen, nextLen);
						richText.SelectionColor = System.Drawing.Color.Black;
					}


					curLen = richText.TextLength;
					richText.AppendText(eventBits[i].ToString());
					nextLen = richText.TextLength;
					richText.Select(curLen, nextLen);

					int col = 200 - ((int)(200 * Math.Abs(eventAnalysis[i]) / eventAnalysisThr));

					richText.SelectionColor = System.Drawing.Color.FromArgb(col, col, col);
						


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

			return str;
		}

		public GearVrDevice(DeviceInformation device)
		{
			this.device = device;
			this.deviceId = device.Id;
		}




		internal async Task Start()
		{
			
			await InitializeEvents();
		}



		private async Task InitializeEvents()
		{
			Guid notifyGuid = new Guid("{c8c51726-81bc-483b-a052-f7a14ea3d281}");
			Guid writeGuid = new Guid("{c8c51726-81bc-483b-a052-f7a14ea3d282}");

			bool success = false;

			foreach (Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic c in allCharacteristics)
			{
				if (
					c.CharacteristicProperties.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.Notify) &&
					c.Uuid == notifyGuid)
				{
					notifyCharacteristic = c;
				}
				else if(
					c.CharacteristicProperties.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.Write) &&
					c.Uuid == writeGuid)
				{
					writeCharacteristic = c;
				}

				if (notifyCharacteristic != null && writeCharacteristic != null)
					break;

			}

			success = notifyCharacteristic != null && writeCharacteristic != null;

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
						success = await InitialKickEvents();
					}

				}
				catch (Exception ex)
				{
					// This usually happens when not all characteristics are found
					// or selected characteristic has no Notify.
				}
			}

		}

		private async Task<bool> InitialKickEvents()
		{
			var writer = new Windows.Storage.Streams.DataWriter();
			short val = 0x0100;
			writer.WriteInt16(val);
			GattCommunicationStatus writeResult = await writeCharacteristic.WriteValueAsync(writer.DetachBuffer());

			bool success = writeResult == GattCommunicationStatus.Success;
			return success;
		}

		private void SelectedCharacteristic_ValueChanged(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic sender, Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs args)
		{
			if(eventData.Length != args.CharacteristicValue.Length)
				eventData = new byte[args.CharacteristicValue.Length];

			Windows.Storage.Streams.DataReader.FromBuffer(args.CharacteristicValue).ReadBytes(eventData);

			val1 = val2 = val3 = 0;

			int bit = 0;
			val1 += (eventData[49] & (1 << 0)) != 0 ? (1 << bit) : 0; bit++;
			val1 += (eventData[49] & (1 << 1)) != 0 ? (1 << bit) : 0; bit++;
			val1 += (eventData[49] & (1 << 2)) != 0 ? (1 << bit) : 0; bit++;
			val1 += (eventData[49] & (1 << 3)) != 0 ? (1 << bit) : 0; bit++;

			bit = 0;
			val2 += (eventData[51] & (1 << 1)) != 0 ? (1 << bit) : 0; bit++;
			val2 += (eventData[51] & (1 << 2)) != 0 ? (1 << bit) : 0; bit++;
			val2 += (eventData[51] & (1 << 3)) != 0 ? (1 << bit) : 0; bit++;
			val2 += (eventData[51] & (1 << 4)) != 0 ? (1 << bit) : 0; bit++;

			bit = 0;
			val3 += (eventData[52] & (1 << 6)) != 0 ? (1 << bit) : 0; bit++;
			val3 += (eventData[52] & (1 << 7)) != 0 ? (1 << bit) : 0; bit++;
			val3 += (eventData[53] & (1 << 0)) != 0 ? (1 << bit) : 0; bit++;
			val3 += (eventData[53] & (1 << 1)) != 0 ? (1 << bit) : 0; bit++;
			//val3 += (eventData[53] & (1 << 2)) != 0 ? (1 << bit) : 0; bit++;





			axisX = axisY = 0;

			axisY += (eventData[55] & (1 << 3)) != 0 ? (1 << 0) : 0;
			axisY += (eventData[55] & (1 << 4)) != 0 ? (1 << 1) : 0;
			axisY += (eventData[55] & (1 << 5)) != 0 ? (1 << 2) : 0;
			axisY += (eventData[55] & (1 << 6)) != 0 ? (1 << 3) : 0;
			axisY += (eventData[55] & (1 << 7)) != 0 ? (1 << 4) : 0;
			axisY += (eventData[54] & (1 << 0)) != 0 ? (1 << 5) : 0;
			axisY += (eventData[54] & (1 << 1)) != 0 ? (1 << 6) : 0;
			axisY += (eventData[54] & (1 << 2)) != 0 ? (1 << 7) : 0;

			axisX += (eventData[55] & (1 << 0)) != 0 ? (1 << 7) : 0;
			axisX += (eventData[56] & (1 << 7)) != 0 ? (1 << 6) : 0;
			axisX += (eventData[56] & (1 << 6)) != 0 ? (1 << 5) : 0;
			axisX += (eventData[56] & (1 << 5)) != 0 ? (1 << 4) : 0;
			axisX += (eventData[56] & (1 << 4)) != 0 ? (1 << 3) : 0;
			axisX += (eventData[56] & (1 << 3)) != 0 ? (1 << 2) : 0;
			axisX += (eventData[56] & (1 << 2)) != 0 ? (1 << 1) : 0;
			axisX += (eventData[56] & (1 << 1)) != 0 ? (1 << 0) : 0;

		

		
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
