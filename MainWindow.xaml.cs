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
using WpfBowlingClub.Pages;

namespace WpfBowlingClub
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppData.Pages.AppConnect.model = new AppData.Bawling_clubdbEntities1();
            AppData.Pages.AppFrame.FrameMain = Frame1;
            Frame1.Navigate(new ManufacturersPage());
        }
    }
}
