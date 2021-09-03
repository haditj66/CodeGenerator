#define TESTING

using CgenCmakeGui.OptionsViewModels;
using CgenCmakeLibrary;
using CgenCmakeLibrary.FileHandlers;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CgenCmakeGui
{

    public enum OptionViewGuiStates
    {
        Hidden,
        Visible
    }

    public class OptionsViewGuiBox
    {



        public void SetState(OptionViewGuiStates optionGuiBoxStates)
        {

            if (optionGuiBoxStates == OptionViewGuiStates.Hidden)
            {
                this.forCombBox.IsEnabled = false;
                //this.forRectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                this.forRectangle.Opacity = .5;
                forCombBox.Visibility = Visibility.Hidden;
                forRectangle.Visibility = Visibility.Hidden;
                forLabelName.Visibility = Visibility.Hidden;
                forLabelDesc.Visibility = Visibility.Hidden;
                ButtonToEditOption.Visibility = Visibility.Hidden;

            }
            else if (optionGuiBoxStates == OptionViewGuiStates.Visible)
            {

                this.forCombBox.IsEnabled = true;
                forCombBox.Visibility = Visibility.Visible;
                forRectangle.Visibility = Visibility.Visible;
                forLabelName.Visibility = Visibility.Visible;
                forLabelDesc.Visibility = Visibility.Visible;
                ButtonToEditOption.Visibility = Visibility.Visible;
                //this.forRectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                this.forRectangle.Opacity = .5;
            }
        }


        public static void Init(GridGeneratorForUI forGrid, Canvas forCanvas, int usethisManyRows, SavedOptionsFileHandler savedOptHandler)
        {

            ForGrid = forGrid;
            ForCanvas = forCanvas;

            SavedOptHandler = savedOptHandler;

            GuiNumberToAddIndex = 0;

            AllOptionViewGui = new List<OptionsViewGuiBox>();
            OptionsViewGuiBox optView = null;

            int ii = 1;
            int jj = 1;
            for (int ind = 0; ind < usethisManyRows * forGrid.Columns; ind++)
            {
                optView = new OptionsViewGuiBox(jj, ii);
                optView.forRectangle = ForGrid.DrawRectangleAroundGrid(jj, ii);
                ForCanvas.Children.Add(optView.forRectangle);

                AllOptionViewGui.Add(optView);
                optView.SetState(OptionViewGuiStates.Hidden);

                jj = jj >= usethisManyRows ? 1 : jj + 1;
                ii = jj == 1 ? ii + 1 : ii;


            }
        }

        static int GuiNumberToAddIndex;



        public static void AddOptionToGui(Option optToAdd)
        {
            var allOpt = OptionsViewGuiBox.AllOptionViewGui[GuiNumberToAddIndex];


            _AddOptionSelectedToGui(optToAdd);

            allOpt.SetState(OptionViewGuiStates.Visible);

        }


        private static void _AddOptionSelectedToGui(Option optSelToAdd)
        {

            var allOpt = OptionsViewGuiBox.AllOptionViewGui[GuiNumberToAddIndex];
            allOpt.ForOption = optSelToAdd;

            allOpt.forLabelName.Content = optSelToAdd.Name;

            allOpt.forLabelDesc.Content = optSelToAdd.Description;

            allOpt.forCombBox.Width = 120;
            allOpt.forCombBox.Visibility = Visibility.Visible;
            allOpt.forCombBox.IsEnabled = true;
            allOpt.forCombBox.Items.Clear();

            allOpt.ButtonToEditOption.Width = 50;
            allOpt.ButtonToEditOption.Height = 50;
            allOpt.ButtonToEditOption.Visibility = Visibility.Visible;
            allOpt.ButtonToEditOption.Click += allOpt.OpenEditButtonPageCallback;

            foreach (var item in optSelToAdd.MyPossibleValues)
            {
                allOpt.forCombBox.Items.Add(item.Name);
            }


            GuiNumberToAddIndex++;
        }


        private OptionsViewGuiBox(int forGridRow, int forGridCol)
        {
            ForGridRow = forGridRow;
            ForGridCol = forGridCol;

            forLabelName = new Label();
            forLabelDesc = new Label();
            forCombBox = new ComboBox();
            ButtonToEditOption = new Button();// butt;



            ButtonToEditOption.Content = "edit\noption";
            ButtonToEditOption.Visibility = Visibility.Visible;
            ButtonToEditOption.IsEnabled = true;



            //set positions
            ForGrid.SetPositionInGrid(forLabelName, ForGridRow, ForGridCol, 0, -25);
            ForGrid.SetPositionInGrid(forLabelDesc, ForGridRow, ForGridCol, -100, 17);
            ForGrid.SetPositionInGrid(forCombBox, ForGridRow, ForGridCol);
            ForGrid.SetPositionInGrid(ButtonToEditOption, ForGridRow, ForGridCol, 100, -25);

            //        var point = ForGrid.GetLocation(ForGridRow, ForGridCol );
            //        TranslateTransform translateTransform1 = //60
            //new TranslateTransform( point.XLocation , point.YLocation);//(_windowThatTheGridIsFor.Width* / 2)
            //        ButtonToEditOption.RenderTransform = translateTransform1;

            ForCanvas.Children.Add(forCombBox);
            ForCanvas.Children.Add(forLabelName);
            ForCanvas.Children.Add(forLabelDesc);
            ForCanvas.Children.Add(ButtonToEditOption);
        }



        private void OpenEditButtonPageCallback(object sender, RoutedEventArgs e)
        {
            OptionEditView optViewWindow = new OptionEditView();


            int rows = ForOption.MyPossibleValues.Count + 1;
            int cols = AllOptionViewGui.Where(opt => opt.ForOption != null).ToList().Count + 1;

            GridGeneratorForUI gridForOptionsEditView =
                new GridGeneratorForUI(10, 10, optViewWindow);

            List<Label> optionLabels = new List<Label>();
            List<Label> possibleValueLabel = new List<Label>();
           


            //put in labels for possible values on rows and options on colums
            for (int i = 1; i < rows; i++)
            {
                Label PVLabel = new Label();
                PVLabel.FontSize = 14;
                PVLabel.Foreground = new SolidColorBrush(System.Windows.Media.Colors.DarkMagenta);
                possibleValueLabel.Add(PVLabel);
                PVLabel.Content = ForOption.MyPossibleValues[i - 1].Name;
                gridForOptionsEditView.SetPositionInGrid(PVLabel, i + 1, 1,-15);
                optViewWindow.MyCanvas.Children.Add(PVLabel);
            }
            for (int jj = 1; jj < cols; jj++)
            {
                Label optLabel = new Label();
                optLabel.FontSize = 14;
                optLabel.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Blue);
                optionLabels.Add(optLabel);
                optLabel.Content = AllOptionViewGui[jj - 1].ForOption.Name;
                gridForOptionsEditView.SetPositionInGrid(optLabel, 1, jj+1,-60,40);
                optViewWindow.MyCanvas.Children.Add(optLabel);
            }

            Label ForOptionLabel = new Label();
            ForOptionLabel.Content = ForOption.Name;
            ForOptionLabel.FontSize = 23;
            ForOptionLabel.Foreground =  new SolidColorBrush(System.Windows.Media.Colors.Blue);
            gridForOptionsEditView.SetPositionInGrid(ForOptionLabel, 1,  1, 0, 0);
            optViewWindow.MyCanvas.Children.Add(ForOptionLabel);

            for (int i = 1; i < rows; i++)
            {
                for (int jj = 1; jj < cols; jj++)
                {
                    if (AllOptionViewGui[jj - 1].ForOption != ForOption)
                    {
                        PossibleValueRestrictionView.Init(gridForOptionsEditView, i + 1, jj + 1, ForOption, ForOption.MyPossibleValues[i - 1], AllOptionViewGui[jj - 1].ForOption, optViewWindow.MyCanvas, SavedOptHandler);
                    }
                    
                }
            }



            optViewWindow.Show();
        }

        public ComboBox forCombBox;
        public Rectangle forRectangle;
        public Label forLabelName;
        public Label forLabelDesc;
        public Button ButtonToEditOption;

        private static GridGeneratorForUI ForGrid;
        private static Canvas ForCanvas;

        public static List<OptionsViewGuiBox> AllOptionViewGui;
        public static SavedOptionsFileHandler SavedOptHandler { get; protected set; }


        public Option ForOption;

        public int ForGridRow { get; }
        public int ForGridCol { get; }
    }

}

