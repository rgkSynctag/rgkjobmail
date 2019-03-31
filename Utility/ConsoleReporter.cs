using System;
using System.Collections.Generic;
using System.Linq;

internal class ConsoleReporter : IProgressReporter
{
  private bool _verbose;

  public ConsoleReporter(bool verbose)
  {
    _verbose = verbose;
  }

  public void ReportStart(string candidate, DateTime startTime)
  {
    Console.WriteLine("\n===========================================");
    Console.WriteLine($"Manpower planner executed on {startTime:g}");
    Console.WriteLine($"\nCandidate: {candidate}");
    Console.WriteLine("===========================================");
  }

  public void ReportLoadedData(IRepository repository)
  {
    Console.WriteLine("\nLoaded data");
    Console.WriteLine($"Task dataset: {repository.TaskDatasetName}\n");

    if (_verbose)
    {
      ReportAllLoadedData();
    }
    else
    {
      ReportLoadedDataSummary();
    }

    void ReportAllLoadedData()
    {
      Console.WriteLine("Skills:\n");
      foreach (var skill in repository.Skills.Values)
      {
        Console.WriteLine(skill);
      }

      Console.WriteLine("\n\nPeople:\n");
      foreach (var person in repository.People.Values)
      {
        Console.WriteLine(person);
      }

      Console.WriteLine("\n\nTasks:\n");
      foreach (var task in repository.Tasks.Values)
      {
        Console.WriteLine(task);
      }
    }

    void ReportLoadedDataSummary()
    {
      Console.WriteLine($"{repository.Skills.Count} Skills");
      Console.WriteLine($"{repository.People.Count} People");
      Console.WriteLine($"{repository.Tasks.Count} Tasks");
    }
  }

  public void ReportResults(string candidate, TimeSpan elapsedTime, IEnumerable<Assignment> assignments)
  {
    Console.WriteLine("\n\n===========================================");
    Console.WriteLine($"RESULTS for candidate {candidate}\n");
    Console.WriteLine($"{assignments.Count()} allocation(s).");

    Console.WriteLine($"\nExecution time: {elapsedTime.TotalMilliseconds} ms");
    Console.WriteLine("===========================================\n");
  }

  public void ReportError(Exception e)
  {
    Console.WriteLine("\nSomething went wrong :(\n");
    Console.WriteLine(e.Message);
    Console.WriteLine("\n\n");
  }
}
