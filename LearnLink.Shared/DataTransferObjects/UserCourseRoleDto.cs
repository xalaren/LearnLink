namespace LearnLink.Shared.DataTransferObjects
{
    public record UserCourseRoleDto
    {
        public UserDto User { get; set; } = null!;
        public CourseDto Course { get; set; } = null!;
        public RoleDto Role { get; set; } = null!;
    }
}
