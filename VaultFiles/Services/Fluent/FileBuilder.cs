using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VaultFiles.Models;

namespace VaultFiles.Services.Fluent
{
    /// <summary>
    /// Fluent builder for saving files using VaultFiles FileService.
    /// </summary>
    public class FileBuilder
    {
        private readonly string _sourceFile;
        private string _destinationFolder;
        private string _password;
        private List<string> _allowedExtensions = new List<string>();
        private long _maxFileSize = long.MaxValue;
        private bool _overwrite = false;

        public FileBuilder(string sourceFile)
        {
            _sourceFile = sourceFile ?? throw new ArgumentNullException(nameof(sourceFile));
        }

        public static FileBuilder Save(string sourceFile)
        {
            return new FileBuilder(sourceFile);
        }

        public FileBuilder ToFolder(string folder)
        {
            _destinationFolder = folder;
            return this;
        }

        public FileBuilder WithPassword(string password)
        {
            _password = password;
            return this;
        }

        public FileBuilder AllowExtensions(params string[] extensions)
        {
            _allowedExtensions = new List<string>(extensions);
            return this;
        }

        public FileBuilder MaxSize(long bytes)
        {
            _maxFileSize = bytes;
            return this;
        }

        public FileBuilder Overwrite(bool overwrite = true)
        {
            _overwrite = overwrite;
            return this;
        }

        public async Task<FileSaveResult> Async(CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(_destinationFolder))
                throw new InvalidOperationException("Destination folder must be set.");

            var options = new FileSaveOptions
            {
                DestinationFolder = _destinationFolder,
                Password = _password,
                AllowedExtensions = _allowedExtensions,
                MaxFileSizeBytes = _maxFileSize,
                Overwrite = _overwrite
            };

            var service = new FileService();
            return await service.SaveFileAsync(_sourceFile, options, token);
        }
    }
}