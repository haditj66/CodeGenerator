#define TESTING

using CgenCmakeLibrary;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CgenCmakeGui
{

    public enum OptionGuiBoxStates
    {
        Hidden,
        Selected,
        Unselected
    }

    public partial class MainWindow
    {
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


            public static void Init(GridGeneratorForUI forGrid, Canvas forCanvas, int usethisManyRows, OutputScrollHandler statushandler, Action callbackPossibleValueSelected)
            {
                statusHandler = statushandler;

                CallbackPossibleValueSelected = callbackPossibleValueSelected;
                ForGrid = forGrid;
                ForCanvas = forCanvas;

                UsethisManyRows = usethisManyRows;

                _Init();
            }

            private static void _Init()
            {

                GuiNumberToAddIndex = 0;

                AllOptionsSelectedGui = new List<OptionsSelectedGuiBox>();
                optionsSelected = new List<OptionsSelected>();

                OptionsSelectedGuiBox optSel;

                int ii = 1;
                int jj = 1;
                for (int ind = 0; ind < UsethisManyRows * ForGrid.Columns; ind++)
                {

                    optSel = new OptionsSelectedGuiBox(jj, ii);
                    optSel.forRectangle = ForGrid.DrawRectangleAroundGrid(jj, ii);
                    ForCanvas.Children.Add(optSel.forRectangle);

                    AllOptionsSelectedGui.Add(optSel);
                    optSel.SetState(OptionGuiBoxStates.Hidden);

                    jj = jj >= UsethisManyRows ? 1 : jj + 1;
                    ii = jj == 1 ? ii + 1 : ii;


                }

            }

            static int UsethisManyRows;
            static int GuiNumberToAddIndex;
            public static void Reset()
            {

                foreach (var item in AllOptionsSelectedGui)
                {
                    item.SetState(OptionGuiBoxStates.Hidden);
                }

                _Init();
            }




            public static void AddOptionSelectedToGui(OptionsSelected optSelToAdd, bool initiatedYet = false)
            {
                var allOpt = OptionsSelectedGuiBox.AllOptionsSelectedGui[GuiNumberToAddIndex];

                _AddOptionSelectedToGui(optSelToAdd, initiatedYet);

                if (string.IsNullOrEmpty(optSelToAdd.possibleValueSelection) == true)
                {
                    allOpt.SetState(OptionGuiBoxStates.Unselected);
                }
                else if (string.IsNullOrEmpty(optSelToAdd.possibleValueSelection) == false)
                {
                    allOpt.SetState(OptionGuiBoxStates.Selected);
                }



            }

            public static void AddOptionSelectedToGui(Option optToAdd, bool initiatedYet = false)
            {
                var allOpt = OptionsSelectedGuiBox.AllOptionsSelectedGui[GuiNumberToAddIndex];

                OptionsSelected optSelToAdd = new OptionsSelected();
                optSelToAdd.option = optToAdd;

                _AddOptionSelectedToGui(optSelToAdd, initiatedYet);

                allOpt.SetState(OptionGuiBoxStates.Unselected);

            }


            private static void _AddOptionSelectedToGui(OptionsSelected optSelToAdd, bool initiatedYet)
            {

                var allOpt = OptionsSelectedGuiBox.AllOptionsSelectedGui[GuiNumberToAddIndex];
                allOpt.ForOptionSelected = optSelToAdd;

                allOpt.forLabelName.Content = optSelToAdd.option.Name;

                allOpt.forLabelDesc.Content = optSelToAdd.option.Description;

                allOpt.forCombBox.SelectionChanged += allOpt.OptionPossibleValueSelected;
                allOpt.forCombBox.Width = 120;
                allOpt.forCombBox.Visibility = Visibility.Visible;

                if (string.IsNullOrEmpty(optSelToAdd.possibleValueSelection))
                {
                    allOpt.forCombBox.Items.Clear();
                }
                else
                {
                    var t = new ComboBoxItem();
                    allOpt.forCombBox.Items.Add(t);
                    allOpt.forCombBox.SelectedItem = t;
                    t.Content = optSelToAdd.possibleValueSelection;
                }


                //get all allowable possible values for this next option 
                List<ConstrictionInfo> constrictInfo = new List<ConstrictionInfo>();
                var allowablevalues = AllOptions.GetAllowablePVs(optSelToAdd.option, optionsSelected, ref constrictInfo);

                int index = 0;
                foreach (var pv in optSelToAdd.option.MyPossibleValues)
                {

                    //if it is not allowable, colow the item red
                    if (allowablevalues.Exists(av => av.Name == pv.Name))
                    {
                        allOpt.forCombBox.Items.Add(pv.Name);
                        //((ComboBoxItem)allOpt.forCombBox.Items[index]).Foreground = new SolidColorBrush(System.Windows.Media.Colors.Red);
                    }
                    else
                    {
                        ComboBoxItem itemcol = new ComboBoxItem();
                        itemcol.Content = pv.Name;
                        itemcol.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Red);
                        allOpt.forCombBox.Items.Add(itemcol);
                    }

                    index++;
                }

                optionsSelected.Add(optSelToAdd);

                GuiNumberToAddIndex++;

                //if the allowable values for this is just one, go ahead and just select it anyways
                if (allowablevalues.Count == 1)
                {
                    if (initiatedYet == true)
                    {
                        allOpt.forCombBox.SelectedItem = allowablevalues[0].Name;
                        _OptionSelected(allowablevalues[0].Name, allOpt);
                        statusHandler.display(allowablevalues[0].Name + " was auto selected as it is the only PV available for option " + allOpt.forLabelName.Content, OutputLevel.Normal);
                    }
                }

                if (allowablevalues.Count == 0)
                {
                    statusHandler.display("Option " + allOpt.forLabelName.Content + " Did not have any available possible values. they were all constricted.", OutputLevel.Problem);
                }


                //display who constricted who
                if (constrictInfo.Count > 0)
                {
                    //AllOptions.Instance.GetOption(constrictInfo[0])
                    var co2 = ConstrictionInfo.Convert_ConstInfo1_To_ConstInfo2(constrictInfo, optSelToAdd.option);
                    foreach (var item in co2)
                    {
                        statusHandler.display(item.GetDisplay(), OutputLevel.Normal);
                    }

                    //foreach (var pv in optSelToAdd.option.MyPossibleValues)
                    //{
                    //    constrictInfo[0].

                    //}

                }

            }

            private void OptionPossibleValueSelected(object sender, SelectionChangedEventArgs e)
            {

                string theSelection = "";
                try
                {
                    theSelection = (string)e.AddedItems[0];
                }
                catch (Exception)
                {
                    statusHandler.display("This possible value is constricted by a previous option.", OutputLevel.Problem);
                    return;
                }

                _OptionSelected(theSelection, this);

            }

            public static void _OptionSelected(string theSelection, OptionsSelectedGuiBox optGui)
            {
                optGui.ForOptionSelected.possibleValueSelection = theSelection;


                //fill rectangle and turn off combobox
                optGui.SetState(OptionGuiBoxStates.Selected);


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

            private static OutputScrollHandler statusHandler;

            public static List<OptionsSelectedGuiBox> AllOptionsSelectedGui;
            public static List<OptionsSelected> optionsSelected;

            public int ForGridRow { get; }
            public int ForGridCol { get; }
            private OptionsSelected ForOptionSelected;
        }

    }

}
