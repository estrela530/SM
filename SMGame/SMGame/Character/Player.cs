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
        public int HP;
        public float AttackPower;
        public bool IsJumpFlag = false;
        private Motion motion;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position"></param>
        /// <param name="gameDevice"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Player(Vector2 position, GameDevice gameDevice, int width, int height)
        {
            this.position = position;

            #region アニメーション関連
            //アニメーション設定
            motion = new Motion(
                new Range(0, 3),
                new CountDownTimer(0.3f));

            for (int i = 0; i < 4; i++)
            {
                motion.Add(i, new Rectangle(32 * i, 0, 32, 32));
            }
            #endregion
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            IsJumpFlag = false;
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

            position = position + velocity;
        }

        /// <summary>
        /// プレイヤー移動
        /// </summary>
        public void PlayerMove()
        {
            velocity.X = Input.GetLeftStickground(PlayerIndex.One).X * moveSpeed;

            //position = position + velocity;
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
            if (Input.IsButtonDown(PlayerIndex.One,Buttons.RightShoulder))
            {
                
            }
        }

    }
}
