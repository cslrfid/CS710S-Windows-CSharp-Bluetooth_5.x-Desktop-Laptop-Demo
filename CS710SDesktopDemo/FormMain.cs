using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using CSLibrary;
using static System.Net.Mime.MediaTypeNames;

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
            _reader.ConnectAsync(CSLibrary.DeviceFinder.GetDeviceInformation(listView1.SelectedIndices[0]), CSLibrary.DeviceFinder.GetDeviceModel(listView1.SelectedIndices[0]));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            deviceCount = 0;
            listView1.Clear();
            CSLibrary.DeviceFinder.SearchDevice();
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
                    listView1.Items.Add(deviceCount + ". " + di.deviceName);
                }
            }
        }

        private void buttonInventory_Click(object sender, EventArgs e)
        {
            _reader.rfid.OnAsyncCallback += new EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs>(TagInventoryEvent);
            _reader.rfid.Options.TagRanging.flags = 0;
            _reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING);
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
            }
        }

        void TagInventoryEvent(object sender, CSLibrary.Events.OnAsyncCallbackEventArgs e)
        {
            if (e.type != CSLibrary.Constants.CallbackType.TAG_RANGING)
                return;

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
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _reader.rfid.StopOperation();
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
            textBox3.Text += "Please wait : disconnecting... Please wait until Bluetooth LED changes back to flash" + Environment.NewLine;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView_EPC.Rows.Clear();
            dataGridView_EPC.Refresh();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            this.Text = this.Text + " " + version.ToString(3);
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
    }
}
