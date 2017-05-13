using System;

namespace Shooter
{
    class Bullet : Entity, IWeapon
    {
        private readonly Entity source;
        public int Damage { get; private set; }

        public Bullet(Entity source, int health = 1, int damage = 1, int howManyCanDamage = 1) :
            this(
                source,
                source.X,
                source.Y,
                source.VelX,
                source.VelY,
                health,
                damage,
                howManyCanDamage)
        {
        }

        public Bullet(
            Entity source,
            float x = 0,
            float y = 0,
            float velX = 0,
            float velY = 0,
            int health = 1,
            float direction = 0,
            int damage = 1,
            int howManyCanDamage = 1) : base(x, y, velX, velY, health, direction)
        {
            this.source = source;
            VelX = source.VelX;
            VelY = source.VelY;
            AlignDirectionByVelocity();

            Damage = damage;
            if (howManyCanDamage < 0)
                throw new ArgumentException(nameof(howManyCanDamage) + " must be non-negative!");
            Health = howManyCanDamage;
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
    }
}