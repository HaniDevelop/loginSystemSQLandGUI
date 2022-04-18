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
    /// Interaction logic for createAccount.xaml
    /// </summary>
    public partial class createAccount : Window
    {
        public createAccount()
        {
            InitializeComponent();
        }

        private async void createBtn_Click(object sender, RoutedEventArgs e)
        {
            if (userNameBoxC.Text.Length == 0)
            {
                warningLabel.Content = "Please create a username";
            }
            else if (passwordBoxC.Password.Length == 0)
            {
                warningLabel.Content = "Please create a password.";
            }
            else if (userNameBoxC.Text.Length < 8)
            {
                warningLabel.Content = "Username must contain at least 8 characters";
            }
            else if(passwordBoxC.Password.Length < 8)
            {
                warningLabel.Content = "Password must contain at least 8 characters";
            }
            else if (passwordBoxC.Password != verifyBox.Password)
            {
                warningLabel.Content = "Passwords do not match.";
            }
            else
            {

                SqlConnection sqlConnection = new SqlConnection(@"Data Source=(localdb)\ProjectModels;Initial Catalog=LoginDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
                
                try
                {
                    sqlConnection.Open();
                    String usernameCheckQuery = "SELECT 1 FROM dbo.userTable WITH(NOLOCK) WHERE username = @username";
                    SqlCommand sqlC = new SqlCommand(usernameCheckQuery, sqlConnection);
                    sqlC.CommandType = System.Data.CommandType.Text;
                    sqlC.Parameters.AddWithValue("@username", userNameBoxC.Text);
                    int exists = Convert.ToInt32(sqlC.ExecuteScalar());
                    if (exists > 0)
                    {
                        warningLabel.Content = "That username already exists.";
                        return;
                    }

                    String query = "SELECT COUNT(*) FROM dbo.userTable";
                    String query2 = "INSERT INTO dbo.userTable VALUES (@userID, @username, @password)";
                    SqlCommand sqlCom = new SqlCommand(query, sqlConnection);
                    SqlCommand sqlCom2 = new SqlCommand(query2, sqlConnection);
                    sqlCom.CommandType = System.Data.CommandType.Text;
                    sqlCom2.CommandType = System.Data.CommandType.Text;
                    int userID = Convert.ToInt32(sqlCom.ExecuteScalar()) + 1;
                    sqlCom2.Parameters.AddWithValue("@userID", userID);
                    sqlCom2.Parameters.AddWithValue("@username", userNameBoxC.Text);
                    sqlCom2.Parameters.AddWithValue("@password", passwordBoxC.Password);
                    sqlCom2.ExecuteNonQuery();

                    warningLabel.Foreground = Brushes.Lime;
                    warningLabel.Content = "Account Created Successfully!";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }

                for (int i = 3; i >= 0; i--)
                {
                    redirecting.Content = "Redirecting in: " + i;
                    await Task.Delay(1000);
                }
                
                loginScreen login = new loginScreen();
                login.Show();
                this.Close();
               
            }
        }
    }
}
