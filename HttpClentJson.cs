using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace test1{
  public class Program1{
    private static readonly HttpClient client = new HttpClient();
    // 代码源自：https://docs.microsoft.com/zh-cn/dotnet/csharp/tutorials/console-webapiclient
    /**
    * 调用异步方法时，Main必须要等到任务执行完毕后退出才算正常执行完毕；而保持等待异步方法执行完毕就需要将返回结果改为：async Task
    **/
    static async Task Main(string[] args){
      await processJson();
    }
    
    private static async Task processJson(){
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json")
      );
      client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
      
      // var stringTask = client.GetStringAsync("https://suggest.taobao.com/sug?code=utf-8&q=%E5%8D%AB%E8%A1%A3");

      // var msg = await stringTask;
      // Console.WriteLine(msg);
      
      var streamTask = client.GetStreamAsync(""https://www.114la.com/head_data_1121.json");
      var result = await JsonSerializer.DeserializerAsync<List<RequestResultJson>>(await streamTask);
      
      Console.WriteLine($" The result has {result.Count} row.");
      foreach( var row in result ){
        Console.WriteLine($"{result.Title} {result.PageClass} {result.Url}");
      }
    }
  }
  
  public class RequestResultJson{
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("pageclass")]
    public string PageClass { get; set; }
    [JsonPropertyName("url")]
    public string Url { get; set; }
  }
  
  /**
  data :
  [{"title":"818\u5168\u7403\u6765\u7535","url":"https:\/\/s.click.taobao.com\/uW8scxu","pageclass":"#FF0000"},{"title":"\u5b89\u500d\u664b\u4e09\u8fdb\u5165\u5e86\u5e94\u5927\u5b66\u533b\u9662","url":"https:\/\/www.baidu.com\/s?wd=\u5b89\u500d\u664b\u4e09\u8fdb\u5165\u5e86\u5e94\u5927\u5b66\u533b\u9662&tn=95795188_hao_pg","pageclass":"2020-08-17 15:00:03"},{"title":"\u7537\u5b50\u5192\u5145\u6218\u75ab\u533b\u751f\u5403\u9738\u738b\u9910","url":"https:\/\/www.baidu.com\/s?wd=\u7537\u5b50\u5192\u5145\u6218\u75ab\u533b\u751f\u5403\u9738\u738b\u9910&tn=95795188_hao_pg","pageclass":"2020-08-17 15:00:03"},{"title":"\u7f8e\u519b\u8f70\u70b8\u673a\u903c\u8fd1\u4e1c\u6d77\u9632\u7a7a\u8bc6\u522b\u533a","url":"https:\/\/www.baidu.com\/s?wd=\u7f8e\u519b\u8f70\u70b8\u673a\u903c\u8fd1\u4e1c\u6d77\u9632\u7a7a\u8bc6\u522b\u533a&tn=95795188_hao_pg","pageclass":"2020-08-17 15:00:03"}]
  
  **/
  
  
  
  
}
