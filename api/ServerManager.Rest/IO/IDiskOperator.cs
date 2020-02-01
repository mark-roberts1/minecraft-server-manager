using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ServerManager.Rest.IO
{
    /// <summary>
    /// Contract for basic disk operations in System.IO, to act as an integration point.
    /// </summary>
    public interface IDiskOperator
    {
        bool FileExists(string filename);
        bool DirectoryExists(string directory);
        DirectoryInfo CreateDirectory(string directory);
        void MoveFile(string from, string to, bool overwrite);
        void CopyFile(string from, string to, bool overwrite);
        void DeleteFile(string filename);
        bool IsFileLocked(string filename);
        void WaitForUnlockOrTimeout(string filename, int timeoutInSeconds);
        string CombinePaths(params string[] paths);
        string GetFileNameFromPath(string path);
        string GetDirectoryName(string path);
        IEnumerable<string> ExtractCompressedFilesTo(string zippedfile, string target);
        IEnumerable<string> ExtractCompressedFilesTo(string zippedfile, string target, string password);
        string GetFileName(string path);
        long GetFileLength(string file);
        string GetFileExtension(string filename);
        string GetFileNameWithoutExtension(string filename);
        void DeleteDirectory(string path, bool recursive);
        string[] GetFiles(string path);
        string[] GetDirectories(string path);
        string GetAppDirectory();
    }
}
