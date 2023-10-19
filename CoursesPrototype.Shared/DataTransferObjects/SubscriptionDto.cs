namespace CoursesPrototype.Shared.DataTransferObjects
{
    public class SubscriptionDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int CourseId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
