using System.Net;
using Xamarin.Forms;
using Android.Webkit;
using System;
using System.IO;
using HiitHard.Droid;
using HiitHard.Utilities;

[assembly: Xamarin.Forms.Dependency(typeof(NativeFileService))]
namespace HiitHard.Droid
{
    public class NativeFileService : INativeFileService
    {
        public string ReadJson(string file)
        {
            //string path = Environment.GetFolderPath(Xamarin.Essentials.FileSystem.AppDataDirectory);
            string filename = Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, file);
            string content = "";
            if (System.IO.File.Exists(filename))
            {
                using (var streamReader = new StreamReader(filename))
                {
                    content = streamReader.ReadToEnd();
                    System.Diagnostics.Debug.WriteLine(content);
                }
            }
            else
            {
                content = "No File present";
            }
            return content;
        }

        public string WriteJson(string contents, string file)
        {
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filename = Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, file);

            using (var streamWriter = new StreamWriter(filename, false))
            {
                streamWriter.Flush();
                streamWriter.Write(contents);
            }

            return filename;
        }

        public void CopyFile(string source, string target)
        {
            System.IO.File.Copy(source, target);
        }

        public string GetPersonalFolderPath()
        {
            return Xamarin.Essentials.FileSystem.AppDataDirectory;
        }

        public void DeleteFile(string source)
        {
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filename = Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, source);
            System.IO.File.Delete(filename);
        }

        public string CreateFile(string file_name, byte[] imageBytes)
        {
            string path = Xamarin.Essentials.FileSystem.AppDataDirectory;
            System.IO.File.WriteAllBytes(path + "/" + file_name + ".jpg", imageBytes);

            return path.ToString() + "/" + file_name + ".jpg";
        }

        public string CreateTextFile(string file_name, string content)
        {
            string path = Xamarin.Essentials.FileSystem.AppDataDirectory;
            System.IO.File.WriteAllText(path + "/" + file_name, content);

            return path.ToString() + "/" + file_name;
        }

        public string GetStoragePath()
        {
            return Xamarin.Essentials.FileSystem.AppDataDirectory;
        }

        public string ConvertToWepb(string source_file_path, string result_file_path)
        {
            Android.Graphics.Bitmap bmp = Android.Graphics.BitmapFactory.DecodeFile(source_file_path);

            string file_extension = "." + source_file_path.Split('.')[1];

            string newPath = result_file_path.Replace(file_extension, ".jpg");
            using (var fs = new FileStream(newPath, FileMode.OpenOrCreate))
            {
                bmp.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 50, fs);
            }

            return newPath;
        }

        public string ResizeAndCompress(string sourceFile, string new_file_name, float maxWidth, float maxHeight)
        {
            string path = Xamarin.Essentials.FileSystem.AppDataDirectory;
            string targetFile = Path.Combine(path, new_file_name);
            string file_size = "";
            bool file_exists = false;
            if (!File.Exists(targetFile) && File.Exists(sourceFile))
            {
                // First decode with inJustDecodeBounds=true to check dimensions
                var options = new Android.Graphics.BitmapFactory.Options()
                {
                    InJustDecodeBounds = false,
                    InPurgeable = true,
                };

                using (var image = Android.Graphics.BitmapFactory.DecodeFile(sourceFile, options))
                {
                    if (image != null)
                    {
                        var sourceSize = new Size((int)image.GetBitmapInfo().Height, (int)image.GetBitmapInfo().Width);

                        var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

                        string targetDir = System.IO.Path.GetDirectoryName(targetFile);
                        if (!Directory.Exists(targetDir))
                            Directory.CreateDirectory(targetDir);

                        if (maxResizeFactor > 0.9)
                        {
                            File.Copy(sourceFile, targetFile);
                        }
                        else
                        {
                            var width = (int)(maxResizeFactor * sourceSize.Width);
                            var height = (int)(maxResizeFactor * sourceSize.Height);

                            using (var bitmapScaled = Android.Graphics.Bitmap.CreateScaledBitmap(image, height, width, true))
                            {
                                using (Stream outStream = File.Create(targetFile))
                                {
                                    bitmapScaled.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 75, outStream);
                                }
                                bitmapScaled.Recycle();
                            }
                        }

                        image.Recycle();
                    }
                }
            }

            return targetFile;
        }

        public string SaveBinary(string filename, byte[] bytes)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filepath = Path.Combine(path, filename);
            if (File.Exists((filepath)))
            {
                File.Delete(filepath);
            }
            File.WriteAllBytes(filepath, bytes);

            return filepath;
        }

    }
}