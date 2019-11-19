using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMGame.Device;
using SMGame.Def;
using SMGame.Util;

namespace SMGame.Character
{
    class Player
    {
        private Vector2 position;
        private Vector2 velocity;
        public float moveSpeed = 7.0f;
        public float Hp;
        public float AttackPower;
        public bool IsJumpFlag = false;
        private Motion motion;
        private int width;
        private int height;
        private int attackPosition = 73;
        private int attackArea = 55;
        private Boss boss;
        private bool HitFlag = false;

        /// <summary>
        /// ベクトルの向きを管理（true->右、false->左）
        /// </summary>
        public bool vecterFlag;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position"></param>
        /// <param name="gameDevice"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Player(Vector2 position, GameDevice gameDevice, int width, int height, Boss boss)
        {
            this.position = position;
            this.width = width;
            this.height = height;
            this.boss = boss;
            Hp = 100;
            AttackPower = 10;
            #region アニメーション関連
            //アニメーション設定
            motion = new Motion(
                new Range(0, 3),
                new CountDownTimer(0.3f));

            for (int i = 0; i < 4; i++)
            {
                motion.Add(i, new Rectangle(128 * i, 0, 128, 128));
            }
            #endregion
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            IsJumpFlag = false;
            vecterFlag = true;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("PlayerTatie", position);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            PlayerMove();
            PlayerJump();
            GroundHit();
            NormalAttack();

            #region Debug確認用
            Console.WriteLine("HitFlag = " + HitFlag);

            #endregion

            if (HitFlag == true)
            {
                velocity = Vector2.Zero;
            }

            position += velocity;

        }

        /// <summary>
        /// プレイヤー移動
        /// </summary>
        public void PlayerMove()
        {
            velocity.X = Input.GetLeftStickground(PlayerIndex.One).X * moveSpeed;
        }

        /// <summary>
        /// プレイヤージャンプ
        /// </summary>
        public void PlayerJump()
        {
            if ((IsJumpFlag == false) && (Input.IsButtonDown(PlayerIndex.One, Buttons.A)
                || Input.GetKeyTrigger(Keys.J)))
            {
                velocity.Y = -12.0f;
                IsJumpFlag = true;
                position.Y -= 10.0f;
            }
            else
            {
                //ジャンプ中だけ落下
                velocity.Y = velocity.Y + 0.4f;
                //落下速度制限
                velocity.Y = (velocity.Y > 16.0f) ? (16.0f) : (velocity.Y);
            }
        }

        /// <summary>
        /// （仮）床との当たり判定
        /// </summary>
        public void GroundHit()
        {
            if (position.Y > Screen.Height - 180)
            {
                velocity.Y = 0.0f;
                position.Y = Screen.Height - 180;
                IsJumpFlag = false;
            }
        }

        /// <summary>
        /// 通常攻撃
        /// </summary>
        public void NormalAttack()
        {
            if (Input.IsButtonDown(PlayerIndex.One, Buttons.RightShoulder))
            {
                #region 確認用
                //if (NormalCollision(boss))
                //{
                //    HitFlag = true;
                //    AttackHit(boss);
                //    boss.NormalCollision(this);
                //}
                #endregion

                if (AttackHit(boss))
                {
                    HitFlag = true;
                    boss.NormalCollision(this);
                    boss.ReceiveDamege(this);
                }
            
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
        /// 攻撃関連の当たり判定
        /// </summary>
        /// <returns></returns>
        public Rectangle GetAttackRectangle()
        {
            Rectangle attackRect = new Rectangle();

            attackRect.X = (int)position.X + attackPosition;
            attackRect.Y = (int)position.Y;
            attackRect.Width = attackArea;
            attackRect.Height = height;

            return attackRect;
        }

        /// <summary>
        /// 通常当たり判定
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool NormalCollision(Boss other)
        {
            return this.GetRectangle().Intersects(other.GetRectangle());
        }

        /// <summary>
        /// 攻撃時当たり判定
        /// </summary>
        public bool AttackHit(Boss other)
        {
            return this.GetAttackRectangle().Intersects(other.GetRectangle());
        }


    }
}
