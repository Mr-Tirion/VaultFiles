using System.Collections.Generic;

namespace VaultFiles.Models
{
    /// <summary>
    /// Options for saving a file, including destination, password, allowed extensions, max size, and overwrite settings.
    /// </summary>
    public class FileSaveOptions
    {
        public string DestinationFolder { get; set; }
        public string Password { get; set; }
        public List<string> AllowedExtensions { get; set; } = new List<string>();
        public long MaxFileSizeBytes { get; set; } = long.MaxValue;
        public bool Overwrite { get; set; } = false;
    }
}