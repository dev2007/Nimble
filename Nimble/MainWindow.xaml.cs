using Nimble.Contact.Imp;
using Nimble.Contact.Interfaces;
using Nimble.Module;
using NimbleBasicText;
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

        private IQCommunication contact;
        public MainWindow()
        {
            InitializeComponent();
            Plugin.LoadDll();
            contact = new QCommunication(Plugin.MessageManager);
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
                    Dispatcher.Invoke(() =>
                    {
                        RefreshQR();
                    });
                    break;
                case Contact.QRStatus.CONFIRMED:
                    timer.Stop();
                    Dispatcher.Invoke(() =>
                    {
                        this.Hide();
                        RunWindow runWindow = new RunWindow();
                        runWindow.ShowDialog();
                    });
                    break;
                case Contact.QRStatus.CONFIRMFAIL:
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("登录异常，请重新扫描登录");
                        RefreshQR();
                    });
                    break;
                default:
                    break;
            }
            requestIsRunning = false;
        }

        private void RefreshQR()
        {
            Stream s = contact.RefreshQR();
            if (s != null)
            {
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.StreamSource = s;
                myBitmapImage.EndInit();
                this.image.Source = myBitmapImage;
            }
        }
    }
}
