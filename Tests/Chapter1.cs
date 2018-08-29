using System;
using Xunit;
using QuickSort.cs;
using System.Linq;
using Xunit.Abstractions;

namespace Tests
{
    public class Chapter1
    {
        private readonly ITestOutputHelper _output;


        public Chapter1(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void QuickSortSequential()
        {
            var dataSample = new int[10] { 3, 6, 73, 9, 11, 8, 66, 3, 22, 11 };

            _output.WriteLine("Before sorting...");
            _output.WriteLine(dataSample.Select(x => x.ToString("###,###,###,###,###")).Aggregate((arg1, arg2) => arg1 + "|" + arg2));

            QuickSort.cs.QuickSort.QuickSort_Sequential(dataSample);

            _output.WriteLine("After sorting...");
            _output.WriteLine(dataSample.Select(x => x.ToString("###,###,###,###,###")).Aggregate((arg1, arg2) => arg1 + "|" + arg2));

        }

        [Fact]
        public void QuickSortParallel()
        {
            var dataSample = new int[10] { 3, 6, 73, 9, 11, 8, 66, 3, 22, 11 };


            _output.WriteLine("Before sorting...");
            _output.WriteLine(dataSample.Select(x => x.ToString("###,###,###,###,###")).Aggregate((arg1, arg2) => arg1 + "|" + arg2));

            QuickSort.cs.QuickSort.QuickSort_Parallel(dataSample);

            _output.WriteLine("After sorting...");
            _output.WriteLine(dataSample.Select(x => x.ToString("###,###,###,###,###")).Aggregate((arg1, arg2) => arg1 + "|" + arg2));

        }
    }
}
