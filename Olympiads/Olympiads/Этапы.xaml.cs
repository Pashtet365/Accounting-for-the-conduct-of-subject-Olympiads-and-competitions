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

namespace Olympiads
{
    /// <summary>
    /// Логика взаимодействия для Этапы.xaml
    /// </summary>
    public partial class Этапы : Window
    {
        private MainWindow _main;

        public Этапы()
        {
            InitializeComponent();
            if (MainWindow.changing == 1)
            {
                mainLabel.Content = "Изменение";
                mainButton.Content = "Сохранить";
                this.Title = "Изменение данных предмета";
                txtName.Text = MainWindow.element2.ToString();
            }
        }

        public Этапы(MainWindow main) : this()
        {
            _main = main;
        }

        private void mainButton_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название этапа!");
                return;
            }

            try
            {
                using (DatabaseConnection dbConnection = new DatabaseConnection())
                {
                    if (dbConnection.OpenConnection())
                    {
                        if (MainWindow.changing == 0) // Добавление записи
                        {
                            // Проверка наличия этапа с таким же названием
                            string checkQuery = "SELECT COUNT(*) FROM Этапы WHERE наименование = @name";
                            SqlCommand checkCommand = new SqlCommand(checkQuery, dbConnection.GetConnection());
                            checkCommand.Parameters.AddWithValue("@name", name);
                            int count = (int)checkCommand.ExecuteScalar();

                            if (count > 0)
                            {
                                MessageBox.Show("Этап с таким названием уже существует!");
                                return;
                            }

                            // Выполнение запроса на добавление новой записи
                            string insertQuery = "INSERT INTO Этапы (наименование) VALUES (@name)";
                            SqlCommand insertCommand = new SqlCommand(insertQuery, dbConnection.GetConnection());
                            insertCommand.Parameters.AddWithValue("@name", name);
                            insertCommand.ExecuteNonQuery();
                            txtName.Text = "";
                            MessageBox.Show("Новый этап успешно добавлен.");
                        }
                        else // Изменение записи
                        {
                            // Проверка наличия этапа с таким же названием, за исключением текущего этапа
                            string checkQuery = "SELECT COUNT(*) FROM Этапы WHERE наименование = @name AND id_этапа != @stageId";
                            SqlCommand checkCommand = new SqlCommand(checkQuery, dbConnection.GetConnection());
                            checkCommand.Parameters.AddWithValue("@name", name);
                            checkCommand.Parameters.AddWithValue("@stageId", MainWindow.element1);
                            int count = (int)checkCommand.ExecuteScalar();

                            if (count > 0)
                            {
                                MessageBox.Show("Этап с таким названием уже существует!");
                                return;
                            }

                            // Выполнение запроса на изменение записи
                            string updateQuery = "UPDATE Этапы SET наименование = @name WHERE id_этапа = @stageId";
                            SqlCommand updateCommand = new SqlCommand(updateQuery, dbConnection.GetConnection());
                            updateCommand.Parameters.AddWithValue("@name", name);
                            updateCommand.Parameters.AddWithValue("@stageId", MainWindow.element1);
                            updateCommand.ExecuteNonQuery();

                            MessageBox.Show("Название этапа успешно изменено.");
                        }

                        // Обновление данных в таблице (если необходимо)
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


        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow.changing = 0;
        }
    }
}
