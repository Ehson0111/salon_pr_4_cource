using pr1_salon.Model;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pr1_salon.Pages
{
    /// <summary>
    /// Логика взаимодействия для Service_page1.xaml
    /// </summary>
    public partial class Service_page1 : Page
    {
        bool admin = false;
        private List<Service> allServices;

        private List<Service> filteredServices; // Отфильтрованные услуги
        private bool isAdmin = false; // Поле для хранения статуса администратора
        private int currentPage = 1; // Текущая страница
        private int pageSize = 10; // Количество услуг на странице

        public Service_page1()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
            LoadServices();

            btnAdd.Visibility = Visibility.Hidden;
            btnAdmin.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
            btnRecords.Visibility = Visibility.Hidden;
            btnUpdate.Visibility = Visibility.Hidden;
            btnWrite.Visibility = Visibility.Hidden;

            //var product = pr1Entities.GetContext().Service.ToList();

            //servicesList.ItemsSource = product;
            ////user = Currentuser;



        }

        public Service_page1(int a)
        {
            InitializeComponent(); this.Loaded += Page_Loaded;
            LoadServices();

            btnAdd.Visibility = Visibility.Visible;
            btnAdmin.Visibility = Visibility.Visible;
            btnDelete.Visibility = Visibility.Visible;
            btnRecords.Visibility = Visibility.Visible;
            btnUpdate.Visibility = Visibility.Visible;
            btnWrite.Visibility = Visibility.Visible;



        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadServices();
        }
        private void LoadServices()
        {
            //UpdateAdminControlsVisibility(); // Устанавливаем начальную видимость контролов

            try
            {
                allServices = GetServicesFromDatabase();

                // Применяем скидки для отображения измененной стоимости
                foreach (var service in allServices)
                {
                    service.DiscountedCost = service.Discount.HasValue && service.Discount > 0
                        ? Math.Round(service.Cost - (service.Cost * (decimal)service.Discount), 2)
                        : service.Cost;

                    service.DiscountDescription = service.Discount.HasValue && service.Discount > 0
                        ? $"Скидка: {service.Discount:P0}"
                        : "Без скидки";

                }

                ApplyFilters(); // Применяем фильтры после загрузки
                DisplayPage(); // Отображаем данные
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки услуг: {ex.Message}");
            }
        }
        private void DisplayPage()
        {
            int totalRecords = filteredServices.Count;
            int totalPages = pr1Entities.GetContext().Service.Count();

            // Получаем текущую страницу данных
            var pagedServices = filteredServices
                //.Skip((currentPage - 1) * pageSize)
                //.Take(pageSize)
                .ToList();

            servicesList.ItemsSource = pagedServices; // Отображаем записи

            // Обновление текста с информацией о количестве записей
            //recordCountText.Text = $"{currentPage} из {totalPages}";

            // Обновление состояния кнопок навигации
            //btnBack.IsEnabled = currentPage > 1;
            //btnNext.IsEnabled = currentPage < totalPages;
        }

 
        private List<Service> GetServicesFromDatabase()
        {
            try
            {
                return pr1Entities.GetContext().Service.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запросе к базе данных: {ex.Message}");
                return new List<Service>();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }
        private bool HasUpcomingOrPastRecords(int serviceId)
        {
            // Проверка записи на услуги
            return pr1Entities.GetContext().ClientService.Any(r => r.ServiceID == serviceId);
        }
        private void btnDelete_Click_1(object sender, RoutedEventArgs e)
        {
            if (servicesList.SelectedItem is Service selectedService)
            {

                if (HasUpcomingOrPastRecords(selectedService.ID))
                {
                    MessageBox.Show($"Услуга '{selectedService.Title}' не может быть удалена, так как на неё имеются записи.");
                    return;
                }
                try
                {
                    using (var context = pr1Entities.GetContext())
                    {
                        // Удаляем связанные фотографии услуги
                        var photosToDelete = context.ServicePhoto.Where(p => p.ServiceID == selectedService.ID).ToList();
                        if (photosToDelete.Count > 0)
                        {
                            context.ServicePhoto.RemoveRange(photosToDelete);
                        }

                        // Удаляем саму услугу
                        context.Service.Remove(selectedService);

                        // Сохраняем изменения в базе данных
                        context.SaveChanges();
                    }

                    MessageBox.Show("Услуга и связанные фотографии успешно удалены!");
                    LoadServices(); // Обновляем список услуг после удаления
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления услуги: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите услугу для удаления.");
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddAndEdit(null));
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (servicesList.SelectedItem is Service selectedService)
            {
                NavigationService.Navigate(new AddAndEdit(selectedService));
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите услугу для обновления.");
            }
        }
        //private void UpdateAdminControlsVisibility()
        //{
        //    btnAdd.IsEnabled = isAdmin;
        //    btnDelete.IsEnabled = isAdmin;
        //    btnUpdate.IsEnabled = isAdmin;
        //}
        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            //AdminLoginWindow loginWindow = new AdminLoginWindow();
            //bool? result = loginWindow.ShowDialog();
            //if (result == true)
            //{
            //    MessageBox.Show("Вы вошли в режим администратора!");
            //    isAdmin = true;
            //    UpdateAdminControlsVisibility();
            //}

            NavigationService.Navigate(new Auth());
        }

        private void btnWrite_Click(object sender, RoutedEventArgs e)
        {
            if (servicesList.SelectedItem is Service selectedService)
            {
                NavigationService.Navigate(new CustomerRecord(selectedService));
            }
            else
            {
                MessageBox.Show("Услуга не выбрана. Выберите услугу для записи, пожалуйста.");
            }
        }

        private void btnRecords_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UpcomingEntries());
        }

        private void txtserach_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters(); // Применяем фильтры при изменении текста в поиске
            currentPage = 1; // Сброс текущей страницы
            DisplayPage();

        }

        private void cbsort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters(); // Применяем фильтры при изменении сортировкdsvи
            currentPage = 1; // Сброс текущей страницы
            DisplayPage();

        }
        private void ApplyFilters()
        {
            string searchText = txtserach.Text.ToLower();
            filteredServices = allServices
                .Where(s => s.Title.ToLower().Contains(searchText))
                .ToList();

            if (cmbprocent.SelectedItem is ComboBoxItem selectedItem)
            {
                filteredServices = FilterByDiscount(filteredServices, selectedItem);
            }

            if (cbsort.SelectedItem is ComboBoxItem sortItem)
            {
                filteredServices = SortServices(filteredServices, sortItem);
            }


            recordCountText.Text = $"{filteredServices.Count} из {pr1Entities.GetContext().Service.Count()}";

        }

        private List<Service> FilterByDiscount(List<Service> services, ComboBoxItem selectedItem)
        {
            double minDiscount = 0, maxDiscount = 1;

            switch (selectedItem.Tag)
            {
                case "0":
                    return services.ToList();
                    break;
                case "1":
                    minDiscount = 0;
                    maxDiscount = 0.05;
                    break;
                case "2":
                    minDiscount = 0.05;
                    maxDiscount = 0.15;
                    break;
                case "3":
                    minDiscount = 0.15;
                    maxDiscount = 0.30;
                    break;
                case "4":
                    minDiscount = 0.30;
                    maxDiscount = 0.70;
                    break;
                case "5":
                    minDiscount = 0.70;
                    maxDiscount = 1;
                    break;
            }

            // Фильтруем услуги по скидке
            return services.Where(s => s.Discount >= minDiscount && s.Discount <= maxDiscount).ToList();
        }

        private List<Service> SortServices(List<Service> services, ComboBoxItem sortItem)
        {
            // Сортируем услуги
            switch (sortItem.Tag)
            {
                case "1":
                    return services.OrderBy(s => s.Cost).ToList(); // По возрастанию стоимости
                case "2":
                    return services.OrderByDescending(s => s.Cost).ToList(); // По убыванию стоимости
                default:
                    return services; // Без сортировки
            }
        }

        private void cmbprocent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
            currentPage = 1; // Сброс текущей страницы
            DisplayPage();
        }

        private void btnDelete_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
