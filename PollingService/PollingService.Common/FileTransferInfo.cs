using System;
using System.Collections.Generic;
using System.Text;

namespace PollingService.Common
{
    public class FileTransferInfo
    {
        public string FileName { get; set; }

        public string Source { get; set; }

        public string Destination { get; set; }

        public bool Replace { get; set; } = false;

        private DateTime curreDateTime { get; set; }

        public FileTransferInfo()
        {
            curreDateTime = DateTime.Now;
        }

        public FileTransferInfo(string fileName, string source, string dest)
        {
            FileName = fileName;
            Source = source;
            Destination = dest;
            curreDateTime = DateTime.Now;
        }
        public FileTransferInfo(string fileName, string source, string dest, bool replace)
        {
            FileName = fileName;
            Source = source;
            Destination = dest;
            Replace = replace;
            curreDateTime = DateTime.Now;
        }

        public FileTransferInfo(string fileName, string source, string dest, bool replace, DateTime currDateTime)
        {
            FileName = fileName;
            Source = source;
            Destination = dest;
            Replace = replace;
            curreDateTime = currDateTime;
        }
    }
}
