﻿namespace LearnLink.Core.Entities
{
    public class CourseCompletion : Completion
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}
