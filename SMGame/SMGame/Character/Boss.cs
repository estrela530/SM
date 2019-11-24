using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SMGame.Device;
using SMGame.Character.Legs;

namespace SMGame.Character
{
    class Boss : IChara
    {
        public float Hp;
        private float AttackPower;
        private Vector2 velocity = new Vector2(0, 1);
        private float MoveSpeed = 0.5f;
        private float seconds;
        private Vector2 position;
        private int width;
        private int height;
        public Player player;
        public bool IsDeadFlag;

        private Leg3 rightFrontLeg;

        public Boss(Vector2 position, GameDevice gameDevice, int Width, int Height)
        {
            this.position = position;
            this.width = Width;
            this.height = Height;
            Hp = 100;
            AttackPower = 10;
            IsDeadFlag = false;
            rightFrontLeg = new Leg3();
        }


        public void Initialize()
        {
            Hp = 100;
            AttackPower = 10;
            seconds = 0;
            IsDeadFlag = false;
            rightFrontLeg = new Leg3();
        }
        public void Draw(Renderer renderer)
        {
            if (IsDeadFlag != true)
            {
                renderer.DrawTexture("body-back", Vector2.Zero);
                renderer.DrawTexture("body-mid", Vector2.Zero);
                rightFrontLeg.Draw(renderer);
                renderer.DrawTexture("body-front", Vector2.Zero);
            }
        }


        public void Update(GameTime gameTime)
        {
            rightFrontLeg.Update(gameTime);
            seconds += 10/* / 60*/;

            IdleMove();

            #region Debug確認用
            //Console.WriteLine("Boss.Hp = " + Hp);
            #endregion

            if (Hp <= 0)
            {
                IsDeadFlag = true;
            }

        }

        public void IdleMove()
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

        /// <summary>
        /// 通常攻撃に対するダメージメソッド
        /// </summary>
        /// <param name="player"></param>
        public void ReceiveDamage(Player player)
        {
            Hp -= player.AttackPower;
        }

        /// <summary>
        /// スキル①攻撃に対するダメージメソッド
        /// </summary>
        /// <param name="player"></param>
        public void Skill1ReceiveDamage(Player player)
        {
            Hp -= player.skill1Power;
        }

        /// <summary>
        /// スキル②攻撃に対するダメージメソッド
        /// </summary>
        /// <param name="player"></param>
        public void Skill2ReceiveDamage(Player player)
        {
            Hp -= player.skill2Power;
        }


        public bool IsDead()
        {
            return IsDeadFlag;
        }
      

        public void GetPlayer(Player player)
        {
            this.player = player;
            rightFrontLeg.GetPlayer(player);
        }
    }
}
