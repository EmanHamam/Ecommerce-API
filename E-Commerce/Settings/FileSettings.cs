namespace E_Commerce.Settings
{
    public static class FileSettings
    {
        public const string _AllowedExtentions = ".jpg,.jpeg,.png";
        public const int _MaxFileSizeInMB = 1;
        public const int _MaxFileSizeInBytes = _MaxFileSizeInMB* 1024 * 1024;
    }
}
