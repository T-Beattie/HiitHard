using System;
using System.Collections.Generic;
using System.Text;

namespace HiitHard.Utilities
{
    public interface INativeFileService
    {
        String ReadJson(string file);
        string WriteJson(string contents, string file);
        void CopyFile(string source, string target);
        void DeleteFile(string source);
        string GetStoragePath();
        string CreateFile(string file_name, byte[] imageBytes);
        string CreateTextFile(string file_name, string content);
        string ConvertToWepb(string file_name, string my_guid);
        string ResizeAndCompress(string sourceFile, string targetFile, float maxWidth, float maxHeight);
        string GetPersonalFolderPath();
        string SaveBinary(string filename, byte[] bytes);
    }
}
