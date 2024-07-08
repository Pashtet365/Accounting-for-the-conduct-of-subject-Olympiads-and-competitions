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
    /// Логика взаимодействия для Мероприятия.xaml
    /// </summary>
    public partial class Мероприятия : Window
    {
        private MainWindow _main;

        public Мероприятия()
        {
            InitializeComponent();
            LoadStages();
            LoadSubjects();
            if (MainWindow.changing == 1)
            {
                mainLabel.Content = "Изменение";
                mainButton.Content = "Сохранить";
                this.Title = "Изменение данных педагога";
                txtName.Text = MainWindow.element2.ToString();
                comboItem.Text = MainWindow.element3.ToString();
                pickerStartDate.Text = MainWindow.element4.ToString();
                comboStep.Text = MainWindow.element5.ToString();
            }
        }

        public Мероприятия(MainWindow main) : this()
        {
            _main = main;
        }

        private void mainButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение значений из полей ввода
            string name = txtName.Text;
            Subject selectedSubject = (Subject)comboItem.SelectedItem;
            Stage selectedStep = (Stage)comboStep.SelectedItem;
            DateTime? startDate = pickerStartDate.SelectedDate;

            if (startDate.HasValue && startDate.Value <= DateTime.Today)
            {
                MessageBox.Show("Дата проведения не может быть сегоднешней, и не может быть в прошлом времени!");
                return;
            }

            // Проверка, что все поля заполнены
            if (string.IsNullOrWhiteSpace(name) || selectedSubject == null || !startDate.HasValue)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // Получение id организации из выбранного объекта Organization
            int idItem = selectedSubject.Id;
            int idStep = selectedStep.Id;

            if (MainWindow.changing == 0)
            {
                try
                {
                    // Проверка на уникальность названия мероприятия
                    string checkQuery = "SELECT COUNT(*) FROM Мероприятия WHERE название = @name";

                    // Создание соединения и команды для проверки
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            SqlCommand checkCommand = new SqlCommand(checkQuery, dbConnection.GetConnection());
                            checkCommand.Parameters.AddWithValue("@name", name);
                            int existingCount = (int)checkCommand.ExecuteScalar();

                            if (existingCount > 0)
                            {
                                MessageBox.Show("Мероприятие с таким названием уже существует.");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ошибка подключения к базе данных.");
                            return;
                        }
                    }

                    // Создание строки запроса для добавления новой записи
                    string insertQuery = "INSERT INTO Мероприятия (название, id_предмета, дата_проведения, id_этапа) VALUES (@name, @subjectId, @startDate, @stepId)";

                    // Создание соединения и команды для добавления записи
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            SqlCommand insertCommand = new SqlCommand(insertQuery, dbConnection.GetConnection());
                            insertCommand.Parameters.AddWithValue("@name", name);
                            insertCommand.Parameters.AddWithValue("@subjectId", idItem);
                            insertCommand.Parameters.AddWithValue("@startDate", startDate);
                            insertCommand.Parameters.AddWithValue("@stepId", idStep);
                            insertCommand.ExecuteNonQuery();

                            MessageBox.Show("Новое мероприятие успешно добавлено.");

                            // Очистка полей ввода
                            txtName.Text = "";
                            comboItem.SelectedIndex = -1;
                            comboStep.SelectedIndex = -1;
                            pickerStartDate.SelectedDate = null;

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
                    // Получение текущего названия мероприятия из базы данных
                    string currentNameQuery = "SELECT название FROM Мероприятия WHERE id_мероприятия = @eventId";

                    string currentName;
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            SqlCommand getNameCommand = new SqlCommand(currentNameQuery, dbConnection.GetConnection());
                            getNameCommand.Parameters.AddWithValue("@eventId", MainWindow.element1);
                            currentName = (string)getNameCommand.ExecuteScalar();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка подключения к базе данных.");
                            return;
                        }
                    }

                    // Проверка на уникальность нового названия (если оно было изменено)
                    if (name != currentName)
                    {
                        string checkQuery = "SELECT COUNT(*) FROM Мероприятия WHERE название = @name";

                        using (DatabaseConnection dbConnection = new DatabaseConnection())
                        {
                            if (dbConnection.OpenConnection())
                            {
                                SqlCommand checkCommand = new SqlCommand(checkQuery, dbConnection.GetConnection());
                                checkCommand.Parameters.AddWithValue("@name", name);
                                int existingCount = (int)checkCommand.ExecuteScalar();

                                if (existingCount > 0)
                                {
                                    MessageBox.Show("Мероприятие с таким названием уже существует.");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Ошибка подключения к базе данных.");
                                return;
                            }
                        }
                    }

                    // Создание строки запроса для обновления записи
                    string updateQuery = "UPDATE Мероприятия SET название = @name, id_предмета = @subjectId, дата_проведения = @startDate, id_этапа = @stepId WHERE id_мероприятия = @eventId";

                    // Создание соединения и команды для обновления записи
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            SqlCommand updateCommand = new SqlCommand(updateQuery, dbConnection.GetConnection());
                            updateCommand.Parameters.AddWithValue("@name", name);
                            updateCommand.Parameters.AddWithValue("@subjectId", idItem);
                            updateCommand.Parameters.AddWithValue("@startDate", startDate);
                            updateCommand.Parameters.AddWithValue("@stepId", idStep);
                            updateCommand.Parameters.AddWithValue("@eventId", MainWindow.element1);
                            updateCommand.ExecuteNonQuery();

                            MessageBox.Show("Мероприятие успешно изменено.");

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

        public class Subject
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Stage
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private void LoadSubjects()
        {
            try
            {
                List<Subject> subjects = new List<Subject>();
                using (DatabaseConnection dbConnection = new DatabaseConnection())
                {
                    if (dbConnection.OpenConnection())
                    {
                        string selectQuery = "SELECT id_предмета, наименование FROM Предметы";
                        SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                        SqlDataReader reader = selectCmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            subjects.Add(new Subject { Id = id, Name = name });
                        }

                        reader.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка подключения к базе данных.");
                    }
                }

                // Привязать список предметов к ComboBox
                comboItem.ItemsSource = subjects;
                comboItem.DisplayMemberPath = "Name"; // Указать, какое свойство использовать для отображения в ComboBox
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void LoadStages()
        {
            try
            {
                List<Stage> stages = new List<Stage>();
                using (DatabaseConnection dbConnection = new DatabaseConnection())
                {
                    if (dbConnection.OpenConnection())
                    {
                        string selectQuery = "SELECT id_этапа, наименование FROM Этапы";
                        SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                        SqlDataReader reader = selectCmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            stages.Add(new Stage { Id = id, Name = name });
                        }

                        reader.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка подключения к базе данных.");
                    }
                }

                // Привязать список этапов к ComboBox
                comboStep.ItemsSource = stages;
                comboStep.DisplayMemberPath = "Name"; // Указать, какое свойство использовать для отображения в ComboBox
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
