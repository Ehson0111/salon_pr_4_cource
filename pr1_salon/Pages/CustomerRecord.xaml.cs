using pr1_salon.Model;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pr1_salon.Pages
{
    /// <summary>
    /// Логика взаимодействия для CustomerRecord.xaml
    /// </summary>
    public partial class CustomerRecord : Page
    {
        private Service selectedService;
        public CustomerRecord(Service service)

        {
            InitializeComponent();

            selectedService = service;
            InitializePage();
        }

        private void btncancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Service_page1(1));

        }

        private void btnsave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbClients.SelectedItem == null || string.IsNullOrWhiteSpace(tbStartTime.Text) || dpServiceDate.SelectedDate == null)
                {
                    MessageBox.Show("Заполните все поля.");
                    return;
                }

                // Проверяем корректность времени
                if (!TimeSpan.TryParse(tbStartTime.Text, out var startTime))
                {
                    MessageBox.Show("Введите корректное время в формате ЧЧ:ММ.");
                    return;
                }

                // Создаём запись
                var newClientService = new ClientService
                {
                    ClientID = (int)cbClients.SelectedValue,
                    ServiceID = selectedService.ID,
                    StartTime = dpServiceDate.SelectedDate.Value + startTime,
                    Comment = null // Можно добавить комментарий, если нужно
                };


                pr1Entities.GetContext().ClientService.Add(newClientService);
                pr1Entities.GetContext().SaveChanges();


                MessageBox.Show("Запись успешно создана!");
                NavigationService.Navigate(new UpcomingEntries());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        private void InitializePage()
        {
            if (selectedService != null)
            {
                tbServiceName.Text = selectedService.Title;
                tbServiceDuration.Text = $"{selectedService.DurationInSeconds / 60} минут";

                // Загрузка списка клиентов из базы данных
                try
                {
                    var context = pr1Entities.GetContext(); // Сохраняем контекст на уровне страницы
                    var clients = context.Client
                        .Select(c => new
                        {
                            c.LastName,
                            c.FirstName,
                            c.Patronymic,
                            ClientId = c.ID
                        })
                        .ToList() // Преобразование в память
                        .Select(c => new
                        {
                            FullName = $"{c.LastName} {c.FirstName} {c.Patronymic}",
                            c.ClientId
                        })
                        .ToList();

                    cbClients.ItemsSource = clients;
                    cbClients.DisplayMemberPath = "FullName";
                    cbClients.SelectedValuePath = "ClientId";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке клиентов: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Не выбрана услуга для записи.");
                NavigationService.Navigate(new Service_page1(1));
            }
        }

        private void tbStartTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!TimeSpan.TryParse(tbStartTime.Text, out var startTime))
            {
                MessageBox.Show("Введите корректное время в формате ЧЧ:ММ.");
                return;
            }

            // Рассчитываем время окончания

            var endTime = startTime.Add(TimeSpan.FromMinutes(selectedService.DurationInSeconds / 60));//selectedService.DurationInSeconds));
            tbEndTime.Text = endTime.ToString(@"hh\:mm");
        }

        private void tbStartTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешаем только цифры и двоеточие
            var regex = new Regex(@"^[0-9:]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}
