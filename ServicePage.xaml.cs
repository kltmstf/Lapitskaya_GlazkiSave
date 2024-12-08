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

namespace Lapitskaya_GlazkiSave
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        public ServicePage()
        {
            InitializeComponent();
            var currentAgent = Lapitskaya_GlazkiSaveEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentAgent;

            ComboSort.SelectedIndex = 0;
            ComboType.SelectedIndex = 0;
        }

        private void UpdateAgents()
        {
            var currentAgent = Lapitskaya_GlazkiSaveEntities.GetContext().Agent.ToList();
            
            //сортировка по типу -- ComboType
            if (ComboType.SelectedIndex == 1)
                currentAgent = currentAgent.Where(p => p.AgentTypeText == "МФО").ToList();
            if (ComboType.SelectedIndex == 2)
                currentAgent = currentAgent.Where(p => p.AgentTypeText == "ООО").ToList();
            if (ComboType.SelectedIndex == 3)
                currentAgent = currentAgent.Where(p => p.AgentTypeText == "ЗАО").ToList();
            if (ComboType.SelectedIndex == 4)
                currentAgent = currentAgent.Where(p => p.AgentTypeText == "МКК").ToList();
            if (ComboType.SelectedIndex == 5)
                currentAgent = currentAgent.Where(p => p.AgentTypeText == "ОАО").ToList();
            if (ComboType.SelectedIndex == 6)
                currentAgent = currentAgent.Where(p => p.AgentTypeText == "ПАО").ToList();
            
            // функция для очистки строки от нежелательных символов
            string CleanPhoneNumber(string phoneNumber)
            {
                return phoneNumber.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            }

            //поиск по наименованию, телефону и email????
            currentAgent = currentAgent.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()) || 
            CleanPhoneNumber(p.Phone).Contains(CleanPhoneNumber(TBoxSearch.Text))).ToList();
            //отображение фильтра и поиска
            AgentListView.ItemsSource = currentAgent.ToList();

            //сортировка по наименованию, скидке????, приоритету -- ComboSort
            if (ComboSort.SelectedIndex == 1)
                AgentListView.ItemsSource = currentAgent.OrderBy(p => p.Title).ToList();
            if (ComboSort.SelectedIndex == 2)
                AgentListView.ItemsSource = currentAgent.OrderByDescending(p => p.Title).ToList();
            if (ComboSort.SelectedIndex == 5)
                AgentListView.ItemsSource = currentAgent.OrderBy(p => p.Priority).ToList();
            if (ComboSort.SelectedIndex == 6)
                AgentListView.ItemsSource = currentAgent.OrderByDescending(p => p.Priority).ToList();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }
    }
}
