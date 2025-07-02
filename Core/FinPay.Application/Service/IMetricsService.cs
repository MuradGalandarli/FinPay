using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Service
{
    public interface IMetricsService
    {
        void IncrementCounter(string name, Dictionary<string, string> labels = null);
        void ObserveHistogram(string name, double value, Dictionary<string, string> labels = null);
    }
}
