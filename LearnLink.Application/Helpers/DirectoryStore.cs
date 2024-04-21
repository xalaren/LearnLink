namespace LearnLink.Application.Helpers
{
    public class DirectoryStore
    {
        public string RootDirectory { get; } = null!;
        public string InternalStorageDirectory { get; } = null!;
        public string UsersStorageDirectory { get; } = null!;
        public string ContentStorageDirectory { get; } = null!;
        public string LessonsStorageDirectory { get; } = null!;

        public const string API_NAME = "api";
        public const string STORAGE_DIRNAME = "Storage";
        public const string USERS_DIRNAME = "Users";
        public const string IMAGES_DIRNAME = "Images";
        public const string CONTENT_DIRNAME = "Content";
        public const string LESSONS_DIRNAME = "Lesson";

        public DirectoryStore(string rootDirectory)
        {
            RootDirectory = rootDirectory;
            InternalStorageDirectory = Path.Combine(RootDirectory, STORAGE_DIRNAME);
            UsersStorageDirectory = Path.Combine(InternalStorageDirectory, USERS_DIRNAME);
            LessonsStorageDirectory = Path.Combine(InternalStorageDirectory, LESSONS_DIRNAME);
            ContentStorageDirectory = Path.Combine(InternalStorageDirectory, CONTENT_DIRNAME);
        }

        public static string GetRelativeDirectoryUrlToUserImages(int userId)
        {
            return $"{API_NAME}/{STORAGE_DIRNAME}/{USERS_DIRNAME}/{userId}/{IMAGES_DIRNAME}/";
        }

        public static string GetRelativeDirectoryUrlToLessonContent(int lessonId, int sectionId)
        {
            return $"{API_NAME}/{STORAGE_DIRNAME}/{LESSONS_DIRNAME}/{lessonId}/{CONTENT_DIRNAME}/{sectionId}/";
        }

        public static string GetRelativeDirectoryUrlToContent(int contentId)
        {
            return $"{API_NAME}/{STORAGE_DIRNAME}/{CONTENT_DIRNAME}/{contentId}/";
        }

        public string GetDirectoryPathToUserImages(int userId)
        {
            return Path.Combine(UsersStorageDirectory, userId.ToString());
        }

        public string GetDirectoryPathToLessonContents(int lessonId, int sectionId)
        {
            return Path.Combine(LessonsStorageDirectory, lessonId.ToString(), CONTENT_DIRNAME, sectionId.ToString());
        }

        public string GetDirectoryPathToContent(int contentId)
        {
            return Path.Combine(ContentStorageDirectory, contentId.ToString());
        }
    }
}
