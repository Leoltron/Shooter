using System;
using System.Diagnostics.CodeAnalysis;
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
    }
}
