using System.Threading;
using System.Threading.Tasks;
using VaultFiles.Models;

namespace VaultFiles.Interfaces
{
    /// <summary>
    /// Provides methods for saving and deleting files asynchronously with options such as password and allowed extensions.
    /// </summary>
    public interface IFileService
    {
        Task<FileSaveResult> SaveFileAsync(string sourceFilePath, FileSaveOptions options, CancellationToken token = default);
        Task DeleteFileAsync(string filePath, CancellationToken token = default);
    }
}