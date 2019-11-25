using Microsoft.Xna.Framework;
using SMGame.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMGame.Character
{
    abstract class LegsManager
    {
        protected Vector2 position;
        protected Vector2 rootPosition;
        protected string rootname;
        protected string tipname;
        protected float MoveSpeed;
        protected Player player;

        protected int Hp;
        protected bool isBrake;

        public LegsManager(string rootName, string tipName)
        {
            this.rootname = rootName;
            this.tipname = tipName;
            isBrake = false;
        }

        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public virtual void GetPlayer(Player player)
        {
            this.player = player;
        }

        public virtual void Draw(Renderer renderer)
        {
            renderer.DrawTexture(rootname, position);
        }

        public virtual void Damage(int damage)
        {
            Hp -= damage;
            if (Hp <= 0)
            {
                isBrake = true;
            }
        }

        public bool IsBrake()
        {
            return isBrake;
        }
    }
}
