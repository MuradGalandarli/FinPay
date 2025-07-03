using FinPay.Application.Service;
using Microsoft.Extensions.Hosting;

public class AlertWorker : BackgroundService, IDisposable
{
    private readonly IMailSender _mailSender;
    private readonly HttpClient _httpClient = new();

    private const string MetricsUrl = "https://localhost:7090/metrics";

    public AlertWorker(IMailSender mailSender)
    {
        _mailSender = mailSender;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var response = await _httpClient.GetAsync(MetricsUrl, stoppingToken);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync(stoppingToken);


                if (IsCpuUsageHigh(content))
                {
                    await _mailSender.SendMail("CPU istifadəsi yüksəkdir!", content);
                    Console.WriteLine($"{DateTime.Now}: Alert göndərildi.");
                }
                else
                {
                    Console.WriteLine($"{DateTime.Now}: CPU istifadəsi normaldır.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Xəta baş verdi: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
        }
    }

    private bool IsCpuUsageHigh(string metrics)
    {
        foreach (var line in metrics.Split('\n'))
        {
            if (line.StartsWith("cpu_usage"))
            {
                var parts = line.Split(' ');
                if (parts.Length == 2 && double.TryParse(parts[1], out double cpu))
                    return cpu > 80;
            }
        }
        return false;
    }

    public override void Dispose()
    {
        _httpClient.Dispose();
        base.Dispose();
    }
}
