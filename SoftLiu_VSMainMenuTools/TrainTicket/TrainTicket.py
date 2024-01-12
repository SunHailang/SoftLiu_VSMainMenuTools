import requests
import pandas as pd;

import json;

url = 'https://kyfw.12306.cn/otn/leftTicket/queryE?leftTicketDTO.train_date=2024-01-20&leftTicketDTO.from_station=AOH&leftTicketDTO.to_station=SRH&purpose_codes=ADULT'
data = {
    'leftTicketDTO.train_date': '2024-01-20',
    'leftTicketDTO.from_station': 'AOH',
    'leftTicketDTO.to_station': 'SRH',
    'purpose_codes': 'ADULT'
}
headers = {
    'Cookie': '_uab_collina=170493635077645363586254; JSESSIONID=313D02FD92E735C36381B020BACD4CF0; BIGipServerpassport=770179338.50215.0000; guidesStatus=off; highContrastMode=defaltMode; cursorStatus=off; route=c5c62a339e7744272a54643b3be5bf64; BIGipServerotn=3973513482.50210.0000; _jc_save_fromStation=%u4E0A%u6D77%u8679%u6865%2CAOH; _jc_save_toStation=%u5BBF%u5DDE%u4E1C%2CSRH; _jc_save_fromDate=2024-01-20; _jc_save_toDate=2024-01-11; _jc_save_wfdc_flag=dc',
    'User-Agent': 'Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Mobile Safari/537.36'
}

response = requests.get(url=url, headers=headers)

response.encoding = response.apparent_encoding

jsonData = response.json()

print(jsonData)

result = response.json()['data']['result']

print(result)

