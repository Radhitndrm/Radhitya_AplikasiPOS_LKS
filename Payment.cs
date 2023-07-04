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
    public partial class Payment : Form
    {
        public class item {
            public int id { get; set; }
            public string customer { get; set; }
            public string product_name { get; set; }
            public int quantity { get; set; }
            public int total { get; set; }
            public int kembalian { get; set; }
        }
        HttpClient httpClient = new HttpClient();
        public string urlApi = "http://localhost:8081";
        public Payment()
        {
            InitializeComponent();
        }

        private void Payment_Load(object sender, EventArgs e)
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
                dataGridView1.Columns.Add("Customer", "Customer");
                dataGridView1.Columns.Add("Product Name", "Product Name");
                dataGridView1.Columns.Add("Quantity", "Quantity");
                dataGridView1.Columns.Add("Total", "Total");





                foreach (var s in items)
                {
                    // Menambahkan row data
                    dataGridView1.Rows.Add(new string[] {
                        s.id.ToString(), s.customer, s.product_name, s.quantity.ToString(), s.total.ToString()
                    })  ;
                }
            }
        }
        private async void Pay(Dictionary<String, String> item)
        {
            if (string.IsNullOrWhiteSpace(item["kembalian"]))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            var values = item;
            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await httpClient.PostAsync("transaction/update.php", content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Payment Success. view details?");
                report report = new report();
                report  .Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Payment Failed");
            }
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!int.TryParse(textBox5.Text, out int pay) || pay <= 0)
            {
                MessageBox.Show("The amount must be a valid number.");
                return;
            }

            if (!int.TryParse(textBox4.Text, out int total) || total <= 0)
            {
                MessageBox.Show("Price must be a valid number.");
                return;
            }

            if(total > pay) {
                MessageBox.Show("not enough money");
            }
            int result = pay - total;

            Dictionary<String, String> item = new Dictionary<String, String>
            {
            {"id",textBox6.Text },
            {"kembalian",result.ToString()}
            };
            Pay(item);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            }
            catch
            {

            }
        }

        private void button_payment_kembali_Click(object sender, EventArgs e)
        {
           cart cart = new cart();
            cart.Show();
            this.Hide();
        }
    }
}
