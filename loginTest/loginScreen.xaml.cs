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

namespace loginTest
{
    /// <summary>
    /// Interaction logic for loginScreen.xaml
    /// </summary>
    public partial class loginScreen : Window
    {
        public loginScreen()
        {
            InitializeComponent();
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(@"Data Source=(localdb)\ProjectModels;Initial Catalog=LoginDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;") ;
            try
            {
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                String query = "SELECT COUNT(1) FROM dbo.userTable WHERE username=@username AND password=@password";
                SqlCommand sqlCom = new SqlCommand(query, sqlConnection);
                sqlCom.CommandType = System.Data.CommandType.Text;
                sqlCom.Parameters.AddWithValue("@username", userNameBox.Text);
                sqlCom.Parameters.AddWithValue("@password", passwordBox.Password);
                int count = Convert.ToInt32(sqlCom.ExecuteScalar());
                if (count == 1)
                {
                    MainWindow dashboard = new MainWindow();
                    dashboard.Show();
                    sqlConnection.Close();
                    this.Close();
                }
                else
                {
                    warningLabel.Content = "Username or Password is incorrect";
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void accountBtn_Click(object sender, RoutedEventArgs e)
        {
            createAccount create = new createAccount();
            create.Show();
            this.Close();
        }
    }
}
