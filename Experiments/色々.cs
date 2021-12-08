using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace Experiments
{
    public class �F�X
    {
        [Fact]
        public void ToCollenction�֐��͎Q�Ƃ�ς��邩()
        {
            var source = new List<int>() { 1, 2, 3 };
            var array = source.ToArray();
            var list = source.ToList();
            var immutableList = source.ToImmutableList();

            source.Remove(1);
            source.Add(21);

            // �����X�g�̕ύX���A�ϊ���ɔ��f����Ȃ���΁A�Q�Ƃ��ς���Ă���
            Assert.True(source.SequenceEqual(source));
            Assert.False(source.SequenceEqual(array));
            Assert.False(source.SequenceEqual(list));
            Assert.False(source.SequenceEqual(immutableList));

            // �ϊ���͓����v�f���������т̂܂�
            Assert.True(list.SequenceEqual(immutableList));
            Assert.True(array.SequenceEqual(immutableList));

            // ���_�F�Q�Ƃ�ς���I�����X�g��ύX���Ă��ATo������͕ς��Ȃ��I
        }

        [Fact]
        public void Collection��GetHash�͎Q�Ƃ���ł͂Ȃ��v�f���琶������邩()
        {
            var list1 = new List<int>() { 1, 2, 3 };
            var list2 = new List<int>() { 1, 2, 3 };

            Assert.True(list1.GetHashCode() == list1.GetHashCode());
            Assert.False(list1.GetHashCode() == list2.GetHashCode()); // �l�������Ȃ�C���X�^���X������Ă��n�b�V���������A�Ȃ�v�f�ɂ�鐸��

            // ���_�F�v�f���琶������Ȃ��B�i���炭�j�Q�Ƃ��琶�������I
        }

        [Fact]
        public void Enumerable��OfType�̓L���X�g�ł��Ȃ��C���X�^���X�����������ɔ����ĕԂ��Ă����̂�()
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

            // ���_�F�L���X�g�ł���z���������������ɔ����Ă����I
        }

        public record BaseRecord();

        public record Inherit1(int Value) : BaseRecord;

        public record Inherit2(int Value) : BaseRecord;

        [Fact]
        public void LINQ��OrderBy�̓\�[�g�ς݂��ă\�[�g���ő����Ȃ�̂�()
        {
            /*
            var source = Enumerable.Range(0, 200);
            var seqence = source.OrderBy(x => x).ToList(); // ��r�p�ɖ����\�[�g
            var randam = source.RandomAll().ToList();�@// ���ԃ����_��Ver

            foreach (var i in Enumerable.Range(0, 10))
            {
                var swFirst = new System.Diagnostics.Stopwatch();
                swFirst.Start();
                _ = randam.OrderBy(x => x).ToList(); // �C���X�^���X�����Ȃ��ƃ\�[�g���ԂłȂ�
                swFirst.Stop();

                var swSecand = new System.Diagnostics.Stopwatch();
                swSecand.Start();
                _ = seqence.OrderBy(x => x).ToList();�@// �C���X�^���X�����Ȃ��ƃ\�[�g���ԂłȂ�
                swSecand.Stop();

                Assert.True(swFirst.Elapsed.TotalSeconds > swSecand.Elapsed.TotalSeconds);
            }
            */

            // ���_�F�\�[�g�ςݍă\�[�g���Ƒ����C�����邯�ǁA���傭���傭�t�]����
            // ���̗Ⴞ�Ɛ��{~100�{ ����������... �Ȃ�ŁH
        }

        [Fact]
        public void HashSet�̓n�b�V���l�͓�������Equals�͈قȂ�f�[�^��ǉ��ł���̂�()
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

            // ���_�F�o����I
            // �n�b�V���l�������ꍇ�́AEqals�ōĊm�F���Ă���Ă�����ۂ��I
        }

        private record SameHashRecord(int Value)
        {
            public override int GetHashCode() => 1;
        }
    }
}
