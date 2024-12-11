using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Agent currentAgent = new Agent();
        public AddEditPage(Agent SelectedAgent)
        {
            InitializeComponent();
            if (SelectedAgent != null) 
                currentAgent = SelectedAgent;

            DataContext = currentAgent;
        }

        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if(myOpenFileDialog.ShowDialog() == true)
            {
                currentAgent.Logo = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentAgent.Title))
                errors.AppendLine("укажите наименование агента");
            if (string.IsNullOrWhiteSpace(currentAgent.Address))
                errors.AppendLine("укажите адрес агента");
            if (string.IsNullOrWhiteSpace(currentAgent.DirectorName))
                errors.AppendLine("укажите ФИО директора");
            if (ComboType.SelectedItem == null)
                errors.AppendLine("укажите тип агента");
            if (string.IsNullOrWhiteSpace(currentAgent.Priority.ToString()))
                errors.AppendLine("укажите приоритет агента");
            if (currentAgent.Priority <= 0)
                errors.AppendLine("укажите положительный приоритет агента");
            if (string.IsNullOrWhiteSpace(currentAgent.INN))
                errors.AppendLine("укажите ИНН агента");
            if (string.IsNullOrWhiteSpace(currentAgent.KPP))
                errors.AppendLine("укажите КПП агента");
            if (string.IsNullOrWhiteSpace(currentAgent.Phone))
                errors.AppendLine("укажите телефон агента");
            else
            {
                string ph = currentAgent.Phone.Replace("(", "").Replace("-", "").Replace("+", "");
                if (((ph[1] == '9' || ph[1] == '4' || ph[1] == '8') && ph.Length != 11)
                    || (ph[1] == '3' && ph.Length != 12))
                    errors.AppendLine("укажите правильно телефон агента");
            }
            if (string.IsNullOrWhiteSpace(currentAgent.Email))
                errors.AppendLine("укажите почту агента");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (currentAgent.ID == 0)
                Lapitskaya_GlazkiSaveEntities.GetContext().Agent.Add(currentAgent);

            try
            {
                Lapitskaya_GlazkiSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var currentAgent = (sender as Button).DataContext as Agent;

            var currentAgentProductSale = Lapitskaya_GlazkiSaveEntities.GetContext().ProductSale.ToList();
            currentAgentProductSale = currentAgentProductSale.Where(p => p.AgentID == currentAgent.ID).ToList();

            if (currentAgentProductSale.Count != 0)
                MessageBox.Show("невозмодно выполнить удаление, так как у агента есть товар на продажу");

            else 
                if (MessageBox.Show("вы точно хотите выполнить удаление?", "внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Lapitskaya_GlazkiSaveEntities.GetContext().Agent.Remove(currentAgent);
                    Lapitskaya_GlazkiSaveEntities.GetContext().SaveChanges();
                    Manager.MainFrame.GoBack();
                    
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
