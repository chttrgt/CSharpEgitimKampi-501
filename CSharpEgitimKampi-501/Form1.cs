using CSharpEgitimKampi_501.Dtos;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpEgitimKampi_501
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection cnn = new SqlConnection("Data Source=CIHATTURGUT\\SQLCHTTRGT;Initial Catalog=EgitimKampi501Db;Integrated Security=True");

        private async void btnList_ClickAsync(object sender, EventArgs e)
        {
            string query = "SELECT * FROM tbl_Product";
            var values = await cnn.QueryAsync<ResultProductDto>(query);
            dataGridView1.DataSource = values.ToList();

        }

        private async void btnAdd_ClickAsync(object sender, EventArgs e)
        {
            string query = "INSERT INTO tbl_Product (ProductName,ProductStock ,ProductPrice,ProductCategory) VALUES (@ProductName,  @ProductStock, @ProductPrice,@ProductCategory)";

            var parameters = new DynamicParameters();
            parameters.Add("@ProductName", txtProductName.Text);
            parameters.Add("@ProductStock", txtProductStock.Text);
            parameters.Add("@ProductPrice", txtProductPrice.Text);
            parameters.Add("@ProductCategory", txtProductCategory.Text);

            await cnn.ExecuteAsync(query, parameters);
            MessageBox.Show("Product Added");

        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM tbl_Product WHERE ProductId = @ProductId";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", txtProductId.Text);
            await cnn.ExecuteAsync(query, parameters);
            MessageBox.Show("Product Deleted");

        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            string query = "UPDATE tbl_Product SET ProductName = @ProductName, ProductStock = @ProductStock, ProductPrice = @ProductPrice, ProductCategory = @ProductCategory WHERE ProductId = @ProductId";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", txtProductId.Text);
            parameters.Add("@ProductName", txtProductName.Text);
            parameters.Add("@ProductStock", txtProductStock.Text);
            parameters.Add("@ProductPrice", txtProductPrice.Text);
            parameters.Add("@ProductCategory", txtProductCategory.Text);
            await cnn.ExecuteAsync(query, parameters);
            MessageBox.Show("Product Updated");

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string query = "SELECT Count(*) FROM tbl_Product";

            var values = await cnn.QueryAsync<int>(query);
            lblTotalProductCount.Text = values.FirstOrDefault().ToString();

            string query2 = "SELECT ProductName FROM tbl_Product WHERE ProductPrice=(SELECT MAX(ProductPrice) FROM tbl_Product)";
            var values2 = await cnn.QueryAsync<string>(query2);
            lblMostExpensiveProduct.Text = values2.FirstOrDefault().ToString();

            string query3 = "SELECT Count(Distinct(ProductCategory)) FROM tbl_Product";
            var values3 = await cnn.QueryAsync<int>(query3);
            lblCategoryCount.Text = values3.FirstOrDefault().ToString();

        }
    }
}
