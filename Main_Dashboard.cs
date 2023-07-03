using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_AplikasiPOS
{
    public partial class Main_Dashboard : Form
    {
        public Main_Dashboard()
        {
            InitializeComponent();
        }

        private void Main_Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void button_produk_Click(object sender, EventArgs e)
        {
            Manajemen_Produk product = new Manajemen_Produk();
            product.Show();
            this.Hide();
        }
    }
}
