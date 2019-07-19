﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.Data
{
    public class Student
    {
        private string[] m_Gender = { "女", "男", "保密" };

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

        public Student(int index, string name, int gender, int age, string phoneNum, string address, string email)
        {
            this.Index = index;
            this.Name = name;
            this.Age = age;
            this.Gender = m_Gender[gender];
            this.PhoneNum = phoneNum;
            this.Address = address;
            this.Email = email;
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
