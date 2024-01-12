using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.TrainTicket
{
    internal class TrainTiketData
    {
        public string SecretKey;
        public string TrainNumber;
        public string FromStation;
        public string ToStation;
        public string FromDate;
        public string ToDate;
        public string DurationDate;
        public string BusinessSeat;
        public string FirstSeat;
        public string SecondSeat;


        public static TrainTiketData GetTrainTicketData(string ticket)
        {
            TrainTiketData data = new TrainTiketData();
            // 0|"EQugtiSWmkXWztYTklqeWso%2BLb9VpbZTxUQrLj2YakTdRvuETJfcrGnqtlvCpUJGeTyQevGOW9oW%0AhryKM4D6pSW1LaXKbQl2304izW4BNNBXIHQXefa%2BZqZX0p2%2BQOYS%2FSVXPc%2BY7waVFNUm8I0JMyAd%0ABB1uORLAt8udQLhmomCqUQZYvOaGpYwn5I2pIVPG5hZ1iAnqoRcNEsND2iUa33Qyhqe9Ci3wCWS9%0A%2FslzFTXxqHBvzmDnBuRqc5Hijh5QeUWejSAqifVV%2B%2BFdsLeS609cezUjMIjcna2VH%2F6D4X%2BiUYM5%0AdG9L3xQOtKCiLPfbgb4NRvI0Ly5ury6hgOXmzIWIWs0%3D
            // 1|预订
            // 2|5l000G180206
            // 3|G1802
            // 4|AOH
            // 5|YIJ
            // 6|AOH
            // 7|SRH
            // 8|06:15
            // 9|08:52
            // 10|02:37
            // 11|Y
            // 12|4nszUyNf%2BLQhbnGsmOmkBeSZ9VT%2FoWZAzf0c6IJbRB%2FfYRzW
            // 13|20240120
            // 14|3
            // 15|HZ
            // 16|01
            // 17|07
            // 18|1
            // 19|0
            // 20|
            // 21|
            // 22|
            // 23|
            // 24|
            // 25|
            // 26|
            // 27|
            // 28|
            // 29|
            // 30|9
            // 31|1
            // 32|无
            // 33|
            // 34|90M0O0
            // |9MO|0|1||9085900000M044400001O026400009|0|||||1|0#1#0#0#z#0#z|||CHN,CHN|||N#N#||90081M0092O0087|202401061330|",
            string[] splitDatas = ticket.Split('|');
            data.SecretKey = splitDatas[0];

            data.TrainNumber = splitDatas[3];
            data.FromStation = splitDatas[4];

            data.FromDate = splitDatas[8];
            data.ToDate = splitDatas[9];
            data.DurationDate = splitDatas[10];

            data.BusinessSeat = splitDatas[32];
            data.FirstSeat = splitDatas[31];
            data.SecondSeat = splitDatas[30];


            return data;
        }
    }
}
