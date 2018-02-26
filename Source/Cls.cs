using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;

// aditional classes used by MainWindow.xaml.cs
namespace WpfTimer
{
    class Cls { } // does absolute nothing

    class RingRing
    {
        private SoundPlayer snd;

        public RingRing(string file) // constructor
        {
            snd = new SoundPlayer(file); // testing for file done in main prg.
        }

        public void play() { snd.Play(); } // play the sound
        public void stop() { snd.Stop(); } // stop the sound
    } // end of ringring cls.

    class CountDown
    {
        private int hrs, min, sec;
        private bool hasStopped = false;

        public CountDown(int newh = 0, int newm = 0, int news = 0) // constructor
        {
            hrs = newh;
            min = newm;
            sec = news;
        }

        // decrease the time
        public void decvals()
        {
            if (!hasStopped) {
                sec--;
                if (sec < (int)time.min)
                {
                    sec = (int)time.max;
                    min--;
                    if (min < (int)time.min)
                    {
                        min = (int)time.max;
                        hrs--;
                        if (hrs < (int)time.min)
                        {
                            hrs = (int)time.maxhr;
                            hasStopped = true;
                        }
                    }
                }
            } // end if !hasstopped
        }

        // set new values to hrs, min, sec variables
        public void setval(int newh = 0, int newm = 0, int news = 0) { hrs = newh; min = newm; sec = news; hasStopped = false; }

        public bool stopped() { return this.hasStopped; } // check if counter reached 0

        // return values (int)
        // int retype [ 0=hours, 1=minutes, 2=seconds ]
        public int i_retval(int retype)
        {
            if (retype == 0) return hrs;
            else if (retype == 1) return min;
            else return sec;
        }

        // return values (string)
        // int retype [ 0=hours, 1=minutes, 2=seconds ]
        public string s_retval(int retype)
        {
            if (retype == 0) return hrs.ToString();
            else if (retype == 1) return min.ToString();
            else return sec.ToString();
        }
    } // end of countdown cls.

} // end of namespace
