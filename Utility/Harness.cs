using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using manpower;

/// <summary>Used to invoke the planner process and generate output</summary>
/// <remarks>The candidate is not expected to modify this class</remarks>
internal class Harness
{
  private readonly string _candidate;
  private readonly IRepository _repository;
  private readonly ITaskPlanner _taskPlanner;
  private readonly IProgressReporter _progressReporter;

  // results of executing the task planner
  private IEnumerable<Assignment> _assignments;
  private TimeSpan _elapsedTime;
  private Exception _error;

  public Harness(
    string candidate,
    IRepository repository,
    ITaskPlanner taskPlanner,
    IProgressReporter progressReporter)
  {
    _candidate = candidate;
    _repository = repository;
    _taskPlanner = taskPlanner;
    _progressReporter = progressReporter;
  }

  public void Execute()
  {
    try
    {
      _repository.LoadData();

      _progressReporter.ReportStart(_candidate, DateTime.Now);

      _progressReporter.ReportLoadedData(_repository);

      Stopwatch stopwatch = Stopwatch.StartNew();

      _assignments = _taskPlanner.Execute();

      stopwatch.Stop();

      _elapsedTime = stopwatch.Elapsed;

      _progressReporter.ReportResults(_candidate, _elapsedTime, _assignments);
    }
    catch (Exception e)
    {
      _error = e;
      _progressReporter.ReportError(e);

      throw;
    }
  }


}