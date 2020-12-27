using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace PicMover
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var mover = new Mover(@"C:\Users\flytz\OneDrive\Pictures\Camera Roll", @"D:\Photos", "*.jpe", "*.nef", "*.jpg", "*.CRW");
            mover.Move();

            var videoMover = new Mover(@"C:\Users\flytz\OneDrive\Pictures\Camera Roll", @"D:\Videos", "*.mov", "*.mp4", "*.avi");
            videoMover.Move();

            var cameraMover = new Mover(@"F:\DCIM\100OLYMP", @"D:\Photos", "*.jpe", "*.nef", "*.jpg", "*.CRW");
            cameraMover.Move();

            var cameraVideoMover = new Mover(@"F:\DCIM\100OLYMP", @"D:\Videos", "*.mov", "*.mp4", "*.avi");
            cameraVideoMover.Move();

            
            
            // foreach (var file in System.IO.Directory.EnumerateFiles(@"D:\Test"))
            // {
            //     var date = DateExtractor.GetCreatedDate(file);
            //     Console.WriteLine("{0} : {1}", file, date?.ToString() ?? "no date");
            // }
            Console.ReadKey();
        }
    }
}
