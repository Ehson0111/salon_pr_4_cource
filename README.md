using dema_ekx.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime;
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

namespace dema_ekx.Pages
{
    /// <summary>
    /// Логика взаимодействия для ManagerPage.xaml
    /// </summary>
    public partial class ManagerPage : Page
    {


        public List<tovar> filtertovar;
        public ManagerPage(users users)
        {
            InitializeComponent();

            this.Loaded+= Page_Loaded;



            //this.Loaded = +ApplyFilter;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {

            LoadTovar();
        }

        public void LoadTovar()
        {


            try
            {

                listtovar.ItemsSource = Model.demka_dem_ekazEntities.GetContext().tovar.ToList();
                List<tovar> tovars = Model.demka_dem_ekazEntities.GetContext().tovar.ToList();


                foreach (var item in tovars)
                {

                    double discountAmount = item.tsena.Value * item.deystvuyushaya_skidka.Value / 100.0;


                    double finalPrice = item.tsena.Value - discountAmount;

                    item.tsena = (int)Math.Round(finalPrice);

                }

                listtovar.ItemsSource = tovars;

            }
            catch (Exception ex)
            {



                MessageBox.Show(ex.ToString());
            }

        }

        private void ApplyFilter()
        {

            // Проверяем, что все контролы существуют
            if (txtsearch == null || cmbfilter == null || cmbsort == null)
                return;
            string textsearch = txtsearch.Text.Trim();



            filtertovar = Model.demka_dem_ekazEntities.GetContext().tovar.Where(x => x.opisanir_tovara != null && x.opisanir_tovara.ToLower().Contains(textsearch)).ToList();


            if (cmbsort.SelectedItem != null)
            {

                var selectedItem = cmbfilter.SelectedItem as ComboBoxItem;
                filtertovar = FilterTovar(filtertovar, selectedItem);

            }
                      
            if (cmbsort.SelectedItem is ComboBox sortitem)
            {
                filtertovar = SortTovars(filtertovar, sortitem);

            }
            listtovar.ItemsSource =  filtertovar.ToList();

        }



        private List<tovar> SortTovars(List<tovar> tovars, ComboBox sortitem) {


            //string select = ;

            switch (sortitem.Tag)
            {
                case "1":
                    return  tovars.OrderBy(x=>x.tsena).ToList();
                case "2":
                    return tovars.OrderByDescending(x=>x.tsena).ToList();
                default:
                    return tovars;
            }


        }
        private List<tovar> FilterTovar(List<tovar> tovars, ComboBoxItem selectedItem)
        {


            if (selectedItem == null || selectedItem.Tag == null)
                return tovars;

            string tag = selectedItem.Tag.ToString();
            string select = "Все";
             switch (tag)
            {
                case "0": // Все
                    return tovars;
                case "1": // Мужские
                    return tovars.Where(x => x.pol != null && x.pol.pol1 == "Мужская обувь").ToList();
                case "2": // Женские
                    return tovars.Where(x => x.pol != null && x.pol.pol1 == "Женская обувь").ToList();
                default:
                    return tovars;
            }
        }
         
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            ApplyFilter();
        }

        private void cmbsort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();

        }

        private void cmbfilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ApplyFilter();
        }
    }
}



<Page x:Class="dema_ekx.Pages.ManagerPage"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
      xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
      xmlns:local="clr-namespace:dema_ekx.Pages"
      mc:Ignorable="d" 
      Background="White"
      d:DesignHeight="450" d:DesignWidth="800"
      Title="ManagerPage">

    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="40">

            </RowDefinition>
            <RowDefinition Height="*"></RowDefinition>
        </Grid.RowDefinitions>


        <StackPanel  Grid.Row="0" Orientation="Horizontal" Margin="10">
            <TextBox TextWrapping="Wrap" Text="TextBox" Margin="0,0,10,0" Width="120" x:Name="txtsearch" TextChanged="TextBox_TextChanged"/>

            <ComboBox Width="140" x:Name="cmbsort" SelectionChanged="cmbsort_SelectionChanged">
                <ComboBoxItem Tag="0"  Content="без сортировки "></ComboBoxItem>
                <ComboBoxItem Tag="1"  Content="по возрастанию стоимости "></ComboBoxItem>
                <ComboBoxItem Tag="2"  Content="по убыванию стоимости"></ComboBoxItem>
                
            </ComboBox>
            <ComboBox Width="140" x:Name="cmbfilter"   SelectionChanged="cmbfilter_SelectionChanged" Margin="5,0,0,0">

                <ComboBoxItem Tag="0"  Content="Все"></ComboBoxItem>
                <ComboBoxItem Tag="1"  Content="Мужские"></ComboBoxItem>
                <ComboBoxItem Tag="2"  Content="Женские"></ComboBoxItem>
            </ComboBox> 

        </StackPanel>
        <ListView x:Name="listtovar" Grid.Row="1" ItemsSource="{Binding tovar}">

            <ListBox.ItemTemplate  >
                <DataTemplate>
                    <Border BorderBrush="Black" BorderThickness="1" Height="90" Width="300">

                        <StackPanel Orientation="Horizontal">
                            <TextBlock Text="{Binding Artikul }"/> 
                            <TextBlock Text="{Binding Path=postavchik.postavchik1}"/>
                            <TextBlock Text="{Binding tsena}"/> 
                        </StackPanel>
                    </Border>
                </DataTemplate>
                
            </ListBox.ItemTemplate>
            
            

        </ListView>



    </Grid>
</Page>
