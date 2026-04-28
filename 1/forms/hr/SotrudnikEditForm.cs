using _1.data;
using Npgsql;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace _1.forms
{
    public partial class SotrudnikEditForm : Form
    {
        private int _sotrudnikId = -1; // -1 = новый

        // Конструктор для добавления
        public SotrudnikEditForm()
        {
            InitializeComponent();
            AttachInputRestrictions();
        }

        // Конструктор для редактирования
        public SotrudnikEditForm(int id)
        {
            InitializeComponent();
            AttachInputRestrictions();
            _sotrudnikId = id;
            LoadData();
        }

        private void AttachInputRestrictions()
        {
            textBoxFio.KeyPress += TextBoxFio_KeyPress;
            textBoxSeriya.KeyPress += TextBoxDigitsOnly_KeyPress;
            textBoxNomerPasporta.KeyPress += TextBoxDigitsOnly_KeyPress;
        }

        private void SotrudnikEditForm_Load(object sender, EventArgs e)
        {
            // Загружаем список должностей
            string sql = "SELECT dolzhnost_id, nazvanie FROM dolzhnost ORDER BY nazvanie";
            var dt = Db.GetData(sql);
            comboBoxDolzhnost.DataSource = dt;
            comboBoxDolzhnost.DisplayMember = "nazvanie";
            comboBoxDolzhnost.ValueMember = "dolzhnost_id";
            if (_sotrudnikId == -1) comboBoxDolzhnost.SelectedIndex = -1;
        }

        private void LoadData()
        {
            string sql = @"
                SELECT fio, nomer_telefona, email, dolzhnost_id,
                       seriya_pasporta, nomer_pasporta, adres, data_rojdeniya
                FROM sotrudniki
                WHERE sotrudnik_id = @id
            ";
            var table = Db.GetData(sql, new NpgsqlParameter("@id", _sotrudnikId));
            if (table.Rows.Count == 0) return;
            var row = table.Rows[0];

            textBoxFio.Text = row["fio"].ToString();
            textBoxPhone.Text = row["nomer_telefona"]?.ToString();
            textBoxEmail.Text = row["email"]?.ToString();
            comboBoxDolzhnost.SelectedValue = row["dolzhnost_id"];

            textBoxSeriya.Text = row["seriya_pasporta"]?.ToString();
            textBoxNomerPasporta.Text = row["nomer_pasporta"]?.ToString();
            textBoxAdres.Text = row["adres"]?.ToString();

            if (row["data_rojdeniya"] != DBNull.Value)
            {
                object val = row["data_rojdeniya"];
                if (val is DateTime dateTimeVal)
                    dateTimePickerBirth.Value = dateTimeVal;
                else if (val is DateOnly dateOnly)
                    dateTimePickerBirth.Value = dateOnly.ToDateTime(TimeOnly.MinValue);
                else
                    dateTimePickerBirth.Value = Convert.ToDateTime(val);
            }
            else
            {
                dateTimePickerBirth.Value = DateTime.Today;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxFio.Text))
            {
                MessageBox.Show("Введите ФИО сотрудника!");
                return;
            }
            if (comboBoxDolzhnost.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите должность!");
                return;
            }

            // Проверка паспортных данных
            if (string.IsNullOrWhiteSpace(textBoxSeriya.Text) || textBoxSeriya.Text.Length != 4)
            {
                MessageBox.Show("Введите корректную серию паспорта (4 цифры)!");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBoxNomerPasporta.Text) || textBoxNomerPasporta.Text.Length != 6)
            {
                MessageBox.Show("Введите корректный номер паспорта (6 цифр)!");
                return;
            }

            // Проверка email
            if (!string.IsNullOrWhiteSpace(textBoxEmail.Text) && !IsValidEmail(textBoxEmail.Text))
            {
                MessageBox.Show("Некорректный email!");
                return;
            }

            using var conn = Db.GetConnection();
            conn.Open();
            using var tr = conn.BeginTransaction();

            try
            {
                if (_sotrudnikId == -1) // Добавление
                {
                    string sql = @"
                        INSERT INTO sotrudniki 
                        (restoran_id, dolzhnost_id, fio, nomer_telefona, email,
                         seriya_pasporta, nomer_pasporta, adres, data_rojdeniya)
                        VALUES 
                        (1, @dolzh, @fio, @phone, @email,
                         @seriya, @nomer_pasporta, @adres, @data_rojdeniya)
                    ";
                    using var cmd = new NpgsqlCommand(sql, conn, tr);
                    cmd.Parameters.AddWithValue("@dolzh", comboBoxDolzhnost.SelectedValue);
                    cmd.Parameters.AddWithValue("@fio", textBoxFio.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", textBoxPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", textBoxEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@seriya", textBoxSeriya.Text.Trim());
                    cmd.Parameters.AddWithValue("@nomer_pasporta", textBoxNomerPasporta.Text.Trim());
                    cmd.Parameters.AddWithValue("@adres", textBoxAdres.Text.Trim());
                    cmd.Parameters.AddWithValue("@data_rojdeniya", dateTimePickerBirth.Value.Date);
                    cmd.ExecuteNonQuery();
                }
                else // Редактирование
                {
                    string sql = @"
                        UPDATE sotrudniki
                        SET dolzhnost_id = @dolzh,
                            fio = @fio,
                            nomer_telefona = @phone,
                            email = @email,
                            seriya_pasporta = @seriya,
                            nomer_pasporta = @nomer_pasporta,
                            adres = @adres,
                            data_rojdeniya = @data_rojdeniya
                        WHERE sotrudnik_id = @id
                    ";
                    using var cmd = new NpgsqlCommand(sql, conn, tr);
                    cmd.Parameters.AddWithValue("@dolzh", comboBoxDolzhnost.SelectedValue);
                    cmd.Parameters.AddWithValue("@fio", textBoxFio.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", textBoxPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", textBoxEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@seriya", textBoxSeriya.Text.Trim());
                    cmd.Parameters.AddWithValue("@nomer_pasporta", textBoxNomerPasporta.Text.Trim());
                    cmd.Parameters.AddWithValue("@adres", textBoxAdres.Text.Trim());
                    cmd.Parameters.AddWithValue("@data_rojdeniya", dateTimePickerBirth.Value.Date);
                    cmd.Parameters.AddWithValue("@id", _sotrudnikId);
                    cmd.ExecuteNonQuery();
                }
                tr.Commit();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Ограничение ввода ФИО – только буквы, пробел, дефис, точка
        private void TextBoxFio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != ' ' && e.KeyChar != '-' && e.KeyChar != '.' && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        // Только цифры для паспортных данных
        private void TextBoxDigitsOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        // Проверка формата email
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}