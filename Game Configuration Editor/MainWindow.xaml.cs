using System;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;


namespace Game_Configuration_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ConfigList();
        }

        // Used to read the file location for scanning and identifying supported config files
        List<string> read = new List<string>();
            
        // Writes each of the configuration files into a rich text box
        private void ConfigList()
        {
            String line;

            try
            {
                // Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Environment.CurrentDirectory + @"\ConfigFiles.txt");

                // Read the first line of text
                line = sr.ReadLine();

                // Continue to read until you reach end of file
                while (line != null)
                {
                    read.Add(line);
                    // Write each line to "rtbConfigFiles"
                    rtbConfigFiles.AppendText(line + "\r");
                    // Read the next line
                    line = sr.ReadLine();
                }

                // Close the file
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        //  Allows the program to scan the current directory for the file types designated
        private void btnScan_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                spGameList.Children.Clear();
                for (int i = 0; i < read.Count; i++)
                {
                    //string[] temp = Directory.GetFiles(txtDestination.Text, read[i], SearchOption.AllDirectories);
                    string[] found = Directory.GetFiles(txtDestination.Text, read[i], SearchOption.AllDirectories);

                    System.Windows.Controls.Button btnGameFile = new System.Windows.Controls.Button();

                    //filePaths.Add(temp);

                    // Inputs the scanned documents to the list box
                    //rtbGameConfigFiles.AppendText(read[i] + "\r");


                    btnGameFile.Content = found[0];
                    btnGameFile.Name = "btn" + i.ToString();

                    spGameList.Children.Add(btnGameFile);
                }
            }
            catch (Exception r)
            {
                Console.WriteLine("Exception: " + r.Message);
                System.Windows.Forms.MessageBox.Show("Please select a directory", "ERROR: No Directory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //  Changes the directory the scanning tool will search in
        private void btnChangeDir_Click(object sender, RoutedEventArgs e)
        {
            // Creates new FolderBrowserDialog 
            FolderBrowserDialog browse = new FolderBrowserDialog();
            // Show the FolderBrowserDialog
            DialogResult result = browse.ShowDialog();

            // Dictates whether the "New Folder" button appears in the browser            
            browse.ShowNewFolderButton = true;

            if (result.ToString() == "OK")
            {
                // Inputs the path destination to the text box "txtDestination"
                txtDestination.Text = browse.SelectedPath;
                // Represents the folder from which the browser starts from
                Environment.SpecialFolder root = browse.RootFolder;
            }   
        }
    }
}