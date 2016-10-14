using Nimble.Contact.Imp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nimble
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer timer = new Timer();
        private bool requestIsRunning = false;
        private QCommunication contact = new QCommunication(new Contact.Message());
        public MainWindow()
        {
            InitializeComponent();
            Stream s = contact.GetLoginQR();
            if (s != null)
            {
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.StreamSource = s;
                myBitmapImage.EndInit();
                this.image.Source = myBitmapImage;
            }

            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;
            timer.Start();
            requestIsRunning = false;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (requestIsRunning)
                return;

            requestIsRunning = true;
            var status = contact.GetQRStatus();

            switch (status)
            {
                case Contact.QRStatus.INVALID:
                    Stream s = contact.RefreshQR();
                    if (s != null)
                    {
                        BitmapImage myBitmapImage = new BitmapImage();
                        myBitmapImage.BeginInit();
                        myBitmapImage.StreamSource = s;
                        myBitmapImage.EndInit();
                        this.image.Source = myBitmapImage;
                    }

                    break;
                case Contact.QRStatus.CONFIRMED:
                    timer.Stop();
                    MessageBox.Show("登录成功");
                    break;
                default:
                    break;
            }
            requestIsRunning = false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            contact.Poll();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            contact.Message_Send(0, "929529331", "test");
        }
    }
}
