using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;

namespace Shooter.Tests
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    class BagelTests
    {
        [Test]
        public void TestCreate()
        {
            var bagel = new BagelEnemy(BagelType.Basic);
        }

        [Test]
        public void TestCreateInvalidBouncingBagel()
        {
            Assert.Throws<ArgumentException>(() => new BagelEnemy(BagelType.Bouncing));
        }

        [Test]
        public void TestCreateInvalidShootingBagel()
        {
            Assert.Throws<ArgumentException>(() => new BagelEnemy(BagelType.Shooting));
        }

        [Test]
        public void TestCreateInvalidCloneBagel()
        {
            Assert.Throws<ArgumentException>(() => new BagelEnemy(BagelType.Shooting));
        }

        [Test]
        public void TestCreateInvalidInvincibleCloneBagel()
        {
            Assert.Throws<ArgumentException>(() => new BagelEnemy(BagelType.Shooting));
        }

        [Test]
        public void TestHeal()
        {
            var bagel = new BagelEnemy(BagelType.Healing, health: 1);
            var entity = new Entity(health: 5);
            bagel.OnCollideWithTarget(entity);
            Assert.AreEqual(6, entity.Health);
        }

        [Test]
        public void TestFiringShootingBagel()
        {
            var game = new Game(200, 200);
            var bagel = new BagelEnemy(BagelType.Shooting, game, game);
            game.AddEntity(bagel);
            for (var i = 0; i < BagelEnemy.ShootingInterval; i++)
                bagel.OnEntityTick();
            game.GameTick();
            Assert.AreEqual(3, game.GetEntities.Count());
        }

        [Test]
        public void TestFiringClonesBagel()
        {
            var game = new Game(200, 200);
            var bagel = new BagelEnemy(BagelType.Clone, game, game,health:10);
            game.AddEntity(bagel);
            for (var i = 0; i < BagelEnemy.ShootingInterval; i++)
                bagel.OnEntityTick();
            game.GameTick();
            Assert.AreEqual(6, game.GetEntities.Count());
        }

        [Test]
        public void TestGetDrawingAction()
        {
            var texture = new BagelEnemy(BagelType.Basic).GetDrawingAction();
        }

        [Test]
        public void TestCollisionBoxChangingAfterDamage()
        {
            var bagel = new BagelEnemy(BagelType.Basic, null, null, health: 3);
            var oldCBW = bagel.CollisionBox.Width;
            var oldCBH = bagel.CollisionBox.Height;
            bagel.DamageEntity(null,1);
            Assert.IsTrue(oldCBW > bagel.CollisionBox.Width);
            Assert.IsTrue(oldCBH > bagel.CollisionBox.Height);
        }

        [Test]
        public void TestInvincibleCloneDyingEarly()
        {
            var game = new Game(200, 200);
            var bagel = new BagelEnemy(BagelType.InvincibleClone, game, game, health: 3);
            bagel.DamageEntity(null, 1);
            Assert.IsFalse(bagel.IsDead);
            bagel.DamageEntity(null, 1);
            Assert.IsTrue(bagel.IsDead);
        }

        [Test]
        public void TestDyingOutside()
        {
            var game = new Game(200, 200);
            var bagel = new BagelEnemy(BagelType.Basic, game, game, 50,50,-100,-100);
            Assert.IsFalse(bagel.IsDead);
            bagel.Move();
            Assert.IsTrue(bagel.IsDead);
        }

        [Test]
        public void TestBouncing()
        {
            var game = new Game(200, 200);
            var bagel = new BagelEnemy(BagelType.Bouncing, game, game, 50, 50, -100, -100);
            Assert.IsFalse(bagel.IsDead);
            bagel.Move();
            Assert.IsFalse(bagel.IsDead);
            Assert.AreEqual(100,bagel.VelX);
            Assert.AreEqual(100,bagel.VelY);
        }
    }
}