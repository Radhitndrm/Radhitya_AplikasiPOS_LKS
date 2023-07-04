using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Desktop_AplikasiPOS
{
    public partial class Main_Dashboard : Form
    {
        public class front { 
           public string last_bought { get; set; }
           public int income { get; set; }
        }
        HttpClient httpclient = new HttpClient();
        public string urlApi = "http://localhost:8081";
        public Main_Dashboard()
        {
            InitializeComponent();
        }

        private void Main_Dashboard_Load(object sender, EventArgs e)
        {
            httpclient.BaseAddress = new Uri(urlApi);
            getFront();
        }

        private void getFront()
        {
            List<front> fronts = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            HttpResponseMessage response = httpclient.GetAsync("/front/get.php").Result;

            // Check apakah response berhasil
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                fronts= serializer.Deserialize<List<front>>(data);

                // Menambahkan ke Data Grid View
                // Menghapus semua kolom dan row yang ada
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();

                // Menambahkan kolom
             
                dataGridView1.Columns.Add("Last Bought", "Last Bought");
                dataGridView1.Columns.Add("Income", "Income");





                foreach (var s in fronts)
                {
                    // Menambahkan row data
                    dataGridView1.Rows.Add(new string[] {
                     s.last_bought,s.income.ToString()
                    }) ;
                }
            }
        }


        private void button_produk_Click(object sender, EventArgs e)
        {
            Manajemen_Produk product = new Manajemen_Produk();
            product.Show();
            this.Hide();
        }

        private void button_order_Click(object sender, EventArgs e)
        {
            cart cart = new cart();
            cart.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
