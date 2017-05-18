using System;
using System.Drawing;
using Shooter.Gui;

namespace Shooter
{
    public class Bullet : Entity, ISingleTexture
    {
        private readonly ISizeProvider sizeProvider;
        private int Damage { get; }
        public Entity Source { get; }

        public Bullet(
            Entity source,
            ISizeProvider sizeProvider,
            TargetType targetType,
            float x = 0,
            float y = 0,
            float velX = 0,
            float velY = 0,
            float direction = 0,
            int damage = 1,
            int howManyCanDamage = 1) : base(x, y, velX, velY, howManyCanDamage, direction)
        {
            this.sizeProvider = sizeProvider;
            Source = source;
            TargetType = targetType;
            AlignDirectionByVelocity();

            Damage = damage;
            if (howManyCanDamage <= 0)
                throw new ArgumentOutOfRangeException(nameof(howManyCanDamage) + " must be positive!");
            Health = howManyCanDamage;
            CollisionBox = new CollisionBox(this, 1, 1);
        }

        public override void OnCollideWithTarget(Entity targetEntity)
        {
            if (targetEntity.IsDead) return;
            targetEntity.DamageEntity(this, Damage);
            DamageEntity(targetEntity, 1);
        }

        public override void OnEntityTick()
        {
            base.OnEntityTick();
            if (IsOutside())
                SetDead(null);
        }

        private bool IsOutside()
        {
            return X < 0 || Y < 0 || X > sizeProvider.Width || Y > sizeProvider.Height;
        }

        public string GetTextureFileName()
        {
            return "Bullet.png";
        }

        public override Action<Graphics, Entity, bool> GetDrawingAction()
        {
            return SingleTextureEntityDrawer.DrawEntity;
        }
    }
}