# FastClone.Fody


## This is an add-in for [Fody](https://github.com/Fody/Fody/) 

![Icon](https://github.com/lsadam0/FastClone.Fody/blob/master/Icons/clone_package.png)

Generate shallow clone methods for objects with the IFastClone<T> interface.

[Introduction to Fody](http://github.com/Fody/Fody/wiki/SampleUsage).

## The nuget package

https://www.nuget.org/packages/FastClone.Fody

    PM> Install-Package FastClone.Fody


## Your Code

    class SomeClass : IFastClone<SomeClass>
    {
        public int ValueA { get; set; }

        public double ValueB { get; set; }
        
        public SomeClass FastClone()
        {
            throw new NotImplementedException();
        }
    }


## What gets compiled

    class SomeClass : IFastClone<SomeClass>
    {
        public int ValueA { get; set; }

        public double ValueB { get; set; }

        public SomeClass FastClone()
        {
            return SomeClass.CloneMethod(this);
        }

        private static SomeClass CloneMethod(SomeClass source)
        {
            return new SomeClass()
            {
                ValueA = Source.ValueA,
                ValueB = Source.ValueB                        
            }
        }
    }

## Usage

You must define the interface IFastClone<T>, and add it to any class that you wish FastClone to rewrite

    interface IFastClone<T>
    {
        T FastClone();
    }

## Why?

Well, for one, I needed an excuse to play around with Fody :)

That said, this approach to cloning is *slightly* faster than MemberwiseClone (Most of the time).   The benchmark below measures the FastClone vs MemberwiseClone of cloning 100k objects.
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