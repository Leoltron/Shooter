using System;
using System.Collections.Generic;

namespace Shooter
{
    class EnemyGenerator
    {
        private IEntityAdder entityAdder;
        private ISizeProvider sizeProvider;

        private readonly List<Action> enemyWaveGenerators;
        private readonly Random rand;

        public EnemyGenerator(IEntityAdder entityAdder, ISizeProvider sizeProvider)
        {
            this.entityAdder = entityAdder;
            this.sizeProvider = sizeProvider;
            rand = new Random();
            enemyWaveGenerators = new List<Action>
            {
                () =>
                {
                    //4-way bounce attack
                    const int health = 3;
                    var farX = sizeProvider.Width - 40;
                    var farY = sizeProvider.Height - 40;
                    entityAdder.AddEntity(
                        new BagelEnemy(BagelType.Bouncing, this.sizeProvider, null, 40, 40, 5, 5, health));
                    entityAdder.AddEntity(
                        new BagelEnemy(BagelType.Bouncing, this.sizeProvider, null, 40, farY, 5, -5, health));

                    entityAdder.AddEntity(
                        new BagelEnemy(BagelType.Bouncing, this.sizeProvider, null, farX, 40, -5, 5, health));
                    entityAdder.AddEntity(
                        new BagelEnemy(BagelType.Bouncing, this.sizeProvider, null, farX, farY, -5, -5, health));
                },
                () =>
                {
                    //clone attack
                    const int health = 10;
                    var farX = sizeProvider.Width - 40;
                    entityAdder.AddEntity(
                        new BagelEnemy(BagelType.Clone, this.sizeProvider, this.entityAdder,
                            40, 40, 0, 0, health));
                    entityAdder.AddEntity(
                        new BagelEnemy(BagelType.Clone, this.sizeProvider, this.entityAdder,
                            farX, 40, 0, 0, health));
                },() =>
                {
                    //healing attack
                    const int health = 10;
                    var farX = sizeProvider.Width - 40;
                    entityAdder.AddEntity(
                        new BagelEnemy(BagelType.Healing, this.sizeProvider, this.entityAdder,
                            40, 40, 0, 0, health));
                    entityAdder.AddEntity(
                        new BagelEnemy(BagelType.InvincibleClone, this.sizeProvider, this.entityAdder,
                            farX, 40, 0, 0, health));
                },
                () =>
                {
                    //inv clone attack
                    const int health = 10;
                    var farX = sizeProvider.Width - 40;
                    entityAdder.AddEntity(
                        new BagelEnemy(BagelType.InvincibleClone, this.sizeProvider, this.entityAdder,
                            40, 40, 0, 0, health));
                    entityAdder.AddEntity(
                        new BagelEnemy(BagelType.InvincibleClone, this.sizeProvider, this.entityAdder,
                            farX, 40, 0, 0, health));
                },
                () =>
                {
                    const int health = 1;
                    for (var i = 0; i > -500; i -= 64)
                    {
                        entityAdder.AddEntity(
                            new BagelEnemy(BagelType.Shooting, this.sizeProvider, this.entityAdder,
                                i, i, 10, 3, health));
                        entityAdder.AddEntity(
                            new BagelEnemy(BagelType.Shooting, this.sizeProvider, this.entityAdder,
                                sizeProvider.Width - i, i, -10, 3, health));
                    }
                },
                () =>
                {
                    const int health = 2;
                    for (var i = 0; i > -500; i -= 64)
                    {
                        entityAdder.AddEntity(
                            new BagelEnemy(BagelType.Basic, this.sizeProvider, this.entityAdder,
                                i, i, 5, 5, health));
                        entityAdder.AddEntity(
                            new BagelEnemy(BagelType.Basic, this.sizeProvider, this.entityAdder,
                                sizeProvider.Width - i, i, -5, 5, health));
                    }
                },
                () =>
                {
                    const int health = 2;
                    for (var i = 40; i < 700; i += 64)
                    {
                        entityAdder.AddEntity(
                            new BagelEnemy(BagelType.Basic, this.sizeProvider, this.entityAdder,
                                i / 2, i - 1000, 0, 5, health));
                        entityAdder.AddEntity(
                            new BagelEnemy(BagelType.Basic, this.sizeProvider, this.entityAdder,
                                sizeProvider.Width - i / 2, i - 1000, 0, 5, health));
                    }
                }
            };






        }

        public void GenerateNewEnemies()
        {
            enemyWaveGenerators[rand.Next(enemyWaveGenerators.Count)]();
        }
    }
}