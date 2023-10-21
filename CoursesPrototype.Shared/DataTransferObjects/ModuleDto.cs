namespace CoursesPrototype.Shared.DataTransferObjects
{
    public record ModuleDto
        (
            int Id,
            string Title,
            string? Description,
            string? Content
        );
}
