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
    /// Логика взаимодействия для Организация.xaml
    /// </summary>
    public partial class Организация : Window
    {
        private MainWindow _main;
        
        public Организация()
        {
            InitializeComponent();
            comboObl.ItemsSource = new List<string> { "Брестская", "Витебская", "Гомельская", "Гродненская", "Минская", "Могилевская" };
            if (MainWindow.changing == 1)
            {
                mainLabel.Content = "Изменение";
                mainButton.Content = "Сохранить";
                this.Title = "Изменение данных Организации";
                txtName.Text = MainWindow.element2.ToString();
                comboObl.Text = MainWindow.element3.ToString();
                comboRegion.Text = MainWindow.element4.ToString();
                /*txtNameOrganization.Text = MainWindow.element5.ToString();
                txtTheme.Text = MainWindow.element6.ToString();
                txtNumberOfHours.Text = MainWindow.element7.ToString();
                txtFioDirector.Text = MainWindow.element8.ToString();
                txtCity.Text = MainWindow.element9.ToString();
                txtNumber.Text = MainWindow.element10.ToString();*/
            }
        }

        public Организация(MainWindow main) : this()
        {
                _main  = main;
        }


        //------------ДОБАВЛЕНИЕ ИЗМЕНЕНИЕ------------
        private void mainButton_Click(object sender, RoutedEventArgs e)
        {

            // Получение значений из полей ввода
            string name = txtName.Text;
            string oblast = comboObl.Text.ToString();
            string region = comboRegion.Text.ToString();

            // Проверка, что все поля заполнены
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(oblast) || string.IsNullOrWhiteSpace(region))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (MainWindow.changing == 0)
            {
                
                    // Создание строки запроса для проверки уникальности названия организации
                    string checkQuery = "SELECT COUNT(*) FROM Организация WHERE наименование = @name";

                // Создание соединения и команды для проверки
                using (DatabaseConnection dbConnection = new DatabaseConnection())
                {
                    SqlConnection connection = dbConnection.GetConnection();

                    try
                    {
                        // Открытие подключения
                        connection.Open();

                        // Создание и выполнение команды проверки
                        SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                        checkCommand.Parameters.AddWithValue("@name", name);
                        int existingCount = (int)checkCommand.ExecuteScalar();

                        // Проверка наличия организации с таким же названием
                        if (existingCount > 0)
                        {
                            MessageBox.Show("Организация с таким названием уже существует.");
                        }
                        else
                        {
                            // Создание строки запроса для вставки новой записи
                            string insertQuery = "INSERT INTO Организация (наименование, район, город) VALUES (@name, @oblast, @region)";

                            // Создание команды вставки
                            SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                            insertCommand.Parameters.AddWithValue("@name", name);
                            insertCommand.Parameters.AddWithValue("@oblast", oblast);
                            insertCommand.Parameters.AddWithValue("@region", region);

                            // Выполнение команды вставки
                            insertCommand.ExecuteNonQuery();

                            MessageBox.Show("Новая организация успешно добавлена.");
                            txtName.Text = "";
                            comboObl.Text = "";
                            comboRegion.Text = "";

                            // Обновление данных в таблице
                            _main.Refresh(sender, e);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка при выполнении запроса: " + ex.Message);
                    }
                    finally
                    {
                        // Закрытие подключения
                        connection.Close();
                    }
                }
            }
            else
            {
                // Создание строки запроса для обновления записи
                string updateQuery = "UPDATE Организация SET наименование = @name, район = @oblast, город = @region WHERE id_организации = @id";

                // Создание соединения и команды для обновления
                using (DatabaseConnection dbConnection = new DatabaseConnection())
                {
                    SqlConnection connection = dbConnection.GetConnection();

                    try
                    {
                        // Открытие подключения
                        connection.Open();

                        // Создание и выполнение команды обновления
                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@name", name);
                        updateCommand.Parameters.AddWithValue("@oblast", oblast);
                        updateCommand.Parameters.AddWithValue("@region", region);
                        updateCommand.Parameters.AddWithValue("@id", MainWindow.element1);
                        updateCommand.ExecuteNonQuery();

                        MessageBox.Show("Данные успешно обновлены.");

                        // Обновление данных в таблице
                        _main.Refresh(sender, e);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка при выполнении запроса: " + ex.Message);
                    }
                    finally
                    {
                        // Закрытие подключения
                        connection.Close();
                    }
                }
            }
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow.changing = 0;
        }

        private void comboObl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получить выбранную область
            string selectedObl = (string)comboObl.SelectedItem;

            // Заполнить второй ComboBox в зависимости от выбранной области
            switch (selectedObl)
            {
                case "Брестская":
                    comboRegion.ItemsSource = new List<string> { "Барановичский", "Берёзовский", "Брестский", "Ганцевичский", "Дрогичинский", "Жабинковский", "Ивановский", "Ивацевичский", "Каменецкий", "Кобринский", "Лунинецкий", "Ляховичский", "Малоритский", "Пинский", "Пружанский", "Столинский" };
                    break;
                case "Гродненская":
                    comboRegion.ItemsSource = new List<string> { "Берестовицкий", "Волковысский", "Вороновский", "Гродненский", "Дятловский", "Зельвенский", "Ивьевский", "Кореличский", "Лидский", "Мостовский", "Новогрудский", "Островецкий", "Ошмянский", "Свислочский", "Слонимский", "Сморгонский", "Щучинский" };
                    break;
                case "Гомельская":
                    comboRegion.ItemsSource = new List<string> { "Брагинский", "Буда-Кошелевский", "Ветковский", "Гомельский", "Добрушский", "Ельский", "Житковичский", "Жлобинский", "Калинковичский", "Кормянский", "Лельчицкий", "Лоевский", "Мозырский", "Наровлянский", "Октябрьский", "Петриковский", "Речицкий", "Рогачевский", "Светлогорский", "Хойникский", "Чечерский" };
                    break;
                case "Витебская":
                    comboRegion.ItemsSource = new List<string> { "Бешенковичский", "Браславский", "Верхнедвинский", "Витебский", "Глубокский", "Городокский", "Докшицкий", "Дубровенский", "Лепельский", "Лиозненский", "Миорский", "Оршанский", "Полоцкий", "Поставский", "Россонский", "Сенненский", "Толочинский", "Ушачский", "Чашникский", "Шарковщинский", "Шумилинский" };
                    break;
                case "Минская":
                    comboRegion.ItemsSource = new List<string> { "Березинский", "Борисовский", "Вилейский", "Воложинский", "Дзержинский", "Клецкий", "Копыльский", "Крупский", "Логойский", "Любанский", "Минский", "Молодечненский", "Мядельский", "Несвижский", "Пуховичский", "Слуцкий", "Смолевичский", "Солигорский", "Стародорожский", "Столбцовский", "Узденский", "Червенский" };
                    break;
                case "Могилевская":
                    comboRegion.ItemsSource = new List<string> { "Белыничский", "Бобруйский", "Быховский", "Глусский", "Горецкий", "Дрибинский", "Кировский", "Климовичский", "Кличевский", "Костюковичский", "Краснопольский", "Кричевский", "Круглянский", "Могилевский", "Мстиславский", "Осиповичский", "Славгородский", "Хотимский", "Чаусский", "Чериковский", "Шкловский" };
                    break;
                default:
                    break;
            }
        }
        //------------КОНЕЦ ДОБАВЛЕНИЕ ИЗМЕНЕНИЕ------------
    }
}
