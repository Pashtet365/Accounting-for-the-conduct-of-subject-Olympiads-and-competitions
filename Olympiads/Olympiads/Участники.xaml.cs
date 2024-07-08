using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Olympiads
{
    /// <summary>
    /// Логика взаимодействия для Участники.xaml
    /// </summary>
    public partial class Участники : Window
    {
        private MainWindow _main;

        public Участники()
        {
            InitializeComponent();
            loadOrg();
            if (MainWindow.changing == 1)
            {
                mainLabel.Content = "Изменение";
                mainButton.Content = "Сохранить";
                this.Title = "Изменение данных участника";
                txtFio.Text = MainWindow.element2.ToString();
                txtStaf.Text = MainWindow.element3.ToString();
                comboOrg.Text = MainWindow.element4.ToString();
                /*txtNameOrganization.Text = MainWindow.element5.ToString();
                txtTheme.Text = MainWindow.element6.ToString();
                txtNumberOfHours.Text = MainWindow.element7.ToString();
                txtFioDirector.Text = MainWindow.element8.ToString();
                txtCity.Text = MainWindow.element9.ToString();
                txtNumber.Text = MainWindow.element10.ToString();*/
            }
        }

        public Участники(MainWindow main) : this()
        {
            _main = main;
        }

        private void mainButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение значений из полей ввода
            string fio = txtFio.Text;
            string staf = txtStaf.Text;
            Organization selectedOrg = (Organization)comboOrg.SelectedItem;

            // Проверка, что все поля заполнены
            if (string.IsNullOrWhiteSpace(fio) || selectedOrg == null)
            {
                MessageBox.Show("Пожалуйста, заполните поля ФИО, и организация.");
                return;
            }

            // Получение id организации из выбранного объекта Organization
            int orgId = selectedOrg.Id;

            if (MainWindow.changing == 0)
            {
                try
                {
                    // Создание строки запроса для добавления новой записи
                    string insertQuery = "INSERT INTO Участники (фио, класс, id_организации) VALUES (@fio, @staf, @orgId)";

                    // Создание соединения и команды для добавления записи
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            SqlCommand insertCommand = new SqlCommand(insertQuery, dbConnection.GetConnection());
                            insertCommand.Parameters.AddWithValue("@fio", fio);
                            insertCommand.Parameters.AddWithValue("@staf", staf);
                            insertCommand.Parameters.AddWithValue("@orgId", orgId);
                            insertCommand.ExecuteNonQuery();

                            MessageBox.Show("Новый участник успешно добавлен.");

                            // Очистка полей ввода
                            txtFio.Text = "";
                            txtStaf.Text = "";
                            comboOrg.SelectedItem = null;

                            // Обновление данных в таблице
                            _main.Refresh(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("Ошибка подключения к базе данных.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при выполнении запроса: " + ex.Message);
                }
            }

            else
            {

                try
                {
                    // Создание строки запроса для обновления записи
                    string updateQuery = "UPDATE Участники SET фио = @fio, класс = @staf, id_организации = @orgId WHERE id_участника = @participantId";

                    // Создание соединения и команды для обновления
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            SqlCommand updateCommand = new SqlCommand(updateQuery, dbConnection.GetConnection());
                            updateCommand.Parameters.AddWithValue("@fio", fio);
                            updateCommand.Parameters.AddWithValue("@staf", staf);
                            updateCommand.Parameters.AddWithValue("@orgId", orgId);
                            updateCommand.Parameters.AddWithValue("@participantId", MainWindow.element1);
                            updateCommand.ExecuteNonQuery();

                            MessageBox.Show("Данные успешно обновлены.");

                            // Очистка полей ввода
                            txtFio.Text = "";
                            txtStaf.Text = "";
                            comboOrg.SelectedItem = null;

                            // Обновление данных в таблице
                            _main.Refresh(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("Ошибка подключения к базе данных.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при выполнении запроса: " + ex.Message);
                }
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow.changing = 0;
        }

        public class Organization
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private void loadOrg()
        {
            try
            {
                List<Organization> organizations = new List<Organization>();
                using (DatabaseConnection dbConnection = new DatabaseConnection())
                {
                    if (dbConnection.OpenConnection())
                    {
                        string selectQuery = "SELECT id_организации, наименование FROM Организация";
                        SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                        SqlDataReader reader = selectCmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            organizations.Add(new Organization { Id = id, Name = name });
                        }

                        reader.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка подключения к базе данных.");
                    }
                }

                // Привязать список организаций к ComboBox
                comboOrg.ItemsSource = organizations;
                comboOrg.DisplayMemberPath = "Name"; // Указать, какое свойство использовать для отображения в ComboBox
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }


        //ввод только букв
        private void txtFio_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    e.Handled = true; // Запрещаем ввод, если символ не является буквой или пробелом.
                    break;
                }
            }
        }

        private void txtStaf_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]{1,2}[А-Я]{0,1}$");
            if (!regex.IsMatch((sender as TextBox).Text + e.Text))
                e.Handled = true;
        }

        private void txtStaf_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]{1,2}[А-Я]{0,1}$");
            if (!regex.IsMatch((sender as TextBox).Text))
            {
                // Если текст не соответствует формату, очистите TextBox
                (sender as TextBox).Text = "";
            }
        }
    }
}
