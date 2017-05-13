using System;
using System.Collections.Generic;
using System.Linq;

namespace Shooter
{
    public class Game
    {
        private readonly Player player;
        private readonly List<Entity> entities;
        private float width;
        private float height;

        private int shootingDelay;
        private int currentShootingDelay;

        public event Action GameOver;

        public int Score { get; private set; }

        public Game(float width, float height)
        {
            this.width = width;
            this.height = height;

            entities = new List<Entity>();
            player = new Player();
            AddEntity(player);
            shootingDelay = 20;
            currentShootingDelay = 0;
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public void GameTick()
        {
            currentShootingDelay = Math.Max(currentShootingDelay - 1, 0);
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
                player.HorizontalMovement = -1;
            else if (player.HorizontalMovement < 0)
                player.HorizontalMovement = 0;
        }

        public void SetPlayerGoingRight(bool isGoingRight)
        {
            if (isGoingRight)
                player.HorizontalMovement = 1;
            else if (player.HorizontalMovement > 0)
                player.HorizontalMovement = 0;
        }

        public void SetPlayerGoingDown(bool isGoingDown)
        {
            if (isGoingDown)
                player.VerticalMovement = 1;
            else if (player.VerticalMovement > 0)
                player.VerticalMovement = 0;
        }

        public void SetPlayerGoingUp(bool isGoingUp)
        {
            if (isGoingUp)
                player.VerticalMovement = -1;
            else if (player.VerticalMovement < 0)
                player.VerticalMovement = 0;
        }

        public void Fire()
        {
            if (currentShootingDelay != 0) return;
            AddEntity(new Bullet(player, player.X, player.Y, 0, -1).SetTargetType(TargetType.Enemy));
            currentShootingDelay = shootingDelay;
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
                if (entity == player)
                    GameOver?.Invoke();
                if (killer != null && killer == player)
                    OnEnemyKilledByPlayer(entity);
            }
        }

        private void OnEnemyKilledByPlayer(Entity enemy)
        {
            Score++;
        }
    }
}