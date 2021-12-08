using System;
using System.Collections.Generic;
using System.Linq;

namespace Experiments
{
    public class PLinqはLinqのメソッドも早くしてくれるのか
    {
        /* 結論
         * してはくれるが
         * ①重たい処理をしたい & ② 件数が多い & ③PCリソースが多いのにシングルスレッドだと活用しきれていない
         * 場合以外は特に早くならないっぽい
         */

        private const int REPIEAT_COUNT = 5;
        private const int COUNT = 100000000;
        private static readonly IEnumerable<int> Soruce = Enumerable.Range(0, COUNT);
        private static readonly IEnumerable<int> Seq = Soruce.OrderBy(x => Guid.NewGuid()); // 順番をランダム化
        private static readonly ParallelQuery<int> Para = Seq.AsParallel();

        /// <summary> 重たい処理。必ずTrueが返る </summary>
        private static bool Heavy(int x)
        {
            Math.Pow(x, 05);
            return true;
        }

        /// <summary> 軽い処理。必ずTrueが返る </summary>
        private static bool Light(int x)
        {
            _ = x * 0.5;
            return true;
        }

        /* 時間かかるので無効化
        public class 重いい処理ver
        {
            private static readonly Func<int, bool> Proc = Heavy;

            [Fact]
            public void All編()
            {
                var swSeq = new System.Diagnostics.Stopwatch();
                var swPara = new System.Diagnostics.Stopwatch();

                foreach (var i in EnumerableLike.RangeTo(REPIEAT_COUNT))
                {
                    swSeq.Reset();
                    swSeq.Start();
                    _ = Seq.All(x => Proc(x));
                    swSeq.Stop();

                    swPara.Reset();
                    swPara.Start();
                    _ = Para.All(x => Proc(x));
                    swPara.Stop();

                    Assert.True(swSeq.Elapsed.TotalSeconds > swPara.Elapsed.TotalSeconds);
                }
            }

            [Fact]
            public void Count編()
            {
                var swSeq = new System.Diagnostics.Stopwatch();
                var swPara = new System.Diagnostics.Stopwatch();

                foreach (var i in EnumerableLike.RangeTo(REPIEAT_COUNT))
                {
                    swSeq.Reset();
                    swSeq.Start();
                    _ = Seq.Count(x => Proc(x));
                    swSeq.Stop();

                    swPara.Reset();
                    swPara.Start();
                    _ = Para.Count(x => Proc(x));
                    swPara.Stop();

                    Assert.True(swSeq.Elapsed.TotalSeconds > swPara.Elapsed.TotalSeconds);
                }
            }

            [Fact]
            public void Select編()
            {
                var swSeq = new System.Diagnostics.Stopwatch();
                var swPara = new System.Diagnostics.Stopwatch();

                foreach (var i in EnumerableLike.RangeTo(REPIEAT_COUNT))
                {
                    swSeq.Reset();
                    swSeq.Start();
                    _ = Seq.Select(x => Proc(x)).ToList();
                    swSeq.Stop();

                    swPara.Reset();
                    swPara.Start();
                    _ = Para.Select(x => Proc(x)).ToList();
                    swPara.Stop();

                    Assert.True(swSeq.Elapsed.TotalSeconds > swPara.Elapsed.TotalSeconds);
                }
            }

            [Fact]
            public void Where編()
            {
                var swSeq = new System.Diagnostics.Stopwatch();
                var swPara = new System.Diagnostics.Stopwatch();

                foreach (var i in EnumerableLike.RangeTo(REPIEAT_COUNT))
                {
                    swSeq.Reset();
                    swSeq.Start();
                    _ = Seq.Where(x => Proc(x)).ToList();
                    swSeq.Stop();

                    swPara.Reset();
                    swPara.Start();
                    _ = Para.Where(x => Proc(x)).ToList();
                    swPara.Stop();

                    Assert.True(swSeq.Elapsed.TotalSeconds > swPara.Elapsed.TotalSeconds);
                }
            }
        */
    }
}
