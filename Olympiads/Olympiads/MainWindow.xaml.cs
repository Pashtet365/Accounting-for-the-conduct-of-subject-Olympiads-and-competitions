using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Data;
using System.Windows.Controls.Primitives;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using Microsoft.Office.Interop.Word;
using Table = Microsoft.Office.Interop.Word.Table;

namespace Olympiads
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {

        public int tableIndex;

        //глобальные переменные
        public static int changing = 0;
        public static object element1;
        public static object element2;
        public static object element3;
        public static object element4;
        public static object element5;
        public static object element6;
        public static object element7;
        public static object element8;
        public static object element9;
        public static object element10;

        public System.Windows.Controls.DataGrid pavlyuchkov;

        private List<UIElement> comboBoxElements;

        private List<UIElement> documentElements;

        public MainWindow()
        {
            InitializeComponent();

            //collection for filtration
            comboBoxElements = new List<UIElement>();
            foreach (UIElement element in menuFilter.Items)
            {
                comboBoxElements.Add(element);
            }
            menuFilter.Items.Clear();

            documentElements = new List<UIElement>();
            foreach (UIElement element in documentsMenu.Items)
            {
                documentElements.Add(element);
            }
            documentsMenu.Items.Clear();

            documentsMenu.Visibility = Visibility.Collapsed;
        }

        //----------------ОТКРЫТИЕ ТАБЛИЦ----------------

        private void LoadData(DatabaseConnection dbConnection, string query)
        {
            try
            {
                SqlConnection connection = dbConnection.GetConnection();
                if (dbConnection.OpenConnection())
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);
                    dataGridForm.ItemsSource = dataTable.DefaultView;
                    dbConnection.CloseConnection();
                }
                else
                {
                    MessageBox.Show("Failed to open connection.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        //организации 1
        private void menuTableOrganizers_Click(object sender, RoutedEventArgs e)
        {
            menuFilter.Items.Clear();
            menuFilter.Items.Add(comboBoxElements[0]);
            menuFilter.Items.Add(comboBoxElements[7]);

            documentsMenu.Visibility = Visibility.Collapsed;
            documentsMenu.Items.Clear();
            DatabaseConnection dbConnection = new DatabaseConnection();
            string query = "SELECT id_организации AS ID, наименование AS Наименование, район AS Область, город AS Район FROM Организация";
            LoadData(dbConnection, query);
            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
            tableIndex = 1;
        }

        //педагоги 2
        private void menuTableTeachers_Click(object sender, RoutedEventArgs e)
        {
            menuFilter.Items.Clear();
            menuFilter.Items.Add(comboBoxElements[0]);
            menuFilter.Items.Add(comboBoxElements[7]);

            documentsMenu.Visibility = Visibility.Collapsed;
            documentsMenu.Items.Clear();
            DatabaseConnection dbConnection = new DatabaseConnection();
            string query = "SELECT p.id_педагога AS ID, p.фио AS ФИО, " +
                "p.должность AS Должность, o.наименование AS [Наименование организации] " +
                           "FROM Педагоги p " +
                           "JOIN Организация o ON p.id_организации = o.id_организации";
            LoadData(dbConnection, query);
            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
            tableIndex = 2;
        }

        //участники 3
        private void menuTableParticipants_Click(object sender, RoutedEventArgs e)
        {
            menuFilter.Items.Clear();
            menuFilter.Items.Add(comboBoxElements[0]);
            menuFilter.Items.Add(comboBoxElements[7]);

            documentsMenu.Visibility = Visibility.Visible;
            documentsMenu.Items.Clear();
            documentsMenu.Items.Add(documentElements[0]);
            DatabaseConnection dbConnection = new DatabaseConnection();
            string query = "SELECT u.id_участника AS ID, u.фио AS ФИО, u.класс AS Класс, o.наименование AS [Наименование организации] " +
                           "FROM Участники u " +
                           "JOIN Организация o ON u.id_организации = o.id_организации";
            LoadData(dbConnection, query);
            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
            tableIndex = 3;
        }

        //предметы 4
        private void menuTableSubjects_Click(object sender, RoutedEventArgs e)
        {
            menuFilter.Items.Clear();
            menuFilter.Items.Add(comboBoxElements[0]);
            menuFilter.Items.Add(comboBoxElements[7]);

            documentsMenu.Visibility = Visibility.Collapsed;
            documentsMenu.Items.Clear();
            DatabaseConnection dbConnection = new DatabaseConnection();
            string query = "SELECT id_предмета AS ID, наименование AS Наименование FROM Предметы";
            LoadData(dbConnection, query);
            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
            tableIndex = 4;
        }

        //этапы 5
        private void menuTableStages_Click(object sender, RoutedEventArgs e)
        {
            menuFilter.Items.Clear();
            menuFilter.Items.Add(comboBoxElements[0]);
            menuFilter.Items.Add(comboBoxElements[7]);

            documentsMenu.Visibility = Visibility.Collapsed;
            documentsMenu.Items.Clear();
            DatabaseConnection dbConnection = new DatabaseConnection();
            string query = "SELECT id_этапа AS ID, наименование AS Наименование FROM Этапы";
            LoadData(dbConnection, query);
            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
            tableIndex = 5;
        }

        //мероприятия 6
        private void menuTableEvents_Click(object sender, RoutedEventArgs e)
        {
            menuFilter.Items.Clear();
            menuFilter.Items.Add(comboBoxElements[0]);
            menuFilter.Items.Add(comboBoxElements[1]);
            menuFilter.Items.Add(comboBoxElements[2]);
            menuFilter.Items.Add(comboBoxElements[7]);

            documentsMenu.Visibility = Visibility.Visible;
            documentsMenu.Items.Clear();
            documentsMenu.Items.Add(documentElements[1]);
            documentsMenu.Items.Add(documentElements[4]);
            documentsMenu.Items.Add(documentElements[2]);
            DatabaseConnection dbConnection = new DatabaseConnection();
            string query = "SELECT e.id_мероприятия AS ID, e.название AS Название, p.наименование AS [Название предмета], " +
                           "FORMAT(e.дата_проведения, 'dd.MM.yyyy') AS [Дата проведения], s.наименование AS [Наименование этапа] " +
                           "FROM Мероприятия e " +
                           "JOIN Предметы p ON e.id_предмета = p.id_предмета " +
                           "JOIN Этапы s ON e.id_этапа = s.id_этапа";
            LoadData(dbConnection, query);
            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
            tableIndex = 6;
        }

        //заявки 7
        private void menuTableApplications_Click(object sender, RoutedEventArgs e)
        {
            menuFilter.Items.Clear();
            menuFilter.Items.Add(comboBoxElements[0]);
            menuFilter.Items.Add(comboBoxElements[3]);
            menuFilter.Items.Add(comboBoxElements[4]);
            menuFilter.Items.Add(comboBoxElements[7]);

            documentsMenu.Visibility = Visibility.Visible;
            documentsMenu.Items.Clear();
            documentsMenu.Items.Add(documentElements[3]);
            DatabaseConnection dbConnection = new DatabaseConnection();
            string query = "SELECT z.id_заявки AS [Номер заявки], p.фио AS [ФИО преподавателя], o.наименование AS [Название организации], m.название AS [Название мероприятия], FORMAT(z.дата_подачи, 'dd.MM.yyyy') AS [Дата подачи] " +
                           "FROM Заявка z " +
                           "JOIN Педагоги p ON z.id_педагога = p.id_педагога " +
                           "JOIN Организация o ON z.id_организации = o.id_организации " +
                           "JOIN Мероприятия m ON z.id_мероприятия = m.id_мероприятия";
            LoadData(dbConnection, query);
            //dataGridForm.Columns[0].Visibility = Visibility.Hidden;
            tableIndex = 7;
        }

        //прохождение этапов 8
        private void menuTableProgress_Click(object sender, RoutedEventArgs e)
        {
            menuFilter.Items.Clear();
            menuFilter.Items.Add(comboBoxElements[0]);
            menuFilter.Items.Add(comboBoxElements[5]);
            menuFilter.Items.Add(comboBoxElements[6]);
            menuFilter.Items.Add(comboBoxElements[7]);

            documentsMenu.Visibility = Visibility.Collapsed;
            documentsMenu.Items.Clear();
            DatabaseConnection dbConnection = new DatabaseConnection();
            string query = "SELECT Прохождение_этапов.id_прохождения AS ID, Участники.фио AS [ФИО участника], N'№ заявки: ' + CAST(Прохождение_этапов.id_заявки AS NVARCHAR(10)) + '\n' + Этапы.наименование AS [Заявка], FORMAT(Прохождение_этапов.дата_прохождения, 'dd.MM.yyyy') AS [Дата прохождения], Прохождение_этапов.баллы AS Баллы, Прохождение_этапов.статус AS Статус " +
                           "FROM Прохождение_этапов " +
                           "INNER JOIN Участники ON Прохождение_этапов.id_участника = Участники.id_участника " +
                           "INNER JOIN Этапы ON Прохождение_этапов.id_этапа = Этапы.id_этапа";
            LoadData(dbConnection, query);
            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
            tableIndex = 8;
        }

        //списки участников 9
        private void menuTableList_Click(object sender, RoutedEventArgs e)
        {
            menuFilter.Items.Clear();
            menuFilter.Items.Add(comboBoxElements[0]);
            menuFilter.Items.Add(comboBoxElements[7]);

            documentsMenu.Visibility = Visibility.Collapsed;
            documentsMenu.Items.Clear();
            DatabaseConnection dbConnection = new DatabaseConnection();
            string query = "SELECT Список.id_списка AS ID, Участники.фио AS [ФИО участника], N'№ заявки: ' + CAST(Заявка.id_заявки AS NVARCHAR(10)) + '\n' + Мероприятия.название AS Заявка " +
                           "FROM Список " +
                           "INNER JOIN Участники ON Список.id_участника = Участники.id_участника " +
                           "INNER JOIN Заявка ON Список.id_заявки = Заявка.id_заявки " +
                           "INNER JOIN Мероприятия ON Заявка.id_мероприятия = Мероприятия.id_мероприятия";
            LoadData(dbConnection, query);
            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
            tableIndex = 9;
        }
        //----------------КОНЕЦ ОТКРЫТИЕ ТАБЛИЦ----------------



        //----------------РЕДАКТИРОВАНИЕ----------------

        //----------------ДОБАВЛЕНИЕ----------------
        private void menuTableAddedRow_Click(object sender, RoutedEventArgs e)
        {
            switch (tableIndex)
            {
                case 1:
                    // Открыть новую форму Организации
                    Организация orgForm = new Организация(this);
                    orgForm.Owner = this;
                    orgForm.ShowDialog();
                    break;
                case 2:
                    // Открыть новую форму Педагоги
                    Педагоги teacherForm = new Педагоги(this);
                    teacherForm.Owner = this;
                    teacherForm.ShowDialog();
                    break;
                case 3:
                    // Открыть новую форму Участники
                    Участники participantForm = new Участники(this);
                    participantForm.Owner = this;
                    participantForm.ShowDialog();
                    break;
                case 4:
                    // Открыть новую форму Предметы
                    Предметы subjectForm = new Предметы(this);
                    subjectForm.Owner = this;
                    subjectForm.ShowDialog();
                    break;
                case 5:
                    // Открыть новую форму Этапы
                    Этапы stageForm = new Этапы(this);
                    stageForm.Owner = this;
                    stageForm.ShowDialog();
                    break;
                case 6:
                    // Открыть новую форму Мероприятия
                    Мероприятия eventForm = new Мероприятия(this);
                    eventForm.Owner = this;
                    eventForm.ShowDialog();
                    break;
                case 7:
                    // Открыть новую форму Заявки
                    Заявки applicationForm = new Заявки(this);
                    applicationForm.Owner = this;
                    applicationForm.ShowDialog();
                    break;
                case 8:
                    // Открыть новую форму Прохождение этапов
                    ПрохождениеЭтапов progressForm = new ПрохождениеЭтапов(this);
                    progressForm.Owner = this;
                    progressForm.ShowDialog();
                    break;
                case 9:
                    // Открыть новую форму Списки участников
                    Списки listForm = new Списки(this);
                    listForm.Owner = this;
                    listForm.ShowDialog();
                    break;
                default:
                    MessageBox.Show("Выберите таблицу!");
                    break;
            }
        }

        //----------------КОНЕЦ ДОБАВЛЕНИЯ----------------


        //----------------ИЗМЕНЕНИЕ----------------
        private void menuTableChanging_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGridForm.SelectedItem;

            if (selectedRow == null)
            {
                MessageBox.Show("Выберите строку!");
                return;
            }

            switch (tableIndex)
            {
                case 1:
                    element1 = selectedRow["ID"].ToString();
                    element2 = selectedRow["Наименование"].ToString();
                    element3 = selectedRow["Область"].ToString();
                    element4 = selectedRow["Район"].ToString();
                    changing = 1;
                    // Открыть новую форму Организации
                    Организация orgForm = new Организация(this);
                    orgForm.Owner = this;
                    orgForm.ShowDialog();
                    break;
                case 2:
                    element1 = selectedRow["ID"].ToString();
                    element2 = selectedRow["ФИО"].ToString();
                    element3 = selectedRow["Должность"].ToString();
                    element4 = selectedRow["Наименование организации"].ToString();
                    changing = 1;
                    // Открыть новую форму Педагоги
                    Педагоги teacherForm = new Педагоги(this);
                    teacherForm.Owner = this;
                    teacherForm.ShowDialog();
                    break;
                case 3:
                    element1 = selectedRow["ID"].ToString();
                    element2 = selectedRow["ФИО"].ToString();
                    element3 = selectedRow["Класс"].ToString();
                    element4 = selectedRow["Наименование организации"].ToString();
                    changing = 1;
                    // Открыть новую форму Участники
                    Участники participantForm = new Участники(this);
                    participantForm.Owner = this;
                    participantForm.ShowDialog();
                    break;
                case 4:
                    element1 = selectedRow["ID"].ToString();
                    element2 = selectedRow["Наименование"].ToString();
                    changing = 1;
                    // Открыть новую форму Предметы
                    Предметы subjectForm = new Предметы(this);
                    subjectForm.Owner = this;
                    subjectForm.ShowDialog();
                    break;
                case 5:
                    element1 = selectedRow["ID"].ToString();
                    element2 = selectedRow["Наименование"].ToString();
                    changing = 1;
                    // Открыть новую форму Этапы
                    Этапы stageForm = new Этапы(this);
                    stageForm.Owner = this;
                    stageForm.ShowDialog();
                    break;
                case 6:
                    element1 = selectedRow["ID"].ToString();
                    element2 = selectedRow["Название"].ToString();
                    element3 = selectedRow["Название предмета"].ToString();
                    element4 = selectedRow["Дата проведения"].ToString();
                    element5 = selectedRow["Наименование этапа"].ToString();
                    changing = 1;
                    // Открыть новую форму Мероприятия
                    Мероприятия eventForm = new Мероприятия(this);
                    eventForm.Owner = this;
                    eventForm.ShowDialog();
                    break;
                case 7:
                    // Передача данных из выбранной строки таблицы "Заявки" в переменные класса MainWindow
                    element1 = selectedRow["Номер заявки"].ToString();
                    element2 = selectedRow["ФИО преподавателя"].ToString();
                    element3 = selectedRow["Название организации"].ToString();
                    element4 = selectedRow["Название мероприятия"].ToString();
                    element5 = selectedRow["Дата подачи"].ToString();
                    // Установка значения changing для указания на редактирование
                    changing = 1;
                    // Открытие новой формы Заявки
                    Заявки applicationForm = new Заявки(this);
                    applicationForm.Owner = this;
                    applicationForm.ShowDialog();
                    break;
                case 8:
                    // Передача данных из выбранной строки таблицы "Прохождение этапов" в переменные класса MainWindow
                    element1 = selectedRow["ID"].ToString();
                    element2 = selectedRow["ФИО участника"].ToString();
                    element3 = selectedRow["Заявка"].ToString();
                    element4 = selectedRow["Дата прохождения"].ToString();
                    element5 = selectedRow["Баллы"].ToString();
                    element6 = selectedRow["Статус"].ToString();
                    changing = 1;
                    // Открытие новой формы Прохождение этапов
                    ПрохождениеЭтапов progressForm = new ПрохождениеЭтапов(this);
                    progressForm.Owner = this;
                    progressForm.ShowDialog();
                    break;
                case 9:
                    // Передача данных из выбранной строки таблицы "Списки участников" в переменные класса MainWindow
                    element1 = selectedRow["ID"].ToString();
                    element2 = selectedRow["ФИО участника"].ToString();
                    element3 = selectedRow["Заявка"].ToString();
                    changing = 1;
                    // Открытие новой формы Списки участников
                    Списки listForm = new Списки(this);
                    listForm.Owner = this;
                    listForm.ShowDialog();
                    break;
                default:
                    MessageBox.Show("Выберите таблицу!");
                    break;
            }
        }

        //----------------КОНЕЦ ИЗМЕНЕНИЯ----------------


        //----------------ОБНОВЛЕНИЕ----------------
        public void menuTableRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh(sender, e);
        }

        public void Refresh(object sender, RoutedEventArgs e)
        {
            switch (tableIndex)
            {
                case 1:
                    menuTableOrganizers_Click(sender, e);
                    break;
                case 2:
                    menuTableTeachers_Click(sender, e);
                    break;
                case 3:
                    menuTableParticipants_Click(sender, e);
                    break;
                case 4:
                    menuTableSubjects_Click(sender, e);
                    break;
                case 5:
                    menuTableStages_Click(sender, e);
                    break;
                case 6:
                    menuTableEvents_Click(sender, e);
                    break;
                case 7:
                    menuTableApplications_Click(sender, e);
                    break;
                case 8:
                    menuTableProgress_Click(sender, e);
                    break;
                case 9:
                    menuTableList_Click(sender, e);
                    break;
                default:
                    MessageBox.Show("Выберите таблицу!");
                    break;
            }
        }
        //----------------КОНЕЦ ОБНОВЛЕНИЯ----------------



        //----------------УДАЛЕНИЕ----------------
        private void menuTableDelete_Click(object sender, RoutedEventArgs e)
        {
            // Получение выбранной строки из DataGrid
            DataRowView selectedRow = (DataRowView)dataGridForm.SelectedItem;

            // Проверка, что строка действительно выбрана
            if (selectedRow != null)
            {
                try
                {
                    // Получение идентификатора из первой колонки (предполагается, что идентификатор находится в первой колонке)
                    int idToDelete = Convert.ToInt32(selectedRow[0]);

                    // Выполнение операции удаления в базе данных в зависимости от выбранной таблицы
                    DatabaseConnection dbConnection = new DatabaseConnection();
                    SqlConnection connection = dbConnection.GetConnection();
                    if (dbConnection.OpenConnection())
                    {
                        SqlCommand command = null;
                        switch (tableIndex)
                        {
                            case 1:
                                command = new SqlCommand("DELETE FROM Организация WHERE id_организации = @ID", connection);
                                break;
                            case 2:
                                command = new SqlCommand("DELETE FROM Педагоги WHERE id_педагога = @ID", connection);
                                break;
                            case 3:
                                command = new SqlCommand("DELETE FROM Участники WHERE id_участника = @ID", connection);
                                break;
                            case 4:
                                command = new SqlCommand("DELETE FROM Предметы WHERE id_предмета = @ID", connection);
                                break;
                            case 5:
                                command = new SqlCommand("DELETE FROM Этапы WHERE id_этапа = @ID", connection);
                                break;
                            case 6:
                                command = new SqlCommand("DELETE FROM Мероприятия WHERE id_мероприятия = @ID", connection);
                                break;
                            case 7:
                                command = new SqlCommand("DELETE FROM Заявка WHERE id_заявки = @ID", connection);
                                break;
                            case 8:
                                command = new SqlCommand("DELETE FROM Прохождение_этапов WHERE id_прохождения = @ID", connection);
                                break;
                            case 9:
                                command = new SqlCommand("DELETE FROM Список WHERE id_списка = @ID", connection);
                                break;
                            default:
                                MessageBox.Show("Выберите таблицу!");
                                return; // Прекращаем выполнение метода, так как нет команды для удаления
                        }

                        // Установка параметра и выполнение команды удаления
                        if (command != null)
                        {
                            command.Parameters.AddWithValue("@ID", idToDelete);
                            command.ExecuteNonQuery();
                        }

                        // Обновление DataGrid после удаления
                        Refresh(sender, e);

                        dbConnection.CloseConnection();
                    }
                    else
                    {
                        MessageBox.Show("Failed to open connection.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Данная запись связана с другой таблицей!");
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления!");
            }
        }
        //----------------КОНЕЦ УДАЛЕНИЯ----------------



        //----------------ВЫВОД В EXCEL----------------
        private void printExsel_Click(object sender, RoutedEventArgs e)
        {
            if (tableIndex == 0)
            {
                MessageBox.Show("Выберите таблицу!");
                return;
            }

            string nameTable = string.Empty;

            // Устанавливаем имя таблицы в зависимости от выбора пользователя
            switch (tableIndex)
            {
                case 1:
                    nameTable = "Организации";
                    break;
                case 2:
                    nameTable = "Педагоги";
                    break;
                case 3:
                    nameTable = "Участники";
                    break;
                case 4:
                    nameTable = "Предметы";
                    break;
                case 5:
                    nameTable = "Этапы";
                    break;
                case 6:
                    nameTable = "Мероприятия";
                    break;
                case 7:
                    nameTable = "Заявки";
                    break;
                case 8:
                    nameTable = "Прохождение этапов";
                    break;
                case 9:
                    nameTable = "Списки участников";
                    break;
            }

            // Создание объекта SaveFileDialog
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Файлы Excel (*.xlsx)|*.xlsx";
            saveFileDialog.Title = "Сохранить как Excel";
            saveFileDialog.DefaultExt = "xlsx";

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Получение выбранного пользователем пути и имени файла
                string filePath = saveFileDialog.FileName;

                // Создание нового объекта приложения Excel
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = false; // Скрываем Excel

                // Создание новой книги Excel
                Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Add(Type.Missing);

                // Создание нового листа Excel
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Sheets[1];

                // Заполнение листа данными из вашего DataGrid
                for (int i = 0; i < dataGridForm.Items.Count; i++)
                {
                    var dataGridRow = (DataGridRow)dataGridForm.ItemContainerGenerator.ContainerFromIndex(i);
                    if (dataGridRow != null)
                    {
                        for (int j = 0; j < dataGridForm.Columns.Count; j++)
                        {
                            var content = dataGridForm.Columns[j].GetCellContent(dataGridRow);
                            if (content is TextBlock)
                            {
                                var text = (content as TextBlock).Text;
                                excelSheet.Cells[i + 2, j + 1] = text; // Начинаем с второй строки
                            }
                        }
                    }
                }

                // Удаление столбца A
                Microsoft.Office.Interop.Excel.Range columnA = (Microsoft.Office.Interop.Excel.Range)excelSheet.Columns["A"];
                columnA.Delete();

                // Объединение ячеек в первой строке
                Microsoft.Office.Interop.Excel.Range headerRange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[1, dataGridForm.Columns.Count - 1]];
                headerRange.Merge();

                // Установка текста в объединенной ячейке
                excelSheet.Cells[1, 1] = nameTable;

                // Выравнивание текста по центру и установка жирного шрифта для первой строки
                headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                headerRange.Font.Bold = true;

                // Добавление обводки для всей таблицы Excel
                Microsoft.Office.Interop.Excel.Range tableRange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[dataGridForm.Items.Count + 1, dataGridForm.Columns.Count - 1]];
                tableRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                tableRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                // Выравнивание ширины столбцов
                excelSheet.UsedRange.Columns.AutoFit();

                // Сохранение книги Excel по выбранному пути
                excelBook.SaveAs(filePath);

                // Закрытие книги и приложения Excel
                excelBook.Close();
                excelApp.Quit();

                // Освобождение ресурсов COM
                Marshal.ReleaseComObject(excelSheet);
                Marshal.ReleaseComObject(excelBook);
                Marshal.ReleaseComObject(excelApp);
            }
        }

        //----------------КОНЕЦ ВЫВОДА В EXCEL----------------


        //----------------КОНЕЦ РЕДАКТИРОВАНИЕ----------------



        //----------------ДОКУМЕНТЫ----------------

        //данные участника
        private void ParticipantData_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridForm.SelectedItem == null)
            {
                MessageBox.Show("Выберите участника!");
                return;
            }

            DataRowView selectedRow = (DataRowView)dataGridForm.SelectedItem;
            string id = selectedRow["ID"].ToString();
            string fio = selectedRow["ФИО"].ToString();
            string clasStunedt = selectedRow["Класс"].ToString();
            string nameOrg = selectedRow["Наименование организации"].ToString();

            //laptop
            //string templateFilePath = @"C:\GitHub\OpenAccess\GGAEK\4course\БД\КурсоваяБД\WordPaper\ИсходныеДокументы\StudentsDate.xlsx";

            //computer
            string templateFilePath = @"G:\Files\ЗаконченыеПроекты\3КУРС\4course\БД\КурсоваяБД\WordPaper\ИсходныеДокументы\StudentsDate.xlsx";

            // Создание диалогового окна "Сохранить файл"
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Файлы Excel (*.xlsx)|*.xlsx";
            saveFileDialog.Title = "Сохранить Excel-документ как";
            saveFileDialog.DefaultExt = "xlsx";

            if (saveFileDialog.ShowDialog() == true)
            {
                string saveFilePath = saveFileDialog.FileName;

                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = false;

                Excel.Workbook templateWorkbook = excelApp.Workbooks.Open(templateFilePath);
                Excel.Worksheet worksheet = (Excel.Worksheet)templateWorkbook.Sheets[1]; // Лист с данными участника

                // Замена данных участника в шаблоне
                worksheet.Range["B1"].Value = fio;
                worksheet.Range["B2"].Value = clasStunedt;
                worksheet.Range["B3"].Value = nameOrg;

                // Получение данных о мероприятиях участника и заполнение соответствующих ячеек
                using (DatabaseConnection dbConnection = new DatabaseConnection())
                {
                    if (dbConnection.OpenConnection())
                    {
                        string selectQuery = @"
                    SELECT 
                    Мероприятия.название, 
                    Прохождение_этапов.баллы, 
                    Прохождение_этапов.статус 
                    FROM 
                    Прохождение_этапов 
                    INNER JOIN Заявка ON Прохождение_этапов.id_заявки = Заявка.id_заявки
                    INNER JOIN Мероприятия ON Заявка.id_мероприятия = Мероприятия.id_мероприятия
                    WHERE 
                    Прохождение_этапов.id_участника = @participantId
                    ";
                        SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                        selectCmd.Parameters.AddWithValue("@participantId", id);

                        SqlDataReader reader = selectCmd.ExecuteReader();

                        int currentRow = 6; // Начинаем с 6 строки

                        while (reader.Read())
                        {
                            string eventName = reader.GetString(0);
                            double points = reader.GetDouble(1);
                            string status = reader.GetString(2);

                            worksheet.Cells[currentRow, 1] = eventName;
                            worksheet.Cells[currentRow, 2] = points;
                            worksheet.Cells[currentRow, 3] = status;

                            currentRow++; // Переход к следующей строке
                        }

                        reader.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка подключения к базе данных.");
                    }
                }

                // Автоподбор ширины столбцов после заполнения данных
                Excel.Range usedRange = worksheet.UsedRange;
                usedRange.Columns.AutoFit();

                // Применение границ к данным
                ApplyBordersToAllWorksheets(templateWorkbook);

                // Сохранение документа Excel
                templateWorkbook.SaveAs(saveFilePath);

                // Закрытие и освобождение ресурсов
                templateWorkbook.Close(false);
                Marshal.ReleaseComObject(templateWorkbook);
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);

                MessageBox.Show("Данные успешно сохранены в Excel.");
            }
        }


        //результаты конкурса
        private void ConcursResult_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridForm.SelectedItem == null)
            {
                MessageBox.Show("Выберите мероприятие!");
                return;
            }

            DataRowView selectedRow = (DataRowView)dataGridForm.SelectedItem;
            string id = selectedRow["ID"].ToString();
            //laptop
            //string templateFilePath = @"C:\GitHub\OpenAccess\GGAEK\4course\БД\КурсоваяБД\WordPaper\ИсходныеДокументы\ConcursResult.docx";
            //computer
            string templateFilePath = @"G:\Files\ЗаконченыеПроекты\3КУРС\4course\БД\КурсоваяБД\WordPaper\ИсходныеДокументы\ConcursResult.docx";

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Документ Word (*.docx)|*.docx";
            saveFileDialog.Title = "Сохранить документ Word как";
            saveFileDialog.DefaultExt = "docx";

            if (saveFileDialog.ShowDialog() == true)
            {
                string saveFilePath = saveFileDialog.FileName;

                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Document doc = wordApp.Documents.Open(templateFilePath);
                doc.Activate();

                // Замена данных в документе
                FindAndReplace(wordApp, "[наименованиеМероприятия]", GetConcursName(id));
                FindAndReplace(wordApp, "[датаПроведения]", GetConcursDate(id));
                FindAndReplace(wordApp, "[организации]", GetOrganizationsList(id));
                FindAndReplace(wordApp, "[участники]", GetParticipantsList(id));

                doc.SaveAs2(saveFilePath);
                doc.Close();
                wordApp.Quit();

                Marshal.ReleaseComObject(doc);
                Marshal.ReleaseComObject(wordApp);

                MessageBox.Show("Данные успешно сохранены в документ Word.");
            }
        }

        private void FindAndReplace(Microsoft.Office.Interop.Word.Application wordApp, object findText, object replaceText)
        {
            foreach (Range range in wordApp.ActiveDocument.StoryRanges)
            {
                range.Find.ClearFormatting();
                range.Find.Execute(FindText: findText, ReplaceWith: replaceText);
            }
        }

        private string GetConcursName(string concursId)
        {
            string concursName = string.Empty;

            using (DatabaseConnection dbConnection = new DatabaseConnection())
            {
                if (dbConnection.OpenConnection())
                {
                    string selectQuery = "SELECT название FROM Мероприятия WHERE id_мероприятия = @concursId";
                    SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                    selectCmd.Parameters.AddWithValue("@concursId", concursId);

                    object result = selectCmd.ExecuteScalar();
                    if (result != null)
                    {
                        concursName = result.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка подключения к базе данных.");
                }
            }

            return concursName;
        }

        private string GetConcursDate(string concursId)
        {
            string concursDateString = string.Empty;

            using (DatabaseConnection dbConnection = new DatabaseConnection())
            {
                if (dbConnection.OpenConnection())
                {
                    string selectQuery = "SELECT дата_проведения FROM Мероприятия WHERE id_мероприятия = @concursId";
                    SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                    selectCmd.Parameters.AddWithValue("@concursId", concursId);

                    object result = selectCmd.ExecuteScalar();
                    if (result != null)
                    {
                        // Преобразуйте строку в DateTime
                        if (DateTime.TryParse(result.ToString(), out DateTime concursDate))
                        {
                            // Если преобразование прошло успешно, используйте ToShortDateString() для получения строки даты
                            concursDateString = concursDate.ToShortDateString();
                        }
                        else
                        {
                            // Обработка случаев, когда строка не может быть преобразована в DateTime
                            MessageBox.Show("Ошибка при преобразовании даты.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка подключения к базе данных.");
                }
            }

            return concursDateString;
        }

        private string GetOrganizationsList(string concursId)
        {
            string organizationsList = string.Empty;

            using (DatabaseConnection dbConnection = new DatabaseConnection())
            {
                if (dbConnection.OpenConnection())
                {
                    string selectQuery = @"
                SELECT 
                    Организация.наименование 
                FROM 
                    Заявка 
                    INNER JOIN Организация ON Заявка.id_организации = Организация.id_организации 
                WHERE 
                    Заявка.id_мероприятия = @concursId";
                    SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                    selectCmd.Parameters.AddWithValue("@concursId", concursId);

                    using (SqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            organizationsList += reader["наименование"].ToString() + ", ";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка подключения к базе данных.");
                }
            }

            // Удаление последней запятой
            if (!string.IsNullOrEmpty(organizationsList))
            {
                organizationsList = organizationsList.Remove(organizationsList.Length - 2);
            }

            return organizationsList;
        }

        private string GetParticipantsList(string concursId)
        {
            string participantsList = string.Empty;

            using (DatabaseConnection dbConnection = new DatabaseConnection())
            {
                if (dbConnection.OpenConnection())
                {
                    string selectQuery = @"
                    SELECT 
                    Участники.фио 
                    FROM 
                    Прохождение_этапов 
                    INNER JOIN Участники ON Прохождение_этапов.id_участника = Участники.id_участника
                    INNER JOIN Заявка ON Прохождение_этапов.id_заявки = Заявка.id_заявки
                    WHERE 
                    Заявка.id_мероприятия = @concursId";
                    SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                    selectCmd.Parameters.AddWithValue("@concursId", concursId);

                    using (SqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            participantsList += reader["фио"].ToString() + ", ";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка подключения к базе данных.");
                }
            }

            // Удаление последней запятой
            if (!string.IsNullOrEmpty(participantsList))
            {
                participantsList = participantsList.Remove(participantsList.Length - 2);
            }

            return participantsList;
        }

        //протокол проведения
        private void Protocol_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridForm.SelectedItem == null)
            {
                MessageBox.Show("Выберите мероприятие!");
                return;
            }

            DataRowView selectedRow = (DataRowView)dataGridForm.SelectedItem;
            string id = selectedRow["ID"].ToString(); // id мероприятия
            string eventName = selectedRow["Название"].ToString(); // Название мероприятия
            string item = selectedRow["Название предмета"].ToString(); // Название предмета

            //laptop
            //string templateFilePath = @"C:\GitHub\OpenAccess\GGAEK\4course\БД\КурсоваяБД\WordPaper\ИсходныеДокументы\Protocol.xlsx";

            //computer
            string templateFilePath = @"G:\Files\ЗаконченыеПроекты\3КУРС\4course\БД\КурсоваяБД\WordPaper\ИсходныеДокументы\Protocol.xlsx";

            // Создание диалогового окна "Сохранить файл"
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Файлы Excel (*.xlsx)|*.xlsx";
            saveFileDialog.Title = "Сохранить Excel-документ как";
            saveFileDialog.DefaultExt = "xlsx";

            if (saveFileDialog.ShowDialog() == true)
            {
                string saveFilePath = saveFileDialog.FileName;

                // Создание экземпляра Excel и открытие шаблона
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = false;
                Excel.Workbook templateWorkbook = excelApp.Workbooks.Open(templateFilePath);
                Excel.Worksheet worksheet = (Excel.Worksheet)templateWorkbook.Sheets[1]; // Лист с данными

                // Замена данных в шаблоне
                worksheet.Range["B2"].Value = eventName;
                worksheet.Range["B3"].Value = item;

                // Получение данных участников мероприятия и заполнение соответствующих ячеек
                using (DatabaseConnection dbConnection = new DatabaseConnection())
                {
                    if (dbConnection.OpenConnection())
                    {
                        string selectQuery = @"
                    SELECT 
                        Участники.фио,
                        Участники.класс,
                        Прохождение_этапов.баллы,
                        Прохождение_этапов.статус
                    FROM 
                        Прохождение_этапов 
                    INNER JOIN 
                        Заявка ON Прохождение_этапов.id_заявки = Заявка.id_заявки
                    INNER JOIN 
                        Участники ON Прохождение_этапов.id_участника = Участники.id_участника
                    WHERE 
                        Заявка.id_мероприятия = @eventId
                    ";
                        SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                        selectCmd.Parameters.AddWithValue("@eventId", id);

                        SqlDataReader reader = selectCmd.ExecuteReader();

                        int currentRow = 6; // Начинаем с 6 строки

                        while (reader.Read())
                        {
                            string participantName = reader.GetString(0);
                            string participantClass = reader.GetString(1);
                            double points = reader.GetDouble(2);
                            string status = reader.GetString(3);

                            worksheet.Cells[currentRow, 1] = participantName;
                            worksheet.Cells[currentRow, 2] = participantClass;
                            worksheet.Cells[currentRow, 3] = points;
                            worksheet.Cells[currentRow, 4] = status;

                            currentRow++; // Переход к следующей строке
                        }

                        reader.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка подключения к базе данных.");
                    }
                }

                // Автоподбор ширины столбцов после заполнения данных
                Excel.Range usedRange = worksheet.UsedRange;
                usedRange.Columns.AutoFit();

                // Применение границ к данным
                ApplyBordersToAllWorksheets(templateWorkbook);

                // Сохранение документа Excel
                templateWorkbook.SaveAs(saveFilePath);

                // Закрытие и освобождение ресурсов
                templateWorkbook.Close(false);
                Marshal.ReleaseComObject(templateWorkbook);
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);

                MessageBox.Show("Данные успешно сохранены в Excel.");
            }
        }


        //заявка
        private void Application_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridForm.SelectedItem == null)
            {
                MessageBox.Show("Выберите заявку!");
                return;
            }

            DataRowView selectedRow = (DataRowView)dataGridForm.SelectedItem;
            string id = selectedRow["Номер заявки"].ToString();
            string fio = selectedRow["ФИО преподавателя"].ToString();
            string nameOrg = selectedRow["Название организации"].ToString();
            string nameEvent = selectedRow["Название мероприятия"].ToString();

            //laptop
            //string templateFilePath = @"C:\GitHub\OpenAccess\GGAEK\4course\БД\КурсоваяБД\WordPaper\ИсходныеДокументы\Zayavka.doc";

            //computer
            string templateFilePath = @"G:\Files\ЗаконченыеПроекты\3КУРС\4course\БД\КурсоваяБД\WordPaper\ИсходныеДокументы\Zayavka.doc";

            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Document doc = wordApp.Documents.Open(templateFilePath);
            doc.Activate();

            // Замена данных в документе
            FindAndReplace(wordApp, "[названиеМероприятия]", nameEvent);
            FindAndReplace(wordApp, "[наименованиеОрганизации]", nameOrg);
            FindAndReplace(wordApp, "[преподователь]", fio);

            // Создание таблицы и заполнение данными участников
            Table table = doc.Tables[1]; // Предполагаем, что таблица находится на первой странице документа
            if (table != null)
            {
                // Получаем список участников для данного мероприятия
                List<Participant> participants = GetParticipantsForConcurs(id);

                // Заполняем таблицу данными участников
                int rowIndex = 2; // Начинаем заполнение таблицы со второй строки
                int number = 1; // Номер участника
                foreach (Participant participant in participants)
                {
                    table.Rows.Add(); // Добавляем новую строку в таблицу
                    table.Cell(rowIndex, 1).Range.Text = number.ToString(); // Заполняем номер участника
                    table.Cell(rowIndex, 2).Range.Text = participant.Fio; // Заполняем ФИО участника
                    table.Cell(rowIndex, 3).Range.Text = participant.Class; // Заполняем класс участника
                    rowIndex++;
                    number++;
                }
            }

            // Создание диалогового окна "Сохранить файл"
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Файлы Word (*.doc)|*.doc";
            saveFileDialog.Title = "Сохранить документ Word как";
            saveFileDialog.DefaultExt = "doc";

            // Сохранение документа
            if (saveFileDialog.ShowDialog() == true)
            {
                string saveFilePath = saveFileDialog.FileName;
                doc.SaveAs(saveFilePath, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument);
                MessageBox.Show("Документ успешно сохранен как Word.");
            }

            // Закрытие документа и Word приложения
            doc.Close();
            wordApp.Quit();
        }

        private void FillParticipantTable(Document doc, string concursId)
        {
            Microsoft.Office.Interop.Word.Table table = doc.Tables[1]; // Предполагается, что таблица начинается с первой таблицы в документе
            int rowIndex = 2; // Начинаем заполнение таблицы с 2 строки (после заголовка)

            // Получение списка участников, связанных с выбранным мероприятием через заявки
            List<Participant> participants = GetParticipantsForConcurs(concursId);

            // Проход по каждому участнику и заполнение таблицы
            foreach (Participant participant in participants)
            {
                string fio = participant.Fio;
                string @class = participant.Class;

                // Вставляем номер участника (начиная с 1)
                table.Cell(rowIndex, 1).Range.Text = (rowIndex - 1).ToString();
                // Вставляем ФИО участника
                table.Cell(rowIndex, 2).Range.Text = fio;
                // Вставляем класс участника
                table.Cell(rowIndex, 3).Range.Text = @class;

                rowIndex++; // Переходим к следующей строке таблицы
            }
        }

        public class Participant
        {
            public string Fio { get; set; }
            public string Class { get; set; }

            public Participant(string fio, string @class)
            {
                Fio = fio;
                Class = @class;
            }
        }


        // Метод для получения списка участников, участвующих в выбранном мероприятии
        private List<Participant> GetParticipantsForConcurs(string concursId)
        {
            List<Participant> participants = new List<Participant>();

            using (DatabaseConnection dbConnection = new DatabaseConnection())
            {
                if (dbConnection.OpenConnection())
                {
                    string selectQuery = @"
                    SELECT 
                    Участники.фио, 
                    Участники.класс 
                    FROM 
                    Участники 
                    INNER JOIN Организация ON Участники.id_организации = Организация.id_организации 
                    INNER JOIN Заявка ON Организация.id_организации = Заявка.id_организации 
                    WHERE 
                    Заявка.id_мероприятия = @concursId
                    ";

                    SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                    selectCmd.Parameters.AddWithValue("@concursId", concursId);

                    SqlDataReader reader = selectCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string fio = reader.GetString(0);
                        string @class = reader.GetString(1);

                        participants.Add(new Participant(fio, @class));
                    }

                    reader.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка подключения к базе данных.");
                }
            }

            return participants;
        }
        

        //график проведения мероприятий
        private void GrapficEvents_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Файлы Excel (*.xlsx)|*.xlsx";
            saveFileDialog.Title = "Сохранить как Excel";
            saveFileDialog.DefaultExt = "xlsx";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = false;

                Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Add();
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets.Add();
                excelSheet.Name = "График проведения мероприятий";

                // Заголовки
                excelSheet.Cells[1, 1] = "Название";
                excelSheet.Cells[1, 2] = "Дата проведения";
                excelSheet.Cells[1, 3] = "Этап";

                int currentRow = 2;

                using (DatabaseConnection dbConnection = new DatabaseConnection())
                {
                    if (dbConnection.OpenConnection())
                    {
                        string selectQuery = "SELECT Мероприятия.название, дата_проведения, Этапы.наименование AS этап " +
                                             "FROM Мероприятия " +
                                             "INNER JOIN Этапы ON Мероприятия.id_этапа = Этапы.id_этапа";

                        SqlCommand selectCmd = new SqlCommand(selectQuery, dbConnection.GetConnection());
                        SqlDataReader reader = selectCmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string eventName = reader.GetString(0);
                            DateTime eventDate = reader.GetDateTime(1);
                            string eventStage = reader.GetString(2);

                            excelSheet.Cells[currentRow, 1] = eventName;
                            excelSheet.Cells[currentRow, 2] = eventDate;
                            excelSheet.Cells[currentRow, 3] = eventStage;

                            currentRow++;
                        }

                        reader.Close();

                        // Создание объекта графика
                        Microsoft.Office.Interop.Excel.ChartObjects chartObjects = (Microsoft.Office.Interop.Excel.ChartObjects)excelSheet.ChartObjects(Type.Missing);
                        Microsoft.Office.Interop.Excel.ChartObject chartObject = chartObjects.Add(100, 80, 300, 300); // Положение и размер графика
                        Microsoft.Office.Interop.Excel.Chart chart = chartObject.Chart;

                        // Укажем данные для графика
                        Microsoft.Office.Interop.Excel.Range chartRange = excelSheet.Range[excelSheet.Cells[2, 1], excelSheet.Cells[currentRow - 1, 3]];
                        chart.SetSourceData(chartRange);

                        // Установим тип графика
                        chart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;

                        // Добавим заголовок для графика
                        chart.HasTitle = true;
                        chart.ChartTitle.Text = "График дат проведения мероприятий";

                        // Добавим подписи для осей
                        chart.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlCategory).HasTitle = true;
                        chart.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlCategory).AxisTitle.Text = "Дата проведения";
                        chart.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlValue).HasTitle = true;
                        chart.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlValue).AxisTitle.Text = "Название мероприятия";

                        // Добавим легенду
                        chart.HasLegend = true;

                        // Отключим автоматическую генерацию меток
                        chart.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowLabel);

                        Microsoft.Office.Interop.Excel.Axis yAxis = (Microsoft.Office.Interop.Excel.Axis)chart.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                        yAxis.HasTitle = true;
                        yAxis.AxisTitle.Text = "Название мероприятия";
                    }
                    else
                    {
                        MessageBox.Show("Ошибка подключения к базе данных.");
                        return;
                    }
                }

                // Автоподбор ширины столбцов
                excelSheet.Columns.AutoFit();

                // Применение границ к данным
                ApplyBordersToAllWorksheets(excelBook);

                // Сохранение файла Excel
                excelBook.SaveAs(filePath);

                // Закрытие и освобождение ресурсов
                excelBook.Close();
                Marshal.ReleaseComObject(excelBook);
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);

                MessageBox.Show("График успешно создан и сохранен.");
            }
        }

        //замена данных на всех листах
        private void FindAndReplace(Excel.Worksheet worksheet, string placeholder, string value)
        {
            Excel.Range usedRange = worksheet.UsedRange;
            Excel.Range range = usedRange.Find(placeholder);

            while (range != null)
            {
                range.Value = range.Value.Replace(placeholder, value);
                range = usedRange.Find(placeholder);
            }
        }

        //обводка данных
        private void ApplyBordersToAllWorksheets(Microsoft.Office.Interop.Excel.Workbook workbook)
        {
            foreach (Microsoft.Office.Interop.Excel.Worksheet worksheet in workbook.Sheets)
            {
                Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;

                foreach (Microsoft.Office.Interop.Excel.Range cell in usedRange)
                {
                    Microsoft.Office.Interop.Excel.Borders borders = cell.Borders;
                    borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                }
            }
        }


        //----------------КОНЕЦ ДОКУМЕНТЫ----------------



        //----------------ПОИСК----------------
        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text;

            if(searchText == "")
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Если поле поиска пустое, очистите подсветку и выходите
                ClearSearchHighlighting();
                return;
            }

            // Пройдитесь по всем строкам и ячейкам в DataGrid
            foreach (DataGridRow row in GetDataGridRows(dataGridForm))
            {
                foreach (DataGridColumn column in dataGridForm.Columns)
                {
                    if (column is DataGridTextColumn)
                    {
                        var cell = GetCell(row, column);
                        if (cell != null)
                        {
                            TextBlock textBlock = cell.Content as TextBlock;
                            if (textBlock != null)
                            {
                                string cellContent = textBlock.Text;
                                if (!string.IsNullOrEmpty(cellContent))
                                {
                                    if (cellContent.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        // Если найдено совпадение, подсветите текст
                                        int index = cellContent.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
                                        string preMatch = cellContent.Substring(0, index);
                                        string match = cellContent.Substring(index, searchText.Length);
                                        string postMatch = cellContent.Substring(index + searchText.Length);

                                        textBlock.Inlines.Clear();
                                        textBlock.Inlines.Add(new Run(preMatch));
                                        Run matchRun = new Run(match);
                                        matchRun.Background = Brushes.Yellow; // Задайте цвет подсветки
                                        textBlock.Inlines.Add(matchRun);
                                        textBlock.Inlines.Add(new Run(postMatch));
                                    }
                                    else
                                    {
                                        // Если совпадение не найдено, очистите подсветку
                                        textBlock.Inlines.Clear();
                                        textBlock.Inlines.Add(new Run(cellContent));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ClearSearchHighlighting()
        {
            // Очистите подсветку во всех ячейках DataGrid
            foreach (DataGridRow row in GetDataGridRows(dataGridForm))
            {
                foreach (DataGridColumn column in dataGridForm.Columns)
                {
                    if (column is DataGridTextColumn)
                    {
                        var cell = GetCell(row, column);
                        if (cell != null)
                        {
                            TextBlock textBlock = cell.Content as TextBlock;
                            if (textBlock != null)
                            {
                                textBlock.Inlines.Clear();
                                textBlock.Inlines.Add(new Run(textBlock.Text));
                            }
                        }
                    }
                }
            }
        }

        private System.Windows.Controls.DataGridCell GetCell(DataGridRow row, DataGridColumn column)
        {
            if (column != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);
                if (presenter == null)
                    return null;

                int columnIndex = dataGridForm.Columns.IndexOf(column);
                if (columnIndex > -1)
                {
                    System.Windows.Controls.DataGridCell cell = (System.Windows.Controls.DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                    return cell;
                }
            }
            return null;
        }

        private List<DataGridRow> GetDataGridRows(System.Windows.Controls.DataGrid grid)
        {
            List<DataGridRow> rows = new List<DataGridRow>();
            for (int i = 0; i < dataGridForm.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)dataGridForm.ItemContainerGenerator.ContainerFromIndex(i);
                if (row != null)
                {
                    rows.Add(row);
                }
            }
            return rows;
        }

        private childItem GetVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = GetVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        //----------------КОНЕЦ ПОИСК----------------



        //----------------ФИЛЬТРАЦИЯ----------------
        private void buttonFilter_Click(object sender, RoutedEventArgs e)
        {
            switch (tableIndex)
            {
                case 1: // Организации
                    try
                    {
                        string filterText = textBoxFilter.Text.Trim(); // Получаем текст фильтрации

                        // Формируем SQL-запрос для фильтрации и выборки данных
                        string sqlQuery = "SELECT id_организации AS ID, наименование AS Наименование, район AS Область, город AS Район FROM Организация WHERE наименование LIKE @FilterText OR район LIKE @FilterText OR город LIKE @FilterText";

                        // Создаем подключение к базе данных
                        using (DatabaseConnection dbConnection = new DatabaseConnection())
                        {
                            if (dbConnection.OpenConnection())
                            {
                                using (SqlCommand command = new SqlCommand(sqlQuery, dbConnection.GetConnection()))
                                {
                                    command.Parameters.AddWithValue("@FilterText", "%" + filterText + "%");

                                    try
                                    {
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            DataTable dataTable = new DataTable();
                                            dataTable.Load(reader);
                                            dataGridForm.ItemsSource = dataTable.DefaultView;
                                            // Скрываем колонку ID, если это нужно
                                            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
                                            // Здесь вы можете использовать dataTable для отображения результатов
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                                    }
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
                    break;
                case 2: // Педагоги
                    try
                    {
                        string filterText = textBoxFilter.Text.Trim(); // Получаем текст фильтрации

                        // Формируем SQL-запрос для фильтрации и выборки данных
                        string sqlQuery = "SELECT p.id_педагога AS ID, p.фио AS ФИО, p.должность AS Должность, " +
                            "o.наименование AS [Наименование организации] " +
                                          "FROM Педагоги p " +
                                          "JOIN Организация o ON p.id_организации = o.id_организации " +
                                          "WHERE p.фио LIKE @FilterText OR p.должность LIKE @FilterText";

                        // Создаем подключение к базе данных
                        using (DatabaseConnection dbConnection = new DatabaseConnection())
                        {
                            if (dbConnection.OpenConnection())
                            {
                                using (SqlCommand command = new SqlCommand(sqlQuery, dbConnection.GetConnection()))
                                {
                                    command.Parameters.AddWithValue("@FilterText", "%" + filterText + "%");

                                    try
                                    {
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            DataTable dataTable = new DataTable();
                                            dataTable.Load(reader);
                                            dataGridForm.ItemsSource = dataTable.DefaultView;
                                            // Скрываем колонку ID, если это нужно
                                            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                                    }
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
                    break;
                case 3: // Участники
                    try
                    {
                        string filterText = textBoxFilter.Text.Trim(); // Получаем текст фильтрации

                        // Формируем SQL-запрос для фильтрации и выборки данных
                        string sqlQuery = "SELECT u.id_участника AS ID, u.фио AS ФИО, u.класс AS Класс, o.наименование AS [Наименование организации] " +
                                          "FROM Участники u " +
                                          "JOIN Организация o ON u.id_организации = o.id_организации " +
                                          "WHERE u.фио LIKE @FilterText OR u.класс LIKE @FilterText";

                        // Создаем подключение к базе данных
                        using (DatabaseConnection dbConnection = new DatabaseConnection())
                        {
                            if (dbConnection.OpenConnection())
                            {
                                using (SqlCommand command = new SqlCommand(sqlQuery, dbConnection.GetConnection()))
                                {
                                    command.Parameters.AddWithValue("@FilterText", "%" + filterText + "%");

                                    try
                                    {
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            DataTable dataTable = new DataTable();
                                            dataTable.Load(reader);
                                            dataGridForm.ItemsSource = dataTable.DefaultView;
                                            // Скрываем колонку ID, если это нужно
                                            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
                                            // Здесь вы можете использовать dataTable для отображения результатов
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                                    }
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
                    break;
                case 4: // Предметы
                    try
                    {
                        string filterText = textBoxFilter.Text.Trim(); // Получаем текст фильтрации

                        // Формируем SQL-запрос для фильтрации и выборки данных
                        string sqlQuery = "SELECT id_предмета AS ID, наименование AS Наименование FROM Предметы WHERE наименование LIKE @FilterText";

                        // Создаем подключение к базе данных
                        using (DatabaseConnection dbConnection = new DatabaseConnection())
                        {
                            if (dbConnection.OpenConnection())
                            {
                                using (SqlCommand command = new SqlCommand(sqlQuery, dbConnection.GetConnection()))
                                {
                                    command.Parameters.AddWithValue("@FilterText", "%" + filterText + "%");

                                    try
                                    {
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            DataTable dataTable = new DataTable();
                                            dataTable.Load(reader);
                                            dataGridForm.ItemsSource = dataTable.DefaultView;
                                            // Скрываем колонку ID, если это нужно
                                            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
                                            // Здесь вы можете использовать dataTable для отображения результатов
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                                    }
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
                    break;

                case 5: // Этапы
                    try
                    {
                        string filterText = textBoxFilter.Text.Trim(); // Получаем текст фильтрации

                        // Формируем SQL-запрос для фильтрации и выборки данных
                        string sqlQuery = "SELECT id_этапа AS ID, наименование AS Наименование FROM Этапы WHERE наименование LIKE @FilterText";

                        // Создаем подключение к базе данных
                        using (DatabaseConnection dbConnection = new DatabaseConnection())
                        {
                            if (dbConnection.OpenConnection())
                            {
                                using (SqlCommand command = new SqlCommand(sqlQuery, dbConnection.GetConnection()))
                                {
                                    command.Parameters.AddWithValue("@FilterText", "%" + filterText + "%");

                                    try
                                    {
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            DataTable dataTable = new DataTable();
                                            dataTable.Load(reader);
                                            dataGridForm.ItemsSource = dataTable.DefaultView;
                                            // Скрываем колонку ID, если это нужно
                                            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
                                            // Здесь вы можете использовать dataTable для отображения результатов
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                                    }
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
                    break;
                case 6: // Мероприятия
                    try
                    {
                        // Получаем текст фильтрации и значения дат из DatePicker
                        string filterText = textBoxFilter.Text.Trim();
                        DateTime? startDate = datePickerFilterFirstEvent.SelectedDate;
                        DateTime? endDate = datePickerFilterLastEvent.SelectedDate;

                        // Формируем SQL-запрос для фильтрации и выборки данных
                        string sqlQuery = "SELECT e.id_мероприятия AS ID, e.название AS Название, p.наименование AS [Название предмета], " +
                                          "FORMAT(e.дата_проведения, 'dd.MM.yyyy') AS [Дата проведения], s.наименование AS [Наименование этапа] " +
                                          "FROM Мероприятия e " +
                                          "JOIN Предметы p ON e.id_предмета = p.id_предмета " +
                                          "JOIN Этапы s ON e.id_этапа = s.id_этапа " +
                                          "WHERE e.название LIKE @FilterText ";

                        // Если выбраны начальная и конечная дата, добавляем условие фильтрации по дате
                        if (startDate != null && endDate != null)
                        {
                            sqlQuery += "AND (e.дата_проведения >= @StartDate AND e.дата_проведения <= @EndDate) ";
                        }

                        // Создаем подключение к базе данных
                        using (DatabaseConnection dbConnection = new DatabaseConnection())
                        {
                            if (dbConnection.OpenConnection())
                            {
                                using (SqlCommand command = new SqlCommand(sqlQuery, dbConnection.GetConnection()))
                                {
                                    // Добавляем параметры фильтрации
                                    command.Parameters.AddWithValue("@FilterText", "%" + filterText + "%");
                                    if (startDate != null && endDate != null)
                                    {
                                        command.Parameters.AddWithValue("@StartDate", startDate);
                                        command.Parameters.AddWithValue("@EndDate", endDate);
                                    }

                                    try
                                    {
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            DataTable dataTable = new DataTable();
                                            dataTable.Load(reader);
                                            dataGridForm.ItemsSource = dataTable.DefaultView;
                                            // Скрываем колонку ID, если это нужно
                                            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
                                            // Здесь вы можете использовать dataTable для отображения результатов
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                                    }
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
                    break;
                case 7: // Заявки
                    try
                    {
                        // Получаем текст фильтрации и значения дат из DatePicker
                        string filterText = textBoxFilter.Text.Trim();
                        DateTime? startDate = datePickerFilterFirstDateOrder.SelectedDate;
                        DateTime? endDate = datePickerFilterLastDateOrder.SelectedDate;

                        // Формируем SQL-запрос для фильтрации и выборки данных
                        string sqlQuery = "SELECT z.id_заявки AS [Номер заявки], p.фио AS [ФИО преподавателя], o.наименование AS [Название организации], m.название AS [Название мероприятия], FORMAT(z.дата_подачи, 'dd.MM.yyyy') AS [Дата подачи] " +
                                          "FROM Заявка z " +
                                          "JOIN Педагоги p ON z.id_педагога = p.id_педагога " +
                                          "JOIN Организация o ON z.id_организации = o.id_организации " +
                                          "JOIN Мероприятия m ON z.id_мероприятия = m.id_мероприятия " +
                                          "WHERE p.фио LIKE @FilterText ";

                        // Если выбраны начальная и конечная дата, добавляем условие фильтрации по дате
                        if (startDate != null && endDate != null)
                        {
                            sqlQuery += "AND (z.дата_подачи >= @StartDate AND z.дата_подачи <= @EndDate) ";
                        }

                        // Создаем подключение к базе данных
                        using (DatabaseConnection dbConnection = new DatabaseConnection())
                        {
                            if (dbConnection.OpenConnection())
                            {
                                using (SqlCommand command = new SqlCommand(sqlQuery, dbConnection.GetConnection()))
                                {
                                    // Добавляем параметры фильтрации
                                    command.Parameters.AddWithValue("@FilterText", "%" + filterText + "%");
                                    if (startDate != null && endDate != null)
                                    {
                                        command.Parameters.AddWithValue("@StartDate", startDate);
                                        command.Parameters.AddWithValue("@EndDate", endDate);
                                    }

                                    try
                                    {
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            DataTable dataTable = new DataTable();
                                            dataTable.Load(reader);
                                            dataGridForm.ItemsSource = dataTable.DefaultView;
                                            // Скрываем колонку с номером заявки, если это нужно
                                            // dataGridForm.Columns[0].Visibility = Visibility.Hidden;
                                            // Здесь вы можете использовать dataTable для отображения результатов
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                                    }
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
                    break;
                case 8: // Прохождение этапов
                    try
                    {
                        // Получаем значения дат из DatePicker
                        DateTime? startDate = datePickerFilterFirstDateStep.SelectedDate;
                        DateTime? endDate = datePickerFilterLastDateStep.SelectedDate;

                        // Формируем SQL-запрос для фильтрации и выборки данных
                        string sqlQuery = "SELECT Прохождение_этапов.id_прохождения AS ID, Участники.фио AS [ФИО участника], N'№ заявки: ' + CAST(Прохождение_этапов.id_заявки AS NVARCHAR(10)) + '\n' + Этапы.наименование AS [Заявка], FORMAT(Прохождение_этапов.дата_прохождения, 'dd.MM.yyyy') AS [Дата прохождения], Прохождение_этапов.баллы AS Баллы, Прохождение_этапов.статус AS Статус " +
                                          "FROM Прохождение_этапов " +
                                          "INNER JOIN Участники ON Прохождение_этапов.id_участника = Участники.id_участника " +
                                          "INNER JOIN Этапы ON Прохождение_этапов.id_этапа = Этапы.id_этапа " +
                                          "WHERE 1=1 ";

                        // Если выбраны начальная и конечная дата, добавляем условие фильтрации по дате
                        if (startDate != null && endDate != null)
                        {
                            sqlQuery += "AND (Прохождение_этапов.дата_прохождения >= @StartDate AND Прохождение_этапов.дата_прохождения <= @EndDate) ";
                        }

                        // Создаем подключение к базе данных
                        using (DatabaseConnection dbConnection = new DatabaseConnection())
                        {
                            if (dbConnection.OpenConnection())
                            {
                                using (SqlCommand command = new SqlCommand(sqlQuery, dbConnection.GetConnection()))
                                {
                                    if (startDate != null && endDate != null)
                                    {
                                        // Добавляем параметры фильтрации
                                        command.Parameters.AddWithValue("@StartDate", startDate);
                                        command.Parameters.AddWithValue("@EndDate", endDate);
                                    }

                                    try
                                    {
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            DataTable dataTable = new DataTable();
                                            dataTable.Load(reader);
                                            dataGridForm.ItemsSource = dataTable.DefaultView;
                                            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                                    }
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
                    break;
                case 9: // Списки участников
                    try
                    {
                        string filterText = textBoxFilter.Text.Trim(); // Получаем текст фильтрации

                        // Формируем SQL-запрос для фильтрации
                        string sqlQuery = "SELECT Список.id_списка AS ID, Участники.фио AS [ФИО участника], N'№ заявки: ' + CAST(Заявка.id_заявки AS NVARCHAR(10)) + '\n' + Мероприятия.название AS Заявка " +
                                          "FROM Список " +
                                          "INNER JOIN Участники ON Список.id_участника = Участники.id_участника " +
                                          "INNER JOIN Заявка ON Список.id_заявки = Заявка.id_заявки " +
                                          "INNER JOIN Мероприятия ON Заявка.id_мероприятия = Мероприятия.id_мероприятия " +
                                          "WHERE Участники.фио LIKE @FilterText";

                        // Создаем подключение к базе данных
                        using (DatabaseConnection dbConnection = new DatabaseConnection())
                        {
                            if (dbConnection.OpenConnection())
                            {
                                using (SqlCommand command = new SqlCommand(sqlQuery, dbConnection.GetConnection()))
                                {
                                    command.Parameters.AddWithValue("@FilterText", "%" + filterText + "%");

                                    try
                                    {
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            DataTable dataTable = new DataTable();
                                            dataTable.Load(reader);
                                            dataGridForm.ItemsSource = dataTable.DefaultView;
                                            // Скрываем колонку с номером списка, если это нужно
                                            dataGridForm.Columns[0].Visibility = Visibility.Hidden;
                                            // Здесь вы можете использовать dataTable для отображения результатов
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                                    }
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
                    break;
                default:
                    MessageBox.Show("Выберите таблицу!");
                    break;
            }
        }
        //----------------КОНЕЦ ФИЛЬТРАЦИЯ----------------
    }
}
