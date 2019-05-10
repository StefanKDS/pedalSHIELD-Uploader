using ArduinoUploader;
using ArduinoUploader.Hardware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace pedalSHIELD_Uploader
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string m_folder;
        private List<string> m_portsList;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Initializes a new instance of the pedalSHIELD_Uploader.MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            m_folder = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            m_folder += "\\HEX";

            if (!Directory.Exists(m_folder))
            {
                try
                { Directory.CreateDirectory(m_folder); }
                catch
                { return; }
            }

            LoadHex();
            m_portsList = GetAllPorts();

            this.MouseLeftButtonDown += delegate { DragMove(); };

            Info.Content = "\r\nReady for TakeOff....";
        }

        /// <summary>
        /// Loads the hexadecimal.
        /// </summary>
        private void LoadHex()
        {
            var files = Directory.EnumerateFiles(m_folder).OrderByDescending(filename => filename);

            foreach (var file in files)
            {
                string filename = System.IO.Path.GetFileName(file.ToString());
                HexFiles.Items.Add(filename);
            }
        }

        /// <summary>
        /// Gets all ports.
        /// </summary>
        /// <returns>
        /// all ports.
        /// </returns>
        public List<string> GetAllPorts()
        {
            List<String> allPorts = new List<String>();
            foreach (String portName in System.IO.Ports.SerialPort.GetPortNames())
            {
                allPorts.Add(portName);
                Ports.Items.Add(portName);
            }
            return allPorts;
        }

        /// <summary>
        /// Event handler. Called by Button for click events.
        /// </summary>
        /// <param name="sender">
        /// Source of the event.
        /// </param>
        /// <param name="e">
        /// Routed event information.
        /// </param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ArduinoModel model = new ArduinoModel();

            if (SHILED.SelectedValue == null || Ports.SelectedValue == null || HexFiles.SelectedItem == null)
            {
                Info.Content = "\r\nPlease set\r\nall parameters !";
                return;
            }

            if (SHILED.SelectedValue.ToString().Contains("MEGA"))
                model = ArduinoModel.Mega2560;
            if (SHILED.SelectedValue.ToString().Contains("UNO"))
                model = ArduinoModel.UnoR3;
            // if (SHILED.SelectedValue.ToString().Contains("DUE"))
            //     model = ArduinoModel.Mega2560;

            Info.Content = "\r\nUploading....";
            System.Threading.Thread.Sleep(500);

            var uploader = new ArduinoSketchUploader(
            new ArduinoSketchUploaderOptions()
            {
                FileName = m_folder + "\\" + HexFiles.SelectedItem,
                PortName = Ports.SelectedValue.ToString(),
                ArduinoModel = model
            });

            try
            {    
                uploader.UploadSketch();
            }catch
            {
                Info.Content = "\r\nSorry\r\n\r\nCouldn't upload file !\r\n\r\nLet's try again....";
            }

            Info.Content = "\r\nDONE !";
        }

        /// <summary>
        /// Event handler. Called by Button_Close for click events.
        /// </summary>
        /// <param name="sender">
        /// Source of the event.
        /// </param>
        /// <param name="e">
        /// Routed event information.
        /// </param>
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
