using System;
using System.Drawing;
using Shooter.Gui;

namespace Shooter
{
    public enum BagelType
    {
        Basic,
        Bouncing,
        Clone,
        Healing,
        InvincibleClone,
        Shooting
    }

    public class BagelEnemy : Enemy
    {
        public readonly BagelType BagelType;
        private readonly ISizeProvider sizeProvider;
        private readonly IEntityAdder entityAdder;
        private static readonly float[] HealthBagelCollisionBoxWidth = {24, 40, 48, 58};
        private static readonly float[] HealthBagelCollisionBoxHeight = {24, 40, 48, 58};
        public const int ShootingInterval = 20;
        private const float ShotSpeed = 5;
        private int shootingDelay;
        private readonly Random rand;

        public BagelEnemy(BagelType bagelType,
            ISizeProvider sizeProvider = null,
            IEntityAdder entityAdder = null,
            float x = 0,
            float y = 0,
            float velX = 0,
            float velY = 0,
            int health = 1,
            float direction = 0) :
            base(x, y, velX, velY, health, direction)
        {
            BagelType = bagelType;

            this.sizeProvider = sizeProvider;
            if (sizeProvider == null && bagelType == BagelType.Bouncing)
                throw new ArgumentException("How I can bounce when there're no walls?");

            this.entityAdder = entityAdder;
            if (entityAdder == null &&
                (bagelType == BagelType.Shooting ||
                 bagelType == BagelType.Clone ||
                 bagelType == BagelType.InvincibleClone))
                throw new ArgumentException("I must add \"bullets\" somewhere!");
            if (bagelType == BagelType.Clone ||
                bagelType == BagelType.InvincibleClone)
                rand = new Random();

            UpdateCollisionBox();
        }

        public override void OnCollideWithTarget(Entity targetEntity)
        {
            if (BagelType == BagelType.Healing && Health == 1)
            {
                targetEntity.DamageEntity(this, -1);
                SetDead(null);
            }
            else
                base.OnCollideWithTarget(targetEntity);
        }

        public override void OnEntityTick()
        {
            base.OnEntityTick();
            shootingDelay++;
            if (shootingDelay == ShootingInterval)
            {
                shootingDelay = 0;
                Fire();
            }
        }

        private void Fire()
        {
            switch (BagelType)
            {
                case BagelType.Shooting:
                    entityAdder.AddEntity(
                        new BagelEnemy(BagelType.Clone,
                            sizeProvider,
                            entityAdder,
                            X, Y,
                            0, ShotSpeed,
                            health:1)
                    );
                    break;
                case BagelType.Clone:
                case BagelType.InvincibleClone:
                    if (Health > 1)
                        AddFourDirectionalBagels(BagelType);
                    break;
            }
        }

        private void AddFourDirectionalBagels(BagelType bagelType)
        {
            var angle = rand.NextDouble() * Math.PI * 2;
            entityAdder.AddEntity(
                new BagelEnemy(bagelType,
                    sizeProvider,
                    entityAdder,
                    X, Y,
                    (float) (ShotSpeed * Math.Cos(angle)),
                    (float) (ShotSpeed * Math.Sin(angle)),
                    1)
            );
            entityAdder.AddEntity(
                new BagelEnemy(bagelType,
                    sizeProvider,
                    entityAdder,
                    X, Y,
                    -(float) (ShotSpeed * Math.Cos(angle)),
                    -(float) (ShotSpeed * Math.Sin(angle)),
                    1)
            );
            entityAdder.AddEntity(
                new BagelEnemy(bagelType,
                    sizeProvider,
                    entityAdder,
                    X, Y,
                    (float) (ShotSpeed * Math.Sin(angle)),
                    -(float) (ShotSpeed * Math.Cos(angle)),
                    1)
            );
            entityAdder.AddEntity(
                new BagelEnemy(bagelType,
                    sizeProvider,
                    entityAdder,
                    X, Y,
                    -(float) (ShotSpeed * Math.Sin(angle)),
                    (float) (ShotSpeed * Math.Cos(angle)),
                    1)
            );
        }

        public override void Move()
        {
            var newX = X + VelX;
            var newY = Y + VelY;
          if ((newX - CollisionBox.Width / 2 < 0 || newX + CollisionBox.Width / 2 > sizeProvider.Width) &&
                !(CollisionBox.Left < 0 || CollisionBox.Right > sizeProvider.Width))
            {
                if (BagelType == BagelType.Bouncing)
                    VelX *= -1;
                else
                    SetDead(null);
            }
            if ((newY - CollisionBox.Height / 2 < 0 || newY + CollisionBox.Height / 2 > sizeProvider.Height)
                && !(CollisionBox.Top < 0 || CollisionBox.Bottom > sizeProvider.Height))
            {
                if (BagelType == BagelType.Bouncing)
                    VelY *= -1;
                else
                    SetDead(null);
            }
            base.Move();
        }

        public override void DamageEntity(Entity source, int damage)
        {
            if (BagelType == BagelType.InvincibleClone && Health < 3)
            {
                if (Health != 1 && Health - damage <= 1)
                    SetDead(source);
            }
            else
            {
                base.DamageEntity(source, damage);
                if (damage != 0 && !IsDead)
                    UpdateCollisionBox();
            }
        }

        private void UpdateCollisionBox()
        {
            var sizeNumber = Math.Min(HealthBagelCollisionBoxWidth.Length - 1, Health - 1);
            if (Health > 0)
                CollisionBox = new CollisionBox(this,
                    HealthBagelCollisionBoxWidth[sizeNumber],
                    HealthBagelCollisionBoxHeight[sizeNumber]);
        }

        public override Action<Graphics, Entity, bool> GetDrawingAction()
        {
            return BagelDrawer.DrawBagel;
        }
    }
}