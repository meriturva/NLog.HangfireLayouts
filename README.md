# NLog.HangfireLayouts

[![Latest version](https://img.shields.io/nuget/v/Bonura.HangfireLayouts.svg)](https://www.nuget.org/packages?q=Bonura.HangfireLayouts)

Few hangfire layouts to use with NLog logging library

Installation
-------------

You can install it using the NuGet Package Console window:

```
PM> Install-Package Bonura.HangfireLayouts
```

After installation, update your NLog settings:

```json
"NLog": {
  "extensions": [
    {
      "assembly": "NLog.HangfireLayouts"
    }
  ]
```

Use
-------------
A simple job that just injects a service and uses it.

```csharp
using Sample.Service;

public class SimpleJob
{
    private ITestService _testService;

    public SimpleJob(ITestService testService)
    {
        _testService = testService;
    }

    public async Task DoJobAsync()
    {
        var jobId = await _testService.GetCurrentJobIdAsync();
    }
}
```

A service that use IPerformContextAccessor
```csharp
public class TestService : ITestService
{
    private IPerformContextAccessor _performContextAccessor;

    public TestService(IPerformContextAccessor performContextAccessor)
    {
        _performContextAccessor = performContextAccessor;
    }

    public async Task<string> GetCurrentJobIdAsync()
    {
        var jobId = _performContextAccessor.PerformingContext.BackgroundJob.Id;
        return await Task.FromResult(jobId);
    }
}
```

Motivation
-------------
Access PerformContext (basically to get JobId) on a custom NLog layout (`hangfire-jobid`):

`"layout": "${longdate}|${level:uppercase=true}|${logger}|${message}|${hangfire-jobid}|${exception:format=toString}",`

PS: NLog *HangfireJobIdLayoutRenderer* is coming soon on a different package

Remarks
-------------
As already defined for IHttpContextAccessor

* IPerformContextAccessor interface should be used with caution. As always, the PerformContext must not be captured outside of the Job execution flow.
* IPerformContextAccessor: Relies on System.Threading.AsyncLocal which can have a negative performance impact on asynchronous calls.
* Creates a dependency on "ambient state" which can make testing more difficult.
* IPerformContextAccessor.PerformContext may be null if accessed outside of the Job execution flow.
* To access information from PerformContext outside the Job execution flow, copy the information inside the execution flow. Be careful to copy the actual data and not just references. For example, rather than copying a reference to an IDictionary, copy the relevant header values or copy the entire dictionary key by key before leaving the flow.
* Don't capture IPerformContextAccessor.PerformContext in a constructor.
