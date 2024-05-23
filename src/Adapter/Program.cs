using Adapter.Config;
using Adapter.Extensions;

var builder = WebApplication.CreateBuilder(args);
var alertsAdapterConfiguration = new AlertsAdapterConfiguration();

builder.Configuration.Bind("AlertsAdapter", alertsAdapterConfiguration);
alertsAdapterConfiguration.Validate();

builder.RegisterServices(alertsAdapterConfiguration);

var app = builder.Build().Configure();

app.Run();
