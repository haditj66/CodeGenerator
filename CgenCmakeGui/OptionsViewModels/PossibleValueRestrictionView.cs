using CgenCmakeLibrary;
using CgenCmakeLibrary.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CgenCmakeGui.OptionsViewModels
{
    public class PossibleValueRestrictionView
    {

        public static void Init(GridGeneratorForUI forGrid, int placedAtRow, int placedAtCol, Option optionThatIsBeingEdited, PossibleValue possibleValueChosen, Option constrictedOption, Canvas forCanvas, SavedOptionsFileHandler savedOptHandler)
        {
            SavedOptHandler = savedOptHandler;

            ForGrid = forGrid;
            ForCanvas = forCanvas;

            
            OptionThatIsBeingEdited = optionThatIsBeingEdited;
            

            GuiNumberToAddIndex = 0;

            if (AllOptionViewGui == null)
            {

                AllOptionViewGui = new List<PossibleValueRestrictionView>();
            }
            PossibleValueRestrictionView optView = null;


            optView = new PossibleValueRestrictionView(placedAtRow, placedAtCol);
            optView.forRectangle = ForGrid.DrawRectangleAroundGrid(placedAtRow, placedAtCol);
            ForCanvas.Children.Add(optView.forRectangle);

            optView.ConstrictedOption = constrictedOption;
            optView.PossibleValueChosen = possibleValueChosen;

            //give the combobox the possible value of the option that could be constricted
            foreach (var pv in constrictedOption.MyPossibleValues)
            {
                optView.forCombBoxAddPV.Items.Add(pv.Name);
                optView.forCombBoxRemovePV.Items.Add(pv.Name);
            }


            //update label
            optView.UpdateConstrictingValuesLabel();

            AllOptionViewGui.Add(optView);



        }






        private PossibleValueRestrictionView(int forGridRow, int forGridCol)
        {
            ForGridRow = forGridRow;
            ForGridCol = forGridCol;
            
            forLabelValuesThatItRestrictsTo = new Label();
            forLabelValuesThatItRestrictsTo.Content = "any";

            valuesThatItRestrictsToTextBlock = new TextBlock();
            valuesThatItRestrictsToTextBlock.Text = "any";
            valuesThatItRestrictsToTextBlock.Foreground = new SolidColorBrush(System.Windows.Media.Colors.DarkMagenta);
            valuesThatItRestrictsToTextBlock.FontSize = 10;

            forButtonToAddPV = new Button();
            forButtonToAddPV.Width = 50;
            forButtonToAddPV.Height = 15;
            forButtonToAddPV.Content = "add PV";
            forButtonToAddPV.FontSize = 10;

            forButtonToRemovePV = new Button(); 
            forButtonToRemovePV.Width = 65;
            forButtonToRemovePV.Height = 15; 
            forButtonToRemovePV.Content = "remove PV";
            forButtonToRemovePV.FontSize = 10;

            forButtonToAddPV.Click += AddPVSelected_click;
            forButtonToRemovePV.Click += RemovePVSelected_click;

            forCombBoxAddPV = new ComboBox();
            forCombBoxAddPV.Visibility = System.Windows.Visibility.Visible;
            forCombBoxAddPV.SelectionChanged += PVConstrictionAdded_selected; 
            forCombBoxAddPV.Width = 60;
            forCombBoxAddPV.Height = 15;

            forCombBoxRemovePV = new ComboBox();
            forCombBoxRemovePV.Visibility = System.Windows.Visibility.Visible;
            forCombBoxRemovePV.SelectionChanged += PVConstrictionRemoved_selected;
            forCombBoxRemovePV.Width = 60;
            forCombBoxRemovePV.Height = 15;

            //ButtonToEditOption.Content = "edit\noption";
            //ButtonToEditOption.Visibility = Visibility.Visible;
            //ButtonToEditOption.IsEnabled = true;



            //set positions
            ForGrid.SetPositionInGrid(valuesThatItRestrictsToTextBlock, ForGridRow, ForGridCol, -60, -5);
            //ForGrid.SetPositionInGrid(forLabelValuesThatItRestrictsTo, ForGridRow, ForGridCol, -30, 10);
            ForGrid.SetPositionInGrid(forCombBoxAddPV, ForGridRow, ForGridCol, -90, -18);
            ForGrid.SetPositionInGrid(forCombBoxRemovePV, ForGridRow, ForGridCol, -10,-18);
            ForGrid.SetPositionInGrid(forButtonToAddPV, ForGridRow, ForGridCol, -90,-32);
            ForGrid.SetPositionInGrid(forButtonToRemovePV, ForGridRow, ForGridCol, -10,-32);

            //ForGrid.SetPositionInGrid(forLabelName, ForGridRow, ForGridCol, 0, -25);
            //ForGrid.SetPositionInGrid(forLabelDesc, ForGridRow, ForGridCol, -100, 17); 
            //ForGrid.SetPositionInGrid(ButtonToEditOption, ForGridRow, ForGridCol, 100, -25);


            
            ForCanvas.Children.Add(valuesThatItRestrictsToTextBlock);
            //ForCanvas.Children.Add(forLabelValuesThatItRestrictsTo);
            ForCanvas.Children.Add(forCombBoxAddPV); 
            ForCanvas.Children.Add(forCombBoxRemovePV);
            ForCanvas.Children.Add(forButtonToAddPV);
            ForCanvas.Children.Add(forButtonToRemovePV);
            //ForCanvas.Children.Add(forLabelName);
            //ForCanvas.Children.Add(forLabelDesc);
            //ForCanvas.Children.Add(ButtonToEditOption);
        }

        public void UpdateConstrictingValuesLabel()
        {
            var copts = PossibleValueChosen.MyConstrictedOptions.Where(co => co.OptionConstricted.Name == this.ConstrictedOption.Name).ToList();
            string CoValues = "";
            foreach (var co in copts)
            {
                CoValues += co.ValueConstricted + "\n";
            }
            valuesThatItRestrictsToTextBlock.Text = string.IsNullOrEmpty(CoValues) ? "any" : CoValues;

            if (string.IsNullOrEmpty(CoValues) == false)
            { 
                forRectangle.Stroke = new SolidColorBrush(System.Windows.Media.Colors.Green);
                forRectangle.IsEnabled = false;
                forRectangle.Opacity = .8;
            }
            else
            {
                forRectangle.Stroke = new SolidColorBrush(System.Windows.Media.Colors.DarkGray);
                forRectangle.IsEnabled = false;
                forRectangle.Opacity = .8;
            }

        }


        private void AddPVSelected_click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PVToAddStr) == false)
            {
                if (PossibleValueChosen.IsConstrictedOptionExists(ConstrictedOption, PVToAddStr) == false)
                {
                    //create a new constriction and add it to the possible value
                    ConstrictedOptions co = new ConstrictedOptions(OptionThatIsBeingEdited, ConstrictedOption, PossibleValueChosen);
                    //ValueList
                    co.AddValueConstricted(PVToAddStr);
                    //co.ValueConstricted.Add(pvChosenStr);

                    PossibleValueChosen.AddConstrictedOption(co);

                    UpdateConstrictingValuesLabel();

                    SavedOptHandler.SaveAllOptions();
                }
            } 

        }
        private void RemovePVSelected_click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PVToRemoveStr) == false)
            {
                if (PossibleValueChosen.IsConstrictedOptionExists(ConstrictedOption, PVToRemoveStr) == true)
                {
                    var constopt = PossibleValueChosen.GetConstrictedOption(ConstrictedOption.Name, PVToRemoveStr);
                    PossibleValueChosen.MyConstrictedOptions.RemoveAll(co=>co.GetUniqieString() == constopt.GetUniqieString());

                    UpdateConstrictingValuesLabel();

                    SavedOptHandler.SaveAllOptions();
                }
            } 

        }

        private void PVConstrictionRemoved_selected(object sender, SelectionChangedEventArgs e)
        { 
                PVToRemoveStr = (string)e.AddedItems[0];
        }

        private void PVConstrictionAdded_selected(object sender, SelectionChangedEventArgs e)
        {
            PVToAddStr = (string)e.AddedItems[0];  
        }

        public int ForGridRow { get; }
        public int ForGridCol { get; }
        

        static int GuiNumberToAddIndex;

        private static GridGeneratorForUI ForGrid;
        private static Canvas ForCanvas;
        public static List<PossibleValueRestrictionView> AllOptionViewGui;
        public Option ConstrictedOption { get; protected set; }

        public static Option OptionThatIsBeingEdited { get; protected set; }
        public  PossibleValue PossibleValueChosen { get; protected set; }
        public static SavedOptionsFileHandler SavedOptHandler { get; protected set; }

        public ComboBox forCombBoxAddPV;
        public ComboBox forCombBoxRemovePV;
        public Rectangle forRectangle;
        public Label forLabelValuesThatItRestrictsTo;
        public Button forButtonToAddPV;
        public Button forButtonToRemovePV;
        public TextBlock valuesThatItRestrictsToTextBlock;


        private string PVToAddStr;
        private string PVToRemoveStr;
    }
}









//#define TESTING

//using CgenCmakeLibrary;
//using System.Collections.Generic;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Media;
//using System.Windows.Shapes;

//namespace CgenCmakeGui
//{

//    public enum OptionViewGuiStates
//    {
//        Hidden,
//        Visible
//    }

//    public class OptionsViewGuiBox
//    {



//        public void SetState(OptionViewGuiStates optionGuiBoxStates)
//        {

//            if (optionGuiBoxStates == OptionViewGuiStates.Hidden)
//            {
//                this.forCombBox.IsEnabled = false;
//                //this.forRectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
//                this.forRectangle.Opacity = .5;
//                forCombBox.Visibility = Visibility.Hidden;
//                forRectangle.Visibility = Visibility.Hidden;
//                forLabelName.Visibility = Visibility.Hidden;
//                forLabelDesc.Visibility = Visibility.Hidden;
//                ButtonToEditOption.Visibility = Visibility.Hidden;

//            }
//            else if (optionGuiBoxStates == OptionViewGuiStates.Visible)
//            {

//                this.forCombBox.IsEnabled = true;
//                forCombBox.Visibility = Visibility.Visible;
//                forRectangle.Visibility = Visibility.Visible;
//                forLabelName.Visibility = Visibility.Visible;
//                forLabelDesc.Visibility = Visibility.Visible;
//                ButtonToEditOption.Visibility = Visibility.Visible;
//                //this.forRectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
//                this.forRectangle.Opacity = .5;
//            }
//        }


 



//        public static void AddOptionToGui(Option optToAdd)
//        {
//            var allOpt = OptionsViewGuiBox.AllOptionViewGui[GuiNumberToAddIndex];


//            _AddOptionSelectedToGui(optToAdd);

//            allOpt.SetState(OptionViewGuiStates.Visible);

//        }


//        private static void _AddOptionSelectedToGui(Option optSelToAdd)
//        {

//            var allOpt = OptionsViewGuiBox.AllOptionViewGui[GuiNumberToAddIndex];
//            allOpt.ForOption = optSelToAdd;

//            allOpt.forLabelName.Content = optSelToAdd.Name;

//            allOpt.forLabelDesc.Content = optSelToAdd.Description;

//            allOpt.forCombBox.Width = 120;
//            allOpt.forCombBox.Visibility = Visibility.Visible;
//            allOpt.forCombBox.IsEnabled = true;
//            allOpt.forCombBox.Items.Clear();

//            allOpt.ButtonToEditOption.Width = 50;
//            allOpt.ButtonToEditOption.Height = 50;
//            allOpt.ButtonToEditOption.Visibility = Visibility.Visible;
//            //allOpt.ButtonToEditOption.Click += allOpt.OpenEditButtonPageCallback;

//            foreach (var item in optSelToAdd.MyPossibleValues)
//            {
//                allOpt.forCombBox.Items.Add(item.Name);
//            }


//            GuiNumberToAddIndex++;
//        }





//        private void OpenEditButtonPageCallback(object sender, RoutedEventArgs e)
//        {

//        }

//        public ComboBox forCombBox;
//        public Rectangle forRectangle;
//        public Label forLabelName;
//        public Label forLabelDesc;
//        public Button ButtonToEditOption;



        

//        public Option ForOption;


//    }

//}

