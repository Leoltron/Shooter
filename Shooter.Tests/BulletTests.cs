using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Shooter.Tests
{
    [TestFixture]
    [SuppressMessage("ReSharper", "UnusedVariable")]
    internal class BulletTests
    {
        [Test]
        public void TestCreate()
        {
            var bullet = new Bullet(null, null, TargetType.None);
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void TestCreateNegativeDamageBullet()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new Bullet(null, null, TargetType.None, howManyCanDamage: -1));
        }

        [Test]
        public void TestDyingOutside()
        {
            var bullet = new Bullet(null,
                new DummySizeProvider {Width = 10, Height = 10},
                TargetType.None,
                0, 0,
                -1, -1);
            Assert.IsFalse(bullet.IsDead);
            bullet.OnEntityTick();
            Assert.IsTrue(bullet.IsDead);
        }

        [Test]
        public void TestGetTextureFileName()
        {
            var textureFileName = new Bullet(null, null, TargetType.None).GetTextureFileName();
            Assert.IsTrue(string.IsNullOrEmpty(textureFileName) || textureFileName.ToLower().EndsWith(".png"));
        }

        [Test]
        public void TestGetDrawingAction()
        {
            var texture = new Bullet(null, null, TargetType.None).GetDrawingAction();
        }

        [Test]
        public void TestSource()
        {
            var entity = new Entity();
            var bullet = new Bullet(entity,null,TargetType.None);
            Assert.AreSame(entity,bullet.Source);
        }

        [Test]
        public void TestCollideWithTarget()
        {
            var player = new Player(null,health:10);
            var bullet = new Bullet(null,null,TargetType.Player,howManyCanDamage:4,damage:5);
            bullet.OnCollideWithTarget(player);
            Assert.AreEqual(5,player.Health);
            Assert.AreEqual(3,bullet.Health);
        }

        [Test]
        public void TestCollideWithDeadTarget()
        {
            var player = new Player(null, health: 10);
            player.SetDead(null);
            var bullet = new Bullet(null, null, TargetType.Player, howManyCanDamage: 4, damage: 5);
            bullet.OnCollideWithTarget(player);
            Assert.AreEqual(10, player.Health);
            Assert.AreEqual(4, bullet.Health);
        }
    }
}