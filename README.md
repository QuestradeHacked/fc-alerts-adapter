# Fincrime Alerts Adapter

This repository is a microservice for Alerts Adapter.

## Purpose

This application receives files with customer data and all related entities for easy ingestion into ActOne for investigation.

## Datadog Dashboard

https://app.datadoghq.com/dashboard/5ng-q3b-mxa/financial-crime-alerts-adapter

### Monitors

https://app.datadoghq.com/monitors/manage?q=team%3Atmj%20tag%3A%28%22service%3Aalerts-adapter%22%29&order=desc

### Health Check

https://fc-alerts-adapter-default.{env}.q3.questech.io/healthz

## Team Contact Information

Slack Channel: #team-tmj

Alerts Channel: #fincrime-fraud-aml-alerts

Email group: questrade-scrumteam-tmj@questrade.com

### Running Unit Test

```
dotnet test src/Tests/Unit/Unit.csproj
```

### Running Integration Tests

```
dotnet test src/Tests/Integration/Integration.csproj
```

# More information

- https://questrade.atlassian.net/wiki/spaces/FINCRIME/pages/315031786/FinCrime+Actimize+Modules+Easy+Ingest
