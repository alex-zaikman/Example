using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace asz
{
    public partial class CGS : Form
    {
        //go to the gui and click on the background worker. in the properties windows, in the events section you will see the methods registered for the start and finish events of the worker
        //these will occur in a background thread. however changes to the gui need to made with a delegate

        public delegate void Callback();  //this is a pointer to a function to avoid cross thread exceptions

        public CGS()
        {
            InitializeComponent();
            start_progress();

        }

        public void DoWork()
        {
            //TODO

          //  Util.raiseOK("ds", "sd");
            
        }

        private void start_progress()
        {
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync(); //sets the do work event of the bg worker.
                                                    //the method that is registered for this event will be called

        
        }

        private void StopBar()
        {
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.MarqueeAnimationSpeed = 0;

        }

        private void StartBar()
        {
            progressBar1.MarqueeAnimationSpeed = 30;
            progressBar1.Style = ProgressBarStyle.Marquee;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (progressBar1.InvokeRequired)
            {
                Callback c = new Callback(StartBar);
                progressBar1.Invoke(c);
            }
            else
            {
                StartBar();
            }
            DoWork();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (progressBar1.InvokeRequired)
            {
                Callback c = new Callback(StopBar);
                progressBar1.Invoke(c);
            }
            else
            {
                StopBar();
                Application.Exit();
            }
        }


    }
}
