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
using System.Net.Http;
using System.Threading;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        HttpClient client = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("http://localhost:56928/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async void SaveTodoListItem(ToDoListItem toDoListItem)
        {
            await client.PostAsJsonAsync("todolists", toDoListItem);
        }

        private async Task GetToDoListItems()
        {
            var response = await client.GetStringAsync("todolists");
            var listItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ToDoListItem>>(response);
            dgTodolist.DataContext = listItems;
        }

        private async Task UpdateStatus(ToDoListItem toDoListItem)
        {
            await client.PutAsJsonAsync("todolists/" + toDoListItem.TaskId, toDoListItem);
        }

        private async Task DeleteTodoListItem(int taskId)
        {
            await client.DeleteAsync("todolists/" + taskId);
        }

        private async void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            await GetToDoListItems();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TxtTaskName.Text != null && TxtTaskName.Text != string.Empty)
            {
                ToDoListItem toDoListItem = new ToDoListItem()
                {
                    TaskId = 0,
                    TaskName = TxtTaskName.Text,
                    TaskDescription = TxtTaskDescription.Text,
                    TaskCompleted = false
                };
                SaveTodoListItem(toDoListItem);
                TxtTaskName.Text = string.Empty;
                TxtTaskDescription.Text = string.Empty;
                MessageBox.Show("Task Added");
                GetToDoListItems();
            }
            else
            {
                MessageBox.Show("Task Name Cannot be empty");
            }
        }

        private async void ChkStatus_Checked(object sender, RoutedEventArgs e)
        {
            ToDoListItem toDoListItem = ((FrameworkElement)sender).DataContext as ToDoListItem;
            toDoListItem.TaskCompleted = true;
            await UpdateStatus(toDoListItem);
            //GetToDoListItems();
        }

        private async void ChkStatus_Unchecked(object sender, RoutedEventArgs e)
        {
            ToDoListItem toDoListItem = ((FrameworkElement)sender).DataContext as ToDoListItem;
            toDoListItem.TaskCompleted = false;
            await UpdateStatus (toDoListItem);
            //GetToDoListItems();
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delte the record?","Delete",MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                ToDoListItem toDoListItem = ((FrameworkElement)sender).DataContext as ToDoListItem;
                await DeleteTodoListItem(Convert.ToInt32(toDoListItem.TaskId));
                await GetToDoListItems();
            }
        }
    }
}
