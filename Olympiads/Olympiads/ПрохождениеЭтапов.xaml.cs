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
using System.Xml.Linq;

namespace Olympiads
{
    /// <summary>
    /// Логика взаимодействия для ПрохождениеЭтапов.xaml
    /// </summary>
    public partial class ПрохождениеЭтапов : Window
    {
        private MainWindow _main;

        public ПрохождениеЭтапов()
        {
            InitializeComponent();
            loadApplication();
            LoadStatusComboBox();
            if (MainWindow.changing == 1)
            {
                LoadAdditionalApplication();
                mainLabel.Content = "Изменение";
                mainButton.Content = "Сохранить";
                this.Title = "Изменение прохождения этапа";
                    comboApl.Text = MainWindow.element3.ToString();
                    comboStudent.Text = MainWindow.element2.ToString();
                txtBall.Text = MainWindow.element5.ToString();
                comboStatus.Text = MainWindow.element6.ToString();
            }
        }

        public ПрохождениеЭтапов(MainWindow main) : this()
        {
            _main = main;
        }

        private void mainButton_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtBall.Text) ||
            comboStudent.SelectedItem == null ||
            comboStatus.SelectedItem == null ||
            comboApl.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста заполните все поля!");
                return;
            }

            // Получение значений из полей ввода
            Student selectedStudent = (Student)comboStudent.SelectedItem;
            Application selectedApplication = (Application)comboApl.SelectedItem;
            string status = comboStatus.Text;
            float ball = float.Parse(txtBall.Text);

            if (MainWindow.changing == 0)
            {
                try
                {
                    // Определение id_этапа для выбранной заявки
                    int eventId;
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            // Получаем id_мероприятия для выбранной заявки
                            string selectEventIdQuery = "SELECT id_мероприятия FROM Заявка WHERE id_заявки = @applicationId";
                            SqlCommand selectEventIdCmd = new SqlCommand(selectEventIdQuery, dbConnection.GetConnection());
                            selectEventIdCmd.Parameters.AddWithValue("@applicationId", selectedApplication.ApplicationId);
                            int eventIdFromApplication = (int)selectEventIdCmd.ExecuteScalar();

                            // Теперь, используя id_мероприятия, получаем id_этапа
                            string selectEventQuery = "SELECT id_этапа FROM Мероприятия WHERE id_мероприятия = @eventIdFromApplication";
                            SqlCommand selectEventCmd = new SqlCommand(selectEventQuery, dbConnection.GetConnection());
                            selectEventCmd.Parameters.AddWithValue("@eventIdFromApplication", eventIdFromApplication);
                            eventId = (int)selectEventCmd.ExecuteScalar();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка подключения к базе данных.");
                            return;
                        }
                    }

                    // Установка даты прохождения на дату проведения мероприятия
                    DateTime eventDate;
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            string selectEventDateQuery = "SELECT дата_проведения FROM Мероприятия WHERE id_мероприятия = (SELECT id_мероприятия FROM Заявка WHERE id_заявки = @applicationId)";
                            SqlCommand selectEventDateCmd = new SqlCommand(selectEventDateQuery, dbConnection.GetConnection());
                            selectEventDateCmd.Parameters.AddWithValue("@applicationId", selectedApplication.ApplicationId);
                            object result = selectEventDateCmd.ExecuteScalar();

                            if (result != null && result != DBNull.Value)
                            {
                                eventDate = (DateTime)result;
                            }
                            else
                            {
                                MessageBox.Show("Не удалось найти мероприятие с указанным id.");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ошибка подключения к базе данных.");
                            return;
                        }
                    }

                    // Добавление записи в таблицу Прохождение_этапов
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            string insertQuery = "INSERT INTO Прохождение_этапов (id_участника, id_этапа, дата_прохождения, баллы, статус, id_заявки) VALUES (@studentId, @eventId, @eventDate, @ball, @status, @applicationId)";
                            SqlCommand insertCmd = new SqlCommand(insertQuery, dbConnection.GetConnection());
                            insertCmd.Parameters.AddWithValue("@studentId", selectedStudent.Id);
                            insertCmd.Parameters.AddWithValue("@eventId", eventId);
                            insertCmd.Parameters.AddWithValue("@eventDate", eventDate);
                            insertCmd.Parameters.AddWithValue("@ball", ball);
                            insertCmd.Parameters.AddWithValue("@status", status);
                            insertCmd.Parameters.AddWithValue("@applicationId", selectedApplication.ApplicationId);
                            insertCmd.ExecuteNonQuery();
                            MessageBox.Show("Запись успешно добавлена.");

                            // Очистка полей ввода
                            comboApl.SelectedIndex = -1;
                            comboStudent.SelectedIndex = -1;
                            comboStudent.ItemsSource = null;
                            comboStatus.SelectedIndex = -1;
                            txtBall.Text = ""; 

                            // Обновление основной формы (если необходимо)
                            _main.Refresh(sender, e);
                            loadApplication();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка подключения к базе данных.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    // Определение значений для обновления
                    float newBall = float.Parse(txtBall.Text);
                    string newStatus = comboStatus.Text;

                    // Получаем id_прохождения для обновления
                    int passingId = Convert.ToInt32(MainWindow.element1);

                    // Получаем id_участника из выбранного элемента comboBox
                    int studentId = ((Student)comboStudent.SelectedItem).Id;

                    // Получаем id_заявки из выбранного элемента comboBox
                    int applicationId = ((Application)comboApl.SelectedItem).ApplicationId;

                    // Подключение к базе данных и выполнение запроса на обновление
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            // SQL запрос на обновление записи
                            string updateQuery = @"
                    UPDATE Прохождение_этапов 
                    SET баллы = @newBall, 
                        статус = @newStatus,
                        id_участника = @studentId,
                        id_заявки = @applicationId
                    WHERE id_прохождения = @passingId";

                            // Создание и настройка команды SQL
                            SqlCommand updateCmd = new SqlCommand(updateQuery, dbConnection.GetConnection());
                            updateCmd.Parameters.AddWithValue("@newBall", newBall);
                            updateCmd.Parameters.AddWithValue("@newStatus", newStatus);
                            updateCmd.Parameters.AddWithValue("@studentId", studentId);
                            updateCmd.Parameters.AddWithValue("@applicationId", applicationId);
                            updateCmd.Parameters.AddWithValue("@passingId", passingId);

                            // Выполнение запроса на обновление
                            int rowsAffected = updateCmd.ExecuteNonQuery();

                            // Проверка успешности выполнения запроса
                            if (rowsAffected > 0)
                            {
                                // Обновление основной формы (если необходимо)
                                _main.Refresh(sender, e);
                                MessageBox.Show("Запись успешно изменена.");
                            }
                            else
                            {
                                MessageBox.Show("Не удалось изменить запись. Запись с указанным ID не найдена.");
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
                    MessageBox.Show("Ошибка: " + ex.Message);
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

        private void comboApl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboApl.SelectedItem != null)
            {
                Application selectedApplication = (Application)comboApl.SelectedItem;

                try
                {
                    List<Student> students = new List<Student>();
                    using (DatabaseConnection dbConnection = new DatabaseConnection())
                    {
                        if (dbConnection.OpenConnection())
                        {
                            // Получаем всех участников, связанных с выбранной заявкой
                            string selectStudentsQuery = @"
                        SELECT Участники.id_участника, Участники.фио
                        FROM Список
                        INNER JOIN Участники ON Список.id_участника = Участники.id_участника
                        WHERE Список.id_заявки = @applicationId";
                            SqlCommand selectStudentsCmd = new SqlCommand(selectStudentsQuery, dbConnection.GetConnection());
                            selectStudentsCmd.Parameters.AddWithValue("@applicationId", selectedApplication.ApplicationId);
                            SqlDataReader reader = selectStudentsCmd.ExecuteReader();

                            while (reader.Read())
                            {
                                int studentId = reader.GetInt32(0);
                                string fullName = reader.GetString(1);
                                students.Add(new Student { Id = studentId, FullName = fullName });
                            }

                            reader.Close();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка подключения к базе данных.");
                        }
                    }

                    // Привязываем список участников к ComboBox
                    comboStudent.ItemsSource = students;
                    comboStudent.DisplayMemberPath = "FullName"; // Отображаем только ФИО участников
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        public class Application
        {
            public int ApplicationId { get; set; }
            public string EventName { get; set; }
            public DateTime EventDate { get; set; } // Добавим свойство для даты проведения мероприятия

            public string DisplayText => $"№ заявки: {ApplicationId}\n{EventName}";
        }


        // Создайте список applications вне методов, чтобы он был доступен в обоих методах
        List<Application> applications = new List<Application>();

        private void loadApplication()
        {
            try
            {
                using (DatabaseConnection dbConnection = new DatabaseConnection())
                {
                    if (dbConnection.OpenConnection())
                    {
                        // Получаем все заявки и соответствующие им этапы
                        string selectApplicationsQuery = @"
                    SELECT Заявка.id_заявки, Мероприятия.название, Этапы.наименование
                    FROM Заявка
                    INNER JOIN Мероприятия ON Заявка.id_мероприятия = Мероприятия.id_мероприятия
                    INNER JOIN Этапы ON Мероприятия.id_этапа = Этапы.id_этапа
                    WHERE NOT EXISTS (
                        SELECT 1 FROM Прохождение_этапов
                        WHERE Прохождение_этапов.id_заявки = Заявка.id_заявки
                    )";
                        SqlCommand selectApplicationsCmd = new SqlCommand(selectApplicationsQuery, dbConnection.GetConnection());
                        SqlDataReader reader = selectApplicationsCmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int applicationId = reader.GetInt32(0);
                            string eventName = reader.GetString(1);
                            string stageName = reader.GetString(2);
                            applications.Add(new Application { ApplicationId = applicationId, EventName = $"{stageName}" });
                            //applications.Add(new Application { ApplicationId = applicationId, EventName = $"{stageName}: {eventName}" });
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

        private void LoadAdditionalApplication()
        {
            try
            {
                // Получаем id прохождения из MainWindow.element1
                int selectedProgressId = Convert.ToInt32(MainWindow.element1);

                using (DatabaseConnection dbConnection = new DatabaseConnection())
                {
                    if (dbConnection.OpenConnection())
                    {
                        // Получаем заявку, соответствующую выбранному id прохождения
                        string selectQuery = @"
                        SELECT Заявка.id_заявки, Этапы.наименование, Мероприятия.дата_проведения
                        FROM Прохождение_этапов
                        INNER JOIN Заявка ON Прохождение_этапов.id_заявки = Заявка.id_заявки
                        INNER JOIN Мероприятия ON Заявка.id_мероприятия = Мероприятия.id_мероприятия
                        INNER JOIN Этапы ON Мероприятия.id_этапа = Этапы.id_этапа
                        WHERE Прохождение_этапов.id_прохождения = @SelectedProgressId
                        ";
                        SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                        selectCmd.Parameters.AddWithValue("@SelectedProgressId", selectedProgressId);
                        SqlDataReader reader = selectCmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int applicationId = reader.GetInt32(0);
                            string eventName = reader.GetString(1);
                            DateTime eventDate = reader.GetDateTime(2);

                            // Создаем новый экземпляр Application и добавляем его в коллекцию applications
                            Application selectedApplication = new Application { ApplicationId = applicationId, EventName = eventName, EventDate = eventDate };
                            applications.Add(selectedApplication);
                        }

                        reader.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка подключения к базе данных.");
                    }
                }

                // Обновляем привязку данных для comboApl
                comboApl.ItemsSource = null;
                comboApl.ItemsSource = applications;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void LoadStatusComboBox()
        {
            // Очистка ComboBox перед добавлением новых элементов
            comboStatus.Items.Clear();

            // Добавление элементов "принят" и "не принят"
            comboStatus.Items.Add("Прошел");
            comboStatus.Items.Add("Не прошел");

            // Установка значения по умолчанию
            //comboStatus.SelectedIndex = 0;
        }


        private void txtName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string input = ((TextBox)sender).Text + e.Text;

            string pattern = @"^[0-9]+([0-9]*)?$";

            Regex regex = new Regex(pattern);
            e.Handled = !regex.IsMatch(input);
        }
    }
}
