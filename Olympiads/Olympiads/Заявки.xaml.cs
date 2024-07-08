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
using static Olympiads.Мероприятия;
using System.Xml.Linq;

namespace Olympiads
{
    /// <summary>
    /// Логика взаимодействия для Заявки.xaml
    /// </summary>
    public partial class Заявки : Window
    {
        private MainWindow _main;

        public Заявки()
        {
            InitializeComponent();
            loadTeachers();
            if (MainWindow.changing == 1)
            {
                mainLabel.Content = "Изменение";
                mainButton.Content = "Сохранить";
                this.Title = "Изменение данных заявки";
                comboTeacher.Text = MainWindow.element2.ToString();
                comboEvent.Text = MainWindow.element4.ToString();
                pickerStartDate.Text = MainWindow.element5.ToString();
                AddSelectedEventToCombo();
            }
        }


        public Заявки(MainWindow main) : this()
        {
            _main = main;
        }

        private void mainButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение значений из полей ввода
            Teacher selectedTeacher = (Teacher)comboTeacher.SelectedItem;
            Event selectedStep = (Event)comboEvent.SelectedItem;
            DateTime? startDate = pickerStartDate.SelectedDate;

            // Проверка, что все поля заполнены
            if (selectedTeacher == null ||  selectedStep == null || !startDate.HasValue)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }
            // Проверка, что дата подачи заявки не позже даты проведения мероприятия
            if (startDate > selectedStep.EventDate)
            {
                MessageBox.Show("Дата подачи заявки не может быть позже даты проведения мероприятия.");
                return;
            }


            if (MainWindow.changing == 0)
            {
                try
                {
                    // Подготовка SQL-запроса для добавления записи
                    string insertQuery = "INSERT INTO Заявка (id_педагога, id_организации, id_мероприятия, дата_подачи) " +
                                         "VALUES (@teacherId, @orgId, @eventId, @submissionDate)";

                    // Создание соединения и команды для выполнения запроса
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            SqlCommand insertCommand = new SqlCommand(insertQuery, dbConnection.GetConnection());

                            // Получение id_организации по id_педагога
                            string orgQuery = "SELECT id_организации FROM Педагоги WHERE id_педагога = @teacherId";
                            SqlCommand orgCommand = new SqlCommand(orgQuery, dbConnection.GetConnection());
                            orgCommand.Parameters.AddWithValue("@teacherId", selectedTeacher.Id);
                            object orgIdResult = orgCommand.ExecuteScalar();

                            // Проверка наличия id_организации
                            if (orgIdResult != null && orgIdResult != DBNull.Value)
                            {
                                int orgId = Convert.ToInt32(orgIdResult);

                                // Передача параметров в SQL-запрос
                                insertCommand.Parameters.AddWithValue("@teacherId", selectedTeacher.Id);
                                insertCommand.Parameters.AddWithValue("@orgId", orgId);
                                insertCommand.Parameters.AddWithValue("@eventId", selectedStep.Id);
                                insertCommand.Parameters.AddWithValue("@submissionDate", startDate);

                                // Выполнение SQL-запроса на добавление записи
                                int rowsAffected = insertCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Заявка успешно добавлена.");

                                    // Очистка полей ввода
                                    comboTeacher.SelectedIndex = -1;
                                    comboEvent.SelectedIndex = -1;
                                    pickerStartDate.SelectedDate = null;

                                    // Обновление основной формы (если необходимо)
                                    _main.Refresh(sender, e);
                                }
                                else
                                {
                                    MessageBox.Show("Не удалось добавить заявку.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Не удалось получить id_организации для данного педагога.");
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
                    MessageBox.Show("Произошла ошибка при добавлении заявки: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    // Получение текущего идентификатора заявки
                    int applicationId = Convert.ToInt32(MainWindow.element1);

                    // Подготовка SQL-запроса для обновления записи
                    string updateQuery = "UPDATE Заявка SET id_педагога = @teacherId," +
                                         "id_мероприятия = @eventId, дата_подачи = @submissionDate WHERE id_заявки = @appId";

                    // Создание соединения и команды для выполнения запроса
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            SqlCommand updateCommand = new SqlCommand(updateQuery, dbConnection.GetConnection());

                            // Передача параметров в SQL-запрос
                            updateCommand.Parameters.AddWithValue("@teacherId", selectedTeacher.Id);
                            updateCommand.Parameters.AddWithValue("@eventId", selectedStep.Id);
                            updateCommand.Parameters.AddWithValue("@submissionDate", startDate);
                            updateCommand.Parameters.AddWithValue("@appId", applicationId);

                            // Выполнение SQL-запроса на обновление записи
                            int rowsAffected = updateCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Заявка успешно обновлена.");

                                // Обновление основной формы (если необходимо)
                                _main.Refresh(sender, e);
                            }
                            else
                            {
                                MessageBox.Show("Не удалось обновить заявку.");
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
                    MessageBox.Show("Произошла ошибка при обновлении заявки: " + ex.Message);
                }
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow.changing = 0;
        }

        public class Event
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime EventDate { get; set; }
        }

        public class Teacher
        {
            public int Id { get; set; }
            public string FullName { get; set; }
        }

        private void loadTeachers()
        {
            try
            {
                List<Teacher> teachers = new List<Teacher>();
                using (DatabaseConnection dbConnection = new DatabaseConnection())
                {
                    if (dbConnection.OpenConnection())
                    {
                        string selectQuery = "SELECT id_педагога, фио FROM Педагоги";
                        SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                        SqlDataReader reader = selectCmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string fullName = reader.GetString(1);
                            teachers.Add(new Teacher { Id = id, FullName = fullName });
                        }

                        reader.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка подключения к базе данных.");
                    }
                }

                comboTeacher.ItemsSource = teachers;
                comboTeacher.DisplayMemberPath = "FullName";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void comboTeacher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Получаем выбранного учителя
                Teacher selectedTeacher = (Teacher)comboTeacher.SelectedItem;

                if (selectedTeacher != null)
                {
                    List<Event> events = new List<Event>();
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            // Извлекаем мероприятия, исключая те, в которых уже участвует выбранный учитель
                            string selectQuery = @"SELECT Мероприятия.id_мероприятия, Мероприятия.название, Мероприятия.дата_проведения 
                                           FROM Мероприятия 
                                           LEFT JOIN Заявка ON Мероприятия.id_мероприятия = Заявка.id_мероприятия 
                                           WHERE Мероприятия.id_мероприятия NOT IN 
                                                 (SELECT id_мероприятия FROM Заявка WHERE id_педагога = @TeacherId)";
                            SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                            selectCmd.Parameters.AddWithValue("@TeacherId", selectedTeacher.Id);
                            SqlDataReader reader = selectCmd.ExecuteReader();

                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string name = reader.GetString(1);
                                DateTime eventDate = reader.GetDateTime(2);
                                events.Add(new Event { Id = id, Name = name, EventDate = eventDate });
                            }

                            reader.Close();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка подключения к базе данных.");
                        }
                    }

                    comboEvent.ItemsSource = events;
                    comboEvent.DisplayMemberPath = "Name";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void AddSelectedEventToCombo()
        {
            try
            {
                if (MainWindow.element1 != null)
                {
                    int selectedEventId = Convert.ToInt32(MainWindow.element1);

                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            // Извлекаем информацию о выбранном мероприятии
                            string selectQuery = @"SELECT Мероприятия.id_мероприятия, Мероприятия.название, Мероприятия.дата_проведения
                       FROM Мероприятия
                       INNER JOIN Заявка ON Мероприятия.id_мероприятия = Заявка.id_мероприятия
                       WHERE Заявка.id_заявки = @SelectedEventId";
                            SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                            selectCmd.Parameters.AddWithValue("@SelectedEventId", selectedEventId);
                            SqlDataReader reader = selectCmd.ExecuteReader();

                            if (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string name = reader.GetString(1);
                                DateTime eventDate = reader.GetDateTime(2);
                                Event selectedEvent = new Event { Id = id, Name = name, EventDate = eventDate };

                                // Получаем текущий источник данных
                                List<Event> events = (List<Event>)comboEvent.ItemsSource;

                                // Добавляем выбранное мероприятие в список
                                events.Add(selectedEvent);

                                // Устанавливаем обновленный список в качестве нового источника данных
                                comboEvent.ItemsSource = events;

                                // Выбираем добавленное мероприятие
                                comboEvent.SelectedItem = selectedEvent;
                            }

                            reader.Close();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка подключения к базе данных.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
