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


            if(controllers.gearVrDevices.Count > 0)
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
            await controllers.Enumerate();
            await controllers.Start();
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
	}


}
