using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesPrototype.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string HashedPassword { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Patronymic { get; set; }
    }
}
