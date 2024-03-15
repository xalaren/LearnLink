namespace LearnLink.Application.Helpers
{
    public class DirectoryStore
    {
        public string RootDirectory { get; } = null!;
        public string InternalStorageDirectory { get; } = null!;
        public string UsersStorageDirectory { get; } = null!;

        public const string STORAGE_DIRNAME = "Storage";
        public const string USERS_DIRNAME = "Users";
        public const string IMAGES_DIRNAME = "Images";

        public DirectoryStore(string rootDirectory)
        {
            RootDirectory = rootDirectory;
            InternalStorageDirectory = Path.Combine(RootDirectory, STORAGE_DIRNAME);
            UsersStorageDirectory = Path.Combine(InternalStorageDirectory, USERS_DIRNAME);
        }

        public static string GetRelativeDirectoryUrlToUserImages(int userId)
        {
            return $"{STORAGE_DIRNAME}/{USERS_DIRNAME}/{userId}/{IMAGES_DIRNAME}/";
        }

        public string GetDirectoryPathToUserImages(int userId)
        {
            return Path.Combine(UsersStorageDirectory, userId.ToString());
        }
    }
}
