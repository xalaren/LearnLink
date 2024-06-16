namespace LearnLink.Application.Helpers
{
    public class DirectoryStore
    {
        public string RootDirectory { get; }
        public string InternalStorageDirectory { get; }
        public string UsersStorageDirectory { get; }
        public string ContentStorageDirectory { get; }
        public string LessonsStorageDirectory { get; }
        public string ObjectivesStorageDirectory { get; }
        public string AnswersStorageDirectory { get; }

        public const string API_NAME = "api";
        public const string STORAGE_DIRNAME = "Storage";
        public const string USERS_DIRNAME = "Users";
        public const string IMAGES_DIRNAME = "Images";
        public const string SECTION_DIRNAME = "Sections";
        public const string CONTENT_DIRNAME = "Content";
        public const string LESSONS_DIRNAME = "Lesson";
        public const string OBJECTIVES_DIRNAME = "Objectives";
        public const string ANSWERS_DIRNAME = "Answers";

        public DirectoryStore(string rootDirectory)
        {
            RootDirectory = rootDirectory;
            InternalStorageDirectory = Path.Combine(RootDirectory, STORAGE_DIRNAME);
            UsersStorageDirectory = Path.Combine(InternalStorageDirectory, USERS_DIRNAME);
            LessonsStorageDirectory = Path.Combine(InternalStorageDirectory, LESSONS_DIRNAME);
            ContentStorageDirectory = Path.Combine(InternalStorageDirectory, CONTENT_DIRNAME);
            ObjectivesStorageDirectory = Path.Combine(InternalStorageDirectory, OBJECTIVES_DIRNAME);
            AnswersStorageDirectory = Path.Combine(InternalStorageDirectory, ANSWERS_DIRNAME);
        }

        public static string GetRelativeDirectoryUrlToUserImages(int userId)
        {
            return $"/{API_NAME}/{STORAGE_DIRNAME}/{USERS_DIRNAME}/{userId}/{IMAGES_DIRNAME}/";
        }

        public static string GetRelativeDirectoryUrlToLessonSectionContent(int lessonId, int sectionId, int contentId)
        {
            return $"/{API_NAME}/{STORAGE_DIRNAME}/{LESSONS_DIRNAME}/{lessonId}/{SECTION_DIRNAME}/{sectionId}/{CONTENT_DIRNAME}/{contentId}/";
        }

        public static string GetRelativeDirectoryUrlToContent(int contentId)
        {
            return $"/{API_NAME}/{STORAGE_DIRNAME}/{CONTENT_DIRNAME}/{contentId}/";
        }

        public static string GetRelativeDirectoryUrlToLessonObjectiveContent(int lessonId, int objectiveId, int contentId)
        {
            return $"/{API_NAME}/{STORAGE_DIRNAME}/{LESSONS_DIRNAME}/{lessonId}/{OBJECTIVES_DIRNAME}/{objectiveId}/{CONTENT_DIRNAME}/{contentId}/";
        }

        public static string GetRelativeDirectoryUrlToLessonObjectiveAnswerContent(int lessonId, int objectiveId, int answerId, int contentId)
        {
            return $"/{API_NAME}/{STORAGE_DIRNAME}/{LESSONS_DIRNAME}/{lessonId}/{OBJECTIVES_DIRNAME}/{objectiveId}/{ANSWERS_DIRNAME}/{answerId}/{CONTENT_DIRNAME}/{contentId}/";
        }

        public string GetDirectoryPathToUserImages(int userId)
        {
            return Path.Combine(UsersStorageDirectory, userId.ToString(), IMAGES_DIRNAME);
        }

        public string GetDirectoryPathToLessonSectionContent(int lessonId, int sectionId, int contentId)
        {
            return Path.Combine(
                LessonsStorageDirectory,
                lessonId.ToString(),
                SECTION_DIRNAME,
                sectionId.ToString(),
                CONTENT_DIRNAME,
                contentId.ToString()
                );
        }

        public string GetDirectoryPathToLessonObjectiveContent(int lessonId, int objectiveId, int contentId)
        {
            return Path.Combine(
                LessonsStorageDirectory,
                lessonId.ToString(),
                OBJECTIVES_DIRNAME,
                objectiveId.ToString(),
                CONTENT_DIRNAME,
                contentId.ToString());
        }

        public string GetDirectoryPathToObjectiveAnswerContent(int lessonId, int objectiveId, int answerId, int contentId)
        {
            return Path.Combine(
                LessonsStorageDirectory,
                lessonId.ToString(),
                OBJECTIVES_DIRNAME,
                objectiveId.ToString(),
                ANSWERS_DIRNAME,
                answerId.ToString(),
                CONTENT_DIRNAME,
                contentId.ToString());
        }

        public string GetDirectoryPathToContent(int contentId)
        {
            return Path.Combine(ContentStorageDirectory, contentId.ToString());
        }
    }
}
