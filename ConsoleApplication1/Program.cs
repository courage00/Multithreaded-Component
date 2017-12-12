using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;





namespace ConsoleApplication1
{
    class Program
    {
        public class Calculator //自定義calss
        {
            public string Name { get; set; }
            public long Result { get; set; }

            private AutoResetEvent _WaitHandle;
            public AutoResetEvent WaitHandle
            {
                get { return _WaitHandle; }
                set { _WaitHandle = value; }
            }
        }
        static WaitHandle[] waitHandles = null;
        static void Main(string[] args)
        {
            Program oProgram = new Program();
            oProgram.Start();

            Console.WriteLine("重回主執行緒");
            Console.WriteLine("ggggggg");
            Console.ReadKey();
           // Console.ReadLine();
        }
        private void Start()
        {
            //設立迴圈加入想要執行緒數量
            List<Calculator> calculator = new List<Calculator>() { };
            for (int t = 0; t < 3; t++)
            {
                calculator.Add (new Calculator { Result = 0, WaitHandle = new AutoResetEvent(false), Name = "NO."+t });
            }
            //把子執行緒總數丟給waitHandles
            waitHandles = new WaitHandle[calculator.Count];
            for (int i = 0; i < calculator.Count; i++)
            {
                waitHandles[i] = calculator[i].WaitHandle;
            }
            //建置子執行緒 並直接用lambda expression
            for (int i = 1; i <= 3; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(
                    o =>                
                    {
                     
                        Calculator calculator1 = (Calculator)o;
                      
                        Console.WriteLine("{0} 進入子執行緒", calculator1.Name);
                        AutoResetEvent reset = calculator1.WaitHandle; 
                        Console.WriteLine("a");
                        Console.WriteLine("b");
                        Console.WriteLine("c");
                        Console.WriteLine("d");
                        Thread.Sleep(1000);
                        reset.Set();
 
                    }), calculator[i-1]);
                    
            };
            if (waitHandles != null) 
            {
                WaitHandle.WaitAll(waitHandles);
            }
            
            Console.WriteLine(waitHandles.Length);
            Console.WriteLine("子執行緒over");
            


        }
    }
}
