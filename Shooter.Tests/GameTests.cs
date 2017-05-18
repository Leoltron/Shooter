using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;

namespace Shooter.Tests
{
    [TestFixture]
    class GameTests
    {
        [Test]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public void TestCreate()
        {
            var game = new Game(100, 100);
        }

        [Test]
        public void TestHavePlayer()
        {
            var game = new Game(100, 100);
            game.GameTick();
            Assert.AreEqual(1, game.GetEntities.Count(e => e is Player));
        }

        [Test]
        public void TestAddEntity()
        {
            var game = new Game(100, 100);
            for (var i = 0; i < 4; i++)
                game.AddEntity(new Enemy());
            game.GameTick();
            Assert.AreEqual(4, game.GetEntities.Count(e => e is Enemy));
        }

        [Test]
        public void TestMovingUp()
        {
            var game = new Game(400, 400);
            var x = game.Player.X;
            var y = game.Player.Y;
            game.SetPlayerGoingUp(true);
            game.GameTick();
            Assert.AreEqual(x, game.Player.X, 1e-5);
            Assert.AreEqual(y - Game.PlayerSpeed, game.Player.Y, 1e-5);
            game.SetPlayerGoingUp(false);
            game.GameTick();
            Assert.AreEqual(x, game.Player.X, 1e-5);
            Assert.AreEqual(y - Game.PlayerSpeed, game.Player.Y, 1e-5);
        }

        [Test]
        public void TestMovingLeft()
        {
            var game = new Game(400, 400);
            var x = game.Player.X;
            var y = game.Player.Y;
            game.SetPlayerGoingLeft(true);
            game.GameTick();
            Assert.AreEqual(x - Game.PlayerSpeed, game.Player.X, 1e-5);
            Assert.AreEqual(y, game.Player.Y, 1e-5);
            game.SetPlayerGoingLeft(false);
            game.GameTick();
            Assert.AreEqual(x - Game.PlayerSpeed, game.Player.X, 1e-5);
            Assert.AreEqual(y, game.Player.Y, 1e-5);
        }

        [Test]
        public void TestMovingDown()
        {
            var game = new Game(400, 400);
            var x = game.Player.X;
            var y = game.Player.Y;
            game.SetPlayerGoingDown(true);
            game.GameTick();
            Assert.AreEqual(x, game.Player.X, 1e-5);
            Assert.AreEqual(y + Game.PlayerSpeed, game.Player.Y, 1e-5);
            game.SetPlayerGoingDown(false);
            game.GameTick();
            Assert.AreEqual(x, game.Player.X, 1e-5);
            Assert.AreEqual(y + Game.PlayerSpeed, game.Player.Y, 1e-5);
        }

        [Test]
        public void TestMovingRight()
        {
            var game = new Game(400, 400);
            var x = game.Player.X;
            var y = game.Player.Y;
            game.SetPlayerGoingRight(true);
            game.GameTick();
            Assert.AreEqual(x + Game.PlayerSpeed, game.Player.X, 1e-5);
            Assert.AreEqual(y, game.Player.Y, 1e-5);
            game.SetPlayerGoingRight(false);
            game.GameTick();
            Assert.AreEqual(x + Game.PlayerSpeed, game.Player.X, 1e-5);
            Assert.AreEqual(y, game.Player.Y, 1e-5);
        }

        [Test]
        public void TestRemovingDead()
        {
            var game = new Game(100, 100);
            game.GameTick();
            Assert.AreEqual(1, game.GetEntities.Count());
            game.Player.SetDead(null);
            game.GameTick();
            Assert.AreEqual(0, game.GetEntities.Count());
            var enemies = new List<Entity>();
            for (var i = 0; i < 4; i++)
            {
                var e = new Enemy();
                enemies.Add(e);
                game.AddEntity(e);
            }
            game.GameTick();
            Assert.AreEqual(4, game.GetEntities.Count());
            foreach (var entity in enemies)
                entity.SetDead(null);
            game.GameTick();
            Assert.AreEqual(0, game.GetEntities.Count());
        }

        [Test]
        public void TestCollide()
        {
            var game = new Game(100, 100);
            var enemy = new Enemy(50,50,0,0,5,0,50,50);
            var bullet = new Bullet(null,game,TargetType.Enemy,50,50,0,0,0,1,1);
            game.AddEntity(enemy);
            game.AddEntity(bullet);
            game.GameTick();
            Assert.IsTrue(bullet.IsDead);
            Assert.IsFalse(enemy.IsDead);
        }

        [Test]
        public void TestOnEntityKilledByPlayerBullet()
        {
            var game = new Game(100, 100);
            var prevScore = game.Score;
            var enemy = new BagelEnemy(BagelType.Basic,null,null,50, 50, 0, 0, 1, 0);
            var bullet = new Bullet(game.Player, game, TargetType.Enemy, 50, 50, 0, 0, 0, 1, 1);
            game.AddEntity(enemy);
            game.AddEntity(bullet);
            game.GameTick();
            Assert.IsTrue(prevScore < game.Score);
        }

        [Test]
        public void TestFire()
        {
            var game = new Game(100, 100);
            game.Fire();
            game.GameTick();
            Assert.AreEqual(2, game.GetEntities.Count());
        }

        [Test]
        public void TestDontFireTooOften()
        {
            var game = new Game(100, 100);
            game.Fire();
            game.Fire();
            game.GameTick();
            Assert.AreEqual(2, game.GetEntities.Count());
        }
    }
}