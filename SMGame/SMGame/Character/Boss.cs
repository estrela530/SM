using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SMGame.Device;

namespace SMGame.Character
{
    class Boss : IChara
    {
        private int Hp;
        private int AttackPower;
        private Vector2 velocity = new Vector2(0, 1);
        private float MoveSpeed = 0.5f;
        private float seconds;
        private Vector2 position;
        private int width;
        private int height;

        public Boss(Vector2 position, GameDevice gameDevice, int Width, int Height)
        {
            this.position = position;
            this.width = Width;
            this.height = Height;
            Hp = 100;
            AttackPower = 10;
        }


        public void Initialize()
        {
            Hp = 100;
            AttackPower = 10;
            seconds = 0;

        }
        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("kumo", position);
        }


        public void Update(GameTime gameTime)
        {
            seconds += 10/* / 60*/;

            IdoleMove();
        }

        public void IdoleMove()
        {
            if (seconds >= 0 && seconds < 1000)
            {
                position.Y += velocity.Y * MoveSpeed;
            }
            else if (seconds >= 1000 && seconds < 2000)
            {
                position.Y -= velocity.Y * MoveSpeed;
            }
            else if (seconds >= 2000)
            {
                seconds = 0;

            }
        }

        /// <summary>
        /// 幅関連の当たり判定
        /// </summary>
        /// <returns></returns>
        public Rectangle GetRectangle()
        {
            Rectangle rect = new Rectangle();

            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
            rect.Width = width;
            rect.Height = height;

            return rect;
        }

        /// <summary>
        /// 通常当たり判定
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool NormalCollision(Player other)
        {
            return this.GetRectangle().Intersects(other.GetRectangle());
        }

    }
}
