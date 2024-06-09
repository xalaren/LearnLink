namespace LearnLink.Core.Entities;

public class LessonSection
{
    public int LessonId { get; init; }
    public Lesson Lesson { get; init; } = null!;
    
    public int SectionId { get; init; }
    public Section Section { get; init; } = null!;
}