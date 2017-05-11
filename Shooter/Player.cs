using System;

namespace Shooter
{
    public class Player : Entity
    {
        public float Speed;
        public float BulletSpeed;

        public Player(Game game,
            float bulletSpeed = 1f,
            int x = 0,
            int y = 0,
            float velX = 0,
            float velY = 0,
            int health = 1,
            float direction = 0,
            float speed = 0) :
            base(game, x, y, velX, velY, health, direction)
        {
            BulletSpeed = bulletSpeed;
            Speed = speed;
            verticalMovement = 0;
            horizontalMovement = 0;
            TargetType = TargetType.None;
        }

        private int verticalMovement;

        public int VerticalMovement
        {
            get => verticalMovement;
            set
            {
                var prevValue = verticalMovement;
                verticalMovement = value;
                if (prevValue != value)
                    UpdateVelocity();
            }
        }

        private int horizontalMovement;

        public int HorizontalMovement
        {
            get => horizontalMovement;
            set
            {
                var prevValue = horizontalMovement;
                horizontalMovement = value;
                if (prevValue != value)
                    UpdateVelocity();
            }
        }

        private void UpdateVelocity()
        {
            VelX = HorizontalMovement * Speed;
            VelY = VerticalMovement * Speed;
        }

        public void Fire()
        {
            Game.AddEntity(new Bullet(Game, this, X, Y, 0, -1).SetTargetType(TargetType.Enemy));
        }
    }
}