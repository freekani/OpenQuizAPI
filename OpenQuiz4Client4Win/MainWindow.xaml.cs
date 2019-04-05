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
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Net.Http;

namespace OpenQuiz4Client4Win
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Quiz4> quizlist = new List<Quiz4>();
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(1);
                var response = await client.GetAsync("http://openquizapi.com/api/quiz4"); // GET

                var responsejson = await response.Content.ReadAsStringAsync();

                var quizidlist = JsonConvert.DeserializeObject<List<int>>(responsejson);
                List<HttpResponseMessage> quizresponse = new List<HttpResponseMessage>();
                foreach(int quizid in quizidlist)
                {
                    quizresponse.Add(await client.GetAsync("http://openquizapi.com/api/quiz4/" + quizid));
                }
                    
                    
               
                quizresponse.ForEach(async quizbody=>{
                    quizlist.Add(JsonConvert.DeserializeObject<Quiz4>(await quizbody.Content.ReadAsStringAsync()));
                });
                this.mainTable.ItemsSource = quizlist;
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
