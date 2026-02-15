namespace VaultFiles.Models
{
    /// <summary>
    /// Result of a file save operation containing success status, file name, and full path.
    /// </summary>
    public class FileSaveResult
    {
        public bool Success { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }
    }
}