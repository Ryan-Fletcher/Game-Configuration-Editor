using System;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Game_Configuration_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
                    rtbSupportedFiles.AppendText(line + "\r");
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

        public MainWindow()
        {
            InitializeComponent();
            ConfigList();
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
       
        //  Allows the program to scan the current directory for the file types designated
        private void btnScan_Click(object sender, RoutedEventArgs e)
        {
            // Used to bring up error message safely
            try
            {
                // Clears the scanned list to prevent repeating buttons
                spGameList.Children.Clear();

                // Scans for the amount of files inside of ConfigFiles.txt
                for (int i = 0; i < read.Count; i++)
                {
                    // String array that designates which folder to search in, what to search for, and which directories to search inside of
                    string[] found = Directory.GetFiles(txtDestination.Text, read[i], SearchOption.AllDirectories);

                    // Creates new button 'btnGameFile'
                    System.Windows.Controls.Button btnGameFile = new System.Windows.Controls.Button();

                    // Inputs the file path for each config file found into the content of each button
                    btnGameFile.Content = found[0];
                    // Inputs the file name for each button to differentiate them
                    btnGameFile.Name = "btn" + i.ToString();
                    // Creates new event for each button that allows each button to be programmed in a separate function
                    btnGameFile.Click += new RoutedEventHandler(btnFile_Click);
                    // Dynamically creates a new button
                    spGameList.Children.Add(btnGameFile);                 
                }
            }         

            // If an issue finding the directory occurs, display the issue to an error
            catch
            {
                // Displays the error to a message box
                System.Windows.Forms.MessageBox.Show("Cannot find file path", "ERROR: Cannot find file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        // Sets the buttons to read the data inside their file pathings to the Configuration Settings
        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            // Sets the newly created buttons to be designated as the same from the "btnScan_Click" function
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
            // Collects the file destination from the buttons created in the "btnScan_click" function
            string fileURL = btn.Content.ToString();

            TextRange range;
            FileStream fStream;


            // Opens the file destination if it can be found
            if (File.Exists(fileURL))
            {
                // Designates where to start and end within the config file document
                range = new TextRange(rtbConfigSettings.Document.ContentStart, rtbConfigSettings.Document.ContentEnd);
                // Opens the file at the "fileURL" destination. This allows each button to open their designated file
                fStream = new FileStream(fileURL, FileMode.OpenOrCreate);
                // Loads text in the document within the range 
                range.Load(fStream, System.Windows.Forms.DataFormats.Text);
                // Closes the file
                fStream.Close();
            }
        }

        // Saves the text inside of the Configuration Settings as a new .ini file
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string rtbText = new TextRange(rtbConfigSettings.Document.ContentStart, rtbConfigSettings.Document.ContentEnd).Text;

            if (rtbText.Length <= 2)
            {
                System.Windows.Forms.MessageBox.Show("No configuration settings found", "ERROR: Nothing to save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                // Allows only .ini file extensions to be saved
                saveFileDialog.Filter = "ini file (*.ini)|*.ini";
                // Title of save file dialog
                saveFileDialog.Title = "Save a configuration file";
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.OpenFile()))
                    {
                        sw.Write(rtbText);
                    }
                }
            }
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void mnuInstructions_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("1. Click 'Change Directory' to select a folder to scan through \n" +
                "2. Press 'Scan' to search through the previously selected directory \n" +
                "3. Select the configuration file found to begin editing \n" +
                "4. The selected file will appear in the right-side panel, here you can edit the configuration file \n" +
                "5. When finished, press 'Save' to save a new .ini file \n\n" +
                "Supported configuration files are shown on the bottom-left \n" +
                "To add more configuration files to the program, insert the configuration file to ConfigFiles.txt", "Tutorial");
        }
    }
}