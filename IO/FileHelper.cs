using System.IO;

namespace Sverto.General.IO
{
    public static class FileUtils
    {

        /// <summary>
        /// Check if a filepath could be valid
        /// </summary>
        /// <param name="thePath"></param>
        /// <returns></returns>
        public static bool IsValidPath(string thePath)
        {
            if (string.IsNullOrEmpty(thePath))
                return false;
            // split directory and filename
            string dir = null;
            string fileName = null;
            try
            {
                dir = Path.GetDirectoryName(thePath);
                fileName = Path.GetFileName(thePath);
                if (dir == null || string.IsNullOrEmpty(fileName.Trim()))
                    return false;
            }
            catch
            {
                return false;
            }
            // check for invalid characters
            bool isDirValid = (dir.IndexOfAny(Path.GetInvalidPathChars()) == -1);
            bool isFileNameValid = (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) == -1);
            if (isDirValid && isFileNameValid)
                return true;
            return false;
        }

        public static bool IsValidDirectory(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                return false;
            string d = null;
            try
            {
                d = Path.GetDirectoryName(directory);
                if (d == null)
                    return false;
            }
            catch //(Exception ex)
            {
                return false;
            }
            // check for invalid characters
            return (d.IndexOfAny(Path.GetInvalidPathChars()) == -1);
        }

        public static bool IsValidFileName(string fileName)
        {
            // check for invalid characters
            return (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) == -1);
        }

        /// <summary>
        /// Create directory and file (this will not overwrite an existing file)
        /// (is exception safe)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CreateFile(string path)
        {
            try
            {
                // create dir
                string dir = System.IO.Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                // create file
                if (!File.Exists(path))
                {
                    FileStream stream = File.Create(path);
                    stream.Dispose();
                }
                // return true if creation succeeded
                if (File.Exists(path))
                    return true;
            }
            catch
            {
            }
            return false;
        }

    }
}
