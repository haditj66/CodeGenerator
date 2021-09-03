using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CgenCmakeGui
{
    /// <summary>
    /// Interaction logic for CmakeSettings.xaml
    /// </summary>
    public partial class CmakeSettings : Window
    {
        public CmakeSettings()
        {
            InitializeComponent();

            HideRemoteUI();
        }

        private void HideRemoteUI()
        {
            remotelabel1.Visibility = Visibility.Hidden;
            remotelabel2.Visibility = Visibility.Hidden;
            remotelabel3.Visibility = Visibility.Hidden;
            remotelabel4.Visibility = Visibility.Hidden;

            RemoteDirectory.Visibility = Visibility.Hidden;
            RemoteIPAddress.Visibility = Visibility.Hidden;
            RemotePassword.Visibility = Visibility.Hidden;
            RemoteUsername.Visibility = Visibility.Hidden;
        }

        private void ShowRemoteUI()
        {
            remotelabel1.Visibility = Visibility.Visible;
            remotelabel2.Visibility = Visibility.Visible;
            remotelabel3.Visibility = Visibility.Visible;
            remotelabel4.Visibility = Visibility.Visible;

            RemoteDirectory.Visibility = Visibility.Visible;
            RemoteIPAddress.Visibility = Visibility.Visible;
            RemotePassword.Visibility = Visibility.Visible;
            RemoteUsername.Visibility = Visibility.Visible;
        }

        private void isRemotePCCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ShowRemoteUI();
        }
         
        private void isRemotePCCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            HideRemoteUI();
        }
    }
}
