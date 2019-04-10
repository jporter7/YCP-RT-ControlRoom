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

namespace VideoRecorder
{
    // Interaction logic for MainWindow.xaml
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {    
                this.Closing += (s, e) => (this.DataContext as IDisposable).Dispose();
            }
            catch (InvalidOperationException e)
            {
                // I dont know what to do until we integrate this into the project. 
                // It can likely be thrown since we are just closing the window anyway
                throw e;
            }
        }
    }
}
