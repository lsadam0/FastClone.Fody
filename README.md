# FastClone.Fody


## This is an add-in for [Fody](https://github.com/Fody/Fody/) 


https://github.com/username/repository/blob/master/img/octocat.png
![Icon](https://github.com/lsadam0/FastClone.Fody/blob/master/Icons/clone_package.png)
Generates ToString method from public properties for class decorated with a `[ToString]` Attribute.

[Introduction to Fody](http://github.com/Fody/Fody/wiki/SampleUsage).


## The nuget package

https://nuget.org/packages/ToString.Fody/

    PM> Install-Package ToString.Fody


## Your Code

    [ToString]
    class TestClass
    {
        public int Foo { get; set; }

        public double Bar { get; set; }
        
        [IgnoreDuringToString]
        public string Baz { get; set; }
    }


## What gets compiled

    class TestClass
    {
        public int Foo { get; set; }

        public double Bar { get; set; }
        
        public string Baz { get; set; }
        
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture, 
                "{{T: TestClass, Foo: {0}, Bar: {1}}}",
                this.Foo,
                this.Bar);
        }
    }

``` ini
BenchmarkDotNet=v0.10.8, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-4770HQ CPU 2.20GHz (Haswell), ProcessorCount=4
Frequency=10000000 Hz, Resolution=100.0000 ns, Timer=UNKNOWN
  [Host]     : Clr 4.0.30319.42000, 32bit LegacyJIT-v4.7.2098.0
  DefaultJob : Clr 4.0.30319.42000, 32bit LegacyJIT-v4.7.2098.0
```
 |                Method |     Mean |     Error |    StdDev |
 |---------------------- |---------:|----------:|----------:|
 |       FastCloneMethod | 27.05 ms | 0.4584 ms | 0.4288 ms |
 | MemberwiseCloneMethod | 36.42 ms | 0.2711 ms | 0.2536 ms |