using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using Window = System.Windows.Window;

namespace Olympiads
{
    /// <summary>
    /// Логика взаимодействия для Педагоги.xaml
    /// </summary>
    public partial class Педагоги : Window
    {
        private MainWindow _main;

        public Педагоги()
        {
            InitializeComponent();
            loadOrg();
            if (MainWindow.changing == 1)
            {
                mainLabel.Content = "Изменение";
                mainButton.Content = "Сохранить";
                this.Title = "Изменение данных педагога";
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

        public Педагоги(MainWindow main) : this()
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
            if (string.IsNullOrWhiteSpace(fio) || string.IsNullOrWhiteSpace(staf) || selectedOrg == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // Получение id организации из выбранного объекта Organization
            int orgId = selectedOrg.Id;

            if (MainWindow.changing == 0)
            {
                try
                {
                    // Создание строки запроса для добавления новой записи
                    string insertQuery = "INSERT INTO Педагоги (фио, должность, id_организации) VALUES (@fio, @staf, @orgId)";

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

                            MessageBox.Show("Новый педагог успешно добавлен.");

                            // Очистка полей ввода
                            txtFio.Text = "";
                            txtStaf.Text = "";
                            comboOrg.Text = "";

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
                    // Получение текущего id_организации у педагога
                    int currentOrgId;
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        string selectOrgIdQuery = "SELECT id_организации FROM Педагоги WHERE id_педагога = @teacherId";
                        SqlCommand selectOrgIdCommand = new SqlCommand(selectOrgIdQuery, dbConnection.GetConnection());
                        selectOrgIdCommand.Parameters.AddWithValue("@teacherId", MainWindow.element1);
                        dbConnection.OpenConnection();
                        currentOrgId = (int)selectOrgIdCommand.ExecuteScalar();
                    }

                    // Создание строки запроса для обновления записи педагога
                    string updateTeacherQuery = "UPDATE Педагоги SET фио = @fio, должность = @staf, id_организации = @orgId WHERE id_педагога = @teacherId";

                    // Создание соединения и команды для обновления
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            SqlCommand updateTeacherCommand = new SqlCommand(updateTeacherQuery, dbConnection.GetConnection());
                            updateTeacherCommand.Parameters.AddWithValue("@fio", fio);
                            updateTeacherCommand.Parameters.AddWithValue("@staf", staf);
                            updateTeacherCommand.Parameters.AddWithValue("@orgId", orgId);
                            updateTeacherCommand.Parameters.AddWithValue("@teacherId", MainWindow.element1);
                            updateTeacherCommand.ExecuteNonQuery();

                            MessageBox.Show("Данные успешно обновлены.");

                            // Проверяем, изменилась ли id_организации у педагога
                            if (currentOrgId != orgId)
                            {
                                // Если изменилась, обновляем связанные записи в таблице Заявка
                                string updateApplicationsQuery = "UPDATE Заявка SET id_организации = @newOrgId WHERE id_педагога = @teacherId";

                                SqlCommand updateApplicationsCommand = new SqlCommand(updateApplicationsQuery, dbConnection.GetConnection());
                                updateApplicationsCommand.Parameters.AddWithValue("@newOrgId", orgId);
                                updateApplicationsCommand.Parameters.AddWithValue("@teacherId", MainWindow.element1);
                                updateApplicationsCommand.ExecuteNonQuery();
                            }

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
            foreach (char c in e.Text)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    e.Handled = true; // Запрещаем ввод, если символ не является буквой или пробелом.
                    break;
                }
            }
        }
    }
}
