using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Avi;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Tiff;

namespace PicMover
{
    public static class DateExtractor
    {
        private static System.Collections.Generic.List<ExtractorInfo> formats = new List<ExtractorInfo>{
                new ExtractorInfo
                {
                    SubDirectory = typeof(ExifIfd0Directory),
                    TagId = ExifDirectoryBase.TagDateTime,
                    DateFormat = "yyyy:MM:dd H:mm:ss"
                },
                new ExtractorInfo
                {
                    SubDirectory = typeof(MetadataExtractor.Formats.QuickTime.QuickTimeTrackHeaderDirectory),
                    TagId = MetadataExtractor.Formats.QuickTime.QuickTimeTrackHeaderDirectory.TagCreated,
                    DateFormat = "ddd MMM dd H:mm:ss yyyy"
                },
                new ExtractorInfo
                {
                    SubDirectory = typeof(ExifSubIfdDirectory),
                    TagId = 36867,
                    DateFormat = "yyyy:MM:dd H:mm:ss"
                },
                new ExtractorInfo
                {
                    SubDirectory = typeof(AviDirectory),
                    TagId = 320,
                    DateFormat = "ddd MMM dd H:mm:ss yyyy"  //"Mon Sep 26 16:16:59 2005"
                }};

        public static DateTime? GetCreatedDate(string file)
        {
            try
            {
                IReadOnlyList<Directory> directories;
                if (System.IO.Path.GetExtension(file) == ".CRW")
                {
                Console.WriteLine(file);
                }
                

                directories = ImageMetadataReader.ReadMetadata(file);

                foreach(var extractorInfo in formats)
                {
                    var subDir = directories.Where(d =>d.GetType() == extractorInfo.SubDirectory).FirstOrDefault();
                    var dateCreatedAsString = subDir?.GetDescription(extractorInfo.TagId);

                    if (dateCreatedAsString != null)
                    {
                        DateTime.TryParseExact(dateCreatedAsString, extractorInfo.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed);
                        if (parsed != default)
                        {
                            return parsed;
                        }
                    }
                }
            }
            catch (ImageProcessingException e)
            {
                return null;
            }
            return null;
        }

        private class ExtractorInfo
        {
            public Type SubDirectory { get; set; }

            public int TagId { get; set; }

            public string DateFormat { get; set; }
        }
    }
}