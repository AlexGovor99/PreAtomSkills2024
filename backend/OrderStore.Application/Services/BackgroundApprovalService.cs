using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderStore.Core.Abstractions;
using OrderStore.Core.Models;

namespace OrderStore.Application.Services;

public class BackgroundApprovalService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly FileConfigOptions _fileConfigOptions;
    private readonly List<Interval> _intervals;
    private DateTime? _lastSendTime;

    public BackgroundApprovalService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _fileConfigOptions = new FileConfigOptions();
        _intervals = LoadJson(_fileConfigOptions.FilePath).ToList();

        Thread.Sleep(TimeSpan.FromSeconds(20));
    } 

    private IEnumerable<Interval> LoadJson(string filePath)
    {
        DateTime? from = null, to = null;

        using StreamReader r = new StreamReader(filePath);
        var line = r.ReadLine();

        while (line != null)
        {
            if (line.Contains("from"))
            {
                var s = line.Split("\"")[3];
                from = DateTime.ParseExact(s, "yyyy-dd-MM HH:mm:ss", null);
            }

            if (line.Contains("to"))
            {
                var s = line.Split("\"")[3];
                to = DateTime.ParseExact(s, "yyyy-dd-MM HH:mm:ss", null);
            }

            if (from.HasValue && to.HasValue)
            {
                yield return new Interval { From = from.Value, To = to.Value };
                from = null;
                to = null;
            }

            line = r.ReadLine();
        }
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken) 
    { 
        while (!cancellationToken.IsCancellationRequested) 
        { 
            await SendAsync(cancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken).ConfigureAwait(false);
        } 
    } 
 
    private async Task SendAsync(CancellationToken cancellationToken)
    {
        var time = DateTime.Now;

        if (_lastSendTime.HasValue && _lastSendTime.Value.AddMinutes(1) > time)
        {
            return;
        }

        if (_intervals.Any(interval => interval.From <= time && time <= interval.To))
        {
            using var scope = _serviceProvider.CreateScope();
            var approvalService = scope.ServiceProvider.GetService<IApprovalService>()!;
            _lastSendTime = await approvalService.SendToApproval();
        }
    } 
}

public class CustomDateTimeConverter : JsonConverter<DateTime>
{
	private readonly string Format;
	public CustomDateTimeConverter(string format)
	{
		Format = format;
	}
	public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
	{
		writer.WriteStringValue(date.ToString(Format));
	}
	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return DateTime.ParseExact(reader.GetString(), Format, null);
	}
}