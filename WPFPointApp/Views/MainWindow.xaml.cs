using System.Windows;
using WPFPointApp.ViewModels;
using WPFPointApp.ViewModels.Services;

namespace WPFPointApp.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var mainViewModel = new MainViewModel(new JsonFileService(), new CanvasDrawService(canvas));
            DataContext = mainViewModel;
            this.Closed += (s, e) => mainViewModel.Dispose();
        }
    }
}
