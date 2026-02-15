using System;
using System.IO;
using System.Security.Cryptography;

namespace VaultFiles.Utils
{
    /// <summary>
    /// Provides methods to encrypt files using AES encryption.
    /// </summary>
    public static class EncryptionHelper
    {
        /// <summary>
        /// Encrypts a file using AES encryption with the specified password.
        /// </summary>
        /// <param name="inputFile">Path of the source file to encrypt.</param>
        /// <param name="outputFile">Path where the encrypted file will be saved.</param>
        /// <param name="password">Password used for encryption.</param>
        public static void EncryptFile(string inputFile, string outputFile, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));

            using var aes = Aes.Create();
            aes.KeySize = 256;

            // Generate Key and IV from password using PBKDF2
            var salt = new byte[] { 0x23, 0x45, 0x56, 0x12, 0x98, 0xAA, 0xBC, 0xDE };
            var key = new Rfc2898DeriveBytes(password, salt, 10000);
            aes.Key = key.GetBytes(32);
            aes.IV = key.GetBytes(16);

            using var inputStream = File.OpenRead(inputFile);
            using var outputStream = File.Create(outputFile);
            using var cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

            inputStream.CopyTo(cryptoStream);
        }
    }
}