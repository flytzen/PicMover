using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PicMover
{
    public class Mover
    {
        private readonly string sourceFolder;
        private readonly string targetFolder;
        private readonly string[] extensions;

        public Mover(string sourceFolder, string targetFolder, params string[] extensions)
        {
            this.sourceFolder = sourceFolder;
            this.targetFolder = targetFolder;
            this.extensions = extensions;
        }

        public void Move()
        {
            this.Move(this.sourceFolder);
        }

        private void Move(string directory)
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
            var t1 = Directory.EnumerateDirectories(directory);
            var t2 = Directory.EnumerateFiles(directory);
            if (!Directory.EnumerateDirectories(directory).Any() && !Directory.EnumerateFiles(directory).Any())
            {
                //Directory.Delete(directory, true);
            }
        }

        private void EvaluateFile(string file)
        {
            DateTime? datePictureTaken = DateExtractor.GetCreatedDate(file);

            if (datePictureTaken.HasValue)
            {
                MoveFile(file, datePictureTaken.Value);
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