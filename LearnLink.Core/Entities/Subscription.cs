﻿namespace LearnLink.Core.Entities
{
    public class Subscription
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public DateTime StartDate { get; set; }
    }
}
