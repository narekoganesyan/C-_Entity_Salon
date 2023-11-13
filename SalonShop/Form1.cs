using SalonShop.ModelEF;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalonShop
{
    public partial class Insert_TB : Form
    {
        Model1 db;
        public Insert_TB()
        {
            db = new Model1();
            InitializeComponent();
        }

        private void productBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.productBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.salonDataSet);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "salonDataSet.Product". При необходимости она может быть перемещена или удалена.
            this.productTableAdapter.Fill(this.salonDataSet.Product);

        }


        private async void Insert_Click(object sender, EventArgs e)
        {
            try
            {
                Product prd = new Product();

                // Генерируем уникальный ID, который не существует в базе данных
                prd.ID = GenerateUniqueID();

                prd.Title = "Вишня";
                prd.Cost = 100;
                prd.Description = "Сладкий";

                prd.IsActive = 0;
                prd.ManufacturedID = 1;

                db.Product.Add(prd);
                await Task.Run(() => db.SaveChanges());

                // Обновляем DataGridView
                productDataGridView.DataSource = db.Product.ToList();
            }
            catch (DbUpdateException ex)
            {
                // Обработка ошибки
                var innerException = ex.InnerException;
                // Выводи информацию об ошибке в консоль или в логи
                Console.WriteLine($"Ошибка при обновлении записей: {innerException.Message}");
            }
        }

        // Метод для генерации уникального ID
        private int GenerateUniqueID()
        {
            // Логика генерации уникального ID, например, случайное число до 8
            Random random = new Random();
            int generatedID = random.Next(10, 15);

            // Проверка, что сгенерированный ID не существует в базе данных
            while (db.Product.Any(p => p.ID == generatedID))
            {
                generatedID = random.Next(10, 15);
            }

            return generatedID;
        }

        private void Update_Click(object sender, EventArgs e)
        {
            Product prd = db.Product.Find(3);

            prd.Title = "Яблоко";

            db.SaveChanges();

            productDataGridView.DataSource = db.Product.ToList();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                Product prd = db.Product.Find(int.Parse(textBox1.Text));


                if (prd != null)
                {
                    db.Product.Remove(prd);
                    db.SaveChanges();
                    productDataGridView.DataSource = db.Product.ToList();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Возникла ошибка:" + ex.Message);
            }
        }

        private async void InsertTB_Click(object sender, EventArgs e)
        {
            Product prd = new Product();
            prd.ID = int.Parse(tb_ID.Text);
            prd.Title = tb_Title.Text;
            prd.Cost = int.Parse(tb_Cost.Text);
            prd.Description = tb_Description.Text;
            prd.IsActive = int.Parse(tb_IsActive.Text);
            prd.ManufacturedID = int.Parse(tb_ManufacturedID.Text);
            db.Product.Add(prd);
            await Task.Run(() => db.SaveChanges());
            MessageBox.Show("Новый объект добавлен");
            productDataGridView.DataSource = db.Product.ToList();
        }

        private void UpdateTB_Click(object sender, EventArgs e)
        {
            if (productDataGridView.SelectedRows.Count > 0)
            {
                int index = productDataGridView.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(productDataGridView[0, index].Value.ToString(), out id);
                if (converted == false) return;

                Product prd = db.Product.Find(id);
                db.Product.Remove(prd);
                db.SaveChanges();

                MessageBox.Show("Объекст удален");
                productDataGridView.DataSource = db.Product.ToList();

            }

        }

        private void update_tb_Click(object sender, EventArgs e)
        {
            try
            {
                Product prd = new Product();
                prd = db.Product.Find(int.Parse(tb_ID.Text));
                //prd.ID = int.Parse(IDProductText.Text);
                prd.Title = tb_Title.Text;
                prd.Cost = decimal.Parse(tb_Cost.Text);
                prd.Description = tb_Description.Text;
                //prd.MainImagePath = 
                prd.IsActive = int.Parse(tb_IsActive.Text);
                prd.ManufacturedID = int.Parse(tb_ManufacturedID.Text);
                db.SaveChanges();
                MessageBox.Show("Объект обновлён");
                if (db.Product != null)
                {
                    productBindingSource.DataSource = db.Product.ToList();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Ошибка: " + exp);
                throw exp;
            }
        }

        private void productDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            if (productDataGridView.SelectedRows[0].Cells[0].Value != null)
                tb_ID.Text = productDataGridView.SelectedRows[0].Cells[0].Value.ToString();
            if (productDataGridView.SelectedRows[0].Cells[1].Value != null)
                tb_Title.Text = productDataGridView.SelectedRows[0].Cells[1].Value.ToString();
            if (productDataGridView.SelectedRows[0].Cells[2].Value != null)
                tb_Cost.Text = productDataGridView.SelectedRows[0].Cells[2].Value.ToString();
            if (productDataGridView.SelectedRows[0].Cells[3].Value != null)
                tb_Description.Text = productDataGridView.SelectedRows[0].Cells[3].Value.ToString();
            if (productDataGridView.SelectedRows[0].Cells[4].Value != null)
                tb_IsActive.Text = productDataGridView.SelectedRows[0].Cells[4].Value.ToString();
            if (productDataGridView.SelectedRows[0].Cells[5].Value != null)
                tb_ManufacturedID.Text = productDataGridView.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void filter_Click(object sender, EventArgs e)
        {
            var filter = db.Product.Local.ToBindingList().Where(x => x.Title.Contains(tb_Title.Text));
            this.productBindingSource.DataSource = filter.Count() > 0 ? filter : null;
                filter.ToArray();

        }
    }
}
