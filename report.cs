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
    public partial class report : Form
    {
        public class item
        {
            public int id { get; set; }
            public string date_time { get; set; }
            public string customer { get; set; }
            public string product_name { get; set; }
            public int quantity { get; set; }
            public int total { get; set; }
            public int kembalian { get; set; }


        }
        HttpClient httpClient = new HttpClient();
        public string urlApi = "http://localhost:8081";
        public report()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void report_Load(object sender, EventArgs e)
        {
            httpClient.BaseAddress = new Uri(urlApi);
            getItem();
        }

        private void getItem()
        {
            List<item> items = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            HttpResponseMessage response = httpClient.GetAsync("/transaction/get.php").Result;

            // Check apakah response berhasil
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                items = serializer.Deserialize<List<item>>(data);

                // Menambahkan ke Data Grid View
                // Menghapus semua kolom dan row yang ada
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();

                // Menambahkan kolom
                dataGridView1.Columns.Add("Id", "Id");
                dataGridView1.Columns.Add("Date Time", "Date Time");
                dataGridView1.Columns.Add("Customer", "Customer");
                dataGridView1.Columns.Add("Product Name", "Product Name");
                dataGridView1.Columns.Add("Quantity", "Quantity");
                dataGridView1.Columns.Add("Total", "Total");
                dataGridView1.Columns.Add("Income", "Income");





                foreach (var s in items)
                {
                    // Menambahkan row data
                    dataGridView1.Rows.Add(new string[] {
                        s.id.ToString(),s.date_time, s.customer, s.product_name, s.quantity.ToString(), s.total.ToString(), s.kembalian.ToString()
                    });
                }
            }
        }

        private void button_report_kembali_Click(object sender, EventArgs e)
        {
            Payment payment = new Payment();
            payment.Show();
            this.Hide();
        }

        private async void addFront(Dictionary<String,String> item)
        {
            var values = item;
            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await httpClient.PostAsync("/front/add.php", content);
            if (response.IsSuccessStatusCode)
            {
               
                
            }
            else
            {
      
            }
        }
        private void button_add_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> front = new Dictionary<String, String>
            {
                {"last_bought",textBox2.Text },
                {"income",textBox3.Text}
            };
            addFront(front);

        }
        private async void deleteItem(Dictionary<String,String> item)
        {
            var values = item;
            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await httpClient.PostAsync("/transaction/delete.php", content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Purchase Confirmed");
         
            }
            else
            {
                MessageBox.Show("Purchase Failed/error");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> front = new Dictionary<String, String>
            {
                {"last_bought",textBox2.Text },
                {"income",textBox3.Text}
            };
            addFront(front);

            Dictionary<String, String> item = new Dictionary<String, String>
            {
                {"id",textBox1.Text }
            };
            deleteItem(item);
           
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            }
            catch
            {

            }
        }

      
        }
    }
