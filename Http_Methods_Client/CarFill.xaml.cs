using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Http_Methods_Client
{
    /// <summary>
    /// Interaction logic for CarFill.xaml
    /// </summary>
    /// 

    public partial class CarFill : Window
    {

        private TaskCompletionSource taskCompletionSource;
        MainWindow mainWindow;
        public CarFill(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            taskCompletionSource = new TaskCompletionSource();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtid.Text != "" && txtmake.Text != "" && txtmodel.Text != "" && txtyear.Text != "" && txtcolor.Text != "")
            {
                var b = int.TryParse(txtid.Text, out int id);
                var b2 = ushort.TryParse(txtyear.Text, out ushort year);
                if (b && b2)
                {
                    mainWindow.command.car = new Car(id, txtmake.Text, txtmodel.Text, year, txtcolor.Text);
                    taskCompletionSource.SetResult();
                    App.Current.Windows[1].Close();
                }
                else MessageBox.Show("Wrong Fill Id or Year");
            }
            else
            {
                MessageBox.Show("Fill");
            }
        }
        private void WindowClosed(object sender, EventArgs e)
        {
            //taskCompletionSource.SetResult();
            if (mainWindow.IsVisible == false) mainWindow.Show();
        }

        public async Task GetResultAsync()
        {
            await taskCompletionSource.Task;
        }
    }
}
