#define TESTING

using CgenCmakeLibrary;
using CgenCmakeLibrary.FileHandlers;
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
        Rectangle rect2;
        StatusTextHandler statusTextHandler;
        OutputScrollHandler outputScrollHandler;
        OutputScrollHandler guioutputScrollHandler;

        NEXTFileParser nextfile;
        CmakeCacheFileParser cmakecachefile;
        CmakeSettingsFile cmakeSettingsFile;
        SavedOptionsFileHandler savedOptionsFileHandler;

        CMDHandler cmdHandler;


        Dispatcher dispatcherForMainWindow;


        public enum OptionGuiBoxStates
        {
            Hidden,
            Selected,
            Unselected
        }
        public class OptionsSelectedGuiBox
        {
            public void SetState(OptionGuiBoxStates optionGuiBoxStates)
            {
                if (optionGuiBoxStates == OptionGuiBoxStates.Selected)
                {
                    this.forCombBox.IsEnabled = false;
                    this.forRectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Aqua);
                    this.forRectangle.Opacity = .05;
                    forCombBox.Visibility = Visibility.Visible;
                    forRectangle.Visibility = Visibility.Visible;
                    forLabelName.Visibility = Visibility.Visible;
                    forLabelDesc.Visibility = Visibility.Visible;
                }
                else if (optionGuiBoxStates == OptionGuiBoxStates.Hidden)
                {
                    this.forCombBox.IsEnabled = false;
                    //this.forRectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                    this.forRectangle.Opacity = 0;
                    forCombBox.Visibility = Visibility.Hidden;
                    forRectangle.Visibility = Visibility.Hidden;
                    forLabelName.Visibility = Visibility.Hidden;
                    forLabelDesc.Visibility = Visibility.Hidden;
                   
                }
                else if (optionGuiBoxStates == OptionGuiBoxStates.Unselected)
                {

                    this.forCombBox.IsEnabled = true;
                    forCombBox.Visibility = Visibility.Visible;
                    forRectangle.Visibility = Visibility.Visible;
                    forLabelName.Visibility = Visibility.Visible;
                    forLabelDesc.Visibility = Visibility.Visible;
                    //this.forRectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                    this.forRectangle.Opacity = 0;
                }
            }


            public static void Init(GridGeneratorForUI forGrid, Canvas forCanvas, Action callbackPossibleValueSelected)
            {
                CallbackPossibleValueSelected = callbackPossibleValueSelected;
                ForGrid = forGrid;
                ForCanvas = forCanvas;

                AllOptionsSelectedGui = new List<OptionsSelectedGuiBox>();
                optionsSelected = new List<OptionsSelected>();

                OptionsSelectedGuiBox optSel;

                int ii = 1;
                int jj = 1;
                for (int ind = 0; ind < forGrid.Rows * forGrid.Columns; ind++)
                {

                    optSel = new OptionsSelectedGuiBox(jj, ii);
                    optSel.forRectangle = ForGrid.DrawRectangleAroundGrid(jj, ii);
                    ForCanvas.Children.Add(optSel.forRectangle);

                    AllOptionsSelectedGui.Add(optSel);
                    optSel.SetState(OptionGuiBoxStates.Hidden);

                    jj = jj >= forGrid.Rows ? 1 : jj + 1;
                    ii = jj == 1 ? ii + 1 : ii;

                     
                }
            }

            public static void AddOptionSelectedToGui(Option optToAdd, int GuiNumberToAdd)
            {

                OptionsSelected optSelToAdd = new OptionsSelected();
                optSelToAdd.option = optToAdd;

                var allOpt = OptionsSelectedGuiBox.AllOptionsSelectedGui[GuiNumberToAdd];
                allOpt.ForOptionSelected = optSelToAdd;

                allOpt.forLabelName.Content = optSelToAdd.option.Name;

                allOpt.forLabelDesc.Content = optSelToAdd.option.Description;

                allOpt.forCombBox.SelectionChanged += allOpt.OptionPossibleValueSelected;
                allOpt.forCombBox.Width = 120;
                allOpt.forCombBox.Visibility = Visibility.Visible;
                foreach (var item in optSelToAdd.option.MyPossibleValues)
                {
                    allOpt.forCombBox.Items.Add(item.Name);
                }

                optionsSelected.Add(optSelToAdd);

                allOpt.SetState(OptionGuiBoxStates.Unselected);
            }

            private void OptionPossibleValueSelected(object sender, SelectionChangedEventArgs e)
            {
                ForOptionSelected.possibleValueSelection = (string)e.AddedItems[0];

                //fill rectangle and turn off combobox
                this.SetState(OptionGuiBoxStates.Selected);


                if (CallbackPossibleValueSelected != null)
                {
                    CallbackPossibleValueSelected();
                }

            }

            private OptionsSelectedGuiBox(int forGridRow, int forGridCol)
            {
                ForGridRow = forGridRow;
                ForGridCol = forGridCol;

                forLabelName = new Label();
                forLabelDesc = new Label();
                forCombBox = new ComboBox();

                //set positions
                ForGrid.SetPositionInGrid(forLabelName, ForGridRow, ForGridCol, 0, -25);
                ForGrid.SetPositionInGrid(forLabelDesc, ForGridRow, ForGridCol, -100, 17);
                ForGrid.SetPositionInGrid(forCombBox, ForGridRow, ForGridCol);


                ForCanvas.Children.Add(forCombBox);
                ForCanvas.Children.Add(forLabelName);
                ForCanvas.Children.Add(forLabelDesc);
            }

            public ComboBox forCombBox;
            public Rectangle forRectangle;
            public Label forLabelName;
            public Label forLabelDesc;

            private static GridGeneratorForUI ForGrid;
            private static Canvas ForCanvas; 
            private static Action CallbackPossibleValueSelected;
            

            public static List<OptionsSelectedGuiBox> AllOptionsSelectedGui;
            public static List<OptionsSelected> optionsSelected;
             
            public int ForGridRow { get; }
            public int ForGridCol { get; }
            private OptionsSelected ForOptionSelected;
        }





        //private void CreateOptionBox(int row, int col, Option forOption)
        //{

        //    Label optionNamelabel = new Label();
        //    optionNamelabel.Content = forOption.Name;

        //    Label optionDescriptionlabel = new Label();
        //    optionDescriptionlabel.Content = forOption.Description;

        //    ComboBox com = new ComboBox();
        //    com.SelectionChanged += OptionPossibleValueSelected;
        //    com.Width = 120;
        //    com.Visibility = Visibility.Visible;
        //    foreach (var item in forOption.MyPossibleValues)
        //    {
        //        com.Items.Add(item.Name);
        //    }

        //    ggForConfigDisplay.SetPositionInGrid(optionNamelabel, row, col, 0, -25);
        //    ggForConfigDisplay.SetPositionInGrid(optionDescriptionlabel, row, col, -100, 17);
        //    ggForConfigDisplay.SetPositionInGrid(com, row, col);
        //    rect2 = ggForConfigDisplay.DrawRectangleAroundGrid(row, col);

        //    mypanel.Children.Add(com);
        //    mypanel.Children.Add(rect2);
        //    mypanel.Children.Add(optionNamelabel);
        //    mypanel.Children.Add(optionDescriptionlabel);


        //    //set in options selected list
        //    optionsSelected.Add(new OptionsSelectedGuiBox() { option = forOption, forCombBox = com, forRectangle = rect2 });
        //}





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
        public static string CMAKE_CURRENT_SOURCE_DIR;
        public static string CGensaveFilesDir;

#if TESTING
        string Dir_Step2 = Environment.CurrentDirectory + "\\..\\..\\..\\..\\CgenCmakeGui\\Dir_Step2.txt";
#else
        string Dir_Step2 = Environment.CurrentDirectory + "\\..\\..\\..\\..\\CgenCmakeGui\\Dir_Step2.txt";
#endif



        public MainWindow()
        {
            InitializeComponent();

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

            dispatcherForMainWindow = this.Dispatcher;


            //load all options from the save file
            //string saveFile = File.ReadAllText(Environment.CurrentDirectory+ "../../../../TestFiles/CGensaveFiles/cgenCmakeGuiSaveFile.txt");
            //string saveFile = File.ReadAllText(CMAKE_CURRENT_BINARY_DIR + "/CGensaveFiles/cgenCmakeGuiSaveFile.txt");
            CGensaveFilesDir = CMAKE_CURRENT_BINARY_DIR + "\\CGensaveFiles";
            string CGensaveFilesRootDir = CMAKE_CURRENT_SOURCE_DIR + "\\CGensaveFiles";
            nextfile = new NEXTFileParser(new DirectoryInfo(CGensaveFilesDir));//( Environment.CurrentDirectory + "../../../../TestFiles/CGensaveFiles"));
            cmakecachefile = new CmakeCacheFileParser(new DirectoryInfo(CGensaveFilesDir));
            cmakeSettingsFile = new CmakeSettingsFile(new DirectoryInfo(CGensaveFilesDir), CMAKE_CURRENT_SOURCE_DIR, CMAKE_CURRENT_BINARY_DIR);
            savedOptionsFileHandler = new SavedOptionsFileHandler(new DirectoryInfo(CGensaveFilesRootDir));


            ggForConfigDisplay = new GridGeneratorForUI(10, 3, mainWindow);
            OptionsSelectedGuiBox.Init(ggForConfigDisplay, mypanel, ()=> {
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
            OptionsSelectedGuiBox.optionsSelected.AddRange(cmakecachefile.LoadOptionsSelected());
            //cmakecachefile.WriteOptionsToFile(optionsSelected.Cast<OptionsSelected>().ToList());
            //optionsSelected.AddRange(cmakecachefile.LoadOptionsSelected());

            //OptionsSelectedGuiBox.AddOptionSelectedToGui(OptionsSelectedGuiBox.optionsSelected[0], 0);


            cmdHandler = new CMDHandler(CMAKE_CURRENT_SOURCE_DIR);//(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\TestFiles");



            // var opt = Option.Deserialize(saveFile);

            int ggui1Row = 6; int ggui1Col = 6;
            GridGeneratorForUI ggUI1 = new GridGeneratorForUI(ggui1Row, ggui1Col, mainWindow);//, mypanel);
                                                                                              //ggUI = ggUI1.CreateGridFromGridUI(1, 2, 8, 1);

            //, mypanel);
            //System.Windows.Controls.ListBox lsbox = new ListBox();

            //outputScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;



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
            //GridGeneratorForUI ggUIFortextBlock = new GridGeneratorForUI(3,1,mainWindow);


            Button newBtn = new Button();
            newBtn.Content = "Go To Options";
            newBtn.Click += button_Click;
            newBtn.Width = 150;
            newBtn.Height = 100;
            ggUI1.SetPositionInGrid(newBtn, 1, ggui1Col);
            Rectangle rect = ggUI1.DrawRectangleAroundGrid(1, ggui1Col);
            mypanel.Children.Add(newBtn);
            mypanel.Children.Add(rect);

            Button btnCmakeConfigSettings = new Button();
            btnCmakeConfigSettings.Content = "cmake settings";
            btnCmakeConfigSettings.Click += button_Click_CmakeSettings;
            btnCmakeConfigSettings.Width = 150;
            btnCmakeConfigSettings.Height = 100;
            ggUI1.SetPositionInGrid(btnCmakeConfigSettings, 2, ggui1Col);
            Rectangle rect2 = ggUI1.DrawRectangleAroundGrid(2, ggui1Col);
            mypanel.Children.Add(btnCmakeConfigSettings);
            mypanel.Children.Add(rect2);

            Button btnOptionsConfig = new Button();
            btnOptionsConfig.Content = "Start config Options";
            btnOptionsConfig.Click += button_Click_ConfigOptions;
            btnOptionsConfig.Width = 150;
            btnOptionsConfig.Height = 100;
            ggUI1.SetPositionInGrid(btnOptionsConfig, 3, ggui1Col);
            Rectangle rect3 = ggUI1.DrawRectangleAroundGrid(3, ggui1Col);
            mypanel.Children.Add(btnOptionsConfig);
            mypanel.Children.Add(rect3);

            TextBlock statusLabel = new TextBlock();
            statusLabel.FontSize = 20;
            statusLabel.Text = "Status";
            ggUI1.SetPositionInGrid(statusLabel, 4, ggui1Col, -60, -60);
            mypanel.Children.Add(statusLabel);

            TextBlock txtBlock = new TextBlock();
            txtBlock.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Blue);
            statusTextHandler = new StatusTextHandler(txtBlock, this.Dispatcher);
            ggUI1.SetPositionInGrid(txtBlock, 4, ggui1Col, -60);
            Rectangle rect4 = ggUI1.DrawRectangleAroundGrid(4, ggui1Col);
            mypanel.Children.Add(txtBlock);
            mypanel.Children.Add(rect4);

            statusTextHandler.display("Initialized", MessageLevel.Normal);



        }



        private void CheckForNextOptionThread()
        {
            int ii = 0; 

            //run cmake at least once if the next file is empty
            if (nextfile.IsFileContentsFilled() == false)
            {
                runCmakeCmd();
            }

            while (true)
            {
                Thread.Sleep(500);

                NextStatus nextStatus = nextfile.ParseNextFile();

                if (nextStatus != NextStatus.Empty)
                {

                    //if it is done then display a done on the output
                    if (nextStatus == NextStatus.Done)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            guioutputScrollHandler.display("Options configuring Done", OutputLevel.Normal);
                            //statusTextHandler.display("Options configuring Done", MessageLevel.Normal);
                        });

                    }
                    else if (nextStatus == NextStatus.OptionFound)
                    {
                        //option must have been found 
                        this.Dispatcher.Invoke(() =>
                        {
                            OptionsSelectedGuiBox.AddOptionSelectedToGui(nextfile.NextOption,ii);

                           // CreateOptionBox(jj, ii, nextfile.NextOption);
                        });

                        ii++;
                    }
                }
            }
        }



        private void CreateOptionBox(int row, int col, Option forOption)
        {

            Label optionNamelabel = new Label();
            optionNamelabel.Content = forOption.Name;

            Label optionDescriptionlabel = new Label();
            optionDescriptionlabel.Content = forOption.Description;

            ComboBox com = new ComboBox();
            com.SelectionChanged += OptionPossibleValueSelected;
            com.Width = 120;
            com.Visibility = Visibility.Visible;
            foreach (var item in forOption.MyPossibleValues)
            {
                com.Items.Add(item.Name);
            }

            ggForConfigDisplay.SetPositionInGrid(optionNamelabel, row, col, 0, -25);
            ggForConfigDisplay.SetPositionInGrid(optionDescriptionlabel, row, col, -100, 17);
            ggForConfigDisplay.SetPositionInGrid(com, row, col);
            rect2 = ggForConfigDisplay.DrawRectangleAroundGrid(row, col);

            mypanel.Children.Add(com);
            mypanel.Children.Add(rect2);
            mypanel.Children.Add(optionNamelabel);
            mypanel.Children.Add(optionDescriptionlabel);


            //set in options selected list
            //optionsSelected.Add(new OptionsSelectedGuiBox() { option = forOption, forCombBox = com, forRectangle = rect2 });
        }




        private void OptionPossibleValueSelected(object sender, SelectionChangedEventArgs e)
        {
            //optionsSelected[optionsSelected.Count - 1].possibleValueSelection = (string)e.AddedItems[0];

            ////fill rectangle and turn off combobox
            //optionsSelected[optionsSelected.Count - 1].forCombBox.IsEnabled = false;
            //optionsSelected[optionsSelected.Count - 1].forRectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Aqua);
            //optionsSelected[optionsSelected.Count - 1].forRectangle.Opacity = .05;


            ////write option to the cgenCmakeCache.cmake 
            //cmakecachefile.WriteOptionsToFile(optionsSelected.Cast<OptionsSelected>().ToList());

            ////run the cmake command to get the next config option.
            //runCmakeCmd();


            ////save/update all options so far
            //savedOptionsFileHandler.SaveAllOptions();

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
            cmakeSettingsFile.SaveData(cm);

        }

        private void button_Click_ConfigOptions(object sender, RoutedEventArgs e)
        {
            if (cmakeSettingsFile.IsDataLoaded == false)
            {
                //statusTextHandler.display("you need to set \n the cmake settings", MessageLevel.Problem);
                guioutputScrollHandler.display("you need to set \n the cmake settings", OutputLevel.Problem);
                return;
            }
            new Thread(CheckForNextOptionThread).Start();
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {


            OptionsView win = new OptionsView();

            int index = 1;
            int translation = 30;
            AllOptions.Instance.Options.ForEach(op =>
            {

                ComboBox com = new ComboBox();
                com.Width = 180;
                TranslateTransform translateTransform1 =
                new TranslateTransform(50, 20 + (index * translation));

                com.RenderTransform = translateTransform1;
                com.Items.Add(op.Name);
                com.IsEditable = false;
                com.Text = op.Name;
                op.MyPossibleValues.ForEach(pv =>
                { com.Items.Add(pv.Name); });

                win.propCanvas.Children.Add(com);

                index++;
            });

            win.Show();
        }


        private void runCmakeCmd()
        {
            dispatcherForMainWindow.Invoke(() =>
            {
                //statusTextHandler.display("running cmake command", MessageLevel.Normal);
                guioutputScrollHandler.display("running cmake command", OutputLevel.Normal);
                string cmd = cmakeSettingsFile.getCmakeCmd();
                cmdHandler.ExecuteCommand(cmd);// ("cmake -S . "); 

                outputScrollHandler.display("\n\n\n\n ###########################################\n###########################################", OutputLevel.Normal);
                outputScrollHandler.display(cmdHandler.Output, OutputLevel.Normal);
                outputScrollHandler.display(cmdHandler.Error, OutputLevel.Problem);

                //statusTextHandler.display("cmake done configuring", MessageLevel.Normal);
                guioutputScrollHandler.display("cmake done configuring", OutputLevel.Normal);
            });


        }

    }
}
