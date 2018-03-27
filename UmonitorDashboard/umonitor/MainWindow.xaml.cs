using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Windows.Threading;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace umonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        DispatcherTimer timer;
        string response;
        ListViewItem item;
        JArray responseArray;
        JObject resObj;
        String msgT = "";

        public static double tTemp = 40.0;
        public static double tSatu = 100.0;
        public static double tBp = 128.0;
        public static double tBpD = 90.0;
        public static double tPulse = 90.0;

        public static double tLTemp = 37.0;
        public static double tLSatu = 80.0;
        public static double tLBp = 120.0;
        public static double tLBpD = 80.0;
        public static double tLPulse = 70.0;




        public MainWindow()
        {
            InitializeComponent();

            item = new ListViewItem();
            item.ContentTemplate = (DataTemplate)this.FindResource("DataTemplate1");
            listView.Items.Add(item);

            ListViewItem item2 = new ListViewItem();
            item2.ContentTemplate = (DataTemplate)this.FindResource("DataTemplate1");
            //listView.Items.Add(item2);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

           

            

            
           

        }
        void timer_Tick(object sender, EventArgs e)
        {
            /* backgroundWorker = new BackgroundWorker();
             backgroundWorker.DoWork +=
                 new DoWorkEventHandler(backgroundWorker_DoWork);
             backgroundWorker.RunWorkerAsync();*/
            GetRequest();
            GetRequestBpPulse();
           
            // textBox.Background = Brushes.Red;

        }
        private void backgroundWorker_DoWork(object sender,
            DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            // Assign the result of the computation
            // to the Result property of the DoWorkEventArgs
            // object. This is will be available to the 
            // RunWorkerCompleted eventhandler.

            responseArray = getJsonFromFirebase();
            e.Result = responseArray;

        }

        // This event handler deals with the results of the
        // background operation.
        private void backgroundWorker_RunWorkerCompleted(
            object sender, RunWorkerCompletedEventArgs e)
        {
            //textBox.Text = responseArray.ToString();
        }

        // This event handler updates the progress bar.
        private void backgroundWorker_ProgressChanged(object sender,
            ProgressChangedEventArgs e)
        {
           
        }



        // This is the method that does the actual work. For this

        public async void GetRequest()
        {
            msgText.Clear();
            msgT = "";
            Uri geturi = new Uri("https://umonitor-b1291.firebaseio.com/periodAssess.json?orderBy=\"$key\"&limitToLast=1&auth=iJeQqe3VH3BC0qiUG9kKXTbG1baklajlfbr6bcpw"); //replace your url  
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage responseGet = await client.GetAsync(geturi);
             response = await responseGet.Content.ReadAsStringAsync();
            resObj = JObject.Parse(response);

            string t=resObj.First.Children().First().ToString();
            JObject obj = JObject.Parse(t);
            string te= obj.GetValue("temp").ToString();
            string temp = te + "Celcius";
            double t1 = Double.Parse(te);
            if ((t1 < tLTemp))
            {
               // msgGrid.Visibility = Visibility.Visible;
                //msgText.Clear();
                msgT += "Temperature Low\n";
            }else if((t1 > tTemp))
            {
                //msgText.Clear();
               // msgGrid.Visibility = Visibility.Visible;
                msgT= "Temperature High\n";
            }

            string satu1 = obj.GetValue("satu").ToString();
            string satu = satu1 + "%";
            double s = Double.Parse(satu1);
            if ((s < tLSatu) )
            {
               // msgText.Clear();
               // msgGrid.Visibility = Visibility.Visible;
                msgT += "Saturation Low\n";
            }else if ((s > tSatu))
            {
                //msgText.Clear();
               // msgGrid.Visibility = Visibility.Visible;
                msgT+= "Saturation High\n";
            }
            string bedNo = "Bed No. "+obj.GetValue("bedNo").ToString();

            //textBox.Background = Brushes.Green;

            //setting data to datatemplate controls

            item = new ListViewItem();

            //DataTemplate d1=FindResource("Data")
            tempText.Text = temp;
            satuText.Text = satu;
            nameText.Text = bedNo;

            //Console.WriteLine(response.ToString());
        }

        public async void GetRequestBpPulse()
        {

            Uri geturi = new Uri("https://umonitor-b1291.firebaseio.com/bpData.json?orderBy=\"$key\"&limitToLast=1&auth=iJeQqe3VH3BC0qiUG9kKXTbG1baklajlfbr6bcpw"); //replace your url  
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage responseGet = await client.GetAsync(geturi);
            response = await responseGet.Content.ReadAsStringAsync();
            resObj = JObject.Parse(response);

            string t = resObj.First.Children().First().ToString();
            JObject obj = JObject.Parse(t);
            string bp11 = obj.GetValue("bp1").ToString();
            string bp1 = bp11 + "mmHg";
            double b1 = Double.Parse(bp11);
            if ((b1 < tLBp))
            {
                //msgText.Clear();
                //msgGrid.Visibility = Visibility.Visible;
                msgT += "SYS BP Low\n";
            }
            else if ((b1 > tBp))
            {
                //msgText.Clear();
               // msgGrid.Visibility = Visibility.Visible;
                msgT+= "SYS BP High\n";
            }
            string bp22 = obj.GetValue("bp2").ToString();
            string bp2 = bp22 + "mmHg";
            double b2 = Double.Parse(bp22);
            if ((b2 < tLBpD))
            {
                //msgText.Clear();
               // msgGrid.Visibility = Visibility.Visible;
                msgT += "DIA BP Low\n";
            }
            else if ((b2 > tBp))
            {
                //msgText.Clear();
                //msgGrid.Visibility = Visibility.Visible;
                msgT += "DIA BP High\n";
            }
            string pu = obj.GetValue("pulse").ToString();
            string pulse = pu + "pulse/min";
            double p1 = Double.Parse(pu);
            if ((p1 < tLPulse))
            {
                //msgText.Clear();
               // msgGrid.Visibility = Visibility.Visible;
                msgT += "Pulse Low\n";
            }
            else if ((p1 > tPulse))
            {
               // msgText.Clear();
               // msgGrid.Visibility = Visibility.Visible;
                msgT += "Pulse High\n";
            }
            string bedNo = "Bed No. " + obj.GetValue("bedNo").ToString();

            //textBox.Background = Brushes.Green;

            //setting data to datatemplate controls

            item = new ListViewItem();

            //DataTemplate d1=FindResource("Data")
            bpText.Text = "SYS "+bp1+"\nDIA "+bp2;
           pulseText.Text = pulse;
            nameText.Text = bedNo;

            if (!msgT.Equals(""))
                msgGrid.Visibility = Visibility.Visible;
            msgText.Text = msgT;
            msgT = "";
            //Console.WriteLine(response.ToString());
        }

        public JArray getJsonFromFirebase()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://umonitor-b1291.firebaseio.com/.json?auth=iJeQqe3VH3BC0qiUG9kKXTbG1baklajlfbr6bcpw");

            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string content = string.Empty;
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    content = sr.ReadToEnd();
                }
            }

            var releases = JArray.Parse(content);
            return releases;
        }

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            EcoThresholds eco= new EcoThresholds();
            eco.Visibility = Visibility.Visible;
        }
    }
}
