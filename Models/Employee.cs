using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRHApp.Models
{
    public class Employee : User
    {
        public string Position { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime HireDate { get; set; }
        public List<Absence> Absences { get; set; } = new List<Absence>();
        public List<ExtraHour> ExtraHours { get; set; } = new List<ExtraHour>();
        public List<VacationRequest> VacationRequests { get; set; } = new List<VacationRequest>();
    }
    public enum UserRole
    {
        HR = 1,
        Employee = 2
    }
}
