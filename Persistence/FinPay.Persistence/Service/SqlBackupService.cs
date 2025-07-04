using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FinPay.Persistence.Service
{
    public class SqlBackupService : BackgroundService
    {
        private readonly IConfiguration _configuration;

        public SqlBackupService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var dbName = _configuration["DatabaseBackupSettings:DatabaseName"];
            var backupPath = _configuration["DatabaseBackupSettings:BackupFolderPath"];
            var intervalHoursStr = _configuration["DatabaseBackupSettings:IntervalInHours"];

            if (!int.TryParse(intervalHoursStr, out int intervalHours))
                intervalHours = 24; 

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await BackupDatabaseAsync(dbName, backupPath);
                    Console.WriteLine($"[{DateTime.Now}] Backup alındı.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Backup zamanı xəta: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromHours(intervalHours), stoppingToken);
            }
        }


        public async Task BackupDatabaseAsync(string databaseName, string backupFolderPath)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentException("Database name can't be empty.", nameof(databaseName));

            if (string.IsNullOrWhiteSpace(backupFolderPath))
                throw new ArgumentException("Backup folder path can't be empty.", nameof(backupFolderPath));

            if (!Directory.Exists(backupFolderPath))
                Directory.CreateDirectory(backupFolderPath);

            string backupFileName = $"{databaseName}_Backup_{DateTime.Now:yyyyMMddHHmmss}.backup";
            string backupFilePath = Path.Combine(backupFolderPath, backupFileName);

            var connectionString = _configuration.GetConnectionString("default");
            var builder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString);

            string pgDumpPath = @"D:\PostgreSQL\bin\pg_dump.exe";


            var arguments = $"-U {builder.Username} -h {builder.Host} -p {builder.Port} -F c -b -v -f \"{backupFilePath}\" {databaseName}";

            var startInfo = new ProcessStartInfo
            {
                FileName = pgDumpPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            startInfo.EnvironmentVariables["PGPASSWORD"] = builder.Password;

            using var process = new Process { StartInfo = startInfo };

            process.Start();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
                throw new Exception($"Backup alınarkən xəta: {error}");

            Console.WriteLine(output);
        }
    }
}
