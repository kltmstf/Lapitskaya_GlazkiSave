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
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;
        
        public ServicePage()
        {
            InitializeComponent();
            var currentAgent = Lapitskaya_GlazkiSaveEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentAgent;

            ComboSort.SelectedIndex = 0;
            ComboType.SelectedIndex = 0;
            UpdateAgents();
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;

            if (CountRecords % 10 > 0)
                CountPage = CountRecords / 10 + 1;
            else
                CountPage = CountRecords / 10;

            Boolean Ifupdate = true;

            int min;
            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                        CurrentPageList.Add(TableList[i]);
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                                CurrentPageList.Add(TableList[i]);
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;


                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                            Ifupdate = false;
                        break;
                }
            }

            if (Ifupdate)
            {
                PageListBox.Items.Clear();

                for (int i = 1; i <= CountPage; i++)
                    PageListBox.Items.Add(i);

                PageListBox.SelectedIndex = CurrentPage;

                

                AgentListView.ItemsSource = CurrentPageList;
                AgentListView.Items.Refresh();
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Lapitskaya_GlazkiSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                AgentListView.ItemsSource = Lapitskaya_GlazkiSaveEntities.GetContext().Agent.ToList();
            }
            UpdateAgents();
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
            CleanPhoneNumber(p.Phone).Contains(CleanPhoneNumber(TBoxSearch.Text)) ||
            p.Email.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            //отображение фильтра и поиска
            AgentListView.ItemsSource = currentAgent.ToList();

            //сортировка по наименованию, скидке????, приоритету -- ComboSort
            if (ComboSort.SelectedIndex == 1)
                currentAgent = currentAgent.OrderBy(p => p.Title).ToList();
            if (ComboSort.SelectedIndex == 2)
                currentAgent = currentAgent.OrderByDescending(p => p.Title).ToList();
            if (ComboSort.SelectedIndex == 5)
                currentAgent = currentAgent.OrderBy(p => p.Priority).ToList();
            if (ComboSort.SelectedIndex == 6)
                currentAgent = currentAgent.OrderByDescending(p => p.Priority).ToList();

            AgentListView.ItemsSource = currentAgent;
            TableList = currentAgent;

            ChangePage(0, 0);

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

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString())-1);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
        }

        
    }
}
