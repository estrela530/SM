using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SMGame.Device;
using SMGame.Character.Legs;
using SMGame.Def;

namespace SMGame.Character
{
    class Boss : IChara
    {
        public float Hp;
        private float MaxHp;
        private float AttackPower;
        private Vector2 velocity = new Vector2(0, 1);
        private float MoveSpeed = 0.5f;
        private float seconds;
        private Vector2 position;
        private int width;
        private int height;
        public Player player;
        public bool IsDeadFlag;

        List<LegsManager> frontLegs;
        List<LegsManager> backLegs;


        //false=>ゲームプレイ true=>タイトル
        bool titleFlag;

        public Boss(Vector2 position, GameDevice gameDevice, int Width, int Height)
        {
            this.position = position;
            this.width = Width;
            this.height = Height;
            Hp = 2000;
            MaxHp = Hp;
            AttackPower = 10;
            IsDeadFlag = false;
            frontLegs = new List<LegsManager>();
            frontLegs.Add(new Leg1(200));
            frontLegs.Add(new Leg2(200));

            backLegs = new List<LegsManager>();
            backLegs.Add(new Leg3(400));
            backLegs.Add(new Leg4(400));
        }


        public Boss(Vector2 position, GameDevice gameDevice, int Width, int Height, bool title)
        {
            this.position = position;
            this.width = Width;
            this.height = Height;
            Hp = 1000000000;
            MaxHp = Hp;
            AttackPower = 10;
            IsDeadFlag = false;
            titleFlag = title;
        }


        public void Initialize()
        {
            Hp = 2000;
            MaxHp = Hp;
            AttackPower = 10;
            seconds = 0;
            IsDeadFlag = false;

            if (titleFlag) return;
            frontLegs = new List<LegsManager>();
            frontLegs.Add(new Leg1(200));
            frontLegs.Add(new Leg2(200));

            backLegs = new List<LegsManager>();
            backLegs.Add(new Leg3(400));
            backLegs.Add(new Leg4(400));
        }
        public void Draw(Renderer renderer)
        {
            if (titleFlag)
            {
                renderer.DrawTexture("Log", position);
                return;
            }
            if (IsDeadFlag != true)
            {
                renderer.DrawTexture("body-back", Vector2.Zero);
                foreach (var leg in backLegs)
                {
                    leg.Draw(renderer);
                }
                renderer.DrawTexture("body-mid", Vector2.Zero);
                foreach (var leg in frontLegs)
                {
                    leg.Draw(renderer);
                }
                renderer.DrawTexture("body-front", Vector2.Zero);
                renderer.DrawTexture("Head", position);

                renderer.DrawTexture("HpBack", Vector2.Zero);
                renderer.DrawTexture("HpBackBar", Vector2.Zero, new Rectangle(0, 0, (int)(Screen.Width * (Hp / MaxHp)), 88));
            }
        }


        public void Update(GameTime gameTime)
        {
            if (titleFlag) return;
            seconds += 10/* / 60*/;


            foreach (var leg in frontLegs)
            {
                leg.Update(gameTime);
            }
            foreach (var leg in backLegs)
            {
                leg.Update(gameTime);
            }

            #region Debug確認用
            Console.WriteLine("Boss.Hp = " + Hp);
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
            foreach (var fleg in frontLegs)
            {
                if (fleg.IsBrake())
                {
                    Hp -= player.AttackPower / 10;
                }
            }
            foreach (var bleg in backLegs)
            {
                if (bleg.IsBrake())
                {
                    Hp -= player.AttackPower / 10;
                }
            }
        }

        /// <summary>
        /// スキル①攻撃に対するダメージメソッド
        /// </summary>
        /// <param name="player"></param>
        public void Skill1ReceiveDamage(Player player)
        {
            Hp -= player.skill1Power;
            foreach (var fleg in frontLegs)
            {
                if (fleg.IsBrake())
                {
                    Hp -= player.skill1Power / 10;
                }
            }
            foreach (var bleg in backLegs)
            {
                if (bleg.IsBrake())
                {
                    Hp -= player.skill1Power / 10;
                }
            }
        }

        /// <summary>
        /// スキル②攻撃に対するダメージメソッド
        /// </summary>
        /// <param name="player"></param>
        public void Skill2ReceiveDamage(Player player)
        {
            Hp -= player.skill2Power;
            foreach (var fleg in frontLegs)
            {
                if (fleg.IsBrake())
                {
                    Hp -= player.skill2Power / 10;
                }
            }
            foreach (var bleg in backLegs)
            {
                if (bleg.IsBrake())
                {
                    Hp -= player.skill2Power / 10;
                }
            }
        }


        public bool IsDead()
        {
            return IsDeadFlag;
        }


        public void GetPlayer(Player player)
        {
            this.player = player;
            foreach (var leg in frontLegs)
            {
                leg.GetPlayer(player);
            }
            foreach (var leg in backLegs)
            {
                leg.GetPlayer(player);
            }
        }
    }
}
