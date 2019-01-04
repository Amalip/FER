using System;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace FER
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;
        bool flag = true;

        private void Form1_Load(object sender, EventArgs e)
        {
            btn_stop.Enabled = false;
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in CaptureDevice)
            {
                cmbInput.Items.Add(device.Name);
            }

            if (cmbInput.Items.Count > 0)
                cmbInput.SelectedIndex = 0;
            FinalFrame = new VideoCaptureDevice();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            btn_stop.Enabled = true;
            FinalFrame = new VideoCaptureDevice(CaptureDevice[cmbInput.SelectedIndex].MonikerString);
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            FinalFrame.Start();
        }

        private async void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pic_box_video.Image = (Bitmap)eventArgs.Frame.Clone();
            FaceAPI api = new FaceAPI();

            if (flag)
            {
                flag = false;
                await api.UploadAndDetectFaces_(pic_box_video.Image);
            }

        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            FinalFrame.Stop();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FinalFrame.IsRunning == true)
            {
                FinalFrame.Stop();
            }
        }
    }
}
