using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;

namespace ProcessShutdownTimer
{
    class ProcessManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Dictionary<ProcessContainer, DateTime> RunningTimers
        {
            get { return runningTimers; }
            set { runningTimers = value; }
        }
        public ObservableCollection<ProcessContainer> ProcessList
        {
            get { return processList; }
            set { processList = value; NotifyPropertyChanged(); }
        }

        Dictionary<ProcessContainer, DateTime> runningTimers;
        ObservableCollection<ProcessContainer> processList;

        public ProcessManager()
        {
            processList = new ObservableCollection<ProcessContainer>();
            runningTimers = new Dictionary<ProcessContainer, DateTime>();
            RefreshProcessList();
        }

        public void RefreshProcessList()
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (!processList.Any(p => p.Id == process.Id))
                {
                    processList.Add(new ProcessContainer(process.ProcessName, (int)(process.WorkingSet64 / 1024), process.Id));
                }
            }
        }

        public void ScheduleShutdown(ProcessContainer process, DateTime time)
        {
            process.SetTerminationTime(time);
        }

        public bool RemoveProcess(ProcessContainer processToRemove)
        {
            //Process.GetProcessById(Id).Kill();
            if (Process.GetProcessById(processToRemove.Id).ProcessName == processToRemove.ProcessName)
            {
                //
            }
            return true;
        }

        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
