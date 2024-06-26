﻿namespace LearnLink.Core.Entities
{
    public class UserCourseLocalRole
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public int LocalRoleId { get; set; }

        public User User { get; set; } = null!;
        public Course Course { get; set; } = null!;
        public LocalRole LocalRole { get; set; } = null!;
    }
}
