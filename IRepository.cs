using System.Collections.Generic;

/// <summary>Loads, saves and exposes the data the application uses</summary>
public interface IRepository
{
  IEnumerable<Skill> Skills { get; }
  IEnumerable<Person> People { get; }
  IEnumerable<Task> Tasks { get; }

  string TaskDatasetName { get; }

  void LoadData();
  void SaveAssignments(IEnumerable<Assignment> assignments);
}
