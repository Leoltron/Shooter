using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

        [TestCase(0, 0, 0, 0, 1, 0, 0)]
        [TestCase(0, 0, 0, 0, 50, 0, 0)]
        [TestCase(0, 0, 1, 0, 1, 1, 0)]
        [TestCase(0, 0, 1, -1, 1, 1, -1)]
        [TestCase(1, 2, 1, -1, 1, 2, 1)]
        [TestCase(1, 50, 1, -1, 50, 51, 0)]
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
            var entity = new Entity(health: 10);
            entity.DamageEntity(null, 6);
            Assert.AreEqual(4, entity.Health);
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
            Assert.AreSame(entityKiller, entity.DeathSource);
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

        [TestCase(0, 0, 0, 0, 1, 0, 0)]
        [TestCase(0, 0, 0, 0, 50, 0, 0)]
        [TestCase(0, 0, 1, 0, 1, 1, 0)]
        [TestCase(0, 0, 1, -1, 1, 1, -1)]
        [TestCase(1, 2, 1, -1, 1, 2, 1)]
        [TestCase(1, 50, 1, -1, 50, 51, 0)]
        public void TestTick(
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
                entity.OnEntityTick();
            Assert.AreEqual(expectedX, entity.X, 1e-5);
            Assert.AreEqual(expectedY, entity.Y, 1e-5);
        }

        [Test]
        public void TestNoMovingTickWhenDead()
        {
            var entity = new Entity(0, 0, 1, -2);
            entity.OnEntityTick();
            entity.SetDead(null);
            entity.OnEntityTick();
            Assert.AreEqual(1f, entity.X, 1e-5);
            Assert.AreEqual(-2f, entity.Y, 1e-5);
        }

        [Test]
        public void TestAlignByVelocity()
        {
            var entity = new Entity(0, 0, -1, -1);
            entity.AlignDirectionByVelocity();
            Assert.AreEqual(Math.PI / 4 * 3, entity.Direction, 1e-5);
        }

        public static bool IsEmptyOrIsATextureNameString(String s)
        {
            return string.IsNullOrEmpty(s) || s.ToLower().EndsWith(".png");
        }

        [Test]
        public void TestCollidesWith()
        {
            var entity1 = new Entity(1, 2, collisionBoxWidth: 4, collisionBoxHeight: 8);
            var entity2 = new Entity(4, 7);

            Assert.IsFalse(entity1.CollidesWith(entity2));
            entity2 = new Entity(4, 7, collisionBoxWidth: 4, collisionBoxHeight: 8);
            Assert.IsTrue(entity1.CollidesWith(entity2));


            entity2 = new Entity(4, 7, collisionBoxWidth: 1, collisionBoxHeight: 1);
            Assert.IsFalse(entity1.CollidesWith(entity2));
        }

        [Test]
        public void TestOnCollideWithTarget()
        {
            var entity = new Entity();
            entity.OnCollideWithTarget(entity);
        }

        [Test]
        public void TestIsTarget()
        {
            foreach (var ttype in Enum.GetValues(typeof(TargetType)).Cast<TargetType>())
            {
                var entity = new Entity(targetType: ttype);
                var target = new Entity();

                Assert.IsFalse(entity.IsTarget(target));
            }
            var e = new Entity(targetType: TargetType.Player);
            var t = new Entity();

            Assert.IsFalse(e.IsTarget(null));
            Assert.IsFalse(e.IsTarget(t));
            t = new Player(null);
            Assert.IsTrue(e.IsTarget(t));
        }

        [Test]
        public void TestGetTextureName()
        {
            var entity = new Entity();
            Assert.IsTrue(IsEmptyOrIsATextureNameString(entity.GetTextureFileName()));
        }

        [Test]
        public void TestToString()
        {
            var entity = new Entity();
            Assert.IsTrue(entity.ToString().Length > 0);
        }
    }
}