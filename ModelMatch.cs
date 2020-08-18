using System;

namespace test1
{
    // 使用模式匹配功能来扩展数据类型
    //代码取自： https://docs.microsoft.com/zh-cn/dotnet/csharp/tutorials/pattern-matching
    public class ModelMatch
    {
        static void Main(string[] args){
            Car c = new Car();
            c.Passengers = 1;
            bool inbound = true;
            var feeInBound  = CalculateToll(c) * PeakTimePremiumFull(DateTime.Now, inbound);
            var feeOutBound = CalculateToll(c) * PeakTimePremiumFull(DateTime.Now, !inbound);
            Console.WriteLine($"The Car inbound fee is {feeInBound:C2}, OutBound fee is {feeOutBound:C2}");
            //The Car inbound fee is ￥3.00, OutBound fee is ￥3.00
        }

        
        /**
        * 因乘客数而异的定价
        没有乘客的汽车和出租车需额外支付 0.50 美元。
        载有两名乘客的汽车和出租车可享受 0.50 美元折扣。
        载有三名或更多乘客的汽车和出租车可享受 1.00 美元折扣。
        乘客数不到满载量 50% 的巴士需额外支付 2.00 美元。
        乘客数超过满载量 90% 的巴士可享受 1.00 美元折扣。
        超过 5000 磅的运货卡车需额外支付 5.00 美元。
        3000 磅以下的轻型卡车可享受 2.00 美元折扣。
        * @vehicle 车辆
        **/
        public static decimal CalculateToll(object vehicle) => vehicle switch{
                Car c => c.Passengers switch{
                    0 => 2.00m + 0.5m,  //没有乘客的汽车和出租车需额外支付 0.50 美元。
                    1 => 2.0m,          //一位乘客
                    2 => 2.0m - 0.5m,   //载有两名乘客的汽车和出租车可享受 0.50 美元折扣。
                    _ => 2.00m - 1.00m  //载有三名或更多乘客的汽车和出租车可享受 1.00 美元折扣。
                },
                Taxi t => t.Fares switch{
                    0 => 3.50m + 0.5m,  //没有乘客的汽车和出租车需额外支付 0.50 美元。
                    1 => 3.50m,
                    2 => 3.50m - 0.5m,  //载有两名乘客的汽车和出租车可享受 0.50 美元折扣。
                    _ => 3.50m - 1.00m  //载有三名或更多乘客的汽车和出租车可享受 1.00 美元折扣。
                },

                Bus b when ((double)b.Riders / (double) b.Capacity) < 0.5 => 5.00m + 2.00m, //乘客数不到满载量 50% 的巴士需额外支付 2.00 美元。
                Bus b when ((double)b.Riders / (double) b.Capacity) > 0.5 => 5.00m - 1.00m, //乘客数超过满载量 90% 的巴士可享受 1.00 美元折扣。
                Bus b => 5.00m,

                DeliveryTruck t when (t.GrossWeightClass > 5000 ) => 10.00m + 5.00m,
                DeliveryTruck t when (t.GrossWeightClass < 3000 ) => 10.00m - 2.00m,
                DeliveryTruck t => 10.00m,

                { } => throw new ArgumentException(message: "Not a known vehicle type ", paramName: nameof(vehicle)),
                null => throw new ArgumentNullException(nameof(vehicle))

            };

            

        /**
        * 计算时段峰值额外附加费的比率系数
        * @timeOfToll 当前时间
        * @inbound 行进方向， true：入城；   false:出城
        **/
        public static decimal PeakTimePremiumFull(DateTime timeOfToll, bool inbound) =>(IsWeekDay(timeOfToll), GetTimeBand(timeOfToll), inbound) switch
        {
            (true, TimeBand.MorningRush, true) => 2.00m,    //工作日 早高峰  入城 
            (true, TimeBand.MorningRush, false) => 1.00m,    //工作日 早高峰  出城 
            // (true, TimeBand.DayTime, true) => 1.50m,    //工作日 日间  入城 
            // (true, TimeBand.DayTime, false) => 1.50m,    //工作日 日间  出城 
            //or 
            (true, TimeBand.DayTime, _) => 1.50m,
            (true, TimeBand.EveningRush, true) => 1.00m,     //工作日 晚高峰  入城 
            (true, TimeBand.EveningRush, false) => 2.00m,   //工作日 晚高峰  出城 
            // (true, TimeBand.Overnight, true) => 0.75m,      //工作日 夜间  入城 
            // (true, TimeBand.Overnight, false) => 0.75m,      //工作日 夜间  出城 
            //or 
            (true, TimeBand.Overnight, _) => 0.75m,
            // (false, TimeBand.MorningRush, true) => 1.00m,    //周末 早高峰  入城 
            // (false, TimeBand.MorningRush, false) => 1.00m,   //周末 早高峰  出城 
            // (false, TimeBand.DayTime, true) => 1.00m,    //周末 日间  入城 
            // (false, TimeBand.DayTime, false) => 1.00m,    //周末 日间  出城 
            // (false, TimeBand.EveningRush, true) => 1.00m,     //周末 晚高峰  入城 
            // (false, TimeBand.EveningRush, false) => 1.00m,   //周末 晚高峰  出城 
            // (false, TimeBand.Overnight, true) => 1.00m,   //周末 夜间  入城 
            // (false, TimeBand.Overnight, false) => 1.00m      //周末 夜间  出城 
            // or
            (false, _, _) => 1.00m
        };

        private enum TimeBand{
            MorningRush,    //早高峰
            DayTime,        //日间
            EveningRush,    //晚高峰
            Overnight       //夜间
        }
        

        /**
        * 判断所属时间段
        *
        **/
        private static TimeBand GetTimeBand(DateTime timeOfToll){
            int hour = timeOfToll.Hour;
            if( hour < 6 ){
                return TimeBand.Overnight;       //夜间
            }else if( hour < 10 ){
                return TimeBand.MorningRush;    //早高峰
            }else if( hour < 16 ){
                return TimeBand.DayTime;       //日间
            }else if( hour < 20 ){
                return TimeBand.EveningRush;   //晚高峰
            }
            return TimeBand.Overnight;      //夜间
        }
        
        private static bool IsWeekDay(DateTime timeOfToll)=> 
            timeOfToll.DayOfWeek switch{
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false,
                // DayOfWeek.Monday => true,
                // DayOfWeek.Thursday => true,
                // DayOfWeek.Wednesday => true,
                // DayOfWeek.Tuesday => true,
                // DayOfWeek.Friday => true,
                // or
                 _ => true
            };
    }

    //
    public class Car{
        public int Passengers { get; set; }  //乘客数
    }

    public class DeliveryTruck{
        public int GrossWeightClass { get; set; }       //总重量级
    }

    public class Taxi{
        public int Fares { get; set; }  //出租车乘客
    }

    public class Bus{
        public int Capacity { get; set; }   //载客量
        public int Riders { get; set; }     //乘客数
    }
}
