using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace manpower
{
    public class Repository : RepositoryBase
    {
        public Repository(FileLocations fileLocations, string configPath) : base(fileLocations, configPath)
        {
 
        }

        public override void SaveAssignments(IEnumerable<Assignment> assignments, string saveToPath)
        {
            string filePath = base.GetPath(saveToPath);
            var csv = new StringBuilder();
            var headerLine = string.Format("{0},{1},{2}", "TaskId", "PersonId", "Day"); 
            csv.AppendLine(headerLine);
            foreach (var assignment in assignments.OrderBy(x => x.Day)) //Dispaly result order by Day for see how many days will take for whole tasks
            {
                var newLine = string.Format("{0},{1},{2}", assignment.Task.Id, assignment.Person.Id,assignment.Day);
                csv.AppendLine(newLine);   
            }
            
            File.WriteAllText(filePath, csv.ToString());
        }
    }
}
