using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
using static Olympiads.Заявки;

namespace Olympiads
{
    /// <summary>
    /// Логика взаимодействия для Списки.xaml
    /// </summary>
    public partial class Списки : Window
    {
        private MainWindow _main;

        public Списки()
        {
            InitializeComponent();
            loadStudents();
            if (MainWindow.changing == 1)
            {
                mainLabel.Content = "Изменение";
                mainButton.Content = "Сохранить";
                this.Title = "Изменение данных списка";
                comboStudent.Text = MainWindow.element2.ToString();
                comboApl.Text = MainWindow.element3.ToString();
            }
        }

        public Списки(MainWindow main) : this()
        {
            _main = main;
        }

        private void mainButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение значений из полей ввода
            Student selectedStudent = (Student)comboStudent.SelectedItem;
            Application selectedAplication = (Application)comboApl.SelectedItem;

            // Проверка, что все поля заполнены
            if (selectedStudent == null || selectedAplication == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if(MainWindow.changing == 0)
            {
                try
                {
                    // Создание SQL-запроса для добавления записи в таблицу Список
                    string insertQuery = "INSERT INTO Список (id_участника, id_заявки) VALUES (@studentId, @applicationId)";

                    // Создание соединения и команды для выполнения запроса
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            SqlCommand insertCommand = new SqlCommand(insertQuery, dbConnection.GetConnection());

                            // Передача параметров в SQL-запрос
                            insertCommand.Parameters.AddWithValue("@studentId", selectedStudent.Id);
                            insertCommand.Parameters.AddWithValue("@applicationId", selectedAplication.ApplicationId);

                            // Выполнение SQL-запроса на добавление записи
                            int rowsAffected = insertCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Запись успешно добавлена в таблицу Список.");

                                // Очистка полей ввода
                                comboApl.SelectedIndex = -1;
                                comboStudent.SelectedIndex = -1;

                                // Обновление основной формы (если необходимо)
                                _main.Refresh(sender, e);
                            }
                            else
                            {
                                MessageBox.Show("Не удалось добавить запись в таблицу Список.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ошибка подключения к базе данных.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при добавлении записи в таблицу Список: " + ex.Message);
                }

            }
            else
            {
                try
                {
                    // Создание SQL-запроса для обновления записи в таблице Список
                    string updateQuery = "UPDATE Список SET id_участника = @studentId, id_заявки = @applicationId WHERE id_списка = @listId";

                    // Создание соединения и команды для выполнения запроса
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            SqlCommand updateCommand = new SqlCommand(updateQuery, dbConnection.GetConnection());

                            // Передача параметров в SQL-запрос
                            updateCommand.Parameters.AddWithValue("@studentId", selectedStudent.Id);
                            updateCommand.Parameters.AddWithValue("@applicationId", selectedAplication.ApplicationId);
                            updateCommand.Parameters.AddWithValue("@listId", MainWindow.element1);

                            // Выполнение SQL-запроса на обновление записи
                            int rowsAffected = updateCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Запись успешно обновлена в таблице Список.");

                                // Обновление основной формы (если необходимо)
                                _main.Refresh(sender, e);
                            }
                            else
                            {
                                MessageBox.Show("Не удалось обновить запись в таблице Список.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ошибка подключения к базе данных.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при обновлении записи в таблице Список: " + ex.Message);
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow.changing = 0;
        }

        public class Student
        {
            public int Id { get; set; }
            public string FullName { get; set; }
        }

        public class Application
        {
            public int ApplicationId { get; set; }
            public string EventName { get; set; }

            public string DisplayText => $"№ заявки: {ApplicationId}\n{EventName}";
        }


        private void comboStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboStudent.SelectedItem != null)
            {
                Student selectedStudent = (Student)comboStudent.SelectedItem;

                try
                {
                    List<Application> applications = new List<Application>();
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            // Получаем информацию об организации участника
                            string selectOrgQuery = "SELECT id_организации FROM Участники WHERE id_участника = @studentId";
                            SqlCommand selectOrgCmd = new SqlCommand(selectOrgQuery, dbConnection.GetConnection());
                            selectOrgCmd.Parameters.AddWithValue("@studentId", selectedStudent.Id);
                            int orgId = (int)selectOrgCmd.ExecuteScalar();

                            // Загружаем заявки, поданные преподавателями данной организации
                            string selectApplicationsQuery = @"
                        SELECT Заявка.id_заявки, Мероприятия.название
                        FROM Заявка
                        INNER JOIN Мероприятия ON Заявка.id_мероприятия = Мероприятия.id_мероприятия
                        WHERE Заявка.id_организации = @orgId
                        AND NOT EXISTS (
                            SELECT 1 FROM Список
                            WHERE id_участника = @studentId AND id_заявки = Заявка.id_заявки
                        )";
                            SqlCommand selectApplicationsCmd = new SqlCommand(selectApplicationsQuery, dbConnection.GetConnection());
                            selectApplicationsCmd.Parameters.AddWithValue("@orgId", orgId);
                            selectApplicationsCmd.Parameters.AddWithValue("@studentId", selectedStudent.Id);
                            SqlDataReader reader = selectApplicationsCmd.ExecuteReader();

                            while (reader.Read())
                            {
                                int applicationId = reader.GetInt32(0);
                                string eventName = reader.GetString(1);
                                applications.Add(new Application { ApplicationId = applicationId, EventName = eventName });
                            }

                            reader.Close();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка подключения к базе данных.");
                        }
                    }

                    // Привязываем список заявок к ComboBox
                    comboApl.ItemsSource = applications;
                    comboApl.DisplayMemberPath = "DisplayText"; // Используем свойство DisplayText для отображения в ComboBox
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void loadStudents()
        {
            try
            {
                List<Student> Students = new List<Student>();
                using (DatabaseConnection dbConnection = new DatabaseConnection())
                {
                    if (dbConnection.OpenConnection())
                    {
                        string selectQuery = "SELECT id_участника, фио FROM Участники";
                        SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                        SqlDataReader reader = selectCmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string fullName = reader.GetString(1);
                            Students.Add(new Student { Id = id, FullName = fullName });
                        }

                        reader.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка подключения к базе данных.");
                    }
                }

                comboStudent.ItemsSource = Students;
                comboStudent.DisplayMemberPath = "FullName";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
