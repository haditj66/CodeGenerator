//#define TESTING

using CgenCmakeLibrary;
using CgenCmakeLibrary.FileHandlers;
using SSHHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace CgenCmakeGui
{



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {





        GridGeneratorForUI ggForConfigDisplay;
        GridGeneratorForUI ggUI;

        GridGeneratorForUI gridForOptionsView;

        Rectangle rect2;
        StatusTextHandler statusTextHandler;
        OutputScrollHandler outputScrollHandler;
        OutputScrollHandler guioutputScrollHandler;

        NEXTFileParser nextfile;
        GridGeneratorForUI ggUI1;
        CmakeCacheFileParser cmakecachefile;
        CmakeSettingsFile cmakeSettingsFile;
        SavedOptionsFileHandler savedOptionsFileHandler;

        CMDHandler cmdHandler;
        SSH_Handler SSHhandler;

        Dispatcher dispatcherForMainWindow;


        OptionsView optionsViewWindow;



        //step 1:
        //Dir_Step1.txt:
        //main CMakeLists.txt will call the cgen_start() function. this function will write to
        //${CMAKE_CURRENT_SOURCE_DIR}/CGensaveFiles/Dir_Step1.txt
        // it will write the build directory ${CMAKE_CURRENT_BINARY_DIR}

        //step 2:
        //Dir_Step2.txt:
        //cgen cmakegui command will be called which will read the Dir_Step1.txt file, and forward that information into 
        //the Dir_Step2.txt which will be located at this CgenCmakeGui project directory. information forwared will be the 
        // ${CMAKE_CURRENT_BINARY_DIR} and the ${CMAKE_CURRENT_SOURCE_DIR}

        public static string CMAKE_CURRENT_BINARY_DIR;
        public static string CGEN_PROJECT_DIRECTORY;
        public static string CGEN_CMAKE_SETTINGS_FILE;
        public static string AERTOS_BASE_DIR;
        public static string CMAKE_CURRENT_SOURCE_DIR;
        public static string PLATFORM;
        public static string CMAKE_BUILD_TYPE;
        public static string CGEN_ORIGINAL_PROJECT_DIRECTORY;
        public static string CGEN_DIRECTORY;   
        public static string CGEN_DIRECTORY_OF_CACHE_PROJECT_FILES;

        public static string CGensaveFilesDir;

#if TESTING
        string Dir_Step2 = Environment.CurrentDirectory + "\\..\\..\\..\\..\\CgenCmakeGui\\Dir_Step2Test.txt";
#else
        string Dir_Step2 = Environment.CurrentDirectory + "\\..\\..\\..\\..\\CgenCmakeGui\\Dir_Step2.txt";
#endif



        public MainWindow()
        {
            InitializeComponent();
            InitCmakeGui();
        }
        public void InitCmakeGui()
        {
            //test aaa = new test();
            //aaa.grow();

            //##############
            //get the cmake project build and source directory
            //##############
            string Dir_Step2Contents = File.ReadAllText(Dir_Step2);
            var regex = new Regex(@"CMAKE_CURRENT_BINARY_DIR: (.*)");
            var match = regex.Match(Dir_Step2Contents);
            if (match.Success)
            {
                CMAKE_CURRENT_BINARY_DIR = match.Groups[1].Value.Trim();
            }

            regex = new Regex(@"CMAKE_CURRENT_SOURCE_DIR: (.*)");
            match = regex.Match(Dir_Step2Contents);
            if (match.Success)
            {
                CMAKE_CURRENT_SOURCE_DIR = match.Groups[1].Value.Trim();
            }

            regex = new Regex(@"PLATFORM: (.*)");
            match = regex.Match(Dir_Step2Contents);
            if (match.Success)
            {
                PLATFORM = match.Groups[1].Value.Trim();
            }

            regex = new Regex(@"CMAKE_BUILD_TYPE: (.*)");
            match = regex.Match(Dir_Step2Contents);
            if (match.Success)
            {
                CMAKE_BUILD_TYPE = match.Groups[1].Value.Trim();
            }

             

            regex = new Regex(@"CGEN_ORIGINAL_PROJECT_DIRECTORY: (.*)");
            match = regex.Match(Dir_Step2Contents);
            if (match.Success)
            {
                CGEN_ORIGINAL_PROJECT_DIRECTORY = match.Groups[1].Value.Trim();
            }

            
            regex = new Regex(@"CGEN_DIRECTORY: (.*)");
            match = regex.Match(Dir_Step2Contents);
            if (match.Success)
            {
                CGEN_DIRECTORY = match.Groups[1].Value.Trim();
            }

            regex = new Regex(@"CGEN_PROJECT_DIRECTORY: (.*)");
            match = regex.Match(Dir_Step2Contents);
            if (match.Success)
            {
                CGEN_PROJECT_DIRECTORY = match.Groups[1].Value.Trim();
            }


            regex = new Regex(@"CGEN_CMAKE_SETTINGS_FILE: (.*)");
            match = regex.Match(Dir_Step2Contents);
            if (match.Success)
            {
                CGEN_CMAKE_SETTINGS_FILE = match.Groups[1].Value.Trim();
            }

            regex = new Regex(@"AERTOS_BASE_DIR: (.*)");
            match = regex.Match(Dir_Step2Contents);
            if (match.Success)
            {
                AERTOS_BASE_DIR = match.Groups[1].Value.Trim();
            }

            

            regex = new Regex(@"CGEN_DIRECTORY_OF_CACHE_PROJECT_FILES: (.*)");
            match = regex.Match(Dir_Step2Contents);
            if (match.Success)
            {
                CGEN_DIRECTORY_OF_CACHE_PROJECT_FILES = match.Groups[1].Value.Trim();
                //make back slashes forward slashes
                //CGEN_DIRECTORY_OF_CACHE_PROJECT_FILES.Replace("\\", "/");
                //if NOT empty, put forward slash in beginning
                CGEN_DIRECTORY_OF_CACHE_PROJECT_FILES = CGEN_DIRECTORY_OF_CACHE_PROJECT_FILES != "" ?
                    "\\" + CGEN_DIRECTORY_OF_CACHE_PROJECT_FILES :
                    CGEN_DIRECTORY_OF_CACHE_PROJECT_FILES;

            }

            dispatcherForMainWindow = this.Dispatcher;


            //load all options from the save file
            //string saveFile = File.ReadAllText(Environment.CurrentDirectory+ "../../../../TestFiles/CGensaveFiles/cgenCmakeGuiSaveFile.txt");
            //string saveFile = File.ReadAllText(CMAKE_CURRENT_BINARY_DIR + "/CGensaveFiles/cgenCmakeGuiSaveFile.txt");

            //CGensaveFilesDir = CMAKE_CURRENT_SOURCE_DIR + "\\CGensaveFiles\\cmakeGui" + CGEN_DIRECTORY_OF_CACHE_PROJECT_FILES;//PLATFORM + "\\" + CMAKE_BUILD_TYPE; //CMAKE_CURRENT_BINARY_DIR + "\\CGensaveFiles";
            string CGensaveFilesRootDir = CMAKE_CURRENT_SOURCE_DIR + "\\CGensaveFiles";
            nextfile = new NEXTFileParser(new DirectoryInfo(CGEN_ORIGINAL_PROJECT_DIRECTORY));// CGEN_PROJECT_DIRECTORY + "\\CGensaveFiles\\cmakeGui" + CGEN_DIRECTORY_OF_CACHE_PROJECT_FILES));//( Environment.CurrentDirectory + "../../../../TestFiles/CGensaveFiles"));
            nextfile.RemoveContents();

            cmakeSettingsFile = new CmakeSettingsFile(new DirectoryInfo(CGEN_CMAKE_SETTINGS_FILE), AERTOS_BASE_DIR, CMAKE_CURRENT_BINARY_DIR);

            //this file will use the CGEN_DIRECTORY as this is the one that is reset when a cgen gui reset command is used to start at a new project directory. 
            savedOptionsFileHandler = new SavedOptionsFileHandler(new DirectoryInfo(CGEN_PROJECT_DIRECTORY + "\\CGensaveFiles"));


            //if the original directory is the same as the current use this constructor. otherwise use the other one. 
            if (CGEN_ORIGINAL_PROJECT_DIRECTORY == CGEN_DIRECTORY)
            {
                cmakecachefile = new CmakeCacheFileParser(new DirectoryInfo(CGEN_ORIGINAL_PROJECT_DIRECTORY));
            }
            else
            {
                cmakecachefile = new CmakeCacheFileParser(new DirectoryInfo(CGEN_ORIGINAL_PROJECT_DIRECTORY), CGEN_PROJECT_DIRECTORY);
            }



            int ggui1Row = 6; int ggui1Col = 6;
            ggUI1 = new GridGeneratorForUI(ggui1Row, ggui1Col, mainWindow);
            

            //#################
            //status and output handlers
            //#################
            //TextBlock txtBlock = new TextBlock();
            //txtBlock.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Blue);
            //statusTextHandler = new StatusTextHandler(txtBlock, this.Dispatcher);
            //ggUI1.SetPositionInGrid(txtBlock, 4, ggui1Col, -60);
            //Rectangle rect4 = ggUI1.DrawRectangleAroundGrid(4, ggui1Col);
            //mypanel.Children.Add(txtBlock);
            //mypanel.Children.Add(rect4);

            guiOutput.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            guiOutput.ScrollToEnd();
            guiOutput.Content = "ss";
            guioutputScrollHandler = new OutputScrollHandler(guiOutput, this.Dispatcher, true);
            guiOutput.Visibility = Visibility.Visible;


            outputScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            outputScroll.ScrollToEnd();
            outputScroll.Content = "ss";
            outputScrollHandler = new OutputScrollHandler(outputScroll, this.Dispatcher, true);
            outputScroll.Visibility = Visibility.Visible;



            //#################
            //OptionsSelectedGuiBox
            //#################
            ggForConfigDisplay = new GridGeneratorForUI(10, 3, mainWindow);
            OptionsSelectedGuiBox.Init(ggForConfigDisplay, mypanel, 6, guioutputScrollHandler, () =>
            {
                //write option to the cgenCmakeCache.cmake
                cmakecachefile.WriteOptionsToFile(OptionsSelectedGuiBox.optionsSelected);

                //run the cmake command to get the next config option.
                runCmakeCmd();

                //save/update all options so far
                savedOptionsFileHandler.SaveAllOptions();
            });



            cmakeSettingsFile.LoadData();
            savedOptionsFileHandler.LoadAllOptions();
            OptionsSelectedGuiBox.optionsSelected.Clear();
            var optsionssel = cmakecachefile.LoadOptionsSelected();
            foreach (var item in optsionssel)
            {
                OptionsSelectedGuiBox.AddOptionSelectedToGui(item);

            }




            cmdHandler = new CMDHandler(AERTOS_BASE_DIR);//(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\TestFiles");





            int ggui1Col2 = ggui1Col + 1;
            Button newBtn = new Button();
            newBtn.Content = "Go To Options";
            newBtn.Click += button_Click;
            newBtn.Width = 150;
            newBtn.Height = 100;
            ggUI1.SetPositionInGrid(newBtn, 1, ggui1Col2);
            Rectangle rect = ggUI1.DrawRectangleAroundGrid(1, ggui1Col2);
            mypanel.Children.Add(newBtn);
            mypanel.Children.Add(rect);

            Button btnCmakeConfigSettings = new Button();
            btnCmakeConfigSettings.Content = "cmake settings";
            btnCmakeConfigSettings.Click += button_Click_CmakeSettings;
            btnCmakeConfigSettings.Width = 150;
            btnCmakeConfigSettings.Height = 100;
            ggUI1.SetPositionInGrid(btnCmakeConfigSettings, 2, ggui1Col2);
            Rectangle rect2 = ggUI1.DrawRectangleAroundGrid(2, ggui1Col2);
            mypanel.Children.Add(btnCmakeConfigSettings);
            mypanel.Children.Add(rect2);

            Button btnOptionsConfig = new Button();
            btnOptionsConfig.Content = "Start config Options";
            btnOptionsConfig.Click += button_Click_ConfigOptions;
            btnOptionsConfig.Width = 150;
            btnOptionsConfig.Height = 100;
            ggUI1.SetPositionInGrid(btnOptionsConfig, 3, ggui1Col2);
            Rectangle rect3 = ggUI1.DrawRectangleAroundGrid(3, ggui1Col2);
            mypanel.Children.Add(btnOptionsConfig);
            mypanel.Children.Add(rect3);

            Button btnStartOver = new Button();
            btnStartOver.Content = "Reset config options";
            btnStartOver.Click += StartOver_Click;
            btnStartOver.Width = 150;
            btnStartOver.Height = 100;
            ggUI1.SetPositionInGrid(btnStartOver, 4, ggui1Col2);
            Rectangle rect44 = ggUI1.DrawRectangleAroundGrid(4, ggui1Col2);
            mypanel.Children.Add(btnStartOver);
            mypanel.Children.Add(rect44);

            //TextBlock statusLabel = new TextBlock();
            //statusLabel.FontSize = 20;
            //statusLabel.Text = "Status";
            //ggUI1.SetPositionInGrid(statusLabel, 4, ggui1Col, -60, -60);
            //mypanel.Children.Add(statusLabel);



            //statusTextHandler.display("Initialized", MessageLevel.Normal);





        }






        CmakeSettings win;

        private void button_Click_CmakeSettings(object sender, RoutedEventArgs e)
        {

            win = new CmakeSettings();

            win.SaveButton.Click += button_Click_SaveCmakeSettings;

            win.GeneratorComboBox.Items.Add("Ninja");
            win.GeneratorComboBox.Items.Add("MinGW Makefiles");
            win.GeneratorComboBox.Items.Add("NMake Makefiles");
            win.GeneratorComboBox.Items.Add("Unix Makefiles");
            win.GeneratorComboBox.Items.Add("Visual Studio 15 2017");
            win.GeneratorComboBox.Items.Add("Visual Studio 16 2019");
            win.GeneratorComboBox.Items.Add("CodeBlocks - Unix Makefiles");

            if (cmakeSettingsFile.IsDataLoaded == true)
            {
                CgenCmakeLibrary.FileHandlers.CmakeSettings cm = cmakeSettingsFile.CmakeSettingsData;
                win.CmakeLocationTextBox.Text = cm.CmakeLocation;
                win.GeneratorComboBox.SelectedItem = (string)cm.Generator;
                win.OptionsTextBox.Text = cm.CmakeOptions;
            }
            else
            {

                win.CmakeLocationTextBox.Text = "cmake";
                win.GeneratorComboBox.SelectedItem = (string)"Ninja";
                win.OptionsTextBox.Text = "";
            }

            win.Show();
        }

        private void button_Click_SaveCmakeSettings(object sender, RoutedEventArgs e)
        {
            if (win.GeneratorComboBox.SelectedItem == null)
            {
                //statusTextHandler.display("you need to set the generator", MessageLevel.Problem);
                guioutputScrollHandler.display("you need to set the generator", OutputLevel.Problem);
                return;
            }
            if (win.CmakeLocationTextBox.Text != "cmake")
            {
                if (File.Exists(win.CmakeLocationTextBox.Text) == false)
                {
                    //statusTextHandler.display("that cmake file location does not exist", MessageLevel.Problem);
                    guioutputScrollHandler.display("that cmake file location does not exist", OutputLevel.Problem);
                    return;
                }
            }
            //if (System.IO.Path.GetExtension(win.CmakeLocationTextBox.Text) != "exe")
            //{
            //    statusTextHandler.display("that cmake file location does  \n not point to a cmake .exe", MessageLevel.Problem);
            //    return;
            //}

            //save the settings
            CgenCmakeLibrary.FileHandlers.CmakeSettings cm = new CgenCmakeLibrary.FileHandlers.CmakeSettings();
            cm.CmakeLocation = win.CmakeLocationTextBox.Text;
            cm.Generator = (string)win.GeneratorComboBox.SelectedItem;
            cm.CmakeOptions = (string)win.OptionsTextBox.Text;

            if (win.isRemotePCCheckBox.IsChecked == true)
            {
                cm.IsRemotePC = true;
                cm.RemoteWorkingDirectory = win.RemoteDirectory.Text;
                cm.RemoteIPAddress = win.RemoteIPAddress.Text;
                cm.RemoteUsername = win.RemoteUsername.Text;
                cm.RemotePassword = win.RemotePassword.Text;
            }
            else
            {
                cm.IsRemotePC = false;
                cm.RemoteWorkingDirectory = "";
                cm.RemoteIPAddress = "";
                cm.RemoteUsername = "";
                cm.RemotePassword = "";
            }
            cmakeSettingsFile.SaveData(cm);

        }

        private void WhenOptionDoneAction()
        {
            this.Dispatcher.Invoke(() =>
            {
                guioutputScrollHandler.display("Options configuring Done. Shutting down in 2 seconds.", OutputLevel.Normal);


            });


            this.Dispatcher.Invoke(() =>
            {
                Thread.Sleep(2000);
                //InitCmakeGui();
                //System.Windows.Application.Current.Shutdown();

            });
        }

        private void WhenOptionFoundAction()
        {
            //first update any options that are already saved of this next option. 
            if (AllOptions.Instance.OptionExists(nextfile.NextOption.Name) == true)
            {
                AllOptions.Instance.SetOption(nextfile.NextOption);

                nextfile._SetNextOption(AllOptions.Instance.GetOption(nextfile.NextOption.Name));

                savedOptionsFileHandler.SaveAllOptions();
            }

            //make sure the previous option has been selected yet
            bool isPrevOptionSelectedYet = true;
            if (OptionsSelectedGuiBox.optionsSelected.Count > 0)
            {
                isPrevOptionSelectedYet = !string.IsNullOrEmpty(OptionsSelectedGuiBox.optionsSelected[OptionsSelectedGuiBox.optionsSelected.Count - 1].possibleValueSelection);
            }

            //option must have been found 
            if (isPrevOptionSelectedYet == true)
            {
                this.Dispatcher.Invoke(() =>
                {
                    //var allowablevalues = AllOptions.GetAllowableOptions(nextfile.NextOption, OptionsSelectedGuiBox.optionsSelected);
                    //if (allowablevalues.Count == 1)
                    //{
                    //    OptionsSelectedGuiBox._OptionSelected(allowablevalues[0].Name, OptionsSelectedGuiBox);
                    //}

                    OptionsSelectedGuiBox.AddOptionSelectedToGui(nextfile.NextOption, true);

                    // CreateOptionBox(jj, ii, nextfile.NextOption);
                });

            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    guioutputScrollHandler.display("You need to select the previous option", OutputLevel.Problem);
                });
            }
        }


        Thread ConfigOptThread;
        bool rancmakeAtLeastOnce = false;
        private void button_Click_ConfigOptions(object sender, RoutedEventArgs e)
        {
            if (cmakeSettingsFile.IsDataLoaded == false)
            {
                //statusTextHandler.display("you need to set \n the cmake settings", MessageLevel.Problem);
                guioutputScrollHandler.display("you need to set \n the cmake settings", OutputLevel.Problem);
                return;
            }


            //run cmake at least once if the next file is empty
            //if (rancmakeAtLeastOnce == false)
            //{
            if (nextfile.IsFileContentsFilled() == false)
            {
                runCmakeCmd();
            }
            //    rancmakeAtLeastOnce = true;
            //}


            //ConfigOptThread = new Thread(CheckForNextOptionThread);
            //ConfigOptThread.Start();

            nextfile.StartNextFileUpdater(() =>
            {
                WhenOptionDoneAction();
            }, () =>
            {
                WhenOptionFoundAction();
            }, () =>
            {

                this.Dispatcher.Invoke(() =>
                {

                    InitCmakeGui();

                });
            }
            );


        }

        private void StartOver_Click(object sender, RoutedEventArgs e)
        {
            cmakecachefile.RemoveContents();
            var optsionssel = cmakecachefile.LoadOptionsSelected();
            OptionsSelectedGuiBox.Reset();

            runCmakeCmd();

        }





        private void button_Click(object sender, RoutedEventArgs e)
        {
            //############################
            //optionsview window stuff
            //############################



            this.Dispatcher.Invoke(() =>
            {
                optionsViewWindow = new OptionsView();

                //List<Button> buttons = new List<Button>();
                //buttons.Add(optionsViewWindow.button1);
                //buttons.Add(optionsViewWindow.button2);
                //buttons.Add(optionsViewWindow.button3);
                //buttons.Add(optionsViewWindow.button4);
                //buttons.Add(optionsViewWindow.button5);
                //buttons.Add(optionsViewWindow.button6);
                //buttons.Add(optionsViewWindow.button7);
                //buttons.Add(optionsViewWindow.button8);
                //buttons.Add(optionsViewWindow.button9);
                //buttons.Add(optionsViewWindow.button10);

                //                <Button x:Name="button1" Content="edit Option" Canvas.Left="95" Canvas.Top="76"/>
                //<Button x:Name="button2" Content="edit Option" Canvas.Left="95" Canvas.Top="76"/>
                //<Button x:Name="button3" Content="edit Option" Canvas.Left="95" Canvas.Top="76"/>
                //<Button x:Name="button4" Content="edit Option" Canvas.Left="95" Canvas.Top="76"/>
                //<Button x:Name="button5" Content="edit Option" Canvas.Left="95" Canvas.Top="76"/>
                //<Button x:Name="button6" Content="edit Option" Canvas.Left="95" Canvas.Top="76"/>
                //<Button x:Name="button7" Content="edit Option" Canvas.Left="95" Canvas.Top="76"/>
                //<Button x:Name="button8" Content="edit Option" Canvas.Left="95" Canvas.Top="76"/>
                //<Button x:Name="button9" Content="edit Option" Canvas.Left="95" Canvas.Top="76"/>
                //<Button x:Name="button10" Content="edit Option" Canvas.Left="218" Canvas.Top="118"/>

                gridForOptionsView = new GridGeneratorForUI(8, 3, optionsViewWindow);
                OptionsViewGuiBox.Init(gridForOptionsView, optionsViewWindow.mypanel, 7, savedOptionsFileHandler);

                foreach (var item in AllOptions.Instance.Options)
                {
                    OptionsViewGuiBox.AddOptionToGui(item);
                }

                optionsViewWindow.Show();
            });
        }


        private void runCmakeCmd()
        {
            dispatcherForMainWindow.Invoke(() =>
            {
                //statusTextHandler.display("running cmake command", MessageLevel.Normal);
                guioutputScrollHandler.display("running cmake command", OutputLevel.Normal);
                string cmd = cmakeSettingsFile.getCmakeCmd();

                //write contents of set(CGEN_GUI_SET FALSE) in file FromGuiOrProject.cmake
                File.WriteAllText(Environment.CurrentDirectory + "\\..\\..\\..\\..\\CgenCmakeGui\\FromGuiOrProject.cmake", "set(CGEN_GUI_SET FALSE)");
                cmdHandler.ExecuteCommand(cmd);
                //write contents of set(CGEN_GUI_SET TRUE) in file FromGuiOrProject.cmake
                File.WriteAllText(Environment.CurrentDirectory + "\\..\\..\\..\\..\\CgenCmakeGui\\FromGuiOrProject.cmake", "set(CGEN_GUI_SET TRUE)");

                outputScrollHandler.display("\n\n\n\n ###########################################\n###########################################", OutputLevel.Normal);
                outputScrollHandler.display(cmdHandler.Output, OutputLevel.Normal);
                outputScrollHandler.display(cmdHandler.Error, OutputLevel.Problem);

                //statusTextHandler.display("cmake done configuring", MessageLevel.Normal);
                guioutputScrollHandler.display("cmake done configuring", OutputLevel.Normal);
            });


        }



        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            ShutDownGui();
        }

        protected void ShutDownGui()
        {

            if (SSHhandler != null)
            {
                SSHhandler.Dispose();
            }



            nextfile.StopNextFileUpdater();

            if (ConfigOptThread != null)
            {
                ConfigOptThread.Abort();
            }

            Application.Current.Shutdown();
        }

    }
}
