# FastClone.Fody


ToDo
- Readme writeup
- Publish



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