using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Diagnostics;
using toolkit = Xceed.Wpf.Toolkit;

namespace ProcessShutdownTimer
{
    public class ProcessContainer
    {
        public string ProcessName { get; set; }
        public int Memory { get; set; }
        public int Id { get; set; }

        DispatcherTimer timer;


            public ProcessContainer(string name, int memory, int id)
        {
            ProcessName = name;
            Memory = memory;
            Id = id;
        }

        public void SetTerminationTime(DateTime time)
        {
            timer = new DispatcherTimer();
            timer.Tick += HandleTick;
            timer.Interval = time - DateTime.Now;
            timer.Start();
        }

        public void HandleTick(object sender, EventArgs e)
        {
            timer.Stop();
            MessageBox.Show("FIRED");
            
        }

        public override string ToString()
        {
            return ProcessName;
        }
    }
}
