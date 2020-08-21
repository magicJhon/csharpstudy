using System;
using System.Collection.Generic;
using System.Linq;

namespace test1{
    
    //代码源自：https://docs.microsoft.com/zh-cn/dotnet/csharp/linq/query-expression-basics
    public class LinqForListSample{
        static void Main(string[] args){
            List<StudentScore> scores = new List<StudentScore>(){
                new StudentScore{ Name="张三", Subject="语文", Score=80},
                new StudentScore{ Name="张三", Subject="数学", Score=90},
                new StudentScore{ Name="张三", Subject="英语", Score=60},
                new StudentScore{ Name="李四", Subject="数学", Score=95},
                new StudentScore{ Name="李四", Subject="语文", Score=70},
                new StudentScore{ Name="王五", Subject="语文", Score=50},
                new StudentScore{ Name="王五", Subject="英语", Score=10}
            }
            
            //分树大于60分的人及科目，按科目和人名分组
            var scoreQuery = from student in scores 
                    where student.Score > 60
                    order by student.Score descending
                    group student by new {student.Subject, student.Name};
            Console.WriteLine("Group Test:");
            foreach( var score in scoreQuery){
                Console.WriteLine($"{score.Key}");
                foreach(var scorein in score){
                    Console.WriteLine($"{scorein.Name} {scorein.Subject} {scorein.Score}");
                }
            }
            
            List<Subject> subjects = new List<Subject>(){
                new Subject { ID = 1, Name = "数学"},
                new Subject { ID = 2, Name = "语文"},
                new Subject { ID = 3, Name = "英语"}
            }
            
            List<Score> scoresOnly = new List<Score>(){
                new Score { StudentID = 1, SubjectID = 1, Mark = 95},
                new Score { StudentID = 1, SubjectID = 2, Mark = 90},
                new Score { StudentID = 1, SubjectID = 3, Mark = 70},
                new Score { StudentID = 2, SubjectID = 1, Mark = 60},
                new Score { StudentID = 2, SubjectID = 2, Mark = 55},
                new Score { StudentID = 2, SubjectID = 3, Mark = 40},
                new Score { StudentID = 3, SubjectID = 2, Mark = 100},
                new Score { StudentID = 3, SubjectID = 1, Mark = 30}
            }
            
            List<Student> students = new List<Student>(){
                new Student { ID = 1, Name = "张三", FirstName = "张", LastName = "三1"},
                new Student { ID = 2, Name = "李四", FirstName = "李", LastName = "四1"},
                new Student { ID = 3, Name = "王五", FirstName = "王", LastName = "五1"}
            }
            
            var studentQuery =  
                    from student in students 
                    let studentName = "真" + "·" + student.FirstName + student.LastName[0] + "·" + student.LastName[1]
                    select new { ID = student.ID, Name = studentName};
            Console.WriteLine("Let Query");
            foreach（var score in studentQuery){
                Console.WriteLine($"{score.Name}");
            }
            
            var scoreQuery1 = 
                    from newscore in ( from score in scoresOnly
                        join student in students on score.StudentID equals student.ID   //连表  联合主键用形如：on new { employee.FirstName, employee.LastName } equals new { student.FirstName, student.LastName }
                        join subject in subjects on score.SubjectID equals subject.ID   //连表
                        select new { StudentID = score.StudentID, StudentName = student.Name    //封装数据返回
                        , SubjectID = score.SubjectID, SubjectName = subject.Name, Mark = score.Mark
                        })
                    let percentile = (int) newscore.Mark / 10   //赋值
                    group newscore by percentile into scoreGroup    //分组；并以percentile为Key，保存符合条件的分组到scoreGroup中  // IEnumerable<IGrouping<int, Score>>
                    // where scoreGroup.Key >= 6    // 60分以上的；  where 条件通过  && || 连接
                    order by scoreGroup.Key descending
                    select scoreGroup;
            Console.WriteLine(" Let & Join & Groupby Query:");
            
            foreach(var scoresec in scoreQuery1){
                // scoresec.Key保存的是形成分组的字段值。还有包含有满足分组条件的数据数组。
                Console.WriteLine($"分数段{scoresec.Key}0-{scoresec.Key}9学生及科目如下:");
                foreach(var score in scoresec){
                    Console.WriteLine($"{score.StudentName}({score.StudentID}) {score.SubjectName}({score.SubjectID}) {score.Mark}");
                }
            }
            
            var scoreQuery2 = 
                from score in scoresOnly
                let percentile = (int) score.Mark / 10
                group score by percentile into socreGroup  // IEnumerable<IGrouping<int, Score>>
                select new { 
                    Level = scoreGroup.Key,
                    HighestScore = ( from score2 in scoreGroup 
                                        select score2.Mark).Max(),  //同分组下分数最高的
                    GroupCount = scoreGroup.Count(),                //分组下有多少条记录
                    Total = ( from score2 in scoreGroup             
                                select score2.Mark).Count(),        //同上
                    Average = ( from score2 in scoreGroup 
                                    select score2.Mark).Average()   //同分组的平均分
                };
                
             Console.WriteLine("子查询");
             foreach(var score in scoreQuery2){
                Console.WriteLine($"Level:{score.Level}  HighestScore:{score.HighestScore} GroupCount:{score.GroupCount} Average:{score.Average} Total:{score.Total}");
             }
        }
    }
    
    public class StudentScore{
        public string Name { get; set; }
        public string Subject { get; set; }
        public int Score { get; set; }
    }
    
    public class Student{
        public int ID { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    
    public class Subject{
        public int ID { get; set; }
        public string Name { get; set; }
    }
    
    public class Score{
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public int Mark { get; set; }   //分数
    }
    
    /**
    输出：
    Group Test:
    { Subject = 数学, Name = 李四 }
    李四 数学 95
    { Subject = 数学, Name = 张三 }
    张三 数学 90
    { Subject = 语文, Name = 张三 }
    张三 语文 80
    { Subject = 语文, Name = 李四 }
    李四 语文 70
    Let Query:
    真·张三·1
    真·李四·1
    真·王五·1
    let & join & group Query:
    分数段100-109学生及科目如下:
    王五(3) 语文(2) 100
    分数段90-99学生及科目如下:
    张三(1) 数学(1) 95
    张三(1) 语文(2) 90
    分数段70-79学生及科目如下:
    张三(1) 英语(3) 70
    分数段60-69学生及科目如下:
    李四(2) 数学(1) 60
    分数段50-59学生及科目如下:
    李四(2) 语文(2) 55
    分数段40-49学生及科目如下:
    李四(2) 英语(3) 40
    分数段30-39学生及科目如下:
    王五(3) 数学(1) 30
    子查询
    Level:9  HighestScore:95 GroupCount:2 Average:92.5 Total:2
    Level:7  HighestScore:70 GroupCount:1 Average:70 Total:1
    Level:6  HighestScore:60 GroupCount:1 Average:60 Total:1
    Level:5  HighestScore:55 GroupCount:1 Average:55 Total:1
    Level:4  HighestScore:40 GroupCount:1 Average:40 Total:1
    Level:10  HighestScore:100 GroupCount:1 Average:100 Total:1
    Level:3  HighestScore:30 GroupCount:1 Average:30 Total:1
    **/
}
