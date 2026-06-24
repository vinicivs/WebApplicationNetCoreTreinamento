using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppNetCoreTreinamento
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            // Sair
            Close();
        }

        private void Mensagem_Click(object sender, RoutedEventArgs e)
        {
            // Mensagem de alerta
            MessageBox.Show("Mensagem de alerta", "Alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}