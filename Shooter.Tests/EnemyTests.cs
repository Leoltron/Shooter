using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Shooter.Tests
{
    [TestFixture]
    internal class EnemyTests
    {
        [Test]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public void TestCreate()
        {
            var enemy = new Enemy();
        }

        [Test]
        public void TestTargetingPlayer()
        {
            var enemy = new Enemy();
            Assert.AreEqual(TargetType.Player,enemy.TargetType);
        }

        [Test]
        public void TestCollideDamage()
        {
            var enemy = new Enemy();
            var player = new Player(null,health:10);
            enemy.OnCollideWithTarget(player);
            Assert.AreEqual(9, player.Health);
        }
    }
}
