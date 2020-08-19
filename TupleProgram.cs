using System;
using System.Collection.Generic;
using System.Linq;

namespace test1{
    //代码源自：https://www.oschina.net/translate/tuple-in-c-sharp-7
    class TupleProgram{
        static void Main(string[] args){
            List<int> numbers = Enumerable.GetRange(1, 100).OrderBy(x => Guid.NewGuid()).Take(10).ToList();
            var data = FindMinMax(numbers);
            //Tuple中没有为元素命名，则使用Itemn (n从1开始）的方式访问
            Console.WriteLine($"{data.Item1} is min and {data.Item2} is max from {String.Join(", ", numbers)}");
            
            var data1 = FindMinMaxTupleName(numbers);
            Console.WriteLine($"{data1.Minimum} is min and {data1.Maximum} is max from {String.Join(", ", numbers)}");
        }
        
        static (int, int) FindMinMax(List<int> list){
            int maximum = int.MinValue;
            int minimum = int.MaxValue;
            
            list.ForEach(n => {
                maximum = n > maximum ? n : maximum;
                minimum = n > minimum ? n : minimum;
            });
            
            return (minimum, maximum);
        }
        
        static (int Minimum, int Maximum) FindMinMax(List<int> list){
            return FindMinMax(list);
        }
    }
}
