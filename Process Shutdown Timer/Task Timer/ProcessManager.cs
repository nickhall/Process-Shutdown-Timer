using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ProcessShutdownTimer
{
    class ProcessManager
    {
        public Dictionary<ProcessContainer, DateTime> RunningTimers
        {
            get { return runningTimers; }
            set { runningTimers = value; }
        }
        public List<ProcessContainer> ProcessList
        {
            get { return processList; }
            set { processList = value; }
        }

        Dictionary<ProcessContainer, DateTime> runningTimers;
        List<ProcessContainer> processList;

        public ProcessManager()
        {
            processList = new List<ProcessContainer>();
            runningTimers = new Dictionary<ProcessContainer, DateTime>();
            RefreshProcessList();
        }

        public void RefreshProcessList()
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (processList.FindIndex(p => p.Id == process.Id) < 1)
                {
                    processList.Add(new ProcessContainer(process.ProcessName, (int)(process.WorkingSet64 / 1024), process.Id));
                }
            }
        }

        public void ScheduleShutdown(ProcessContainer process, DateTime time)
        {
            process.SetTerminationTime(time);
        }

        public void RemoveProcess(ProcessContainer processToRemove)
        {
            //Process.GetProcessById(Id).Kill();
        }
    }
}
