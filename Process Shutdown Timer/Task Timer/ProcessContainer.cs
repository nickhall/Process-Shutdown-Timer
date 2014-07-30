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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using toolkit = Xceed.Wpf.Toolkit;

namespace ProcessShutdownTimer
{
    public class ProcessContainer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string ProcessName { get; set; }
        public int Memory { get; set; }
        public int Id { get; set; }
        public DateTime TerminationTime { get; set; }
        public bool IsScheduled
        {
            get { return isScheduled; }
            set { isScheduled = value; NotifyPropertyChanged(this.ToString()); }
        }

        bool isScheduled;

        public ProcessContainer(string name, int memory, int id)
        {
            ProcessName = name;
            Memory = memory;
            Id = id;
            isScheduled = false;
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return ProcessName;
        }
    }
}
