using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExifLib;

namespace PicMover
{
    public class Mover
    {
        private static List<string> extensions = new List<string>{ "*.jpg", "*.mrw" };
        private readonly string targetFolder;

        public Mover(string targetFolder)
        {
            this.targetFolder = targetFolder;
        }

        public void Move(string directory)
        {
            var subDirs = Directory.EnumerateDirectories(directory);
            foreach(var subdir in subDirs)
            {
                Console.WriteLine("Moving {0}", subdir);
                this.Move(subdir);
            }
            
            foreach(var ext in extensions)
            {
                foreach(var file in Directory.EnumerateFiles(directory, ext))
                {
                    EvaluateFile(file);
                }
            }
            if (!Directory.EnumerateDirectories(directory).Any() && !Directory.EnumerateFiles(directory).Any())
            {
                Directory.Delete(directory);
            }
        }

        private void EvaluateFile(string file)
        {
            DateTime datePictureTaken = default;

            try
            {
                using (ExifReader reader = new ExifReader(file))
                {
                    reader.GetTagValue<DateTime>(ExifTags.DateTimeDigitized, out datePictureTaken);
                }
            }
            catch (ExifLibException e)
            {
                Console.WriteLine("{0} - No date taken: {1}:", file, e.Message);
            }
            catch (System.IO.EndOfStreamException)
            {
                Console.WriteLine("{0} is empty", file);
            }
            if (datePictureTaken != default)
            {
                MoveFile(file, datePictureTaken);
            }
            else
            {
                Console.WriteLine("{0} - No date taken", file);
            }
        }

        private void MoveFile(string file, DateTime dateTaken)
        {
            var targetPath = Path.Combine(
                this.targetFolder,
                dateTaken.Year.ToString(),
                dateTaken.Month.ToString("D2"),
                Path.GetFileName(file)
            );
            if (File.Exists(targetPath))
            {
                if (AreTheseFilesIdentical(file, targetPath))
                {
                    Console.WriteLine("Deleting duplicate {0}", file);
                    File.Delete(file);
                }                
            }
            else 
            {
                Console.WriteLine("Moving {0} to {1}", file, targetPath);
                var dir = Path.GetDirectoryName(targetPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                File.Move(file, targetPath, false);
            }
        }

        private bool AreTheseFilesIdentical(string file1, string file2)
        {
            var bytes1 = File.ReadAllBytes(file1);
            var bytes2 = File.ReadAllBytes(file2);

            return Enumerable.SequenceEqual(bytes1, bytes2);
        }
    }    
}