using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.Utils
{
    public class ThreadPoolManager
    {

        public ThreadPoolManager()
        {
            ThreadPool.SetMinThreads(5, 5);
        }

    }
}
