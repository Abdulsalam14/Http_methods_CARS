using http_methods_server;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

List<Car> list = new List<Car>() { new Car(1, "Hyundai", "Sonata", 2023, "Gray") };
Command command;
var ip = IPAddress.Loopback;
var port = 27002;

var listener = new TcpListener(ip, port);



listener.Start();

Console.WriteLine($"{listener.Server.LocalEndPoint} listening...");

var client = listener.AcceptTcpClient();
var stream = client.GetStream();
BinaryReader br = new BinaryReader(stream);
BinaryWriter bw = new BinaryWriter(stream);

while (true)
{
    var msg = br.ReadString();
    command = JsonSerializer.Deserialize<Command>(msg)!;
    switch (command.method)
    {
        case Httpmethods.GET:
            Get();
            break;
        case Httpmethods.POST:
            bw.Write(Post());
            break;
        case Httpmethods.PUT:
            bw.Write(Put());
            break;
        case Httpmethods.DELETE:
            bw.Write(Delete());
            break;
        default:
            break;
    }
}


void Get()
{

    if (command == null) return;
    if (command != null && command.car != null)
    {
        var car = list.FirstOrDefault(c => c.Id == command.car.Id);
        bw.Write(JsonSerializer.Serialize(car));
    }
    else
    {
        var cars = JsonSerializer.Serialize(list);
        bw.Write(cars);
    }
}


bool Post()
{
    if (command.car != null)
    {
        list.Add(command.car);
        return true;
    }
    else
    {
        Console.WriteLine("yanlish");
        return false;
    }
}

bool Put()
{
    if (command.car != null)
    {
        var changedcar=list.FirstOrDefault(c => c.Id == command.car.Id);
        if (changedcar != null)
        {
            int carid=list.IndexOf(changedcar);
            list[carid]=command.car;
            return true;
        }
        else return false;
    }
    else return false;
}


bool Delete()
{
    if (command.car != null)
    {
        var deletedcar = list.FirstOrDefault(c => c.Id == command.car.Id);
        if (deletedcar != null)
        {
            list.Remove(deletedcar);
            return true;
        }
        else return false;
    }
    else return false;
}
public enum Httpmethods { GET, POST, PUT, DELETE };