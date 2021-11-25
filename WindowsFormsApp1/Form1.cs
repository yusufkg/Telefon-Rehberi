using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelefonRehberi
{
    public partial class Form1 : Form
    {
        Persons model = new Persons();
        public Form1()
        {
            InitializeComponent();
        }

        void Clear()
        {
            txtFirstName.Text = txtLastName.Text = txtCity.Text = txtCity.Text = "";
            btnEkle.Text = "Ekle";
            btnSil.Enabled = false;
            model.PersonID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulatedDataGridView();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            model.FirstName = txtFirstName.Text.Trim();
            model.LastName = txtLastName.Text.Trim();
            model.City = txtCity.Text.Trim();
            model.Address = txtAddress.Text.Trim();

            using (TelefonRehberiDbEntities db = new TelefonRehberiDbEntities())
            {
                if (model.PersonID == 0)
                    db.Persons.Add(model);
                else
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            Clear();
            PopulatedDataGridView();
            MessageBox.Show("Kaydetme başarılı");
        }

        void PopulatedDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            using (TelefonRehberiDbEntities db = new TelefonRehberiDbEntities())
            {
                dataGridView1.DataSource = db.Persons.ToList<Persons>();
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                model.PersonID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["PersonID"].Value);
                using (TelefonRehberiDbEntities db = new TelefonRehberiDbEntities())
                {
                    model = db.Persons.Where(x => x.PersonID == model.PersonID).FirstOrDefault();
                    txtFirstName.Text = model.FirstName;
                    txtLastName.Text = model.LastName;
                    txtCity.Text = model.City;
                    txtAddress.Text = model.Address;
                }
                btnEkle.Text = "Güncelle";
                btnSil.Enabled = true;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Silme istediğinden emin misin?", "EF CRUD Operation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (TelefonRehberiDbEntities db = new TelefonRehberiDbEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == System.Data.Entity.EntityState.Detached)
                        db.Persons.Attach(model);
                    db.Persons.Remove(model);
                    db.SaveChanges();
                    PopulatedDataGridView();
                    Clear();
                    MessageBox.Show("Silme Başarılı");

                }
            }

            {

            }
        }
    }
}
