using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MembershipDemoMVVM.View;

namespace MembershipDemoMVVM.View
{
    /// <summary>
    /// Interaction logic for MembershipMaintenance.xaml
    /// </summary>
    public partial class MembershipMaintenance : Window, IClosable
    {
        public MembershipMaintenance() //6
        {
            InitializeComponent();
        }
    }
}
