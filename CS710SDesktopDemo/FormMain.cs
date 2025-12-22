using CSLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace CS710SDesktopDemo
{
    public partial class FormMain : Form
    {
        HighLevelInterface _reader = new HighLevelInterface();

        int deviceCount = 0;
        bool exit = false;

        public FormMain()
        {
            InitializeComponent();

            CSLibrary.DeviceFinder.OnSearchCompleted += DeviceWatcher_Added;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedIndices.Count < 1)
            {
                System.Console.WriteLine("Please select reader first!");
                return;
            }

            textBox3.Text = "Please wait, connecting..." + Environment.NewLine;
            _reader.rfid.OnStateChanged += new EventHandler<CSLibrary.Events.OnStateChangedEventArgs>(StateChangedEvent);
            _reader.notification.OnVoltageEvent += new EventHandler<CSLibrary.Notification.VoltageEventArgs>(VoltageEvent);
            _reader.notification.OnKeyEvent += new EventHandler<CSLibrary.Notification.HotKeyEventArgs>(Notification_OnKeyEvent);
            _reader.ConnectAsync(CSLibrary.DeviceFinder.GetDeviceInformation(listView1.SelectedIndices[0]), CSLibrary.DeviceFinder.GetDeviceModel(listView1.SelectedIndices[0]));
        }

        private void Notification_OnKeyEvent(object sender, Notification.HotKeyEventArgs e)
        {
            this.Invoke((MethodInvoker)(() =>
            {
                if (e.KeyCode == CSLibrary.Notification.Key.BUTTON)
                {
                    if (e.KeyDown && buttonInventory.Enabled == true)
                    {
                        buttonInventory.PerformClick();
                }
                    else if (!e.KeyDown && buttonStopInventory.Enabled == true)
                    {
                        buttonStopInventory.PerformClick();
                    }
                }
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            deviceCount = 0;
            listView1.Clear();
            buttonConnect.Enabled = false;
            textBox3.Text = "Searching RFID reader, Please wait 6 seconds...";
            CreateAndRunTaskWithDelay("search", 6000);
            CSLibrary.DeviceFinder.SearchDevice(checkBoxMacAddressFiltering.Checked);
        }

        async Task CreateAndRunTaskWithDelay(string taskName, int delayMilliseconds)
        {
            await Task.Delay(delayMilliseconds);
            this.Invoke((MethodInvoker)(() =>
            {
                buttonConnect.Enabled = true;
                textBox3.Text += "Search Completed";
            }));
        }

        private async void DeviceWatcher_Added(object sender, object deviceInfo)
        {
            // We must update the collection on the UI thread because the collection is databound to a UI element.
            //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                lock (this)
                {
                    CSLibrary.DeviceFinder.DeviceFinderArgs dfa = (CSLibrary.DeviceFinder.DeviceFinderArgs)deviceInfo;
                    CSLibrary.DeviceFinder.DeviceInfomation di = (CSLibrary.DeviceFinder.DeviceInfomation)dfa.Found;
                    //DeviceInformation ndi = (DeviceInformation)di.nativeDeviceInformation;
                    deviceCount++;
                    string a = String.Format("Added {0} {1} {2}", deviceCount, di.ID, di.deviceName);
                    Debug.WriteLine(a);
                    listView1.Items.Add(deviceCount + ". " + di.deviceName + " ; MAC:" + MacAddress(di.macAdd) + " ; " + CSLibrary.DeviceFinder.GetDeviceModel((int)di.ID));
                }
            }
        }

        string MacAddress (long address)
        {
            return string.Format("{0:X2}:{1:X2}:{2:X2}:{3:X2}:{4:X2}:{5:X2}",
        (address >> (8 * 5)) & 0xff,
        (address >> (8 * 4)) & 0xff,
        (address >> (8 * 3)) & 0xff,
        (address >> (8 * 2)) & 0xff,
        (address >> (8 * 1)) & 0xff,
        (address >> (8 * 0)) & 0xff);
        }

        private void buttonInventory_Click(object sender, EventArgs e)
        {
            _reader.rfid.SetTriggerReleaseAbortRFID(false);
            _reader.rfid.AntennaPortSetState(0, CSLibrary.Constants.AntennaPortState.ENABLED);
            _reader.rfid.SetPowerLevel(300);

            string[] parts = comboBox_profiles.SelectedItem.ToString().Split(':');
            int profile = int.Parse(parts[0].Trim());
            _reader.rfid.SetCurrentLinkProfile((uint)profile);
            _reader.rfid.OnAsyncCallback += new EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs>(TagInventoryEvent);
            _reader.rfid.Options.TagRanging.flags = 0;
            _reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING);
            ClassBattery.SetBatteryMode(ClassBattery.BATTERYMODE.INVENTORY);
        }

        void StateChangedEvent(object sender, CSLibrary.Events.OnStateChangedEventArgs e)
        {
            if (e.state == CSLibrary.Constants.RFState.INITIALIZATION_COMPLETE)
            {
                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;
                buttonInventory.Enabled = true;
                buttonStopInventory.Enabled = true;
                textBox3.Text += "Connected" + Environment.NewLine;

                string [] profiles = _reader.rfid.GetActiveLinkProfileName ();

                comboBox_profiles.Items.Clear();

                foreach (string profile in profiles)
                    comboBox_profiles.Items.Add(profile);

                comboBox_profiles.SelectedIndex = 19;
            }
        }

        void TagInventoryEvent(object sender, CSLibrary.Events.OnAsyncCallbackEventArgs e)
        {
            if (e.type != CSLibrary.Constants.CallbackType.TAG_RANGING)
                return;

            this.BeginInvoke((System.Threading.ThreadStart)delegate ()
            {
                foreach (DataGridViewRow row in dataGridView_EPC.Rows)
                {
                    if (row.Cells[0].Value != null)
                        if (row.Cells[0].Value.ToString() == e.info.epc.ToString())
                        {
                            row.Cells[1].Value = Math.Round(e.info.rssi, 1, MidpointRounding.AwayFromZero);
                            return;
                        }
                }

                int index = dataGridView_EPC.Rows.Add();
                dataGridView_EPC.Rows[index].Cells[0].Value = e.info.epc.ToString();
                dataGridView_EPC.Rows[index].Cells[1].Value = Math.Round(e.info.rssi, 1, MidpointRounding.AwayFromZero);
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _reader.rfid.StopOperation();
            ClassBattery.SetBatteryMode(ClassBattery.BATTERYMODE.IDLE);
            _reader.rfid.OnAsyncCallback -= new EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs>(TagInventoryEvent);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            buttonConnect.Enabled = true;
            buttonDisconnect.Enabled = false;
            buttonInventory.Enabled = false;
            buttonStopInventory.Enabled = false;
            _reader.rfid.OnStateChanged -= new EventHandler<CSLibrary.Events.OnStateChangedEventArgs>(StateChangedEvent);
            _reader.DisconnectAsync();
            textBox3.Text += "Please wait : disconnecting... Please wait until Bluetooth LED changes back to flashing" + Environment.NewLine;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView_EPC.Rows.Clear();
            dataGridView_EPC.Refresh();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            this.Text = this.Text + " " + version.ToString(3) + " (backward compatible to CS108)";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (buttonDisconnect.Enabled)
            {
                MessageBox.Show("Please DISCONNECT the reader before exiting the program!!!!");
            }
            else
            {
                this.Close();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (buttonDisconnect.Enabled)
            {
                MessageBox.Show("Please DISCONNECT the reader before exiting the program!!!!");
                e.Cancel = true;
            }
            else
                e.Cancel = false;
        }

        void VoltageEvent(object sender, CSLibrary.Notification.VoltageEventArgs e)
        {
            this.BeginInvoke((System.Threading.ThreadStart)delegate ()
            {
                if (e.Voltage == 0xffff)
                {
                    labelVoltage.Text = "ERROR";
                }
                else
                {
                    double voltage = (double)e.Voltage / 1000;

                    labelVoltage.Text = ClassBattery.Voltage2Percent(voltage).ToString("0") + "%" + " / " + voltage.ToString("0.000") + "v"; //			%
                }
            });
        }

        public static class ClassBattery
        {
            public enum BATTERYMODE
            {
                INVENTORY = 1,
                IDLE = 2,
            }

            public enum BATTERYLEVELSTATUS
            {
                NORMAL = 0,
                LOW = 1,
            }


            // for inventory mode
            readonly static double[] voltageTable1 = new double[] { 4.106, 4.017, 3.98, 3.937, 3.895, 3.853, 3.816, 3.779, 3.742, 3.711, 3.679, 3.658, 3.637, 3.626, 3.61, 3.584, 3.547, 3.515, 3.484, 3.457, 3.431, 3.399, 3.362, 3.32, 3.251, 3.135 };
            readonly static double[] capacityTable1 = new double[] { 100, 96, 92, 88, 84, 80, 76, 72, 67, 63, 59, 55, 51, 47, 43, 39, 35, 31, 27, 23, 19, 15, 11, 7, 2, 0 };
            readonly static double[] voltageSlope1 = new double[voltageTable1.Length - 1];

            // for non-inventory mode
            readonly static double[] voltageTable2 = new double[] { 4.212, 4.175, 4.154, 4.133, 4.112, 4.085, 4.069, 4.054, 4.032, 4.011, 3.99, 3.969, 3.953, 3.937, 3.922, 3.901, 3.885, 3.869, 3.853, 3.837, 3.821, 3.806, 3.79, 3.774, 3.769, 3.763, 3.758, 3.753, 3.747, 3.742, 3.732, 3.721, 3.705, 3.684, 3.668, 3.652, 3.642, 3.626, 3.615, 3.605, 3.594, 3.584, 3.568, 3.557, 3.542, 3.531, 3.510, 3.494, 3.473, 3.457, 3.436, 3.41, 3.362, 3.235, 2.987, 2.982 };
            readonly static double[] capacityTable2 = new double[] { 100, 98, 96, 95, 93, 91, 89, 87, 85, 84, 82, 80, 78, 76, 75, 73, 71, 69, 67, 65, 64, 62, 60, 58, 56, 55, 53, 51, 49, 47, 45, 44, 42, 40, 38, 36, 35, 33, 31, 29, 27, 25, 24, 22, 20, 18, 16, 15, 13, 11, 9, 7, 5, 4, 2, 0 };
            readonly static double[] voltageSlope2 = new double[voltageTable2.Length - 1];

            static double[] voltageTable;
            static double[] capacityTable;
            static double[] voltageSlope;

            static BATTERYMODE _currentInventoryMode = BATTERYMODE.IDLE;

            static ClassBattery()
            {
                int cnt;

                for (cnt = 0; cnt < voltageTable1.Length - 2; cnt++)
                    voltageSlope1[cnt] = (capacityTable1[cnt] - capacityTable1[cnt + 1]) / (voltageTable1[cnt] - voltageTable1[cnt + 1]);

                for (cnt = 0; cnt < voltageTable2.Length - 2; cnt++)
                    voltageSlope2[cnt] = (capacityTable2[cnt] - capacityTable2[cnt + 1]) / (voltageTable2[cnt] - voltageTable2[cnt + 1]);

                SetBatteryMode(BATTERYMODE.IDLE);
            }

            public static void SetBatteryMode(BATTERYMODE bm)
            {
                _currentInventoryMode = bm;

                if (bm == BATTERYMODE.INVENTORY)
                {
                    voltageTable = voltageTable1;
                    capacityTable = capacityTable1;
                    voltageSlope = voltageSlope1;
                }
                else
                {
                    voltageTable = voltageTable2;
                    capacityTable = capacityTable2;
                    voltageSlope = voltageSlope2;
                }
            }

            public static BATTERYLEVELSTATUS BatteryLow(double voltage)
            {
                if (Voltage2Percent(voltage) <= 20.0)
                    return BATTERYLEVELSTATUS.LOW;

                return BATTERYLEVELSTATUS.NORMAL;
            }

            public static double Voltage2Percent(double voltage)
            {
                int cnt;

                if (voltage > voltageTable[0])
                    return 100;

                if (voltage <= voltageTable[voltageTable.Length - 1])
                    return 0;

                for (cnt = voltageTable.Length - 2; cnt >= 0; cnt--)
                {
                    if (voltage > voltageTable[cnt])
                        continue;

                    if (voltage == voltageTable[cnt])
                        return capacityTable[cnt];

                    double percent = 0;

                    percent = (voltage - voltageTable[cnt + 1]) * voltageSlope[cnt] + capacityTable[cnt + 1];

                    return percent;
                }

                return 0;
            }
        }

    }
}
