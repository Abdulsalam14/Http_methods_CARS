using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Http_Methods_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public enum HttpMethods { GET, POST, PUT, DELETE }
    public partial class MainWindow : Window
    {
        TaskCompletionSource<int> selectedid;
        TcpClient client;
        public BinaryWriter bw;
        public BinaryReader br;
        public Command? command { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            combo.ItemsSource = new List<HttpMethods>() { HttpMethods.GET, HttpMethods.POST, HttpMethods.PUT, HttpMethods.DELETE };
            client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27002));
            bw = new BinaryWriter(client.GetStream());
            br = new BinaryReader(client.GetStream());
            selectedid = new TaskCompletionSource<int>();
        }

        private async void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (combo.SelectedIndex)
            {
                case 0:
                    fillid.Visibility = Visibility.Hidden;
                    command = new Command(HttpMethods.GET, null!);
                    bw.Write(JsonSerializer.Serialize(command));
                    var msg = br.ReadString();
                    var cars = JsonSerializer.Deserialize<List<Car>>(msg);
                    list.Visibility = Visibility.Visible;
                    list.ItemsSource = cars;
                    break;
                case 1:
                    fillid.Visibility = Visibility.Hidden;
                    list.Visibility = Visibility.Collapsed;
                    combo.SelectedItem = null;
                    command = new Command(HttpMethods.POST, null!);
                    CarFill fill = new CarFill(this);
                    Hide();
                    fill.Show();
                    await fill.GetResultAsync();
                    Show();
                    bw.Write(JsonSerializer.Serialize(command));
                    var result = br.ReadBoolean();
                    if (result) MessageBox.Show("Car added");
                    else MessageBox.Show("Car did not added");
                    break;
                case 2:
                    combo.SelectedItem = null;
                    list.Visibility = Visibility.Hidden;
                    fillid.Visibility = Visibility.Hidden;
                    command = new Command(HttpMethods.PUT, null!);
                    Hide();
                    CarFill change = new CarFill(this);
                    change.Show();
                    await change.GetResultAsync();
                    Show();
                    bw.Write(JsonSerializer.Serialize(command));
                    var resultchange = br.ReadBoolean();
                    if (resultchange) MessageBox.Show("Car Changed");
                    else MessageBox.Show("Car is not found");
                    break;
                case 3:
                    list.Visibility = Visibility.Hidden;
                    fillid.Visibility = Visibility.Visible;
                    int selectid = await SelectedIdResult();
                    command = new Command(HttpMethods.DELETE, new Car(selectid, string.Empty, string.Empty, 0, string.Empty));
                    bw.Write(JsonSerializer.Serialize(command));
                    var resultdel = br.ReadBoolean();
                    if (resultdel) MessageBox.Show("Car Deleted");
                    else MessageBox.Show("Car is not found");
                    fillid.Visibility = Visibility.Hidden;
                    combo.SelectedItem = null;
                    break;
                default:
                    break;
            }
        }

        private void Okbtn_Click(object sender, RoutedEventArgs e)
        {
            var b = int.TryParse(txtid.Text, out int changedid);
            if (!b)
            {
                MessageBox.Show("Only ID number");
                return;
            }
            selectedid.SetResult(changedid);
            txtid.Text = string.Empty;
            selectedid = new();

        }

        public async Task<int> SelectedIdResult()
        {
            return await selectedid.Task;
        }

    }
}
