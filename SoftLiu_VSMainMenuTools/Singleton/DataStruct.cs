using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.Utils
{
    class DataStruct<T> : IDisposable
    {
        public delegate void DataStructCallbackDelegate(params object[] mParams);

        private T m_beforValue;
        private T m_currentValue;

        private DataStructCallbackDelegate m_callback = null;

        public T Value
        {
            get { return m_currentValue; }
        }

        public DataStruct(T value, DataStructCallbackDelegate callback)
        {
            m_beforValue = value;
            m_currentValue = value; ;

            m_callback = callback;
        }

        public void SetValue(T value, params object[] mParams)
        {
            m_currentValue = value;
            if (!m_beforValue.Equals(m_currentValue))
            {
                m_callback(mParams);
            }
            m_beforValue = m_currentValue;
        }



        public void Dispose()
        {
            m_beforValue = default(T);
            m_currentValue = default(T);

            m_callback = null;
        }
    }
}
