using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VaultFiles.Interfaces;
using VaultFiles.Models;
using VaultFiles.Exceptions;
using VaultFiles.Utils;

namespace VaultFiles.Services
{
    /// <summary>
    /// Provides functionality to save and delete files asynchronously with support for password encryption and file type/size validation.
    /// </summary>
    public class FileService : IFileService
    {
        public async Task<FileSaveResult> SaveFileAsync(string sourceFilePath, FileSaveOptions options, CancellationToken token = default)
        {
            if (!File.Exists(sourceFilePath))
                throw new FileNotFoundException("Source file not found.", sourceFilePath);

            var extension = Path.GetExtension(sourceFilePath).ToLower();

            if (options.AllowedExtensions.Count > 0 && !options.AllowedExtensions.Contains(extension))
                throw new InvalidFileTypeException($"File extension '{extension}' is not allowed.");

            var fileInfo = new FileInfo(sourceFilePath);
            if (fileInfo.Length > options.MaxFileSizeBytes)
                throw new Exception("File size exceeds the maximum allowed.");

            if (!Directory.Exists(options.DestinationFolder))
                Directory.CreateDirectory(options.DestinationFolder);

            var fileName = Path.GetFileName(sourceFilePath);
            var destinationPath = Path.Combine(options.DestinationFolder, fileName);

            if (!options.Overwrite && File.Exists(destinationPath))
                destinationPath = Path.Combine(options.DestinationFolder, $"{Guid.NewGuid()}{extension}");

            if (!string.IsNullOrEmpty(options.Password))
            {
                // Encrypt the file using AES
                EncryptionHelper.EncryptFile(sourceFilePath, destinationPath, options.Password);
            }
            else
            {
                // Async copy using FileStream
                await using var sourceStream = File.OpenRead(sourceFilePath);
                await using var destinationStream = File.Create(destinationPath);
                await sourceStream.CopyToAsync(destinationStream, cancellationToken: token);
            }

            return new FileSaveResult
            {
                Success = true,
                FileName = Path.GetFileName(destinationPath),
                FullPath = destinationPath
            };
        }

        public Task DeleteFileAsync(string filePath, CancellationToken token = default)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            return Task.CompletedTask;
        }
    }
}