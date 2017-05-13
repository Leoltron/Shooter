using System;
using System.Deployment.Application;

namespace Shooter
{
    public class Entity
    {
        public int Health { get; protected set; }
        private CollisionBox collisionBox = null;
        public TargetType TargetType { get; protected set; }
        public Entity DeathSource { get; private set; }

        public float X;
        public float Y;

        public float Direction;

        public float VelX;
        public float VelY;

        public bool IsDead { get; protected set; }


        public Entity(float x = 0,
            float y = 0,
            float velX = 0,
            float velY = 0,
            int health = 1,
            float direction = 0)
        {
            X = x;
            Y = y;
            Direction = direction;
            VelX = velX;
            VelY = velY;
            Health = health;
            IsDead = false;
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
            Direction = (float) Math.Atan2(VelY, VelX);
        }

        public virtual void OnCollideWithTarget(Entity targetEntity)
        {
        }

        public bool CollidesWith(Entity entity)
        {
            if (collisionBox == null || entity.collisionBox == null)
                return false;

            var halfW = collisionBox.Width / 2;
            var thisLeft = X - halfW;
            var thisRight = X + halfW;

            var halfH = collisionBox.Height / 2;
            var thisTop = Y + halfH;
            var thisBottom = Y - halfH;

            halfW = entity.collisionBox.Width / 2;
            var otherLeft = entity.X - halfW;
            var otherRight = entity.X + halfW;

            halfH = entity.collisionBox.Height / 2;
            var otherTop = entity.Y + halfH;
            var otherBottom = entity.Y - halfH;

            return thisLeft <= otherRight &&
                   thisRight >= otherLeft &&
                   thisTop <= otherBottom &&
                   thisBottom >= otherTop;
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
    }
}