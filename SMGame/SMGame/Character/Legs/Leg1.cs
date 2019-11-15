using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SMGame.Device;
using SMGame.Def;

namespace SMGame.Character.Legs
{
    class Leg1 : LegsManager
    {
        /// <summary>
        /// フィールド
        /// </summary>
        private Vector2 axis, scale;
        private float rotate;
        private bool isAttack;

        public Leg1()
            : base("LegU", Vector2.Zero)
        {
            scale = new Vector2(1.0f);
            axis = new Vector2(20, 115);
            position = new Vector2(Screen.Width / 2, Screen.Height / 2);
        }
        public override void Initialize()
        {
            isAttack = false;
        }

        public override void Update(GameTime gameTime)
        {
            Attack();
        }
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture("LegU", position + axis, rotate, axis, scale);
            renderer.DrawTexture("LegU", position);
        }

        public void Attack()
        {
            if (isAttack == false)
            {
                rotate += 0.1f;
            }
            if (isAttack == true)
            {
                rotate -= 0.02f;
            }
            if (rotate <= 0)
            {
                isAttack = false;
            }
            if (rotate >= 1)
            {
                isAttack = true;
            }
        }
    }
}
