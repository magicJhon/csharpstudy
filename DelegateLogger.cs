using System;
using System.IO;
using System.Reflection;

namespace test1{
  // 代码源自：https://docs.microsoft.com/zh-cn/dotnet/csharp/delegates-patterns
  public class DelegateLogger{
  
    static void Main(string[] args){
      FileLogger fileLog = new FileLogger("./log.txt");
      Logger.WriteMessage += ConsoleLogger.LogToConsole;

      Logger.LogMessage(Severity.Error, nameof(DelegateLogger), $"There is an Error in {MehtodBase.GetCurrentMethod().DeclaringType.FullName} method is {MethodBase.GetCurrentMehtod().Name}");
    }
  } 
  
  public class FileLogger{
    private readonly string logPath;
    
    public FileLogger(string path){
      logPage = path;
      Logger.WriteMessage += LogMessage;
    }
    
    public void detachLog() => Logger.WriteMessage -= LogMessage;
    
    // 确保不要出现异常；若有异常则会抛出异常，其他委托的执行将不会继续
    public void LogMessage(string msg){
      try{
        using(var log = File.AppendText(logPath)){
          log.WriteLine(msg);
          log.Flush();
        }
      }catch(Exception){
          // Hmm. We caught an exception while
          // logging. We can't really log the
          // problem (since it's the log that's failing).
          // So, while normally, catching an exception
          // and doing nothing isn't wise, it's really the
          // only reasonable option here.
      }
    }
  }
  
  public static class ConsoleLogger{
    public static void LogToConsole(string msg){
      Console.Error.WriteLine(msg);
    }
  }
  
  public class Logger{
    public static Action<string> WriteMessage;
    
    public static Severity LogLevel { get; set; } = Severity.Warning;
    
    
    public stataic void LogMessage(Severity s, string component, string msg){
      if( s < LogLevel ) {
        return ;
      }
      string output = $"{DateTime.Now}\t{s}\t{component}\t{msg}";
      WriteMessage(output);
    }
  }
  
  public enum Severity{
    Verbose,
    Trace,
    Infomation,
    Warning,
    Error,
    Critical
  }
}
