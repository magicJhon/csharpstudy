using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace test1
{
    public class BreakfastAsyncTask
    {
        // 参考：https://docs.microsoft.com/zh-cn/dotnet/csharp/async
        // 代码取自： https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/concepts/async/
        /**
        做早餐：
            倒一杯咖啡。
            加热平底锅，然后煎两个鸡蛋。
            煎三片培根。
            烤两片面包。
            在烤面包上加黄油和果酱。
            倒一杯橙汁。
        */

        static async Task Main(string[] args){
            Console.WriteLine("同步早餐任务执行流程：");
            Coffee cup = PourCoffe();
            Console.WriteLine("Coffee已完成");

            Egg eggs = FryEggs(2);
            Console.WriteLine("完成两个煎蛋");

            Bacon bacon = FryBacon(3);
            Console.WriteLine("已煎好3片培根");

            Toast toast = ToastBread(2);
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine("面包片已烤好，并抹上黄油和果酱");

            Juice juice = PourJuice();
            Console.WriteLine("已倒好一杯果汁");
            Console.WriteLine("同步早餐任务完成");

            Console.WriteLine("异步早餐任务执行流程：");
            Coffee asyncCoffee = PourCoffe();
            Console.WriteLine("Coffee已完成");

            var eggsTask = FryEggsAsync(2);
            var baconTask = FryBaconAsync(3);
            var toastTask = MakeToastWithButterAndJamAsync(2);
/**
            // 第一种方式， I/O绑定异步，顺序声明调用; 仍是开始执行仍是顺序执行，中间步骤不是同步的；主进程不会被锁死，仍在当前线程中
            var asyncEggs = await eggsTask;
            Console.WriteLine("完成两个煎蛋");
            
            var asyncBacon = await baconTask;
            Console.WriteLine("已煎好3片培根");

            var asyncToast = await toastTask;
            Console.WriteLine("面包片已烤好，并抹上黄油和果酱");

            // 异步早餐任务执行流程：
            // 倒出Coffee
            // Coffee已完成
            // 热油中...
            // 放3片培根到烤盘上
            // 烤一面培根中...
            // 放一片面包到烤面包机上
            // 放一片面包到烤面包机上
            // 烤面包中...
            // 打2个鸡蛋
            // 将一片培根翻面
            // 取出烤面包机中的面包
            // 煎鸡蛋中...
            // 将一片培根翻面
            // 将一片培根翻面
            // 第二面烤制中...
            // 黄油涂到面包上
            // 果酱涂到面包上
            // 煎好鸡蛋放到盘中
            // 完成两个煎蛋
            // 烤好培根，放到盘上
            // 已煎好3片培根
            // 面包片已烤好，并抹上黄油和果酱
            // 倒出果汁
            // 已倒好一杯果汁
            // 异步早餐任务完成
**/

/**
            //第二种方式；启动一个后台线程；CPU绑定异步
            await Task.WhenAll(eggsTask, baconTask, toastTask);
            Console.WriteLine("完成两个煎蛋");
            Console.WriteLine("已煎好3片培根");
            Console.WriteLine("面包片已烤好，并抹上黄油和果酱");
            // 步早餐任务执行流程：
            // 倒出Coffee
            // Coffee已完成
            // 热油中...
            // 放3片培根到烤盘上
            // 烤一面培根中...
            // 放一片面包到烤面包机上
            // 放一片面包到烤面包机上
            // 烤面包中...
            // 打2个鸡蛋
            // 煎鸡蛋中...
            // 取出烤面包机中的面包
            // 将一片培根翻面
            // 将一片培根翻面
            // 将一片培根翻面
            // 第二面烤制中...
            // 黄油涂到面包上
            // 果酱涂到面包上
            // 煎好鸡蛋放到盘中
            // 烤好培根，放到盘上
            // 完成两个煎蛋
            // 已煎好3片培根
            // 面包片已烤好，并抹上黄油和果酱
            // 倒出果汁
            // 已倒好一杯果汁
            // 异步早餐任务完成
**/

            //第三种方式：Task.WhenAny， CPU绑定异步；启动一个后台线程
            var breakfastTasks = new List<Task>{eggsTask, baconTask, toastTask};
            while (breakfastTasks.Count > 0){
                Task finishedTask = await Task.WhenAny(breakfastTasks);
                if (finishedTask == eggsTask){
                    Console.WriteLine("完成两个煎蛋");
                }else if (finishedTask == baconTask){
                    Console.WriteLine("已煎好3片培根");
                }else if (finishedTask == toastTask){
                    Console.WriteLine("面包片已烤好，并抹上黄油和果酱");
                }
                breakfastTasks.Remove(finishedTask);
            }
            // 异步早餐任务执行流程：
            // 倒出Coffee
            // Coffee已完成
            // 热油中...
            // 放3片培根到烤盘上
            // 烤一面培根中...
            // 放一片面包到烤面包机上
            // 放一片面包到烤面包机上
            // 烤面包中...
            // 打2个鸡蛋
            // 煎鸡蛋中...
            // 取出烤面包机中的面包
            // 将一片培根翻面
            // 将一片培根翻面
            // 将一片培根翻面
            // 第二面烤制中...
            // 黄油涂到面包上
            // 果酱涂到面包上
            // 面包片已烤好，并抹上黄油和果酱
            // 煎好鸡蛋放到盘中
            // 烤好培根，放到盘上
            // 完成两个煎蛋
            // 已煎好3片培根
            // 倒出果汁
            // 已倒好一杯果汁
            // 异步早餐任务完成


            Juice asyncJuice = PourJuice();
            Console.WriteLine("已倒好一杯果汁");
            Console.WriteLine("异步早餐任务完成");

        }


        //同步方法
        /// <summary>
        /// 倒咖啡
        /// </summary>
        /// <param name="coffee"></param>
        static Coffee PourCoffe(){
            Console.WriteLine("倒出Coffee");
            return new Coffee();
        }

        /// <summary>
        /// 煎鸡蛋
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static Egg FryEggs(int number){
            Console.WriteLine("热油中...");
            Task.Delay(3000).Wait();
            Console.WriteLine($"打{number}个鸡蛋");
            Console.WriteLine("煎鸡蛋中...");
            Task.Delay(3000).Wait();
            Console.WriteLine("煎好鸡蛋放到盘中");

            return new Egg(); 
        }

        /// <summary>
        /// 煎培根
        /// </summary>
        /// <param name="slices"></param>
        /// <returns></returns>
        static Bacon FryBacon(int slices){
            Console.WriteLine($"放{slices}片培根到烤盘上");
            Console.WriteLine("烤一面培根中...");
            Task.Delay(3000).Wait();
            for( int slice = 0; slice < slices; slice ++){
                Console.WriteLine("将一片培根翻面");
            }
            Console.WriteLine("第二面烤制中...");
            Task.Delay(3000).Wait();
            Console.WriteLine("烤好培根，放到盘上");

            return new Bacon();
        }

        static Toast ToastBread(int slices){
            for(int slice =0; slice < slices; slice++){
                Console.WriteLine("放一片面包到烤面包机上");
            }

            Console.WriteLine("烤面包中...");
            Task.Delay(3000).Wait();
            Console.WriteLine("取出烤面包机中的面包");

            return new Toast();
        }

        /// <summary>
        /// 抹黄油
        /// </summary>
        /// <param name="toast"></param>
        static void ApplyButter(Toast toast) => 
            Console.WriteLine("黄油涂到面包上");

        /// <summary>
        /// 抹果酱
        /// </summary>
        /// <param name="toast"></param>
        static void ApplyJam(Toast toast) =>
            Console.WriteLine("果酱涂到面包上");

        /// <summary>
        /// 倒果汁
        /// </summary>
        /// <returns></returns>
        static Juice PourJuice(){
            Console.WriteLine("倒出果汁");
            return new Juice();
        }

        //异步方法
        /// <summary>
        /// 面包处理，包括烤面包，抹黄油和果酱
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static async Task<Toast> MakeToastWithButterAndJamAsync(int number){
            var toast = await ToastBreadAsync(number);
            ApplyButter(toast);
            ApplyJam(toast);

            return toast;
        }

        /// <summary>
        /// 烤面包
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static async Task<Toast> ToastBreadAsync(int number){
            for(int slice = 0; slice < number; slice++){
                Console.WriteLine("放一片面包到烤面包机上");
            }
            Console.WriteLine("烤面包中...");
            await Task.Delay(3000);
            Console.WriteLine("取出烤面包机中的面包");

            return new Toast();
        }

        /// <summary>
        /// 异步烤培根
        /// </summary>
        /// <param name="slices"></param>
        /// <returns></returns>
        static async Task<Bacon> FryBaconAsync(int slices){
            Console.WriteLine($"放{slices}片培根到烤盘上");
            Console.WriteLine("烤一面培根中...");
            await Task.Delay(3000);
            for(int slice = 0; slice < slices; slice++){
                Console.WriteLine("将一片培根翻面");
            }
            Console.WriteLine("第二面烤制中...");
            await Task.Delay(3000);
            Console.WriteLine("烤好培根，放到盘上");

            return new Bacon();
        }

        static async Task<Egg> FryEggsAsync(int howMany){
            Console.WriteLine("热油中...");
            await Task.Delay(3000);
            Console.WriteLine($"打{howMany}个鸡蛋");
            Console.WriteLine("煎鸡蛋中...");
            await Task.Delay(3000);
            Console.WriteLine("煎好鸡蛋放到盘中");

            return new Egg();
        }
    }

    /// <summary>
    /// 咖啡
    /// </summary>
    public class Coffee{

    }

    /// <summary>
    /// 果汁
    /// </summary>
    public class Juice{

    }

    /// <summary>
    /// 面包
    /// </summary>
    public class Toast{

    }

    /// <summary>
    /// 培根
    /// </summary>
    public class Bacon{

    }

    /// <summary>
    /// 鸡蛋
    /// </summary>
    public class Egg{

    }
}
