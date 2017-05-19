using System;
using System.Drawing;
using Shooter.Gui;

namespace Shooter
{
    public class Player : Entity
    {
        private readonly float speed;
        private readonly ISizeProvider sizeProvider;
        private float SpeedMultiplier = 1;

        public int BoostersLevel { get; private set; }
        public const int MaxBoostersLevel = 3;
        public int GunsAmountLevel { get; private set; }
        public const int MaxGunsAmountLevel = 1;

        public int InvincibilityTime { get; private set; }
        public const int MaxInvincibilityTime = 80;

        public Player(
            ISizeProvider sizeProvider,
            int x = 0,
            int y = 0,
            float velX = 0,
            float velY = 0,
            int health = 1,
            float direction = 0,
            float speed = 1) :
            base(x, y, velX, velY, health, direction)
        {
            this.sizeProvider = sizeProvider;
            this.speed = speed;
            verticalMovement = 0;
            horizontalMovement = 0;
            TargetType = TargetType.None;
            CollisionBox = new CollisionBox(this, 64, 64);
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
            VelX = HorizontalMovement * SpeedMultiplier * speed;
            VelY = VerticalMovement * SpeedMultiplier * speed;
        }

        public override void OnEntityTick()
        {
            base.OnEntityTick();
            if (InvincibilityTime > 0)
                InvincibilityTime--;
        }

        public override void DamageEntity(Entity source, int damage)
        {
            if (InvincibilityTime > 0) return;
            base.DamageEntity(source, damage);
            if(damage > 0)
                InvincibilityTime = MaxInvincibilityTime;
        }

        public override void Move()
        {
            var maxLeftMovement = -CollisionBox.Left;
            var maxRightMovement = sizeProvider.Width - CollisionBox.Right;
            var maxUpMovement = -CollisionBox.Top;
            var maxDownMovement = sizeProvider.Height - CollisionBox.Bottom;
            X += Math.Min(Math.Max(VelX, maxLeftMovement), maxRightMovement);
            Y += Math.Min(Math.Max(VelY, maxUpMovement), maxDownMovement);
        }

        public void UpgradeBoosters()
        {
            BoostersLevel = Math.Min(BoostersLevel + 1, MaxBoostersLevel);
            SpeedMultiplier = BoostersLevel*0.25f + 1;
        }

        public void UpgradeGunsAmount()
        {
            GunsAmountLevel = Math.Min(GunsAmountLevel + 1, MaxGunsAmountLevel);
        }

        public override Action<Graphics, Entity, bool> GetDrawingAction()
        {
            return PlayerDrawer.DrawPlayer;
        }
    }
}