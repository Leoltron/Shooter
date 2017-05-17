using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Shooter.Tests
{
    [TestFixture]
    class CollisionBoxTests
    {
        [Test]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public void TestCreate()
        {
            var cb = new CollisionBox(null, 1f, 1f);
        }

        [TestCase(0, 0)]
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, -1)]
        public void TestNotPositiveSize(float width, float height)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CollisionBox(null, width, height));
        }

        [Test]
        public void TestSides()
        {
            var entity = new Entity(1, 2);
            var cb = new CollisionBox(entity, 4, 8);
            Assert.AreEqual(6, cb.Bottom);
            Assert.AreEqual(-2, cb.Top);
            Assert.AreEqual(-1, cb.Left);
            Assert.AreEqual(3, cb.Right);
        }

        [Test]
        public void TestCollidesWith()
        {
            var entity1 = new Entity(1, 2);
            var cb1 = new CollisionBox(entity1, 4, 8);
            var entity2 = new Entity(4, 7);
            var cb2 = new CollisionBox(entity2, 4, 8);
            Assert.IsTrue(cb1.CollidesWith(cb2));
            Assert.IsFalse(cb1.CollidesWith(null));

            cb2 = new CollisionBox(entity2, 1, 1);
            Assert.IsFalse(cb1.CollidesWith(cb2));
        }
    }
}