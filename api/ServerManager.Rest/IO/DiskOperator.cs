using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.IO
{
    [ExcludeFromCodeCoverage, Description("Integration point")]
    public class DiskOperator : IDiskOperator
    {
        public string CombinePaths(params string[] paths)
        {
            return Path.Combine(paths);
        }

        public void CopyFile(string from, string to, bool overwrite)
        {
            File.Copy(from, to, overwrite);
        }

        public DirectoryInfo CreateDirectory(string directory)
        {
            return Directory.CreateDirectory(directory);
        }

        public void DeleteFile(string filename)
        {
            File.Delete(filename);
        }

        public bool DirectoryExists(string directory)
        {
            return Directory.Exists(directory);
        }

        /// <summary>
        /// Unpacks compressed file into provided directory. Assumes that target directory exists.
        /// Able to unpack:
        ///     - Zip
        ///     - GZip
        ///     - Tar
        ///     - BZip2
        ///     - LZip
        ///     - Rar (< Rar 5, single-volume only)
        /// </summary>
        /// <param name="compressedFile">File to unpack.</param>
        /// <param name="targetDirectory">Directory to which to extract compressed contents.</param>
        /// <returns>List of files extracted in the target directory.</returns>
        public IEnumerable<string> ExtractCompressedFilesTo(string compressedFile, string targetDirectory)
        {
            return ExtractCompressedFilesTo(compressedFile, targetDirectory, (ReaderOptions)null);
        }

        /// <summary>
        /// Unpacks password-protected compressed file into provided directory. Assumes that target directory exists.
        /// Able to unpack:
        ///     - Zip
        ///     - GZip
        ///     - Tar
        ///     - BZip2
        ///     - LZip
        ///     - Rar (< Rar 5, single-volume only)
        /// </summary>
        /// <param name="compressedFile">File to unpack.</param>
        /// <param name="targetDirectory">Directory to which to extract compressed contents.</param>
        /// <param name="password">Password used to decrypt compressed file.</param>
        /// <returns>List of files extracted in the target directory.</returns>
        public IEnumerable<string> ExtractCompressedFilesTo(string compressedFile, string targetDirectory, string password)
        {
            return ExtractCompressedFilesTo(compressedFile, targetDirectory, new ReaderOptions { Password = password });
        }

        private IEnumerable<string> ExtractCompressedFilesTo(string compressedFile, string targetDirectory, ReaderOptions options)
        {
            // open the archive
            using (var fileStream = File.Open(compressedFile, FileMode.Open))
            using (var reader = ReaderFactory.Open(fileStream, options))
            {
                // loop through archive
                while (reader.MoveToNextEntry())
                {
                    // ignore directories
                    if (reader.Entry.IsDirectory) continue;

                    // extract to provided target
                    reader.WriteEntryToDirectory(targetDirectory, new ExtractionOptions { Overwrite = true });

                    // return absolute paths to newly extracted files
                    yield return CombinePaths(targetDirectory, reader.Entry.Key);
                }
            }
        }

        public bool FileExists(string filename)
        {
            return File.Exists(filename);
        }

        public string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public string GetFileExtension(string filename)
        {
            return Path.GetExtension(filename);
        }

        public long GetFileLength(string file)
        {
            return new FileInfo(file).Length;
        }

        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public string GetFileNameFromPath(string path)
        {
            return Path.GetFileName(path);
        }

        public string GetFileNameWithoutExtension(string filename)
        {
            return Path.GetFileNameWithoutExtension(filename);
        }

        /// <summary>
        /// Attempts to open a file. If IOException (not FileNotFoundException) is thrown, assumes that the file is locked.
        /// </summary>
        /// <param name="filename">file to check for lock.</param>
        /// <returns>Returns <see langword="true"/> if locked, <see langword="false"/> if not.</returns>
        /// <exception cref="SecurityException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public bool IsFileLocked(string filename)
        {
            var file = new FileInfo(filename);
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            // catch FileNotFoundException to rethrow, since it implements IOException
            catch (FileNotFoundException)
            {
                throw;
            }
            // catch DirectoryNotFoundException to rethrow, since it implements IOException
            catch (DirectoryNotFoundException)
            {
                throw;
            }
            catch (IOException)
            {
                return true;
            }
            // any other Exception should be thrown.
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        /// <summary>
        /// In order to allow overwrite specification, this method does a copy and then deletes the source.
        /// </summary>
        public void MoveFile(string from, string to, bool overwrite)
        {
            CopyFile(from, to, overwrite);
            DeleteFile(from);
        }

        /// <summary>
        /// Waits for a file to become unlocked, or until the provided timeout period elapses.
        /// </summary>
        /// <param name="filename">File to check for lock.</param>
        /// <param name="timeoutInSeconds">Number of seconds to wait before throwing TimeoutException.</param>
        /// <exception cref="TimeoutException"></exception>
        /// <exception cref="SecurityException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void WaitForUnlockOrTimeout(string filename, int timeoutInSeconds)
        {
            int i = 0;

            while (IsFileLocked(filename) && i < timeoutInSeconds)
            {
                Thread.Sleep(1000);
                i++;
            }

            if (IsFileLocked(filename))
                throw new TimeoutException($"The timeout period of {timeoutInSeconds} elapsed while waiting for {filename} to finish uploading.");
        }

        public string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }

        public string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            Directory.Delete(path, recursive);
        }

        public string GetAppDirectory()
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            return GetDirectoryName(pathToExe);
        }
    }
}
