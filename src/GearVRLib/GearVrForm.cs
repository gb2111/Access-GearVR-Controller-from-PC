using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Driver4VR.GearVR
{
    public partial class GearVrForm : Form
    {

        public GearVrForm()
        {
            InitializeComponent();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
        }

     


		DevcieCollection controllers = new DevcieCollection();
		private int bitStart;
		private int bitEnd;
		private int bitValue;

		private void timer1_Tick(object sender, EventArgs e)
        {
			string txt = "Found: " + controllers.gearVrDevices.Count;


			if (labelFoundNo.Text != txt)
				labelFoundNo.Text = txt;

			if (controllers.gearVrDevices.Count > 0)
			{
				txt = "Connected: " + (controllers.gearVrDevices[0].IsConnected ? "yes" : "no");
				if (labelConnected.Text != txt)
					labelConnected.Text = txt;

				txt = "Listening: " + (controllers.gearVrDevices[0].startSuccess ? "yes" : "no");
				if (labelResult.Text != txt)
					labelResult.Text = txt;

				if (labelKicking.Visible != controllers.gearVrDevices[0].kickingEvents)
				{
					labelKicking.Visible = controllers.gearVrDevices[0].kickingEvents;
				}

			}

			if (controllers.gearVrDevices.Count > 0)
            {

				controllers.gearVrDevices[0].Draw(richTextBox1);
				label1.Text = controllers.gearVrDevices[0].ToString();

				textBox3.Text = controllers.gearVrDevices[0].BitValue.ToString();
			}
			else
			{
				label2.Text = "device not found";
			}

			if (controllers.gearVrDevices.Count > 1)
			{
				label2.Text = controllers.gearVrDevices[1].ToString();
			}
			else
			{
				label2.Text = "second device not found";
			}


		}





        private async void button3_Click(object sender, EventArgs e)
        {
            await controllers.StartScan();
//            await controllers.Start();
        }

		private void textBits_TextChanged(object sender, EventArgs e)
		{
			if (controllers.gearVrDevices.Count > 0)
			{


				try
				{
					controllers.gearVrDevices[0].bitStart = int.Parse(textBitStart.Text);
					controllers.gearVrDevices[0].bitEnd = int.Parse(textBitEnd.Text);


				}
				catch
				{
				}
			}
		}

		private void textBits_TextChanged(object sender, KeyEventArgs e)
		{

		}

		private async void buttonStart_Click(object sender, EventArgs e)
		{
			if (controllers.gearVrDevices.Count == 0)
			{
				MessageBox.Show("No devices found");
				return;
			}

			await controllers.gearVrDevices[0].Initialize();
		}

		private async void buttonConnect_Click_1(object sender, EventArgs e)
		{
			if (controllers.gearVrDevices.Count == 0)
			{
				MessageBox.Show("No devices found");
				return;
			}

			await controllers.gearVrDevices[0].Connect();
		}

		private void buttonKickEvents_Click(object sender, EventArgs e)
		{
			if (controllers.gearVrDevices.Count == 0)
			{
				MessageBox.Show("No devices found");
				return;
			}

			controllers.gearVrDevices[0].KickEvents();
		}
	}


}
