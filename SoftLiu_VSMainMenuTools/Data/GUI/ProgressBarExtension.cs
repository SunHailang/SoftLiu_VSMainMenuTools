using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftLiu_VSMainMenuTools.Data.GUI
{
    class ProgressBarExtension
    {
        private ProgressBar m_progressBar = null;

        private int m_value = 0;

        public System.Action<int> onChanged = null;

        public int Value
        {
            set
            {
                if (m_value != value)
                {
                    m_progressBar.Value = value;
                    m_value = m_progressBar.Value;
                    if (onChanged != null)
                    {
                        onChanged(m_progressBar.Value);
                    }
                }
            }
            get
            {
                return m_progressBar.Value;
            }
        }

        public ProgressBarExtension(ProgressBar bar)
        {
            m_progressBar = bar;
        }
    }
}
