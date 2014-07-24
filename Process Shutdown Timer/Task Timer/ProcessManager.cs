using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;

namespace ProcessShutdownTimer
{
    class ProcessManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Dictionary<ProcessContainer, DateTime> RunningTimers
        {
            get { return runningTimers; }
            set { runningTimers = value; NotifyPropertyChanged(); }
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

        public void ScheduleShutdown(IList processes, DateTime time)
        {
            //process.SetTerminationTime(time);
            foreach (ProcessContainer process in processes)
            {
                SetTimer(process, time);
                runningTimers.Add(process, time);
            }
        }

        public bool RemoveProcess(ProcessContainer processToRemove)
        {
            //Process.GetProcessById(Id).Kill();
            if (Process.GetProcessById(processToRemove.Id).ProcessName == processToRemove.ProcessName)
            {
                processList.Remove(processToRemove);
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

        private void SetTimer(ProcessContainer process, DateTime time)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += HandleTick;
            timer.Interval = time - DateTime.Now;
            timer.Tag = process;
            timer.Start();
        }

        private void HandleTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = sender as DispatcherTimer;
            ProcessContainer process = timer.Tag as ProcessContainer;
            if (timer != null && process != null)
            {
                timer.Stop();
                processList.Remove(process);
            }
        }
    }
}
