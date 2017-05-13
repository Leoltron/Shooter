using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Shooter.Tests
{
    [TestFixture]
    [SuppressMessage("ReSharper", "UnusedVariable")]
    public class EntityTests
    {
        [Test]
        public void TestCreateEntity()
        {
            var entity = new Entity();
        }

        [TestCase(0,0,0,0,1,0,0)]
        [TestCase(0,0,0,0,50,0,0)]
        [TestCase(0,0,1,0,1,1,0)]
        [TestCase(0,0,1,-1,1,1,-1)]
        [TestCase(1,2,1,-1,1,2,1)]
        [TestCase(1,50,1,-1,50,51,0)]
        public void TestMoveEntity(
            float startX,
            float startY,
            float velX,
            float velY,
            int movesCount,
            float expectedX,
            float expectedY)
        {
            var entity = new Entity(startX, startY, velX, velY);
            for (var i = 0; i < movesCount; i++)
                entity.Move();
            Assert.AreEqual(expectedX, entity.X, 1e-5);
            Assert.AreEqual(expectedY, entity.Y, 1e-5);
        }

        [Test]
        public void TestDamage()
        {
            var entity = new Entity(health:10);
            entity.DamageEntity(null,6);
            Assert.AreEqual(4,entity.Health);
        }

        [Test]
        public void TestKill()
        {
            var entity = new Entity(health: 10);
            entity.DamageEntity(null, 10);
            Assert.IsTrue(entity.IsDead);
        }

        [Test]
        public void TestNoRevive()
        {
            var entity = new Entity(health: 10);
            entity.DamageEntity(null, 10);
            entity.DamageEntity(null, -10);
            Assert.IsTrue(entity.IsDead);
        }

        [Test]
        public void TestDeathReason()
        {
            var entity = new Entity(health: 10);
            var entityKiller = new Entity();
            entity.DamageEntity(entityKiller, 10);
            Assert.IsTrue(entity.IsDead);
            Assert.AreSame(entityKiller,entity.DeathSource);
        }

        [Test]
        public void TestNoDeathReasonOverwriting()
        {
            var entity = new Entity(health: 10);
            var entityKiller = new Entity();
            var entityKiller2 = new Entity();
            entity.DamageEntity(entityKiller, 20);
            entity.DamageEntity(entityKiller, -20);
            entity.DamageEntity(entityKiller2, 20);
            Assert.IsTrue(entity.IsDead);
            Assert.AreSame(entityKiller, entity.DeathSource);
        }
    }
}