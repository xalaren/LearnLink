﻿namespace LearnLink.Shared.DataTransferObjects;

public class ClientLessonDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool Completed { get; set; }
    public int CompletionProgress { get; set; }
}