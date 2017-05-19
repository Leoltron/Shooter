using System;
using System.Collections.Generic;
using System.Linq;

namespace Shooter
{
    public class Game : ISizeProvider, IEntityAdder
    {
        public const float PlayerSpeed = 5;

        private EnemyGenerator enemyGenerator;

        public readonly Player Player;
        private readonly List<Entity> entities;
        private readonly Queue<Entity> addingQueue;
        public IEnumerable<Entity> GetEntities => entities;
        public float Width { get; }
        public float Height { get; }

        private const int PlayerShootingDelay = 20;
        private int playerCurrentShootingDelay;
        private const float PlayerBulletSpeed = 20;

        public event Action GameOver;

        public int Score { get; private set; }

        public Game(float width, float height, bool generatorEnabled = false)
        {
            Width = width;
            Height = height;

            entities = new List<Entity>();
            addingQueue = new Queue<Entity>();
            Player = new Player(this, (int) (width / 2) + 16, (int) height - 64, speed: PlayerSpeed, health: 10);
            AddEntity(Player);
            if (generatorEnabled)
                enemyGenerator = new EnemyGenerator(this, this);
            //AddEntity(new BagelEnemy(BagelType.Bouncing, this, this, (width / 2) + 16, 64,5,5, health: 10));
        }

        public void AddEntity(Entity entity)
        {
            addingQueue.Enqueue(entity);
        }

        public void GameTick()
        {
            while (addingQueue.Count > 0)
                entities.Add(addingQueue.Dequeue());
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
            if (entities.Count(entity => entity is Enemy) == 0)
                enemyGenerator?.GenerateNewEnemies();
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
            AddEntity(new Bullet(Player, this, TargetType.Enemy,
                Player.X,
                Player.Y,
                Player.VelX,
                -Player.VelY - PlayerBulletSpeed));
            var l = Player.GunsAmountLevel;
            if (l >= 3)
            {
                AddEntity(new Bullet(Player, this, TargetType.Enemy,
                    Player.X + 50,
                    Player.Y,
                    Player.VelX,
                    -Player.VelY - PlayerBulletSpeed));
                AddEntity(new Bullet(Player, this, TargetType.Enemy,
                    Player.X - 50,
                    Player.Y,
                    Player.VelX,
                    -Player.VelY - PlayerBulletSpeed));
            }
            if (l >= 2)
            {
                AddEntity(new Bullet(Player, this, TargetType.Enemy,
                    Player.X - 30,
                    Player.Y,
                    Player.VelX,
                    -Player.VelY - PlayerBulletSpeed));
                AddEntity(new Bullet(Player, this, TargetType.Enemy,
                    Player.X + 30,
                    Player.Y,
                    Player.VelX,
                    -Player.VelY - PlayerBulletSpeed));
            }
            if (l >= 1)
            {
                AddEntity(new Bullet(Player, this, TargetType.Enemy,
                    Player.X + 10,
                    Player.Y,
                    Player.VelX,
                    -Player.VelY - PlayerBulletSpeed));
                AddEntity(new Bullet(Player, this, TargetType.Enemy,
                    Player.X - 10,
                    Player.Y,
                    Player.VelX,
                    -Player.VelY - PlayerBulletSpeed));
            }
            playerCurrentShootingDelay = PlayerShootingDelay;
        }

        private void OnEntityKilled(Entity entity, Entity killer)
        {
            var killerWeapon = killer as Bullet;
            if (killerWeapon != null)
                OnEntityKilled(entity, killerWeapon.Source);
            else
            {
                if (entity == Player)
                    GameOver?.Invoke();
                if (killer != null && killer == Player)
                    OnEnemyKilledByPlayer(entity);
            }
        }

        private void OnEnemyKilledByPlayer(Entity enemy)
        {
            Score += GetPointsAddingByKilling(enemy);
            var upgradesAquired = Player.BoostersLevel + Player.GunsAmountLevel;
            if (upgradesAquired != Player.MaxBoostersLevel + Player.MaxGunsAmountLevel)
            {
                if ((upgradesAquired+1) * 25 < Score)
                {
                    if (upgradesAquired % 2 == 0)
                        Player.UpgradeGunsAmount();
                    else
                        Player.UpgradeBoosters();
                }
            }
        }

        private int GetPointsAddingByKilling(Entity enemy)
        {
            var bagelEnemy = enemy as BagelEnemy;
            if (bagelEnemy != null)
            {
                switch (bagelEnemy.BagelType)
                {
                    case BagelType.Basic:
                    case BagelType.Clone:
                        return 1;
                    case BagelType.Bouncing:
                    case BagelType.Shooting:
                        return 5;
                    case BagelType.Healing:
                        return 0;
                    case BagelType.InvincibleClone:
                        return 25;
                }
            }
            return 0;
        }
    }
}