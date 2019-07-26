using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.Data
{
    public class Student
    {
        private string[] m_Gender = { "女", "男", "保密" };
        private string[] m_IsDelete = { "No", "Yes" };

        public int Index
        {
            private set;
            get;
        }
        public string Name
        {
            private set;
            get;
        }
        private int m_gender = 0;
        public string Gender
        {
            private set;
            get;
        }
        public int Age
        {
            private set;
            get;
        }
        public string PhoneNum
        {
            private set;
            get;
        }
        public string CardID
        {
            private set;
            get;
        }
        private string m_Address = string.Empty;
        public string Address
        {
            private set;
            get;
        }
        public string Email
        {
            private set;
            get;
        }

        public string IsDelete
        {
            private set;
            get;
        }

        public Student(int index, string name, int gender, int age, string phoneNum, string address, string email, string cardID, int isDelete)
        {
            this.Index = index;
            this.Name = name;
            this.Age = age;
            this.m_gender = gender;
            this.Gender = this.m_Gender[gender];
            this.PhoneNum = phoneNum;
            this.m_Address = address;
            this.Address = address.Replace("$", "");
            this.Email = email;
            this.CardID = cardID;
            this.IsDelete = this.m_IsDelete[isDelete];
        }

        public string GetAddressFromDatabase()
        {
            return this.m_Address;
        }

        public int GetGenderFromDatabase()
        {
            return this.m_gender;
        }

        private int DeleteStudent()
        {
            return 0;
        }

        private int ModifyStudent()
        {
            return 0;
        }

    }
}
