using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using AssemblyToProcess;

namespace Benchmark
{
    internal class Program
    {
        private static readonly Random rNum = new Random();

        private static BasicTest GetTestEntity()
        {
            return new BasicTest(
                rNum.Next(),
                rNum.Next(),
                rNum.Next(),
                rNum.Next(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                (byte) rNum.Next(0, 255),
                (short) rNum.Next(0, 32000),
                (ushort) rNum.Next(0, 64000),
                (uint) rNum.Next(),
                rNum.Next(0, 2000000000),
                (ulong) rNum.Next(0, 2000000000),
                rNum.NextDouble(),
                (float) rNum.NextDouble(),
                new object(),
                DateTime.Now,
                rNum.Next()
            );
        }

        private static BasicTest FastCloneMethod(BasicTest e)
        {
            return e.FastClone();
        }

        private static BasicTest SerializationMethod(BasicTest e)
        {
            return e.SerializationMethod();
        }

        private static BasicTest BinarySerializationMethod(BasicTest e)
        {
            return e.BinarySerializationMethod();
        }

        private static BasicTest MemberwiseMethod(BasicTest e)
        {
            return e.MemberWiseMethod();
        }

        private static BasicTest ReflectionMethod(BasicTest e)
        {
            return e.ReflectionMethod();
        }

        private static void Initialize(Func<BasicTest, BasicTest> method, string name)
        {
            var e = GetTestEntity();
            var initial = method(e);

            if (initial == null)
                Console.WriteLine($"{name} - Method returned null");

            if (ReferenceEquals(e, initial))
                Console.WriteLine($"{name} - Clone not implemented");

            if (!ReferenceEquals(e.M, initial.M))
                Console.WriteLine($"{name} - Method is not a Shallow clone");
        }

        private static void Main(string[] args)
        {
            var testData = Get(100);

            Initialize(FastCloneMethod, nameof(FastCloneMethod));
            Initialize(MemberwiseMethod, nameof(MemberwiseMethod));
            Initialize(BinarySerializationMethod, nameof(BinarySerializationMethod));
            Initialize(SerializationMethod, nameof(SerializationMethod));
            Initialize(ReflectionMethod, nameof(ReflectionMethod));


            Console.WriteLine($"Begin? {testData.Count}");
            Console.ReadLine();
            
            Console.WriteLine(RunTest(testData, FastCloneMethod, nameof(FastCloneMethod)));
            Console.WriteLine(RunTest(testData,MemberwiseMethod, nameof(MemberwiseMethod)));
            Console.WriteLine(RunTest(testData,BinarySerializationMethod, nameof(BinarySerializationMethod)));
            Console.WriteLine(RunTest(testData,SerializationMethod, nameof(SerializationMethod)));
            Console.WriteLine(RunTest(testData,ReflectionMethod, nameof(ReflectionMethod)));

            //  var newPass = Audit(testData, FastCloneMethod);
            // var mPass = Audit(testData, MemberwiseMethod);
         
         //   Console.WriteLine($"{il} New Res Seconds {newPass}");
           // Console.WriteLine($"{m} M Res Seconds {mPass}");
            Console.ReadLine();
        }

        private static List<BasicTest> Get(int size)
        {
            var res = new List<BasicTest>(size);
            for (var i = 0; i < size; ++i)
                res.Add(GetTestEntity());

            return res;
        }


        private static string RunTest(List<BasicTest> dataSet, Func<BasicTest, BasicTest> method, string name)
        {
            var res = new List<BasicTest>(dataSet.Count);
            var i = 0;
            Console.WriteLine("RunTest");
            var watch = Stopwatch.StartNew();
            try
            {
                watch.Start();

                foreach (var e in dataSet)
                {
                    res.Add(method(e));
                    ++i;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{name} - " + e.ToString());
            }
            finally
            {
                watch.Stop();
                // Console.WriteLine("End Test");
                // Console.ReadLine();
                Console.WriteLine($"E: {i} A: {res.Count}");
                GC.Collect(2, GCCollectionMode.Forced);
                GC.WaitForFullGCComplete();
            }
            // return watch.Elapsed.TotalSeconds;

            return $"{name} - Elapsed {watch.Elapsed.TotalSeconds}";
        }

        private static bool Audit(List<BasicTest> dataSet, Func<BasicTest, BasicTest> method)
        {
            var res = true;
            try
            {
                foreach (var e in dataSet)
                {
                    var c = method(e);
                    if (c == null)
                        throw new Exception();
                    if (ReferenceEquals(c, e))
                        throw new Exception();
                    if (!c.Equals(e))
                        throw new Exception();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                res = false;
            }
            return res;
        }
    }
}