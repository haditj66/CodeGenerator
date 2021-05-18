using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading; 
using System.Windows.Media;

namespace CgenCmakeGui
{

    public enum OutputLevel
    {
        Normal,
        Warning,
        Problem
    }

    public class OutputScrollHandler
    {

         
        public OutputScrollHandler(ScrollViewer forTextBlock, System.Windows.Threading.Dispatcher dispatcher, bool newLineIt = false)
        {
            ForTextBlock = forTextBlock;
            Dispatcher = dispatcher;
            NewLineIt = newLineIt;
            ForTextBlock.Content = "Initialized";
            ForTextBlock.FontSize = 12;
        }

        public void display(string msgToDisplay, OutputLevel messageLevel)
        {
            SolidColorBrush color = new SolidColorBrush(System.Windows.Media.Colors.Blue);
            if (messageLevel == OutputLevel.Normal)
            {
                color = new SolidColorBrush(System.Windows.Media.Colors.Blue);
            }
            if (messageLevel == OutputLevel.Warning)
            {
                color = new SolidColorBrush(System.Windows.Media.Colors.DarkOrange);
            }
            if (messageLevel == OutputLevel.Problem)
            {
                color = new SolidColorBrush(System.Windows.Media.Colors.Red);
            }

            if (NewLineIt == true)
            {
                msgToDisplay = msgToDisplay + "\n";
            }

            Dispatcher.Invoke(() =>
            {
                ForTextBlock.Foreground = color;

                ForTextBlock.Content = ForTextBlock.Content + msgToDisplay;
            });

        }



        public ScrollViewer ForTextBlock { get; protected set; }
        public Dispatcher Dispatcher { get; }
        public bool NewLineIt { get; }
    }
}
