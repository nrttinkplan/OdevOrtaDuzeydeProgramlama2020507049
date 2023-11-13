
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace nwap3010
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlCommand cmdtedarik;

        string constr = "Data Source=DESKTOP-J07VOSM\\SQLEXPRESS;Initial Catalog = dbnwind; Integrated Security = True";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnkaydet_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(constr);

            con.Open();
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"insert into Products(ProductName,SupplierID,CategoryID,UnitPrice) " +
                $"values('{txturunad.Text.ToString()}',{cmbtedarik.SelectedValue},{cmbkategori.SelectedValue},{nupbirimfiyat.Value})";
            cmd.ExecuteNonQuery();
            con.Close();
            tazele();

        }


        private void tazele()
        {
            con = new SqlConnection(constr);
            con.Open();

            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from Products order by " +
                "ProductID desc";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con = new SqlConnection(constr);
            con.Open();


            //Kategori bilgileri cmbkategori combosuna aktarýlýyor
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select CategoryID,CategoryName from Categories";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            cmbkategori.ValueMember = "CategoryID";
            cmbkategori.DisplayMember = "CategoryName";
            cmbkategori.DataSource = dt;


            //Tedarikçiler bilgileri cmbtedarik combosuna aktarýlýyor

            cmdtedarik = new SqlCommand();
            cmdtedarik.Connection = con;
            cmdtedarik.CommandText = "select SupplierID,CompanyName from Suppliers";
            cmdtedarik.ExecuteNonQuery();
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmdtedarik);
            da2.Fill(dt2);
            cmbtedarik.ValueMember = "SupplierID";
            cmbtedarik.DisplayMember = "CompanyName";
            cmbtedarik.DataSource = dt2;
            tazele();
            con.Close();





        }

        private void btnsil_Click(object sender, EventArgs e)
        {
            // DataGridView'da seçili satýra bak
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Seçili satýrýn indeksini al
                int selectedRowIndex = dataGridView1.SelectedRows[0].Index;

                // DataGridView'da seçili satýrdaki verinin ID'sini al
                int ProductID = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["ProductID"].Value);

                // Northwind veritabanýna baðlan

                {
                    con.Open();

                    // Veriyi silme SQL komutunu oluþtur
                    string deleteQuery = "Delete From Products WHERE ProductID = @ProductID";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", ProductID);

                        // Komutu çalýþtýr ve etkilenen satýr sayýsýný kontrol et
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Veri baþarýyla silindi.");
                            tazele();

                        }
                        else
                        {
                            MessageBox.Show("Veri silinirken bir hata oluþtu.");
                            tazele();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silinecek bir satýr seçin.");
                tazele();
            }


            con.Close();

        }

        private void btnbul_Click(object sender, EventArgs e)
        {

            // Kullanýcýdan metni al
            string arananMetin = txturunad.Text;

            if (!string.IsNullOrEmpty(arananMetin))
            {
                // Veritabanýndan verileri çek
                // Örneðin, SQL sorgusuyla Northwind veritabanýndan veri çekiyoruz:
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    con.Open();
                    string query = "SELECT * FROM Products WHERE ProductName LIKE @arananMetin";
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@arananMetin", "%" + arananMetin + "%");
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable table = new DataTable();
                        adapter.Fill(table);


                        // DataGridView'i güncelle
                        dataGridView1.DataSource = table;
                    }
                }
            }
            con.Close();

        }

        private void btnguncel_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedProductId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ProductID"].Value);

                string selectedProductName = dataGridView1.SelectedRows[0].Cells["ProductName"].Value.ToString();
                int selectedSupplierId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["SupplierID"].Value);
                int selectedCategoryId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CategoryID"].Value);
                decimal selectedUnitPrice = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["UnitPrice"].Value);

                con.Open();
                cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT CategoryID, CategoryName FROM Categories";
                cmd.ExecuteNonQuery();
                DataTable dtCategory = new DataTable();
                SqlDataAdapter daCategory = new SqlDataAdapter(cmd);
                daCategory.Fill(dtCategory);
                cmbkategori.ValueMember = "CategoryID";
                cmbkategori.DisplayMember = "CategoryName";
                cmbkategori.DataSource = dtCategory;

                cmdtedarik = new SqlCommand();
                cmdtedarik.Connection = con;
                cmdtedarik.CommandText = "SELECT SupplierID, CompanyName FROM Suppliers";
                cmdtedarik.ExecuteNonQuery();
                DataTable dtSupplier = new DataTable();
                SqlDataAdapter daSupplier = new SqlDataAdapter(cmdtedarik);
                daSupplier.Fill(dtSupplier);
                cmbtedarik.ValueMember = "SupplierID";
                cmbtedarik.DisplayMember = "CompanyName";
                cmbtedarik.DataSource = dtSupplier;
                con.Close();

                cmbkategori.SelectedValue = selectedCategoryId;
                cmbkategori.SelectedValue = selectedSupplierId;
                nupbirimfiyat.Value = selectedUnitPrice;
                txturunad.Text = selectedProductName;

                txturunad.Enabled = true;
                cmbkategori.Enabled = true;
                cmbtedarik.Enabled = true;
                nupbirimfiyat.Enabled = true;

                btnkaydet.Enabled = false;
                btnsil.Enabled = false;
            }
            else
            {
                MessageBox.Show("Lütfen güncellenecek bir satýr seçin.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(constr);
            con.Open();

            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Urunekleyeni";
            cmd.Parameters.AddWithValue("@vProductName", txturunad.Text);
            cmd.Parameters.AddWithValue("@vSupplierID", cmbtedarik.SelectedValue);
            cmd.Parameters.AddWithValue("@vCategoryID", cmbkategori.SelectedValue);
            cmd.Parameters.AddWithValue("@vUnitPrice", nupbirimfiyat.Value);
            cmd.ExecuteNonQuery();

            con.Close();
            tazele();

        }

        private void btnspsil_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(constr);
            con.Open();

            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_ProductsDelete";
            cmd.Parameters.AddWithValue("@ProductID", Convert.ToInt32(dataGridView1.CurrentRow.Cells["ProductID"].Value));

            cmd.ExecuteNonQuery();

            con.Close();
            tazele();
        }

        private void btnTazele_Click(object sender, EventArgs e)
        {
            tazele();
        }

        private void btnspara_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(constr);
            con.Open();

            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SearchProductsyeni";
            cmd.Parameters.AddWithValue("@ProductName", txturunad.Text);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView1.DataSource = dt;

            //cmd.ExecuteNonQuery();



            con.Close();
        }
    }
}
