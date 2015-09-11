using System.Windows;
using EdlinSoftware.BlackJack.UI.ViewModel;

namespace EdlinSoftware.BlackJack.UI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new GameViewModel();
        }
    }
}
