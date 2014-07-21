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
using System.Diagnostics;

namespace Task_Timer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<ProcessContainer> items = new List<ProcessContainer>();
            foreach (Process process in Process.GetProcesses())
            {
                items.Add(new ProcessContainer(process.ProcessName, 10));
            }

            ProcessBox.ItemsSource = items;
        }
    }

    public class ProcessContainer
    {
        public string ProcessName { get; set; }
        public int Memory {get; set; }

        public ProcessContainer(string name, int memory)
        {
            ProcessName = name;
            Memory = memory;
        }
    }

}