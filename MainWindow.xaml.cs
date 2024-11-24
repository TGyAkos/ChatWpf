using MySql.Data.MySqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string ConnectionString = "server=127.0.0.1;uid=root;database=chatapp";
        public MySqlConnection connection;
        public MySqlCommand command;
        public int currentUserId;

        public MainWindow()
        {
            InitializeComponent();
            ConnectToDatabase();

            InitComboBox();
        }

        private void InitComboBox()
        {
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = "SELECT id, username FROM user";
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = reader.GetString("username");
                item.Tag = reader.GetInt32("id");
                user.Items.Add(item);
            }
            connection.Close();
        }

        private void ConnectToDatabase()
        {
            connection = new MySqlConnection(ConnectionString);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (user.SelectedItem is ComboBoxItem selectedItem)
            {
                int userId = (int)selectedItem.Tag;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = $"SELECT content FROM chat WHERE sender_id = {userId} OR to_id = {userId}";

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    chat.Items.Add(reader.GetString("content"));
                }
                connection.Close();

            }
        }
    }
}