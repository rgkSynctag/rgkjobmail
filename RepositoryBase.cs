using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using manpower;
using Microsoft.Extensions.Configuration;

/// <summary>An implementation of IRepository that works with .csv files</summary>
internal abstract class RepositoryBase : IRepository
{
  protected RepositoryBase(FileLocations fileLocations)
  {
    FileLocations = fileLocations ?? throw new ArgumentNullException(nameof(fileLocations));
  }

  protected FileLocations FileLocations { get; }

  public Dictionary<int, Skill> Skills { get; private set; }
  public Dictionary<int, Person> People { get; private set; }
  public Dictionary<int, Task> Tasks { get; private set; }
  public string TaskDatasetName { get => FileLocations.Tasks; }

  public abstract void SaveAssignments(IEnumerable<Assignment> assignments);

  public void LoadData()
  {
    Skills = LoadSkills();
    People = LoadPeople(Skills);
    Tasks = LoadTasks(Skills);
  }

  private Dictionary<int, Skill> LoadSkills()
  {
    using (var reader = new StreamReader(FileLocations.Skills))
    using (var csv = new CsvReader(reader))
    {
      return csv.GetRecords<Skill>().ToDictionary(i => i.Id);
    }
  }

  private Dictionary<int, Person> LoadPeople(Dictionary<int, Skill> skills)
  {
    Dictionary<int, Person> people;

    using (var reader = new StreamReader(FileLocations.People))
    using (var csv = new CsvReader(reader))
    {
      people = csv.GetRecords<Person>().ToDictionary(i => i.Id);
    }

    using (var reader = new StreamReader(FileLocations.SkillMatrix))
    using (var csv = new CsvReader(reader))
    {
      var skillMatrixTypeDefinition = new
      {
        PersonId = default(int),
        SkillId = default(int)
      };

      var skillMatrix = csv.GetRecords(skillMatrixTypeDefinition);

      foreach (var item in skillMatrix)
      {
        if (!people.TryGetValue(item.PersonId, out var person))
          throw new InvalidOperationException($"Invalid skills matrix - no person found with id {item.PersonId}");

        if (!skills.TryGetValue(item.SkillId, out var skill))
          throw new InvalidOperationException($"Invalid skills matrix - no skill found with id {item.SkillId}");

        // add skill to person
        person.Skills.Add(skill);
      }
    }

    return people;
  }

  private Dictionary<int, Task> LoadTasks(Dictionary<int, Skill> skills)
  {
    using (var reader = new StreamReader(FileLocations.Tasks))
    using (var csv = new CsvReader(reader))
    {
      var rawTaskDefinition = new
      {
        Id = default(int),
        SkillRequired = default(int),
        IsPriority = default(bool)
      };

      return csv
        .GetRecords(rawTaskDefinition)
        .Select(item =>
        {
          if (!skills.TryGetValue(item.SkillRequired, out var skill))
            throw new InvalidOperationException($"Invalid task list - no skill found with id {item.SkillRequired}");

          return new Task { Id = item.Id, SkillRequired = skill, IsPriority = item.IsPriority };
        })
        .ToDictionary(i => i.Id);
    }
  }
}