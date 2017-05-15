using System;

namespace Shooter
{
    class Bullet : Entity, IWeapon
    {
        private readonly Entity source;
        private ISizeProvider sizeProvider;
        public int Damage { get; private set; }

        public Bullet(
            Entity source,
            ISizeProvider sizeProvider,
            float x = 0,
            float y = 0,
            float velX = 0,
            float velY = 0,
            int health = 1,
            float direction = 0,
            int damage = 1,
            int howManyCanDamage = 1) : base(x, y, velX, velY, health, direction)
        {
            this.sizeProvider = sizeProvider;
            this.source = source;
            AlignDirectionByVelocity();

            Damage = damage;
            if (howManyCanDamage < 0)
                throw new ArgumentException(nameof(howManyCanDamage) + " must be non-negative!");
            Health = howManyCanDamage;
            CollisionBox = new CollisionBox(this, 1, 1);
        }

        public override void OnCollideWithTarget(Entity targetEntity)
        {
            if (Damage == 0 || targetEntity.IsDead) return;
            targetEntity.DamageEntity(this, Damage);
            DamageEntity(targetEntity, 1);
        }

        public Entity GetSource()
        {
            return source;
        }

        public Bullet SetTargetType(TargetType targetType)
        {
            TargetType = targetType;
            return this;
        }

        public override void OnEntityTick()
        {
            if (IsOutside())
                SetDead(null);
            else
                base.OnEntityTick();
        }

        private bool IsOutside()
        {
            return X < 0 || Y < 0 || X > sizeProvider.Width || Y > sizeProvider.Height;
        }

        public override string GetTextureFileName()
        {
            return "Bullet.png";
        }
    }
}