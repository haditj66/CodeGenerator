using CgenCmakeLibrary;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CgenCmakeGui
{
    public class GridGeneratorForUI : GridGenerator
    {

        
        public GridGeneratorForUI(int rows, int columns, FrameworkElement windowThatTheGridIsFor)//, Canvas forcanvas)//, StackPanel forStackPanel) 
            : base( rows,columns, (int)windowThatTheGridIsFor.Width, (int)windowThatTheGridIsFor.Height)
        {
            _windowThatTheGridIsFor = windowThatTheGridIsFor;
            //_forcanvas = forcanvas;
            //_forStackPanel = forStackPanel;
            //_elements = new List<UIElement>();
        }

        public GridGeneratorForUI(int rows, int columns, int xRes, int yRes)//, Canvas forcanvas)//, StackPanel forStackPanel) 
    : base(rows, columns, xRes, yRes)
        {
            //_windowThatTheGridIsFor = windowThatTheGridIsFor;
            //_forcanvas = forcanvas;
            //_forStackPanel = forStackPanel;
            //_elements = new List<UIElement>();
        }

        public Rectangle DrawRectangleAroundGrid( int row, int col)
        {
           
            Rectangle rect = new Rectangle();
            //_elements.Add(rect);
            rect.Width = this.XResolution / this.Columns;
            rect.Height = this.YResolution / this.Rows;
            rect.Visibility = Visibility.Visible;
            rect.Stroke = new SolidColorBrush(System.Windows.Media.Colors.Black);
            rect.StrokeThickness = 3;

            int recOffsetx = this._isHasParent ? 0 : -100; // (int)-rect.Width / 3;
            this.SetPositionInGrid(rect, row, col, recOffsetx, 10-deltaY/2);// -this.YResolution / 2);

            //add rectangle to panel
            //_forcanvas.Children.Add(rect);
            return rect;
        }


        public void SetPositionInGrid(ContentControl element, int row, int column, int offsetx = 0, int offsety = 0)
        {
            int adjx = double.IsNaN(element.Width) ? 0 : (int)element.Width;
            int adjy = double.IsNaN(element.Height) ? 0 : (int)element.Height;

            _SetPositionInGrid(element, row, column, offsetx - adjx, offsety- (int)(adjy * .5)); 
        } 
        public void SetPositionInGrid(Rectangle element, int row, int column, int offsetx = 0, int offsety = 0)
        {
            _SetPositionInGrid(element, row, column, offsetx - 60, offsety); 
        } 
        public void SetPositionInGrid(ComboBox element, int row, int column, int offsetx = 0, int offsety = 0)
        {
            _SetPositionInGrid(element, row, column, offsetx - 60, offsety);   
        }

        public void SetPositionInGrid(UIElement element, int row, int column, int offsetx = 0, int offsety = 0)
        {
            _SetPositionInGrid(element, row, column, offsetx - 60, offsety);
        }

        public void SetPositionInGrid(Label element, int row, int column, int offsetx = 0, int offsety = 0)
        {
            var point = this.GetLocation(row, column);
            TranslateTransform translateTransform1 = //60
                new TranslateTransform(offsetx + point.XLocation - 60, offsety + point.YLocation);//(_windowThatTheGridIsFor.Width* / 2)
            element.RenderTransform = translateTransform1;


        }

        //public void SetPositionInGrid(Button element, int row, int column, int offsetx = 0, int offsety = 0)
        //{
        //    int adjx = double.IsNaN(element.Width) ? 0 : (int)element.Width;
        //    int adjy = double.IsNaN(element.Height) ? 0 : (int)element.Height;

        //    var point = this.GetLocation(row, column);
        //    TranslateTransform translateTransform1 =  
        //        new TranslateTransform(point.XLocation+ offsetx - adjx, point.YLocation + offsety - (int)(adjy * .5));//(_windowThatTheGridIsFor.Width* / 2)
        //    element.RenderTransform = translateTransform1;


        //}



        private void _SetPositionInGrid(UIElement element, int row, int column, int offsetx = 0, int offsety = 0)
        { 

            var point = this.GetLocation(row, column);
            TranslateTransform translateTransform1 = //60
                new TranslateTransform(offsetx + point.XLocation, offsety + point.YLocation);//(_windowThatTheGridIsFor.Width* / 2)
            element.RenderTransform = translateTransform1;


        }


        public GridGeneratorForUI CreateGridFromGridUI(int forParentRow, int forParentColumn, int row, int column)
        {
            int xresNew = XResolution / row;
            int yresNew = YResolution / column;
            

            GridGeneratorForUI ggret = new GridGeneratorForUI(row, column, xresNew, yresNew);

            //ggret.XResolution = xresNew;
            //ggret.YResolution = yresNew;
            //ggret.Grid1PointLocation = new GridPoint(deltaX / 2, deltaY / 2);
            //ggret.deltaX = xresNew / column;
            //ggret.deltaY = yresNew / row;

            ggret._parentGrid = this;
            ggret._parentRow = forParentRow;
            ggret._parentColumn = forParentColumn;
            ggret._isHasParent = true;

            return ggret;
        }

        //public void SetAllInPanel()
        //{
        //    foreach (var item in _elements)
        //    {
        //        _forcanvas.Children.Add(item);
        //    }
        //}

        List<UIElement> _elements;
        FrameworkElement _windowThatTheGridIsFor;
        //Canvas _forcanvas;
        //StackPanel _forStackPanel;
    }
}
