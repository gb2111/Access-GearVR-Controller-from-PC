//using Driver4Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Driver4VR
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{8814ea4c-2574-4342-b8aa-d1a2cb5adb89}");

        internal enum RpcAuthnLevel
        {
            Default = 0,
            None = 1,
            Connect = 2,
            Call = 3,
            Pkt = 4,
            PktIntegrity = 5,
            PktPrivacy = 6
        }

        internal enum RpcImpLevel
        {
            Default = 0,
            Anonymous = 1,
            Identify = 2,
            Impersonate = 3,
            Delegate = 4
        }

        internal enum EoAuthnCap
        {
            None = 0x00,
            MutualAuth = 0x01,
            StaticCloaking = 0x20,
            DynamicCloaking = 0x40,
            AnyAuthority = 0x80,
            MakeFullSIC = 0x100,
            Default = 0x800,
            SecureRefs = 0x02,
            AccessControl = 0x04,
            AppID = 0x08,
            Dynamic = 0x10,
            RequireFullSIC = 0x200,
            AutoImpersonate = 0x400,
            NoCustomMarshal = 0x2000,
            DisableAAA = 0x1000
        }

        [System.Runtime.InteropServices.DllImport("ole32.dll")]
        internal static extern int CoInitializeSecurity(IntPtr pVoid, int
            cAuthSvc, IntPtr asAuthSvc, IntPtr pReserved1, RpcAuthnLevel level,
            RpcImpLevel impers, IntPtr pAuthList, EoAuthnCap dwCapabilities, IntPtr
            pReserved3);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Threading.Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

            int result = CoInitializeSecurity(
                IntPtr.Zero,
                -1,
                IntPtr.Zero,
                IntPtr.Zero,
                RpcAuthnLevel.Default,
                RpcImpLevel.Identify,
                IntPtr.Zero,
                EoAuthnCap.None,
                IntPtr.Zero);


            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                try
                {

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Type t = Type.GetType("Driver4VR.GearVR.GearVrForm, Driver4Lib", true);
                    object o = Activator.CreateInstance(t);

                    Form f = (Form)o;

                    Application.Run(f);

                    mutex.ReleaseMutex();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Fatal error. Program will be closed!" + Environment.NewLine + ex.Message);
                }
            }
        }
    }
}
