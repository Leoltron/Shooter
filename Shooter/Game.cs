using System;
using System.Collections.Generic;
using System.Linq;

namespace Shooter
{
    public class Game : ISizeProvider
    {
        public readonly Player Player;
        private readonly List<Entity> entities;
        public IEnumerable<Entity> GetEntities => entities;
        public float Width { get; }
        public float Height { get; }
        
        private readonly int playerShootingDelay;
        private int playerCurrentShootingDelay;
        private readonly float playerBulletSpeed;

        public event Action GameOver;

        public int Score { get; private set; }

        public Game(float width, float height)
        {
            Width = width;
            Height = height;
            
            entities = new List<Entity>();
            Player = new Player(this,(int) (width/2)+16,(int)height-64,speed: 5f);
            AddEntity(Player);
            playerShootingDelay = 20;
            playerCurrentShootingDelay = 0;
            playerBulletSpeed = 20;
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public void GameTick()
        {
            playerCurrentShootingDelay = Math.Max(playerCurrentShootingDelay - 1, 0);
            foreach (var entity in entities)
                entity.OnEntityTick();

            foreach (var entity1 in entities)
            foreach (var entity2 in entities)
                if (entity1.IsTarget(entity2) && entity1.CollidesWith(entity2))
                    entity1.OnCollideWithTarget(entity2);

            foreach (var entity in entities.Where(e => e.IsDead))
                OnEntityKilled(entity, entity.DeathSource);

            entities.RemoveAll(e => e.IsDead);
        }

        public void SetPlayerGoingLeft(bool isGoingLeft)
        {
            if (isGoingLeft)
                Player.HorizontalMovement = -1;
            else if (Player.HorizontalMovement < 0)
                Player.HorizontalMovement = 0;
        }

        public void SetPlayerGoingRight(bool isGoingRight)
        {
            if (isGoingRight)
                Player.HorizontalMovement = 1;
            else if (Player.HorizontalMovement > 0)
                Player.HorizontalMovement = 0;
        }

        public void SetPlayerGoingDown(bool isGoingDown)
        {
            if (isGoingDown)
                Player.VerticalMovement = 1;
            else if (Player.VerticalMovement > 0)
                Player.VerticalMovement = 0;
        }

        public void SetPlayerGoingUp(bool isGoingUp)
        {
            if (isGoingUp)
                Player.VerticalMovement = -1;
            else if (Player.VerticalMovement < 0)
                Player.VerticalMovement = 0;
        }

        public void Fire()
        {
            if (playerCurrentShootingDelay != 0) return;
            AddEntity(new Bullet(Player, this,Player.X, Player.Y, Player.VelX, -Player.VelY - playerBulletSpeed)
                .SetTargetType(TargetType.Enemy));
            playerCurrentShootingDelay = playerShootingDelay;
        }

        private void OnEntityKilled(Entity entity, Entity killer)
        {
            var killerWeapon = killer as IWeapon;
            if (killerWeapon != null)
                OnEntityKilled(entity, killerWeapon.GetSource());
            else
            {
                if (entity == null)
                    return;
                if (entity == Player)
                    GameOver?.Invoke();
                if (killer != null && killer == Player)
                    OnEnemyKilledByPlayer(entity);
            }
        }

        private void OnEnemyKilledByPlayer(Entity enemy)
        {
            Score++;
        }
    }
}