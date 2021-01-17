using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HttpChat
{
    public class Client
    {
        public string IP { get; set; }
    }
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }
        public List<Client> clients = new List<Client>();

        private async Task WorkingMessages()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://192.168.0.123:8888/messages/");
            listener.Start();

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                var request = context.Request;
                var response = context.Response;
                Stream stream = request.InputStream;
                var reader = new StreamReader(stream);
                if (request.Headers["Mode"] == "Connect")
                {
                    var newClient = new Client { IP = request.RemoteEndPoint.Address.ToString() };
                    clients.Add(newClient);

                    richTextBox1.Text += GetTime()+"Подключён клиент - " + request.RemoteEndPoint.Address.ToString()+"\n";
                    var message = "Вы подключены к чату";
                    var buffer = Encoding.UTF8.GetBytes(message);
                    response.ContentLength64 = buffer.Length;
                    var output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                    
                }

                if (request.Headers["Mode"] == "Message")
                {
                    var message = reader.ReadToEnd();
                    richTextBox1.Text += "\n Сообщение:" + message;

                    var buffer = Encoding.UTF8.GetBytes(message);
                    response.ContentLength64 = buffer.Length;

                    using (Stream output = response.OutputStream)
                    {
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    }
                }  

                if (request.Headers["Mode"] == "Files")
                {
                    using (FileStream outputFileStream = new FileStream(request.Headers["FileName"], FileMode.Create))
                    {
                        stream.CopyTo(outputFileStream);
                    }

                    var message = "Файл отправлен";

                    var dataMessage = Encoding.UTF8.GetBytes(message);

                    using (Stream @out = response.OutputStream)
                    {
                        @out.Write(dataMessage, 0, dataMessage.Length);
                        @out.Close();
                    }
                }
            }
        }

        private string GetTime()
        {
            return string.Format("({0}) ", DateTime.Now.ToLongTimeString());
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await WorkingMessages();
        }

    }
}
