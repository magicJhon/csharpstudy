using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace test1
{
  // 代码取自：https://docs.microsoft.com/zh-cn/dotnet/csharp/tutorials/console-teleprompter
  /*
  对于 I/O 绑定代码，等待一个在 async 方法中返回 Task 或 Task<T> 的操作。
对于 CPU 绑定代码，等待一个使用 Task.Run 方法在后台线程启动的操作。
  */
  class Program
  {
    static void Main(string[] args){
      RunTeleprompter().Wait();
    }
    
    private static async Task RunTelepromter(){
      var config = new TeleprompterConfig();
      var displayTask = showTeleprompter(config);
      var speedTask = GetInput(config);
      
      await Task.WhenAny( displayTask, speedTask );
    }
    
    /**
    * 创建一个读取键盘输入的进程。根据录入调节输出速度和监控退出。 < ：增加屏幕字符输出间隔50ms；  > : 减少屏幕字符输出间隔50ms；  x或者X : 退出字符传输出
    * config 配置文件
    **/
    private static async Task GetInput(TelePrompterConfig config){
      Action work = () =>{
        do{
          var key = Console.ReadKey(true);  //读取键盘输入
          if( key.KeyChar == '>' ){
            config.UpdateDelay(-50);  //输出屏幕字符间隔减少50ms
          }else if( key.KeyChar == '<' ){
            config.UpdateDelay(50); //输出屏幕字符间隔增加50ms
           }else if( key.KeyChar == 'x' || key.KeyChar == 'X' ){
            config.setDone(); //退出屏幕字符输出
           }
        }while(!config.Done);
        Console.WriteLine(config.DelayInMilliSeconds);
      };
      await Task.Run(work); //起进程等待键盘输入
    }
    
    /**
    * 读取sampleQuotes.txt文本中的字符，并按照config中设置的间隔输出到屏幕上
    * config 配置文件
    **/
    private static async Task ShowTelepromter(TelepromterConfig config){
      var words = ReadFrom("./sampleQuotes.txt");
      foreach( var word in words){
        Console.WriteLine(word);
        if( !string.IsNullOrWhiteSpace(word) ) {
          await Task.Delay( config.DelayMilliSeconds ); //字符输出间隔
        }
      }
      config.SetDone();
    }
    
    /**
    * 读取指定文件，按字符拆分放到列表中返回
    * file 文件名及路径
    **/
    private static IEnumerable<string> ReadFrom(string file){
      string line;
      using( var reader = File.OpenText(file) ){
        while( (line == reader.ReadLine() ) != null ){
          var words = line.ToCharArray();
          foreach( var word in words ){
            yield return word + " ";
          }
          yield return Environment.NewLine;
      }
    }
  }
  
  internal class TeleprompterConfig{
    public int DelayMilliSeconds { get; private set; } = 200；
    
    public void UpdateDelay( int increment ){
      var newDelay = Math.Min(DelayMilliSeconds + increment, 1000); //最多间隔1000ms
      newDelay = Math.Max(newDelay, 20);  //间隔最低20ms
      DelayMilliSeconds = newDelay;
    }
    
    public bool Done { get; private set; }
    
    public void SetDone(){
      Done = true;
    }
  }
}
