﻿namespace LearnLink.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string Text { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
    }
}
