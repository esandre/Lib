using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Lib.Random.Test
{
    [TestClass]
    public class RandomExtensionsTest
    {
        private static readonly System.Random FakeGenerator = new FakeRandom();

        [TestMethod]
        public void Next_Throws_ForNonCoreTypes()
        {
            Check.ThatCode(() => FakeGenerator.Next(typeof(RandomExtensionsTest))).Throws<ArgumentOutOfRangeException>();
            Check.ThatCode(() => FakeGenerator.Next<RandomExtensionsTest>()).Throws<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Next_ReturnsGoodInstances_ByType()
        {
            var assertions = new List<(Type type, object value, Func<System.Random, object> generator)>
            {
                (typeof(bool), default(bool), r => r.NextBool()),
                (typeof(byte), default(byte), r => r.NextByte()),
                (typeof(char), default(char), r => r.NextChar()),
                (typeof(DateTime), default(DateTime), r => r.NextDateTime()),
                (typeof(decimal), default(decimal), r => r.NextDecimal()),
                (typeof(double), default(double), r => r.NextDouble()),
                (typeof(int), default(int), r => r.NextInt()),
                (typeof(short), default(short), r => r.NextShort()),
                (typeof(long), default(long), r => r.NextLong()),
                (typeof(sbyte), default(sbyte), r => r.NextSByte()),
                (typeof(float), default(float), r => r.NextFloat()),
                (typeof(ushort), default(ushort), r => r.NextUShort()),
                (typeof(uint), default(uint), r => r.NextUInt()),
                (typeof(ulong), default(ulong), r => r.NextULong())
            };

            foreach (var (type, expected, generator) in assertions)
            {
                Check.That(FakeGenerator.Next(type)).IsInstanceOfType(type).And.Equals(expected);
                Check.That(generator(FakeGenerator)).IsInstanceOfType(type).And.Equals(expected);

                var assertGenericMethod = typeof(RandomExtensionsTest).GetMethod(nameof(AssertGenericNextMethod), BindingFlags.NonPublic | BindingFlags.Static);
                assertGenericMethod.MakeGenericMethod(type).Invoke(null, new[] {expected});
            }
        }

        private static void AssertGenericNextMethod<TRandom>(object expected)
        {
            Check.That(FakeGenerator.Next<TRandom>()).Equals(expected);
        }
    }
}
