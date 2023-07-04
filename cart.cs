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
    public partial class cart : Form
    {
        public class product
        {
            public int id { set; get; }
            public string name { set; get; }
            public int price { set; get; }
            public int stocks { set; get; }
            public string date_time { get; set; }
        }
        public HttpClient httpClient = new HttpClient();
        public string urlAPI = "http://localhost:8081";
        public cart()
        {
            InitializeComponent();
        }

       

        private void cart_Load(object sender, EventArgs e)
        {
            httpClient.BaseAddress = new Uri(urlAPI);
            getProduct();
        }

        private void getProduct()
        {
            List<product> products = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            HttpResponseMessage response = httpClient.GetAsync("/product/get.php").Result;

            // Check apakah response berhasil
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                products = serializer.Deserialize<List<product>>(data);

                // Menambahkan ke Data Grid View
                // Menghapus semua kolom dan row yang ada
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();

                // Menambahkan kolom
                dataGridView1.Columns.Add("Id", "Id");
                dataGridView1.Columns.Add("Product Name", "Product Name");
                dataGridView1.Columns.Add("Price", "Price");
                dataGridView1.Columns.Add("Stocks", "Stocks");



                foreach (var s in products)
                {
                    // Menambahkan row data
                    dataGridView1.Rows.Add(new string[] {
                        s.id.ToString(), s.name, s.price.ToString(), s.stocks.ToString(),
                    });
                }
            }
        }

        private async void addItem(Dictionary<String, String> item)
        {
            if (string.IsNullOrWhiteSpace(item["customer"]) ||
                string.IsNullOrWhiteSpace(item["quantity"]))
            {
                MessageBox.Show("Please fill in all fields..");
                return;
            }

            var values = item;
            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await httpClient.PostAsync("transaction/add.php", content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Item sucessfully added to cart");
                Payment payment = new Payment();
                payment.Show();
                this.Hide();
                getProduct();
            }
            else
            {
                MessageBox.Show("Item failed to be added to cart");
            }
        }

        private void button_post_cart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
               string.IsNullOrWhiteSpace(textBox2.Text) ||
               string.IsNullOrWhiteSpace(textBox3.Text) ||
               string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please fill in all fields..");
                return;
            }
            if (!int.TryParse(textBox4.Text,out int stocks) || stocks <= 0)
            {
                MessageBox.Show("Product Stocks Unavaible.");
                return;
            }
            if (!int.TryParse(textBox5.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("The Amount must be a valid number.");
                return;
            }

            if (!int.TryParse(textBox3.Text, out int price) || price <= 0)
            {
                MessageBox.Show("The Price must be a valid number.");
                return;
            }

            int total = quantity * price;

            Dictionary<String, String> item = new Dictionary<String, String>
            {
            {"customer",textBox1.Text },
            {"product_name",textBox2.Text},
            {"quantity",quantity.ToString() },
            {"date_time", dateTimePicker1.Value.ToString("yyyy-MM-dd")},
            {"total",total.ToString() }
            };
            addItem(item);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
            catch
            {

            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button_cart_kembali_Click(object sender, EventArgs e)
        {
            Main_Dashboard main = new Main_Dashboard();
            main.Show();
            this.Hide();
        }
    }
}
