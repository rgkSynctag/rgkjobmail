public interface IProgressReporter
{
  void ReportStart(string candidate, System.DateTime startTime);
  void ReportLoadedData(IRepository repository);
  void ReportResults(string candidate, System.TimeSpan elapsedTime, System.Collections.Generic.IEnumerable<Assignment> assignments);
  void ReportError(System.Exception e);
}
