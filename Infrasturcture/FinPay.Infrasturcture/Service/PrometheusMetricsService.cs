using FinPay.Application.Service;
using Prometheus;
using System.Diagnostics.Metrics;

public class PrometheusMetricsService : IMetricsService
{
    private readonly Dictionary<string, Counter> _counters = new();
    private readonly Dictionary<string, Histogram> _histograms = new();

    public void IncrementCounter(string name, Dictionary<string, string>? labels = null)
    {
        var counter = GetOrCreateCounter(name, labels);
        if (labels == null || labels.Count == 0)
        {
            counter.Inc();
        }
        else
        {
            counter.WithLabels(labels.Values.ToArray()).Inc();
        }
    }

    public void ObserveHistogram(string name, double value, Dictionary<string, string>? labels = null)
    {
        var histogram = GetOrCreateHistogram(name, labels);
        if (labels == null || labels.Count == 0)
        {
            histogram.Observe(value);
        }
        else
        {
            histogram.WithLabels(labels.Values.ToArray()).Observe(value);
        }
    }

    private Counter GetOrCreateCounter(string name, Dictionary<string, string>? labels)
    {
        if (!_counters.TryGetValue(name, out var counter))
        {
            if (labels == null || labels.Count == 0)
            {
                counter = Metrics.CreateCounter(name, $"Counter metric {name}");
            }
            else
            {
                counter = Metrics.CreateCounter(name, $"Counter metric {name}", labels.Keys.ToArray());
            }
            _counters[name] = counter;
        }
        return counter;
    }

    private Histogram GetOrCreateHistogram(string name, Dictionary<string, string>? labels)
    {
        if (!_histograms.TryGetValue(name, out var histogram))
        {
            if (labels == null || labels.Count == 0)
            {
                histogram = Metrics.CreateHistogram(name, $"Histogram metric {name}");
            }
            else
            {
                histogram = Metrics.CreateHistogram(name, $"Histogram metric {name}", labels.Keys.ToArray());
            }
            _histograms[name] = histogram;
        }
        return histogram;
    }
}
