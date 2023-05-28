using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Unicode;
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

namespace EuroGUI
{
    public partial class MainWindow : Window
    {

        // 1. feladat: CREATE DATABASE eurovizio DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;

        private readonly string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=eurovizio;";
        private  MySqlConnection connection;
        ObservableCollection<Song> songs = new ObservableCollection<Song>();


        public MainWindow()
        {
            InitializeComponent();
            SQLOpen();

            string sqlDataGrid = "SELECT * FROM dal";
            MySqlCommand sqlDatagridCommand = new MySqlCommand(sqlDataGrid, connection);
            MySqlDataReader sqlDataReader = sqlDatagridCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                Song song = new Song(sqlDataReader.GetInt32("ev"), sqlDataReader.GetString("eloado"), sqlDataReader.GetString("cim"), sqlDataReader.GetInt32("helyezes"), sqlDataReader.GetInt32("pontszam"));
                songs.Add(song);
            }
            sqlDataReader.Close();
            SQLClose();
            dgSong.ItemsSource = songs;
            dgSong.SelectedIndex = 0;
        }

        private void SQLOpen()
        {
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
            }
            catch(Exception ex) {
                MessageBox.Show("Nem lehet kapcsolódni az adatbázishoz!");
            }
        }

        private void SQLClose()
        {
            connection.Close();
            connection.Dispose();
        }

        private void btnTask4_Click(object sender, RoutedEventArgs e)
        {
            SQLOpen();

            string sqlDataGrid = "SELECT COUNT(*), MIN(helyezes) FROM dal WHERE orszag='Magyarország'";
            MySqlCommand sqlDatagridCommand = new MySqlCommand(sqlDataGrid, connection);
            MySqlDataReader sqlDataReader = sqlDatagridCommand.ExecuteReader();

            while(sqlDataReader.Read())
            {
                string db = sqlDataReader.GetValue(0).ToString();
                string minPlace = sqlDataReader.GetValue(1).ToString();
                MessageBox.Show($"{db} darab magyar versenyző van és a legkisebb helyezés amit elértek a {minPlace}.");
            }
            
            sqlDataReader.Close();
            SQLClose();
        }

        private void btnTask5_Click(object sender, RoutedEventArgs e)
        {
            SQLOpen();

            string sqlDataGrid = "SELECT AVG(pontszam) FROM dal WHERE orszag='Németország'";
            MySqlCommand sqlDatagridCommand = new MySqlCommand(sqlDataGrid, connection);
            MySqlDataReader sqlDataReader = sqlDatagridCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                string score = sqlDataReader.GetValue(0).ToString();
                MessageBox.Show($"Németország átlagosan {Math.Round(double.Parse(score), 2)} pontot ért el a versenyek alatt.");
            }

            sqlDataReader.Close();
            SQLClose();
        }

        private void btnTask6_Click(object sender, RoutedEventArgs e)
        {
            SQLOpen();

            string sqlDataGrid = "SELECT eloado, cim FROM dal WHERE cim LIKE '%Luck'";
            MySqlCommand sqlDatagridCommand = new MySqlCommand(sqlDataGrid, connection);
            MySqlDataReader sqlDataReader = sqlDatagridCommand.ExecuteReader();

            string result = "";
            while (sqlDataReader.Read())
            {
                result += $"{sqlDataReader.GetValue(0)} - {sqlDataReader.GetValue(1)}, ";
            }

            MessageBox.Show(result);

            sqlDataReader.Close();
            SQLClose();
        }

        private void btnTask7_Click(object sender, RoutedEventArgs e)
        {
            SQLOpen();

            string sqlDataGrid = $"SELECT cim FROM dal WHERE eloado LIKE '%{txtName.Text}%' ORDER BY eloado ASC, cim ASC";
            MySqlCommand sqlDatagridCommand = new MySqlCommand(sqlDataGrid, connection);
            MySqlDataReader sqlDataReader = sqlDatagridCommand.ExecuteReader();

            ObservableCollection<string> results = new ObservableCollection<string>();
            while (sqlDataReader.Read())
            {
                results.Add(sqlDataReader.GetValue(0).ToString());
            }

            lbResults.ItemsSource = results;

            sqlDataReader.Close();
            SQLClose();
        }

        private void btnTask8_Click(object sender, RoutedEventArgs e)
        {
            SQLOpen();

            string sqlDataGrid = $"SELECT datum FROM verseny WHERE ev = {songs[dgSong.SelectedIndex].Year}";
            MySqlCommand sqlDatagridCommand = new MySqlCommand(sqlDataGrid, connection);
            MySqlDataReader sqlDataReader = sqlDatagridCommand.ExecuteReader();

            string result = "";
            while (sqlDataReader.Read())
            {
                result = sqlDataReader.GetValue(0).ToString().Substring(0, 13);
            }

            lbDate.Content = result;

            sqlDataReader.Close();
            SQLClose();
        }
    }
}
