# DotNetCoreLogging

It is a project where I'm trying to make my logging mechanism extensible and lossely coupled with the project. This means, this approach will allow me to switch among
diffrent logging approach without touching the project code. As an example this demo currently has implementation of Serilog logger. Below are the different project here:

### 1. Demo.Logging.Model
This project defines the model and interface for Log implementation. i.e. ***ILogger*** interface and ***LogLevels*** enum. 

### 2. Demo.Logging.Serilog
This is the project which has one defined logging mechanism using serilog ***SerilogFileLogger***. I would prefer to have separate projects like this for any such different logging
mechanism. i.e. **Demo.Logging.NLog** etc.

### 3. Demo.Logging
This project defines the static class abstraction which allows consumer to use the logging without worring about the internal logging mechanism also make consumer free from worrying of 
resolving dependency for logging. In a real scenario this would be available as a nuget package and consumer would know only the static class ***Logger*** which can be used anywhere
in the project which simplify the uses nicely I guess.
***LoggerFactory*** here is the key to instantiate the proper ***ILogger*** class and the code here can enhanced nicely as per the need. I also tried to demonstrated here to log
data to a specific files named with class/module/contoller name if needed.

### 4. DotNetCoreLogging
This is a .Net core web api project which uses ***Demo.Logging*** package to log. Code example:
```
Logger.Info("Information");
Logger.Error(ex, "message");
```
Logging to a seperate file by class/controller name:
```
Logger<WeatherForecast>.Info("logging Information...");
Logger<WeatherForecast>.Error("logging Error...");
```

