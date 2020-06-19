using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace manpower
{
    public class TaskPlanner : ITaskPlanner
    {
        IRepository _repository;
        public TaskPlanner(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Assignment> Execute()
        {
            List<Assignment> assignments = new List<Assignment>();
            int day = 1;
            foreach(var task in _repository.Tasks.OrderByDescending(x => x.IsPriority).ToList()) //Take priority tasks first.
            {
                // Get people who are all not have task in the given day.
                IEnumerable<Person> persons = _repository.People.Where(x => !assignments.Where(y => y.Day == day).Select(y => y.Person.Id).Contains(x.Id)).ToList();
                day = persons.Count() > 0 ? day : day++;
                //Get assignments
                assignments = ProcessAssignments(assignments, task, day);
            }
            return assignments;
        }

        private List<Assignment> ProcessAssignments(List<Assignment> assignments, Task task, int day)
        {
            Assignment assignment = new Assignment();
            // Get people who are all not have task in the given day.
            IEnumerable<Person> persons = _repository.People.Where(x => !assignments.Where(y => y.Day == day).Select(y => y.Person.Id).Contains(x.Id)).ToList();
            assignment.Task = task;

            if (persons.Count() > 0)
            {
                // Check the person has required skills or not
                Person person = persons.Where(x => x.Skills.Contains(task.SkillRequired)).FirstOrDefault();
                if (person != null)
                {
                    assignment.Person = person;
                    assignment.Day = day;
                    //Add assignment in list.
                    assignments.Add(assignment);
                }
                else
                    ProcessAssignments(assignments, task, ++day);   //Recursive function for assign task in next day if people has not available on the given day
            }
            else
                ProcessAssignments(assignments, task, ++day);   ///Recursive function for assign task in next day if people has not available on the given day

            return assignments;
        }
    }
}
