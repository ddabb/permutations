using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    internal class Program
    {
        /// <summary>
        /// 序列改变测试
        /// </summary>
        private static int _permuteTimes = 0;

        public static void Main()
        {
            try
            {
                Func<int, int> ConvertFuc()
                {
                    return c =>
                    {
                        if (c <= 9)
                            return c % 3;
                        return c;
                    };
                }

          
                Console.WriteLine("11个球并在一起处理的全排列开始" + DateTime.Now);
                List<int> elevenNumber = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
                List<List<int>> newpermutations = PermuteRecrusive(elevenNumber);
                Console.WriteLine("11个球并在一起处理的全排列结束" + DateTime.Now);
                var allCount = CountDistinct(newpermutations, new List<int> { 3, 3, 5 }, ConvertFuc());

                Console.WriteLine($"三红球三黄球三篮球一黑一白共11个球分成3,3,5三堆的组合是：{allCount}");
                List<int> nineNumber = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                DateTime now1 = DateTime.Now;
                Console.WriteLine("11个球排除掉黑白两个球的全排列开始" + now1);
                _permuteTimes = 0;


                List<List<int>> permutations = PermuteRecrusive(nineNumber);
                DateTime now2 = DateTime.Now;
                Console.WriteLine("全排列结束" + now2);
                var inAc = CountDistinct(permutations, new List<int> {2, 3, 4}, ConvertFuc());
                Console.WriteLine($"黑白球在A,C两个箱子的组合数为{inAc}*2={inAc * 2}");
                var inAb = CountDistinct(permutations, new List<int> {2, 2, 5}, ConvertFuc());
                Console.WriteLine($"黑白球在A,B两个箱子的组合数为{inAb}*2={inAb * 2}");
                var inBc = CountDistinct(permutations, new List<int> {3, 2, 4}, ConvertFuc());
                Console.WriteLine($"黑白球在B,C两个箱子的组合数为{inBc}*2={inBc * 2}");
                var inA = CountDistinct(permutations, new List<int> {1, 3, 5}, ConvertFuc());
                Console.WriteLine($"黑白球都在A箱子的组合数为{inA}");
                var inB = CountDistinct(permutations, new List<int> {3, 1, 5}, ConvertFuc());
                Console.WriteLine($"黑白球都在B箱子的组合数为{inB}");
                var inC = CountDistinct(permutations, new List<int> {3, 3, 3}, ConvertFuc());
                Console.WriteLine($"黑白球都在C箱子的组合数为{inC}");
                Console.WriteLine($"所有组合的情况加起来的数量为{inAc * 2 + inAb * 2 + inBc * 2 + inA + inB + inC}");

        
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常代码是" + ex);
                Console.ReadLine();
            }
        }

        private static int CountDistinct(List<List<int>> permutations, List<int> list, Func<int, int> convertFuc)
        {
            List<string> source1 = new List<string>();
            int num = 0;
            foreach (List<int> source2 in permutations)
            {
                ++num;
                if (num % 100000 == 0)
                {
                    //Console.WriteLine();
                    //Console.WriteLine("去重前" + source1.Count);
                    source1 = source1.Distinct().ToList();
                    //Console.WriteLine("去重后" + source1.Count);
                    Console.WriteLine("已处理" + num / 10000 + "万条数据的去重");
                    //Console.WriteLine();
                }

                string modList = ConvertToString(source2.ToList(), list, convertFuc);
                source1.Add(modList);
            }

            IEnumerable<string> source3 = source1.Distinct();
            return source3.Count();
        }

        public static List<List<int>> PermuteRecrusive(List<int> ints)
        {
            List<List<int>> resSet = new List<List<int>>();
            if (ints == null || ints.Count == 0) return resSet;
            ints.Sort();
            PermuteHelper(ints, new List<int>(), resSet);
            return resSet;

        }

        public static void PermuteHelper(List<int> ints, List<int> subset, List<List<int>> resSet)
        {
            if (subset.Count == ints.Count)
            {
                ++_permuteTimes;
                if (_permuteTimes % 10000 == 0)
                {
                    //System.GC.Collect();
                    Console.WriteLine("已处理" + _permuteTimes / 10000 + "万条数据");
                }
                resSet.Add(new List<int>(subset));
            }
            else
            {
                foreach (int num in ints)
                {
                    if (subset.Contains(num)) continue;
                    subset.Add(num);
                    PermuteHelper(ints, subset, resSet);
                    subset.RemoveAt(subset.Count - 1);
                }
            }
        }

        private static string ConvertToString(List<int> newList, List<int> groups, Func<int, int> convertFuncC)
        {
            List<int> resultList = new List<int>();
            int before = 0;
            int after = 0;
            foreach (var groupcount in groups)
            {
                after += groupcount;
                List<int> tempList = new List<int>();
                for (int i = before; i < after; i++)
                {
                    tempList.Add(newList[i]);
                }

                tempList = tempList.Select(convertFuncC).OrderBy(c => c).ToList();
                before = after;
                resultList.AddRange(tempList);
            }


            return string.Join(",", resultList);
        }
    }
}