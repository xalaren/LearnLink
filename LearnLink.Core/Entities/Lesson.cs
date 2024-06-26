﻿using LearnLink.Core.Exceptions;

namespace LearnLink.Core.Entities
{
    public class Lesson
    {
        private string title = string.Empty;

        public int Id { get; set; }

        public string Title
        {
            get => title;
            set
            {
                if (!string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ValidationException("Название не было заполнено");
                }

                title = value;
            }
        }

        public string? Description { get; set; }
    }
}
