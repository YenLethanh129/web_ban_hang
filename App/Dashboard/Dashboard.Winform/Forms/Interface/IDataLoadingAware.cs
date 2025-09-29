using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Winform.Forms.Interface
{
    public interface IDataLoadingAware
    {
        Task WaitForDataLoadingComplete();
    }
}
