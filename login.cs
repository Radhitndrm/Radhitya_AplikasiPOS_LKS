using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Desktop_AplikasiPOS
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
        HttpClient httpClient = new HttpClient();
        public string urlApi = "http://localhost:8081";
       

        private void login_Load(object sender, EventArgs e)
        {
            httpClient.BaseAddress = new Uri(urlApi);
        }

        private async void CheckLogin(Dictionary<String,String> login)
        {
            if (string.IsNullOrWhiteSpace(login["user"]) ||
              string.IsNullOrWhiteSpace(login["password"]))
            {
                MessageBox.Show("Harap isi semua field.");
                return;
            }
            var values = login;
            var content = new FormUrlEncodedContent(login);

            HttpResponseMessage response = await httpClient.PostAsync("users/login.php", content);

            if (response.IsSuccessStatusCode)
            {
                Dictionary<String, String> resultBody = JsonConvert.DeserializeObject<Dictionary<String, String>>(await response.Content.ReadAsStringAsync());

                Account.token = resultBody["token"];
                Account.user = resultBody["username"];

                MessageBox.Show("Login berhasil");

                Main_Dashboard main = new Main_Dashboard();
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Login Gagal");
            }
        }
        private void button_login_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> user = new Dictionary<String, String>
            {
                {"user",username.Text },
                {"password",password.Text }
            };
            CheckLogin(user);        
        }

    }
}
