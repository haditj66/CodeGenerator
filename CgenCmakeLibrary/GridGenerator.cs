using System;
using System.Collections.Generic;
using System.Text;

namespace CgenCmakeLibrary
{


    public class GridPoint
    {

        public GridPoint()
        {
        }


        public GridPoint(int xLocation, int yLocation)
        {
            XLocation = xLocation;
            YLocation = yLocation;
        }

        public int XLocation { get; set; }
        public int YLocation { get; set; }
    }





    public class GridGenerator
    {

        public GridGenerator(int rows, int columns, int xResolution, int yResolution)
        {
            Rows = rows;
            Columns = columns;
            XResolution = xResolution;
            YResolution = yResolution;

            deltaX = xResolution / columns;
            deltaY = yResolution / rows;

            _isHasParent = false;

            Grid1PointLocation = new GridPoint(deltaX/2, deltaY/2);
        }


        public GridPoint GetLocation(int row, int column)
        {
            GridPoint ret = new GridPoint();

            ret.XLocation = Grid1PointLocation.XLocation + ((column- 1) * deltaX);
            ret.YLocation = Grid1PointLocation.YLocation + ((row - 1) * deltaY);

            //get parent location and change location to relative to this one
            if (_isHasParent)
            {
                //change coordinates so that it is relative to grid origin
                ret.XLocation = ret.XLocation - (_parentGrid.deltaX / 2);
                ret.YLocation = ret.YLocation - (_parentGrid.deltaY / 2);

                GridPoint parentpoint = _parentGrid.GetLocation(_parentRow, _parentColumn);
                ret.XLocation = ret.XLocation + parentpoint.XLocation;
                ret.YLocation = ret.YLocation + parentpoint.YLocation;
            }

            return ret;
        }

        public  GridGenerator CreateGridFromGrid(int forParentRow, int forParentColumn, int row, int column)
        {
            int xresNew = XResolution / row;
            int yresNew = YResolution / column;

            GridGenerator ggret = new GridGenerator(row, column, xresNew, yresNew);

            ggret._parentGrid = this;
            ggret._parentRow = forParentRow;
            ggret._parentColumn = forParentColumn;
            ggret._isHasParent = true;

            return ggret;
        }

        public GridGenerator[,] CreateGridFromAllGrid(int withNewGridRow, int withNewGridcolumn)
        {
            GridGenerator[,] grids = new GridGenerator[this.Rows, this.Columns];

            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    grids[i, j] = this.CreateGridFromGrid(i,j, withNewGridRow, withNewGridcolumn);
                }
            }

            return grids;
        }

        public int Rows {  get; }
        public int Columns { get; }
        public int XResolution { get;  }
        public int YResolution { get; }

        public int deltaX { get;  }
        public int deltaY { get; }

        protected GridPoint Grid1PointLocation;

        protected bool _isHasParent;
        protected GridGenerator _parentGrid;
        protected int _parentRow;
        protected int _parentColumn;
    }
}
