using NUnit.Framework;

namespace Shooter.Tests
{
    [TestFixture]
    class PlayerTests
    {
        [Test]
        public void TestCreate()
        {
            var player = new Player(new DummySizeProvider());
        }

        [Test]
        public void TestMovement()
        {
            var player = new Player(new DummySizeProvider(), speed: 25f);
            player.HorizontalMovement = 1;
            Assert.AreEqual(25f, player.VelX, 1e-5);

            player.VerticalMovement = -1;
            Assert.AreEqual(-25f, player.VelY, 1e-5);
        }

        [Test]
        public void TestMoving()
        {
            var player = new Player(new DummySizeProvider {Height = 100,Width = 100},50,50, speed: 5f);
            player.HorizontalMovement = 1;
            player.Move();

            Assert.AreEqual(55,player.X,1e-5);
            Assert.AreEqual(50,player.Y,1e-5);


            player.VerticalMovement = -2;
            player.HorizontalMovement = -2;
            player.Move();

            Assert.AreEqual(45, player.X, 1e-5);
            Assert.AreEqual(40, player.Y, 1e-5);
        }

        [Test]
        public void TestMovingInsideBox()
        {
            var player = new Player(new DummySizeProvider { Height = 100, Width = 100 }, 50, 50, speed: 10f);
            player.HorizontalMovement = 1000;
            player.Move();
            Assert.IsTrue(player.X <= 100);
        }


        [Test]
        public void TestUpgrades()
        {
            var player = new Player(new DummySizeProvider());

            for (var level = 0; level <= Player.MaxBoostersLevel; level++)
            {
                Assert.AreEqual(level, player.BoostersLevel);
                player.UpgradeBoosters();
            }
            Assert.AreEqual(Player.MaxBoostersLevel, player.BoostersLevel);

            for (var level = 0; level <= Player.MaxGunsAmountLevel; level++)
            {
                Assert.AreEqual(level, player.GunsAmountLevel);
                player.UpgradeGunsAmount();
            }
            Assert.AreEqual(Player.MaxGunsAmountLevel, player.GunsAmountLevel);
        }
    }

    class DummySizeProvider : ISizeProvider
    {
        public float Width { get; set; }
        public float Height { get; set; }
    }
}