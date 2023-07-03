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
    public partial class Manajemen_Produk : Form
    {
        public class category_list
        {
            public int id { set; get; }
            public string category { get; set; }
        }
        public class product
        {
            public int id { set; get; }
            public string name { set; get; }
            public int price { set; get; }
            public int stocks { set; get; }
            public string category { get; set; }
        }
        public Manajemen_Produk()
        {
            InitializeComponent();
        }
        public HttpClient httpClient = new HttpClient();
        public string urlAPI = "http://localhost:8081";
        private void Manajemen_Produk_Load(object sender, EventArgs e)
        {
            httpClient.BaseAddress = new Uri(urlAPI);
            getProduct();
            getCategory();
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
                dataGridView1.Columns.Add("Name", "Name");
                dataGridView1.Columns.Add("Price", "Price");
                dataGridView1.Columns.Add("Stocks", "Stocks");
                dataGridView1.Columns.Add("Category", "Category");



                foreach (var s in products)
                {
                    // Menambahkan row data
                    dataGridView1.Rows.Add(new string[] {
                        s.id.ToString(), s.name, s.price.ToString(), s.stocks.ToString(), s.category
                    }); ;
                }
            }
        }

        private async void addProduct(Dictionary<String, String> product)
        {
            if (string.IsNullOrWhiteSpace(product["name"]) ||
                string.IsNullOrWhiteSpace(product["price"]) ||
                string.IsNullOrWhiteSpace(product["stocks"]) ||
                string.IsNullOrWhiteSpace(product["category"]))
            {
                MessageBox.Show("Harap isi semua field.");
                return;
            }

            var values = product;
            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await httpClient.PostAsync("product/add.php", content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Product sucessfully added");
                getProduct();
            }
            else
            {
                MessageBox.Show("Product failed to be added");
            }
        }

        private void button_post_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> product = new Dictionary<String, String>
            {
                {"name",textBox2.Text },
                {"price",textBox3.Text},
                {"stocks",textBox4.Text},
                {"category",comboBox1.Text }
            };
            addProduct(product);
        }

        private async void updateProduct(Dictionary<String,String> product)
        {
            if (string.IsNullOrWhiteSpace(product["name"]) ||
               string.IsNullOrWhiteSpace(product["price"]) ||
               string.IsNullOrWhiteSpace(product["stocks"]) ||
               string.IsNullOrWhiteSpace(product["category"]))
            {
                MessageBox.Show("Harap isi semua field.");
                return;
            }

            var values = product;
            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await httpClient.PostAsync("/product/update.php", content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Product Succesfully Updated");
                getProduct();
            }
            else
            {
                MessageBox.Show("Product Failed to be Updated");
            }

        }

        private void button_update_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> product = new Dictionary<String, String>
            {
                {"id",textBox1.Text },
                {"name",textBox2.Text },
                {"price",textBox3.Text},
                {"stocks",textBox4.Text},
                {"category",comboBox1.Text }
            };
            updateProduct(product);
        }

        private async void deleteProduct(Dictionary<String, String> product)
        {
            

            var values = product;
            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await httpClient.PostAsync("/product/delete.php", content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Product Succesfully Deleted");
                getProduct();
            }
            else
            {
                MessageBox.Show("Product Failed to be Deleted");
            }

        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> product = new Dictionary<String, String>
            {
                {"id",textBox1.Text }
            };
            deleteProduct(product);
        }

        private void getCategory()
        {
            List<category_list> categories = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            HttpResponseMessage response = httpClient.GetAsync("/category/get.php").Result;

            // Check apakah response berhasil
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                categories = serializer.Deserialize<List<category_list>>(data);

                // Menambahkan ke Data Grid View
                // Menghapus semua kolom dan row yang ada
                dataGridView2.Columns.Clear();
                dataGridView2.Rows.Clear();

                // Menambahkan kolom
                dataGridView2.Columns.Add("Id", "Id");
                dataGridView2.Columns.Add("Category", "Category");



                foreach (var s in categories)
                {
                    // Menambahkan row data
                    dataGridView2.Rows.Add(new string[] {
                        s.id.ToString(), s.category
                    }); ;
                }
            }
        }

        

        private async void addCategory(Dictionary<String, String> category)
        {
            if (string.IsNullOrWhiteSpace(category["category"]))
            {
                MessageBox.Show("Harap isi semua field.");
                return;
            }
            var values = category;
            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await httpClient.PostAsync("category/add.php", content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Category sucessfully added");
                getCategory();
            }
            else
            {
                MessageBox.Show("Category failed to be added");
            }
        }
        private void button_insert_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> category = new Dictionary<String, String>
            {
                {"category",textBox6.Text }
            };
            addCategory(category);
        }

        private async void updateCategory(Dictionary<String, String> category)
        {
            if (string.IsNullOrWhiteSpace(category["category"]))
            {
                MessageBox.Show("Harap isi semua field.");
                return;
            }
            var values = category;
            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await httpClient.PostAsync("/category/update.php", content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Category Succesfully Updated");
                getCategory();
            }
            else
            {
                MessageBox.Show("Category Failed to be Updated");
            }
        }

        private void button_Ucategory_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> category = new Dictionary<String, String>
            {
                {"id",textBox5.Text },
                {"category",textBox6.Text }
            };
            updateCategory(category);
        }

        private void button_dirrect_main_Click(object sender, EventArgs e)
        {
            Main_Dashboard main = new Main_Dashboard();
            main.Show();
            this.Hide();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();

            }
            catch
            {

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBox5.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBox6.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
            catch
            {

            }
        }

       
    }
}
