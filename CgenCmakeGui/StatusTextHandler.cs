using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace CgenCmakeGui
{
    public enum MessageLevel
    {
        Normal,
        Warning,
        Problem
    }

    public class StatusTextHandler
    { 
        public StatusTextHandler(TextBlock forTextBlock, System.Windows.Threading.Dispatcher dispatcher )
        {
            ForTextBlock = forTextBlock;
            Dispatcher = dispatcher;
            ForTextBlock.Text = "Initialized";
            ForTextBlock.FontSize = 20;
        }

        public void display(string msgToDisplay, MessageLevel messageLevel)
        {
            SolidColorBrush color = new SolidColorBrush(System.Windows.Media.Colors.Blue);
            if (messageLevel == MessageLevel.Normal)
            {
                 color = new SolidColorBrush(System.Windows.Media.Colors.Blue);
            }
            if (messageLevel == MessageLevel.Warning)
            {
                 color = new SolidColorBrush(System.Windows.Media.Colors.DarkOrange);
            }
            if (messageLevel == MessageLevel.Problem)
            {
                 color = new SolidColorBrush(System.Windows.Media.Colors.Red);
            }

            //Dispatcher.Invoke(() =>
            //{
                ForTextBlock.Foreground = color;
                ForTextBlock.Text = msgToDisplay;
            //});

        }



        public TextBlock ForTextBlock { get; protected set; }
        public Dispatcher Dispatcher { get; }
    }
}
