using System;
using System.Deployment.Application;

namespace Shooter
{
    public class Entity : ICoordinatesProvider
    {
        public int Health { get; protected set; }
        public CollisionBox CollisionBox { get; protected set; }
        public TargetType TargetType { get; protected set; }
        public Entity DeathSource { get; private set; }

        public float X { get; protected set; }
        public float Y { get; protected set; }

        public float Direction;

        public float VelX { get; protected set; }
        public float VelY { get; protected set; }

        public bool IsDead { get; protected set; }


        public Entity(
            float x = 0,
            float y = 0,
            float velX = 0,
            float velY = 0,
            int health = 1,
            float direction = 0,
            TargetType targetType = TargetType.None,
            float collisionBoxWidth = -1,
            float collisionBoxHeight = -1
            )
        {
            X = x;
            Y = y;
            Direction = direction;
            VelX = velX;
            VelY = velY;
            Health = health;
            TargetType = targetType;
            IsDead = false;
            if(collisionBoxWidth > 0 && collisionBoxHeight > 0)
                CollisionBox = new CollisionBox(this,collisionBoxWidth,collisionBoxHeight);
        }

        public virtual void OnEntityTick()
        {
            if (!IsDead)
            {
                Move();
            }
        }

        public virtual void Move()
        {
            X += VelX;
            Y += VelY;
        }

        public void DamageEntity(Entity source, int damage)
        {
            if (!IsDead)
            {
                Health -= damage;
                if (Health <= 0)
                    SetDead(source);
            }
        }

        public void SetDead(Entity source)
        {
            IsDead = true;
            DeathSource = source;
        }

        public void AlignDirectionByVelocity()
        {
            Direction = (float) Math.Atan2(-VelY, VelX);
        }

        public virtual void OnCollideWithTarget(Entity targetEntity)
        {
        }

        public bool CollidesWith(Entity entity)
        {
            return CollisionBox != null && CollisionBox.CollidesWith(entity.CollisionBox);
        }

        public bool IsTarget(Entity entity)
        {
            if (entity == null)
                return false;
            switch (TargetType)
            {
                case TargetType.Enemy:
                    return entity is Enemy;
                case TargetType.EnemyBullets:
                    var s = entity as Bullet;
                    var source = s?.GetSource();
                    return source is Enemy;
                case TargetType.Player:
                    return entity is Player;
                default:
                    return false;
            }
        }

        public virtual String GetTextureFileName()
        {
            return "";
        }

        public override string ToString()
        {
            return $"{{{GetType().Name}: X: {X} Y: {Y} VelX: {VelX} VelY: {VelY}}}";
        }
    }
}