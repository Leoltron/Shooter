namespace Shooter
{
    public class Enemy : Entity
    {
        public Enemy(Game game, 
            float x = 0,
            float y = 0, 
            float velX = 0,
            float velY = 0, 
            int health = 1,
            float direction = 0) :
            base(x, y, velX, velY, health, direction)
        {
            TargetType = TargetType.Player;
        }

        public override void OnCollideWithTarget(Entity targetEntity)
        {
            targetEntity.DamageEntity(this,1);
        }
    }
}