namespace Shooter
{
    public class Enemy : Entity
    {
        public Enemy(
            float x = 0,
            float y = 0,
            float velX = 0,
            float velY = 0,
            int health = 1,
            float direction = 0,
            float collisionBoxWidth = -1,
            float collisionBoxHeight = -1) :
            base(x, y, velX, velY, health, direction, TargetType.Player, collisionBoxWidth, collisionBoxHeight)
        {
        }

        public override void OnCollideWithTarget(Entity targetEntity)
        {
            targetEntity.DamageEntity(this, 1);
        }
    }
}