#File Polling Service
This project is about a file polling service which will watch for any new files in the root folder and
move/transfer the files to the destination folder.

Requisites:
1) We have a Root Folder (C:\Root) and Destination Folder (C:\Destination).
Root folder and destination folder paths should be configurable.
2) Multiple files can come into the root folder and can be concurrently transferred
to the destination folder
3) Number of files transferred parallelly can be configured
4) Logging

This project has developed using Microsoft .Net core technologies to make environment agnostic use and this has five different components/projects as described below:
1. PollingService.Common: This project contains the common services, models, classes used by entire projects. i.e. It has model class, interfaces related to data and configuration provider related services which are resolved through Microsoft .net core dependency resolver with singleton pattern. We can extend the services here to create another implementation and then plug to resolver for further use.
Note: To save the last successful pull datetime for later use for pulling files only in incremented way so i used a local text file for that and in real it could be from some storage. But still with this also this could be a production code as reading/writing to file is thread safe using lock.  
2. PollingService.Puller: This project is meant to have an actual puller which transfers the file from one location to another. This is also an interface based implementation so that we can create further implementation based on our need. As of now the problem statement was to move files from one directory to another directory hence I have used LocalFilePuller to do so.
3. PollingService.Queue: This project has implementation of Blocking concurrent queue to make sure any point of time only configured no. of files will be processed to transfer and rest will be in queue. This was the requirement and crucial piece of the project.
4. PollingService.Worker: This is the main project which creates a worker service which will run continuously and internally there is a timer implemented to look the configured folder in every x seconds (configurable) to move the file.
5. PollingService.Worker.Tests: This is in InProgress and InComplete.
Configurations:
1) Root folder and destination folder configuration:
 => configurable through appsettings.json.

 "appSettings": {
    ...
    "SourceFolder": "C://Test_Source",
    "DestFolder": "C://Test_Dest"
  }
2) Multiple files can come into the root folder and can be concurrently transferred to the destination folder, this is the configuration is to limit the concurrency (i.e. 5 files at a time) and time (how frequently polling will happen)
=> Configurable through appsettings.json.

 "appSettings": {
    "MaxAllowedConcurrentFilePulling": 5,
    "PullEverySeconds": 10,
     ...
  }
3) Logging

=> I'm using Serilog for logging here. Log level and log file location can be configured here too through app settings.json.

"Serilog": {
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "formatter": "Serilog.Formatting.Compact.RenderedCompactJsonFormatter, Serilog.Formatting.Compact",
          "path": "C://temp//PollingService.txt",
          "fileSizeLimitBytes": 1073741824, //1GB
          "shared": true,
          "flushToDiskInterval": 1, //seconds
          "rollOnFileSizeLimit": true
        }
      }
    ]
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Warning"
    }
  },
