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

namespace HttpClient
{
    public partial class Form1 : Form
    {
        string addressMainClient = string.Empty;
        string addressServer = string.Empty;
        string username = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 2;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            WebRequest request = WebRequest.Create(addressServer);
            request.Credentials = CredentialCache.DefaultCredentials;

            var message = "["+DateTime.Now.ToShortTimeString()+"] "+username+": "+textBox1.Text;
            var messageData = Encoding.UTF8.GetBytes(message);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = messageData.Length;
            request.Method = "POST";
            request.Headers.Add("Mode", "Message");
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(messageData, 0, messageData.Length);
            }
            
            var response = request.GetResponse();
            
            using(Stream stream = response.GetResponseStream())
            {
                var reader = new StreamReader(stream);
                richTextBox1.Text += Environment.NewLine + reader.ReadToEnd();
                reader.Close();
            }
            textBox1.Text = string.Empty;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            addressServer = comboBox1.Text;
            username = textBox4.Text;

            if (!string.IsNullOrWhiteSpace(addressServer) && !string.IsNullOrWhiteSpace(username))
            {
                WebRequest request = WebRequest.Create(addressServer);
                request.Credentials = CredentialCache.DefaultCredentials;

                var message = username+":"+textBox1.Text;
                var messageData = Encoding.UTF8.GetBytes(message);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = 0;
                request.Method = "POST";
                request.Headers.Add("Mode", "Connect");
                var response = request.GetResponse();

                using (Stream stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream);
                    richTextBox1.Text += Environment.NewLine + reader.ReadToEnd();
                    reader.Close();
                }
            }

            else
            {
                MessageBox.Show("Введите адрес сервера и никнейм");
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            addressServer = comboBox1.Text;
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            addressMainClient = textBox2.Text;
        }

        private void Button3_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog1.FileName;
                var buffer = File.ReadAllBytes(path);
                WebRequest request = WebRequest.Create(addressServer);
                request.ContentType = "multipart/form-data";
                request.ContentLength = buffer.Length;
                request.Method = "POST";
                request.Headers.Add("Mode", "Files");
                request.Headers.Add("FileName", Path.GetFileName(path));
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Close();
                }
                var result = request.GetResponse();

            }
        }
    }
}
