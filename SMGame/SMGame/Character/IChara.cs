using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SMGame.Device;

namespace SMGame.Character
{
    interface IChara
    {
        void Initialize();

        void Update(GameTime gameTime);

        void Draw(Renderer renderer);

        bool IsDead();
    }
}
