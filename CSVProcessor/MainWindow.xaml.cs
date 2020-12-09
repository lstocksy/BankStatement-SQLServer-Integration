using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using CSVProcessor.Classes;

namespace CSVProcessor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetCSVFile(object sender, RoutedEventArgs e)
        {
            try
            {
                string txtPath = "";
                var dlg = new OpenFileDialog();

                //Open the Pop-Up Window to select the file 
                if (dlg.ShowDialog() == true)
                {
                    new FileInfo(dlg.FileName);
                    using (Stream s = dlg.OpenFile())
                    {
                        TextReader reader = new StreamReader(s);
                        string st = reader.ReadToEnd();
                        txtPath = dlg.FileName;
                        Console.WriteLine(txtPath);
                    }
                }

                ProcessCSV fileProccessor = new ProcessCSV(txtPath);
                List<string> unpaidList = fileProccessor.getMembers();

                UnpaidMembersGrid.ItemsSource = unpaidList;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error checking file");
            }

        }

        //allow window to be moved on click
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        //close window when button pressed
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
