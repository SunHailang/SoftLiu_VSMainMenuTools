using SoftLiu_VSMainMenuTools.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;

namespace SoftLiu_VSMainMenuTools.TrainTicket
{
    public partial class TrainTicketLogin : Form
    {
        private const string userAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Mobile Safari/537.36";
        // https://kyfw.12306.cn/passport/web/create-qr64
        private const string qrUrl = "https://kyfw.12306.cn/passport/web/create-qr64";

        // https://kyfw.12306.cn/passport/web/checkqr
        private const string qrCheck = "https://kyfw.12306.cn/passport/web/checkqr";

        private string uuid = "";

        public event Action LoginCompletedCallback;

        private StringBuilder textSB = new StringBuilder();

        public TrainTicketLogin()
        {
            InitializeComponent();
        }

        private void TrainTicketLogin_Load(object sender, EventArgs e)
        {
            TimerQueryQR.Enabled = false;
            TimerQueryQR.Interval = 1000;

            GetLogo();

            
        }

        private void GetLogo()
        {
            string url = "https://kyfw.12306.cn/otn/resources/images/logo@2x.png";
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.Method = "GET";
            //var cookies = "_passport_session=b3c6225f49754b08a1a6664b9fa1950b2324; guidesStatus=off; highContrastMode=defaltMode; cursorStatus=off; _jc_save_fromStation=%u4E0A%u6D77%u8679%u6865%2CAOH; _jc_save_toStation=%u5BBF%u5DDE%u4E1C%2CSRH; _jc_save_toDate=2024-01-11; _jc_save_wfdc_flag=dc; _jc_save_fromDate=2024-01-25; route=9036359bb8a8a461c164a04f8f50b252; BIGipServerotn=2698445066.64545.0000; BIGipServerpassport=954728714.50215.0000";
            //request.Headers.Add("Cookie", cookies);
            //request.Host = "kyfw.12306.cn";
            //request.KeepAlive = true;

            //byte[] formData = Encoding.UTF8.GetBytes("appid=otn");

            //request.ContentLength = formData.Length;
            //request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = "https://kyfw.12306.cn/otn/resources/css/public.css";
            request.UserAgent = userAgent;

            //using (Stream newStream = request.GetRequestStream())
            //{
            //    newStream.Write(formData, 0, formData.Length);
            //    newStream.Flush();
            //    newStream.Close();
            //}

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                //string charSet = response.CharacterSet;
                var stream = response.GetResponseStream();
                //Encoding encoding = Encoding.GetEncoding("GBK");
                //if (charSet != "ISO-8859-1")
                //{
                //    encoding = Encoding.GetEncoding(charSet);
                //}
                //var sr = new StreamReader(stream, encoding);
                //var data = sr.ReadToEnd();
                Bitmap map = new Bitmap(stream);
                pictureLogo.Image = map;

            }
        }

        private void SendQrUrl()
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(qrUrl);
            request.Method = "POST";
            var cookies = "_passport_session=b3c6225f49754b08a1a6664b9fa1950b2324; guidesStatus=off; highContrastMode=defaltMode; cursorStatus=off; _jc_save_fromStation=%u4E0A%u6D77%u8679%u6865%2CAOH; _jc_save_toStation=%u5BBF%u5DDE%u4E1C%2CSRH; _jc_save_toDate=2024-01-11; _jc_save_wfdc_flag=dc; _jc_save_fromDate=2024-01-25; route=9036359bb8a8a461c164a04f8f50b252; BIGipServerotn=2698445066.64545.0000; BIGipServerpassport=954728714.50215.0000";
            request.Headers.Add("Cookie", cookies);
            request.Host = "kyfw.12306.cn";
            request.KeepAlive = true;

            byte[] formData = Encoding.UTF8.GetBytes("appid=otn");

            request.ContentLength = formData.Length;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = "https://kyfw.12306.cn/otn/resources/login.html";
            request.UserAgent = userAgent;

            using (Stream newStream = request.GetRequestStream())
            {
                newStream.Write(formData, 0, formData.Length);
                newStream.Flush();
                newStream.Close();
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                string charSet = response.CharacterSet;
                var stream = response.GetResponseStream();
                Encoding encoding = Encoding.GetEncoding("GBK");
                if (charSet != "ISO-8859-1")
                {
                    encoding = Encoding.GetEncoding(charSet);
                }
                var sr = new StreamReader(stream, encoding);
                var data = sr.ReadToEnd();
                Dictionary<string, object> jsonData = MiniJSON.Deserialize(data) as Dictionary<string, object>;
                string image = jsonData["image"].ToString();
                //string message = jsonData["result_message"];
                //string code = jsonData["result_code"];
                uuid = jsonData["uuid"].ToString();
                byte[] imageBytes = Convert.FromBase64String(image);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    Bitmap map = new Bitmap(ms);
                    pictureQR.Image = map;
                }
            }
            // 开启定时器查询
            TimerQueryQR.Enabled = true;
        }

        private void TimerQueryQR_Tick(object sender, EventArgs e)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(qrUrl);
            request.Method = "POST";
            var cookies = "_passport_session=7b764fcb01424a3397bb2fe014e57b4a6007; guidesStatus=off; highContrastMode=defaltMode; cursorStatus=off; _jc_save_fromStation=%u4E0A%u6D77%u8679%u6865%2CAOH; _jc_save_toStation=%u5BBF%u5DDE%u4E1C%2CSRH; _jc_save_toDate=2024-01-11; _jc_save_wfdc_flag=dc; _jc_save_fromDate=2024-01-25; route=c5c62a339e7744272a54643b3be5bf64; BIGipServerotn=1927807242.24610.0000; BIGipServerpassport=786956554.50215.0000";
            request.Headers.Add("Cookie", cookies);
            request.Host = "kyfw.12306.cn";
            request.KeepAlive = true;

            string formStr = $"uuid={uuid}&appid=otn";
            byte[] formData = Encoding.UTF8.GetBytes(formStr);

            request.ContentLength = formData.Length;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = "https://kyfw.12306.cn/otn/resources/login.html";
            request.UserAgent = userAgent;

            using (Stream newStream = request.GetRequestStream())
            {
                newStream.Write(formData, 0, formData.Length);
                newStream.Flush();
                newStream.Close();
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                string charSet = response.CharacterSet;
                var stream = response.GetResponseStream();
                Encoding encoding = Encoding.GetEncoding("GBK");
                if (charSet != "ISO-8859-1")
                {
                    encoding = Encoding.GetEncoding(charSet);
                }
                var sr = new StreamReader(stream, encoding);
                var data = sr.ReadToEnd();
                Dictionary<string, object> jsonData = MiniJSON.Deserialize(data) as Dictionary<string, object>;
                string message = jsonData["result_message"].ToString();
                string code = jsonData["result_code"].ToString();
                switch (code)
                {
                    case "0": //二维码状态查询成功
                        {
                            string time = DateTime.Now.ToString("HH:mm:ss");
                            textSB.Append($"{time} - {code} : {message}");
                            richTextBoxLogin.Text = textSB.ToString();
                        }
                        break;
                    case "3": // 二维码已过期
                        {
                            TimerQueryQR.Enabled = false;
                        }
                        break;
                }
            }
        }

        private void tabControlLogin_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControlLogin.SelectedIndex)
            {
                case 0: // 用户登录
                    {
                        TimerQueryQR.Enabled = false;
                    }
                    break;
                case 1: // 扫码登录
                    {
                        SendQrUrl();
                    }
                    break;
            }
        }

        private void btnQRRefresh_Click(object sender, EventArgs e)
        {
            SendQrUrl();
        }
    }
}
