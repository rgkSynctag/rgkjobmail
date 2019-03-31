using System.Collections.Generic;

/// <summary>Loads, saves and exposes the data the application uses</summary>
public interface IRepository
{
  Dictionary<int, Skill> Skills { get; }
  Dictionary<int, Person> People { get; }
  Dictionary<int, Task> Tasks { get; }

  string TaskDatasetName { get; }

  void LoadData();
  void SaveAssignments(IEnumerable<Assignment> assignments);
}
