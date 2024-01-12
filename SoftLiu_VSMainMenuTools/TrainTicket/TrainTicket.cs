using SoftLiu_VSMainMenuTools.UGUI;
using SoftLiu_VSMainMenuTools.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftLiu_VSMainMenuTools.TrainTicket
{

    public partial class TrainTicket : Form
    {
        // "https://kyfw.12306.cn/otn/leftTicket/queryE?leftTicketDTO.train_date=2024-01-20&leftTicketDTO.from_station=AOH&leftTicketDTO.to_station=SRH&purpose_codes=ADULT";
        // https://kyfw.12306.cn/otn/leftTicket/queryE?leftTicketDTO.train_date={0}&leftTicketDTO.from_station=AOH&leftTicketDTO.to_station=SRH&purpose_codes=ADULT
        private const string queryUrl = "https://kyfw.12306.cn/otn/leftTicket/queryE?leftTicketDTO.train_date={0}&leftTicketDTO.from_station=AOH&leftTicketDTO.to_station=SRH&purpose_codes=ADULT";
        // "_uab_collina=170493635077645363586254; JSESSIONID=313D02FD92E735C36381B020BACD4CF0; BIGipServerpassport=770179338.50215.0000; guidesStatus=off; highContrastMode=defaltMode; cursorStatus=off; route=c5c62a339e7744272a54643b3be5bf64; BIGipServerotn=3973513482.50210.0000; _jc_save_fromStation=%u4E0A%u6D77%u8679%u6865%2CAOH; _jc_save_toStation=%u5BBF%u5DDE%u4E1C%2CSRH; _jc_save_fromDate=2024-01-20; _jc_save_toDate=2024-01-11; _jc_save_wfdc_flag=dc";
        private const string queryCookie = "_uab_collina=170493635077645363586254; JSESSIONID=313D02FD92E735C36381B020BACD4CF0; BIGipServerpassport=770179338.50215.0000; guidesStatus=off; highContrastMode=defaltMode; cursorStatus=off; route=c5c62a339e7744272a54643b3be5bf64; BIGipServerotn=3973513482.50210.0000; _jc_save_fromStation=%u4E0A%u6D77%u8679%u6865%2CAOH; _jc_save_toStation=%u5BBF%u5DDE%u4E1C%2CSRH; _jc_save_fromDate={0}; _jc_save_toDate={1}; _jc_save_wfdc_flag=dc";
        public TrainTicket()
        {
            InitializeComponent();
        }

        private readonly List<TrainTiketData> trainTicketList = new List<TrainTiketData>();

        private readonly DataTable ticketDataTable = new DataTable();

        private void TrainTicket_Load(object sender, EventArgs e)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            ticketGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ticketGridView.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            ticketDataTable.Columns.Add("TrainNum", typeof(string));
            ticketDataTable.Columns.Add("FromToStation", typeof(string));
            ticketDataTable.Columns.Add("FromToDate", typeof(string));
            ticketDataTable.Columns.Add("DurationDate", typeof(string));
            ticketDataTable.Columns.Add("BusinessSeat", typeof(string));
            ticketDataTable.Columns.Add("FirstSeat", typeof(string));
            ticketDataTable.Columns.Add("SecondSeat", typeof(string));
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string fromDate = dateTimeFrom.Value.ToString("yyyy-MM-dd");

            string url = string.Format(queryUrl, fromDate);

            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.Method = "GET";

            // request.Headers.Set("Accept", "*/*");
            //request.Headers.Add("Accept-Encoding", "gzip, deflate, b");
            //request.Headers.Add("Accept-Language", "en-US,en;q=0.9,zh-CN;q=0.8,zh;q=0.7");
            // request.Headers.Set("Content-Type", "application/json;charset=UTF-8");
            //request.Headers.Add("Connection", "keep-alive");
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string cookieVal = string.Format(queryCookie, fromDate, today);
            request.Headers.Add("Cookie", cookieVal);
            //request.Headers.Add("User-Agent", "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Mobile Safari/537.36");

            request.Accept = "*/*";
            request.KeepAlive = true;
            request.ContentType = "application/json";
            //Cookie cookie = new Cookie();            
            //cookie.Value = cookieVal;
            //CookieContainer cookieContainer = new CookieContainer();
            //cookieContainer.Add(cookie);
            //request.CookieContainer = cookieContainer;
            request.Host = "kyfw.12306.cn";
            request.Referer = "https://kyfw.12306.cn/otn/leftTicket/init?linktypeid=dc";
            request.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Mobile Safari/537.36";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                string charSet = response.CharacterSet;
                Console.WriteLine($"CharacterSet: {charSet}");
                var stream = response.GetResponseStream();
                Encoding encoding = Encoding.GetEncoding("GBK");
                if (charSet != "ISO-8859-1")
                {
                    encoding = Encoding.GetEncoding(charSet);
                }
                var sr = new StreamReader(stream, encoding);
                var data = sr.ReadToEnd();

                Dictionary<string, object> deData = MiniJSON.Deserialize(data) as Dictionary<string, object>;
                if (deData.TryGetValue("data", out object queryData))
                {
                    if (queryData is Dictionary<string, object> result)
                    {
                        if (result.TryGetValue("result", out object resultData))
                        {
                            string[] listData = MiniJSON.GetArray<string>(resultData);

                            trainTicketList.Clear();
                            foreach (var item in listData)
                            {
                                var ticketData = TrainTiketData.GetTrainTicketData(item);
                                trainTicketList.Add(ticketData);
                            }
                            ShowGridView();
                        }
                    }
                }
            }
        }

        private void ShowGridView()
        {
            ticketDataTable.Rows.Clear();
            foreach (var ticketData in trainTicketList)
            {
                if (!CanShowTicket(ticketData)) continue;

                var row = ticketDataTable.NewRow();
                row["TrainNum"] = ticketData.TrainNumber;
                row["FromToStation"] = $"{ticketData.FromStation} - {ticketData.ToStation}";
                row["FromToDate"] = $"{ticketData.FromDate} - {ticketData.ToDate}";
                row["DurationDate"] = ticketData.DurationDate;
                row["BusinessSeat"] = ticketData.BusinessSeat;
                row["FirstSeat"] = ticketData.FirstSeat;
                row["SecondSeat"] = ticketData.SecondSeat;
                ticketDataTable.Rows.Add(row);
            }
            ticketGridView.DataSource = ticketDataTable;
            ticketGridView.Refresh();
        }

        private bool CanShowTicket(TrainTiketData ticket)
        {
            return true;
        }

        private void TrainTicket_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormManager.Instance.BackClose();
        }

        private void checkTrainG_CheckedChanged(object sender, EventArgs e)
        {
            ShowGridView();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            TrainTicketLogin login = new TrainTicketLogin();
            login.LoginCompletedCallback += LoginCompletedCallback;
            login.ShowDialog();
        }

        private void LoginCompletedCallback()
        {

        }
    }
}
