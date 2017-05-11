using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter
{
    public class Game
    {
        public Player Player;
        private List<Entity> entities;
        private bool IsPaused;
        private float width;
        private float height;

        public event Action GameOver;

        public int Score { get; private set; }

        public Game()
        {
            entities = new List<Entity>();
            //Player = new Player();
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public void OnGameTick()
        {
            if (IsPaused)
                return;
            Player.OnEntityTick();
            foreach (var entity in entities)
                entity.OnEntityTick();

            foreach (var VARIABLE in entities.Where(e => e.TargetType == TargetType.Enemy))
            {
                
            }

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

        public void Pause()
        {
            IsPaused = true;
        }

        public void Unpause()
        {
            IsPaused = false;
        }

        public void OnEntityKilled(Entity entity, Entity killer)
        {
            var killerWeapon = killer as IWeapon;
            if (killerWeapon != null)
                OnEntityKilled(entity, killerWeapon.GetSource());
            else
            {
                if (entity == null)
                    return;
                if (entity == Player)
                {
                    Pause();
                    GameOver?.Invoke();
                }
                if (killer != null && killer == Player)
                    OnEnemyKilledByPlayer(entity);
            }
        }

        public void OnEnemyKilledByPlayer(Entity enemy)
        {
            Score++;
        }
    }
}