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
        ProcessManager Manager;

        public MainWindow()
        {
            InitializeComponent();
            Manager = new ProcessManager();
            Manager.RefreshProcessList();

            TimePickerBox.Value = DateTime.Now;
            ScheduledBox.ItemsSource = Manager.RunningTimers;
            ProcessBox.ItemsSource = Manager.ProcessList;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ProcessBox.ItemsSource);
            view.Filter = ProcessFilter;
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
                Manager.ScheduleShutdown(selected, finalTime);
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
    }
}