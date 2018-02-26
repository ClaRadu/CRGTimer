using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Timers;
using System.IO;

public enum time : int { min = 0, max = 59, maxhr = 23, };
public enum ttype : int { hour = 0, min, sec, };

namespace WpfTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CountDown cnt;
        private RingRing ring;
        private bool usingSound = true, showAlert = false;

        public MainWindow()
        {
            InitializeComponent();

            cnt = new CountDown();
            // check if sound file exists, if yes create a new instance of class RingRing
            string filep = @"c:\windows\media\chord.wav";
            if (File.Exists(filep)) ring = new RingRing(filep);
            else {
                MessageBox.Show("Unable to find sound file '" + filep + "'\nSounds disabled.", "bloody message..");
                usingSound = false;
            }
            
            // initial status for controls
            this.lblAlert.Visibility = Visibility.Hidden;
            this.checkBox1.IsChecked = this.checkBox2.IsChecked = false;
            this.txtbmin.IsEnabled = this.txtbseth.IsEnabled = this.txtbsetm.IsEnabled = false;
            this.cmbset.IsEnabled = this.txtbcnt.IsEnabled = false;
            // initial values
            this.txtbmin.Text = this.txtbseth.Text = this.txtbsetm.Text = "0";
            // create timer
            // thanks to msdn.microsoft.com
            // https://msdn.microsoft.com/en-us/library/system.windows.threading.dispatchertimer(v=vs.110).aspx
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            txtbtime.Text = DateTime.Now.ToString("HH:mm:ss");
            if (txtbcnt.IsEnabled) {
                if (cnt.stopped()) {
                    // countdown reached a halt so play the sound;
                    // the sound will be played every tick so it will go on until stop has been pressed
                    ring.play();
                    // toggle alert visible / hidden
                    showAlert = !showAlert;
                    if (showAlert)
                        this.lblAlert.Visibility = Visibility.Visible;
                    else
                        this.lblAlert.Visibility = Visibility.Hidden;
                } else cnt.decvals(); // decrease values with every tick
                if ((bool)this.checkBox1.IsChecked)
                    txtbcnt.Text = cnt.s_retval((int)ttype.hour) + ':' + cnt.s_retval((int)ttype.min) + ':' + cnt.s_retval((int)ttype.sec);
                else
                    txtbcnt.Text = txtbseth.Text.Trim() + ':' + txtbsetm.Text.Trim() + ":0 (" + cnt.s_retval((int)ttype.hour) + ':' + cnt.s_retval((int)ttype.min) + ':' + cnt.s_retval((int)ttype.sec) + ')';
            }
        }

        // quit the app
        private void cmbclose_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        // set the alarm
        private void cmbset_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)this.checkBox1.IsChecked) { // stopwatch mode
                txtbcnt.IsEnabled = true;
                int min = Convert.ToInt32(txtbmin.Text);
                cnt.setval(0, min, 0);
            } else { // alarm clock mode
                txtbcnt.IsEnabled = true;
                int difh = Convert.ToInt32(txtbseth.Text) - DateTime.Now.Hour;
                int difm = Convert.ToInt32(txtbsetm.Text) - DateTime.Now.Minute;
                int difs = 60-DateTime.Now.Second;
                //int difs = 0;
                cnt.setval((difh<2) ? 0 : difh, (difm<2) ? 0 : difm, difs);
            }
        }

        // test the alarm sound
        private void cmbtst_Click(object sender, RoutedEventArgs e)
        {
            if (usingSound) ring.play();
        }

        // stop the alarm and reset controls
        private void cmbstop_Click(object sender, RoutedEventArgs e)
        {
            if (usingSound) {
                ring.stop();
                this.checkBox1.IsChecked = this.checkBox2.IsChecked = false;
                txtbmin.IsEnabled = cmbset.IsEnabled = txtbcnt.IsEnabled  = false;
                this.txtbmin.Text = this.txtbseth.Text = this.txtbsetm.Text = this.txtbcnt.Text = "0";
                this.txtbmin.IsEnabled = this.txtbseth.IsEnabled = this.txtbsetm.IsEnabled = false;
            }
            showAlert = false;
            this.lblAlert.Visibility = Visibility.Hidden;
        }

        private void checkBox1_Click(object sender, RoutedEventArgs e)
        {
            // enable the needed controls, disable unneeded ones
            if ((bool)this.checkBox1.IsChecked) {
                txtbmin.IsEnabled = true;
                txtbmin.Focus();
                txtbseth.IsEnabled = txtbsetm.IsEnabled = false;
                cmbset.IsEnabled = true;
                if ((bool)checkBox2.IsChecked) checkBox2.IsChecked = false;
            } else {
                txtbmin.IsEnabled = false;
                cmbset.IsEnabled = false;
            }
        }

        private void checkBox2_Click(object sender, RoutedEventArgs e)
        {
            // enable the needed controls, disable unneeded ones
            if ((bool)this.checkBox2.IsChecked) {
                txtbmin.IsEnabled = false;
                txtbseth.IsEnabled = txtbsetm.IsEnabled = true;
                txtbseth.Focus();
                cmbset.IsEnabled = true;
                if ((bool)checkBox1.IsChecked) checkBox1.IsChecked = false;
            } else {
                txtbseth.IsEnabled = txtbsetm.IsEnabled = false;
                cmbset.IsEnabled = false;
            }
        }

    } // end of main class
} // end of namespace

