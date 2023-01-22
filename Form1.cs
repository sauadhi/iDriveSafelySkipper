using Emgu.CV;
using Emgu.CV.Structure;
using I_Drive.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;


namespace I_Drive
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread((theered));
            thread.Start();
        }

        public static Image<Gray, byte> IdentifyContours(Bitmap colorImage)
        {

            #region Conversion To grayscale
            var grayImage = BitmapExtension.ToImage<Gray, byte>(colorImage);
            Image<Gray, byte> imgOutput = grayImage;



            Image<Gray, byte> imgout = new Image<Gray, byte>(grayImage.Width, grayImage.Height, new Gray(0));
            Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
            Mat hier = new Mat();

            CvInvoke.FindContours(imgOutput, contours, hier, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            CvInvoke.DrawContours(imgout, contours, -1, new MCvScalar(255, 0, 0));

            grayImage.Dispose();

            return imgout;
            #endregion
        }

        void theered()
        {

            while (true)
            {
               
                   Thread.Sleep(5000);
                
                var screen = Screen.AllScreens[1];
                var bounds = screen.Bounds;

                var bitmap = new Bitmap(bounds.Width, bounds.Height);
                var graphics = Graphics.FromImage(bitmap);
                graphics.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size);
                bitmap = bitmap.Clone(new Rectangle(0, 0, 1920, 1080), bitmap.PixelFormat);



                var imegInput = new Bitmap(bitmap).ToImage<Bgr, byte>();


                var Te = new Bitmap(bitmap).ToImage<Bgr, byte>();
                Bitmap image = Resources.Don;
                Image<Bgr, byte> template = Te.Convert<Bgr, byte>();



                var IN = new Bitmap(image).ToImage<Bgr, byte>();


                Mat mat = new Mat();
                CvInvoke.MatchTemplate(template, IN, mat, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed);



                double minVal = 0.0;
                double maxVal = 0.0;
                Point minLoc = new Point();
                Point maxLoc = new Point();
                CvInvoke.MinMaxLoc(mat, ref minVal, ref maxVal, ref minLoc, ref maxLoc);


                //MessageBox.Show(minVal.ToString() + "       " + maxVal.ToString() + "       " + pixelColorStringValue);
                
                
                Rectangle r = new Rectangle(maxLoc, IN.Size);
                CvInvoke.Rectangle(imegInput, r, new MCvScalar(255, 0, 0), 3);
                //pictureBox1.Image = imegInput.AsBitmap();

                Te.Dispose();
                mat.Dispose();
                IN.Dispose();
                image.Dispose();
                template.Dispose();
                if(maxVal > 0.999)
                {
                    mouse_event(0x0001,0, -1000, 0, 0);
                    mouse_event(0x0001,100, 1000, 0, 0);
                    mouse_event(0x0001 ,- 3000, 0, 0, 0);
                    mouse_event(0x0001 ,- 22220, -10000, 0, 0);
                    mouse_event(0x0001,maxLoc.X *2, maxLoc.Y*2, 0, 0);
                    mouse_event(0x0001,4, 4, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                }
            }
        }

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
