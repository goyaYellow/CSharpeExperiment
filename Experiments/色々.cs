using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace Experiments
{
    public class 色々
    {
        [Fact]
        public void ToCollenction関数は参照を変えるか()
        {
            var source = new List<int>() { 1, 2, 3 };
            var array = source.ToArray();
            var list = source.ToList();
            var immutableList = source.ToImmutableList();

            source.Remove(1);
            source.Add(21);

            // 元リストの変更が、変換先に反映されなければ、参照が変わっている
            Assert.True(source.SequenceEqual(source));
            Assert.False(source.SequenceEqual(array));
            Assert.False(source.SequenceEqual(list));
            Assert.False(source.SequenceEqual(immutableList));

            // 変換先は同じ要素＆同じ並びのまま
            Assert.True(list.SequenceEqual(immutableList));
            Assert.True(array.SequenceEqual(immutableList));

            // 結論：参照を変える！元リストを変更しても、Toしたやつは変わらない！
        }

        [Fact]
        public void CollectionのGetHashは参照からではなく要素から生成されるか()
        {
            var list1 = new List<int>() { 1, 2, 3 };
            var list2 = new List<int>() { 1, 2, 3 };

            Assert.True(list1.GetHashCode() == list1.GetHashCode());
            Assert.False(list1.GetHashCode() == list2.GetHashCode()); // 値が同じならインスタンスが違ってもハッシュが同じ、なら要素による精製

            // 結論：要素から生成されない。（恐らく）参照から生成される！
        }

        [Fact]
        public void EnumerableのOfTypeはキャストできないインスタンスをいい感じに抜いて返してくれるのか()
        {
            var firsts = new List<Inherit1>() { new(1), new(2), new(3) };
            var seconds = new List<Inherit2>() { new(5), new(6), new(7) };

            var bases = new List<BaseRecord>();
            bases.AddRange(firsts.Cast<BaseRecord>());
            bases.AddRange(seconds.Cast<BaseRecord>());

            var recastFirsts = bases.OfType<Inherit1>();
            var recastSeconds = bases.OfType<Inherit2>();

            Assert.True(firsts.SequenceEqual(recastFirsts));
            Assert.True(seconds.SequenceEqual(recastSeconds));
            Assert.True(!bases.OfType<int>().Any());

            // 結論：キャストできる奴だけをいい感じに抜いてくれる！
        }

        public record BaseRecord();

        public record Inherit1(int Value) : BaseRecord;

        public record Inherit2(int Value) : BaseRecord;

        [Fact]
        public void LINQのOrderByはソート済みを再ソート時で早くなるのか()
        {
            /*
            var source = Enumerable.Range(0, 200);
            var seqence = source.OrderBy(x => x).ToList(); // 比較用に明示ソート
            var randam = source.RandomAll().ToList();　// 順番ランダムVer

            foreach (var i in Enumerable.Range(0, 10))
            {
                var swFirst = new System.Diagnostics.Stopwatch();
                swFirst.Start();
                _ = randam.OrderBy(x => x).ToList(); // インスタンス化しないとソート時間でない
                swFirst.Stop();

                var swSecand = new System.Diagnostics.Stopwatch();
                swSecand.Start();
                _ = seqence.OrderBy(x => x).ToList();　// インスタンス化しないとソート時間でない
                swSecand.Stop();

                Assert.True(swFirst.Elapsed.TotalSeconds > swSecand.Elapsed.TotalSeconds);
            }
            */

            // 結論：ソート済み再ソートだと早い気がするけど、ちょくちょく逆転する
            // この例だと数倍~100倍 程差がある... なんで？
        }

        [Fact]
        public void HashSetはハッシュ値は同じだがEqualsは異なるデータを追加できるのか()
        {
            var list = new List<SameHashRecord>() {
                new SameHashRecord(1),
                new SameHashRecord(2),
                new SameHashRecord(3),
            };

            var set = new HashSet<SameHashRecord>();

            Assert.True(set.Add(list[0]));
            Assert.True(set.Add(list[1]));
            Assert.True(set.Add(list[2]));
            Assert.False(set.Add(list[2]));
            Assert.True(set.SetEquals(list));

            // 結論：出来る！
            // ハッシュ値が同じ場合は、Eqalsで再確認してくれているっぽい！
        }

        private record SameHashRecord(int Value)
        {
            public override int GetHashCode() => 1;
        }
    }
}
