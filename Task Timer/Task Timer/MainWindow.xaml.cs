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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<ProcessContainer, DateTime> runningTimers;
        List<ProcessContainer> processList;

        public MainWindow()
        {
            InitializeComponent();
            processList = new List<ProcessContainer>();
            runningTimers = new Dictionary<ProcessContainer, DateTime>();
            RefreshProcessList();

            TimePickerBox.Value = DateTime.Now;
            ScheduledBox.ItemsSource = runningTimers;
            ProcessBox.ItemsSource = processList;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ProcessBox.ItemsSource);
            view.Filter = ProcessFilter;
        }

        private void RefreshProcessList()
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (processList.FindIndex(p => p.Id == process.Id) < 1)
                {
                    processList.Add(new ProcessContainer(process.ProcessName, (int)(process.WorkingSet64 / 1024), process.Id));
                }
            }
        }

        private bool ProcessFilter(object item)
        {
            if (String.IsNullOrEmpty(FilterInput.Text))
            {
                return true;
            }
            else
            {
                return ((item as ProcessContainer).ProcessName.IndexOf(FilterInput.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TimePickerBox.Value != null)
            {
                DateTime finalTime = TimePickerBox.Value ?? default(DateTime);

                if (TimePickerBox.Value < DateTime.Now)
                {
                    finalTime += TimeSpan.FromHours(24d);
                    MessageBox.Show("Changed time: " + finalTime.ToString());
                }
                ProcessContainer selected = (ProcessContainer)ProcessBox.SelectedItem;
                selected.SetTerminationTime(finalTime);
            }
            else
            {
                MessageBox.Show("Please enter a valid time.");
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ProcessBox.ItemsSource).Refresh();
        }

        public void RemoveProcess(ProcessContainer processToRemove)
        {
            //Process.GetProcessById(Id).Kill();
        }
    }

    public class ProcessContainer
    {
        public string ProcessName { get; set; }
        public int Memory {get; set; }
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