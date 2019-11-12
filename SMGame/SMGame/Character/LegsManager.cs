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
        protected string name;
        protected float MoveSpeed;

        public LegsManager(string name, Vector2 position)
        {
            this.name = name;
            this.position = position;
        }

        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);

        public virtual void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position);
        }

    }
}
