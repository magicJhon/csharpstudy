using System;

namespace test1{
    public struct Str1{
        private string middle;
        //不被当作属性使用；认定为方法
        public string Middle { get { return middle??"default"; } set => middle = value??"value"; }
        
        //当作属性
        public string First { get; private set; };  // 不能通过="aa";设置默认值。
        public string Last { get; set; }
        
        public Str1(string first){
            First = first;
            Last = "11";
            middle = string.Empty; //必须给middle赋值。struct要给所有成员赋值。
            // Middle = "middle";   //可以；但是如果没有上一行的 middle = string.Empty; 仍会报错middle未被赋值 被认为调用方法；而middle仍未赋值。
        }
        
        public Str1(string first, string last){
            First = first;
            Last = last;
            middle = null;
        }
    }
    
    class StructTestDemo{
        static void Main(string[] args){
            Str1 test1 = new Str1("testfirst1");
            // test1.First = "first";   //报错。First的set是私有的，故不能调用
            test1.Last = "testlast1";   
            
            Console.WriteLine($"test1 value is First:{test1.First}\tmiddle:{test1.Middle}\tLast:{test1.Last}");
            
            Str1 test2 = new Str1("testfirst2", "testlast2");
            Console.WriteLine($"test2 value is First:{test2.First}\tmiddle:{test1.Middle}\tLast:{test2.Last}");
            
            TestModifyTest(test2);  //值传递，变量test2的Last不变
            Console.WriteLine($"after TestModifyTest test2 value is First:{test2.First}\tmiddle:{test2.Middle}\tLast:{test2.Last}");
            
            TestRefModifyTest(ref test2);      //引用传递，值变了。在TestRefModifyTest方法中的修改同步过来了
            Console.WriteLine($"after TestRefModifyTest test2 value is First:{test2.First}\tmiddle:{test2.Middle}\tLast:{test2.Last}");
            
            TestOutModifyTest(out test2);   //引用传递，实例变了。在TestOutModifyTest中，必须给test2赋个新值。即是说test2肯定会变成别的
            Console.WriteLine($"after TestOutModifyTest test2 value is First:{test2.First}\tmiddle:{test2.Middle}\tLast:{test2.Last}");
        }
        
        static void TestModifyTest(Str1 test){
            test.Last = "In TestModifyTest modify test");
            test.Middle = "I Modify middle in TestModifyTest");
        }
        
        static void TestRefModifyTest(ref Str1 test){
            test.Last = "In TestRefModifyTest modify test";
            test.Middle = "I Modify middle in TestModifyTest");
        }
        
        /** 
        * 参数test每次进来都是未初始化的，不能修改。且方法返回前，必须要给test赋个值
        **；
        static void TestOutModifyTest(out Str1 test){
            // test.Last = "In TestRefModifyTest modify test"; //错误，未给test赋值
            Console.WriteLine("In function TestOutModifyTest:");
            //Console.WriteLine($"Old Value test's value is First:{test.First}\tmiddle:{test.Middle}\tLast:{test.Last}");    //报错：使用了未赋值的 out 参数“test” [F:\mayi\netcore31\test1\test1.csproj]
            test = new Str1("in TestOutModifyTest new a Str1 instance");        
            test.Middle = "I Modify middle in TestOutModifyTest"; 
        }
    }
}
