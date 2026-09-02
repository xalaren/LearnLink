namespace LearnLink.Application.Storage;

public class Storage
{
    private static Storage? _instance;

    public string RootDirectory { get; }
    public string InternalDirectory => Path.Combine(RootDirectory, Directory);
    public string UsersDirectory => Path.Combine(InternalDirectory, Users);
    public string ContentsDirectory => Path.Combine(InternalDirectory, Contents);
    public string LessonsDirectory => Path.Combine(InternalDirectory, Lessons);
    public string ObjectivesDirectory => Path.Combine(InternalDirectory, Objectives);
    public string AnswersDirectory => Path.Combine(InternalDirectory, Answers);

    public const string Api = "api";
    public const string Directory = "Storage";
    public const string Users = "Users";
    public const string Images = "Images";
    public const string Sections = "Sections";
    public const string Contents = "Contents";
    public const string Lessons = "Lessons";
    public const string Objectives = "Objectives";
    public const string Answers = "Answers";

    public static Storage Instance(string rootDirectory)
    {
        _instance ??= new Storage(rootDirectory);
        return _instance;
    }

    private Storage(string rootDirectory)
    {
        RootDirectory = rootDirectory;
    }

    /*

    public static string GetRelativeDirectoryUrlToLessonSectionContent(int lessonId, int sectionId, int contentId)
    {
        return $"/{ApiName}/{StorageDirName}/{LessonsDirName}/{lessonId}/{SectionDirName}/{sectionId}/{ContentDirName}/{contentId}/";
    }

    public static string GetRelativeDirectoryUrlToContent(int contentId)
    {
        return $"/{ApiName}/{StorageDirName}/{ContentDirName}/{contentId}/";
    }

    public static string GetRelativeDirectoryUrlToLessonObjectiveContent(int lessonId, int objectiveId, int contentId)
    {
        return $"/{ApiName}/{StorageDirName}/{LessonsDirName}/{lessonId}/{ObjectivesDirname}/{objectiveId}/{ContentDirName}/{contentId}/";
    }

    public static string GetRelativeDirectoryUrlToLessonObjectiveAnswerContent(int lessonId, int objectiveId, int answerId, int contentId)
    {
        return $"/{ApiName}/{StorageDirName}/{LessonsDirName}/{lessonId}/{ObjectivesDirname}/{objectiveId}/{AnswersDirName}/{answerId}/{ContentDirName}/{contentId}/";
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
            SectionDirName,
            sectionId.ToString(),
            ContentDirName,
            contentId.ToString()
            );
    }

    public string GetDirectoryPathToLessonObjectiveContent(int lessonId, int objectiveId, int contentId)
    {
        return Path.Combine(
            LessonsStorageDirectory,
            lessonId.ToString(),
            ObjectivesDirname,
            objectiveId.ToString(),
            ContentDirName,
            contentId.ToString());
    }

    public string GetDirectoryPathToObjectiveAnswerContent(int lessonId, int objectiveId, int answerId, int contentId)
    {
        return Path.Combine(
            LessonsStorageDirectory,
            lessonId.ToString(),
            ObjectivesDirname,
            objectiveId.ToString(),
            AnswersDirName,
            answerId.ToString(),
            ContentDirName,
            contentId.ToString());
    }

    public string GetDirectoryPathToContent(int contentId)
    {
        return Path.Combine(ContentStorageDirectory, contentId.ToString());
    }
    */
}


