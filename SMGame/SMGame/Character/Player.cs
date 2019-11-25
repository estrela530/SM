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
using SMGame.Character.Legs;
using SMGame.Scene;

namespace SMGame.Character
{
    class Player
    {
        public Vector2 position;
        private Vector2 velocity;
        public float moveSpeed = 7.0f;
        public float Hp;
        public float AttackPower;
        public bool IsJumpFlag = false;
        private Motion motion;
        private Motion motionRun;
        private Motion skillmotion;
        private Motion skillGmotion;
        private int width;
        private int height;
        private int attackPosition = 73;
        private int attackArea = 55;
        private Boss boss;
        private bool AttackHitFlag = false;
        private bool ComboFlag = false;
        public int comboCount = 0;
        private bool AvoidFlag = false;
        public float avoidSpeed = 340.0f;
        public float avoidDrawPosition;
        public int avoidCoolTime;
        private Vector2 currentPosition;
        private Vector2 previousPosition;
        private Rectangle rectangle;
        //public float alpha = 1 / 25f;
        public float alpha = 1;
        private Sound sound;
        private int seconds = 0;
        public int skill1Power = 20;
        public int skill2Power = 30;
        public int whichSkillCheck = 0;
        public bool IsSkillHitFlag = false;
        public Vector2 Skill1MovePosition;
        public int skill1Combo = 0;
        public int skillCoolTime;
        private Vector2 directionD;
        private Vector2 stickDirection;
        private double stickAngle;
        private double angle = 0;
        private Leg1 leg1;
        private Leg2 leg2;
        private Leg3 leg3;
        private Leg4 leg4;
        private SceneManager sceneManager;
        private SceneName sceneNameP;
        private SceneName sceneNameT;

        /// <summary>
        /// 攻撃したときに次の入力がコンボに繋がるのか？カウンター
        /// </summary>
        private int weakAttackCounter;

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
            weakAttackCounter = 0;
            rectangle = new Rectangle(0, 0, 30, 128);
            gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();


            #region アニメーション関連
            //idleアニメーション設定
            motion = new Motion(
                new Range(0, 1),
                new CountDownTimer(0.3f));

            for (int i = 0; i < 2; i++)
            {
                motion.Add(i, new Rectangle(128 * i, 0, 128, 128));
            }

            //runアニメーション設定
            motionRun = new Motion(
                new Range(0, 3),
                new CountDownTimer(0.3f));

            for (int i = 0; i < 4; i++)
            {
                motionRun.Add(i, new Rectangle(128 * i, 0, 128, 128));
            }

            //スキルエフェクト
            skillmotion = new Motion(
                new Range(0,9),
                new CountDownTimer(0.075f));
            for (int i = 0; i <10; i++)
            {
                skillmotion.Add(i, new Rectangle(0, 240*i, 320, 240));
            }

            //スキルエフェクト五右衛門
            skillGmotion = new Motion(
                new Range(0, 9),
                new CountDownTimer(0.075f));
            for (int i = 0; i < 9; i++)
            {
                skillGmotion.Add(i, new Rectangle(0, 240 * i, 320, 240));
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
            weakAttackCounter = 0;
            ComboFlag = false;
            comboCount = 0;
            AvoidFlag = false;
            avoidCoolTime = 0;
            avoidDrawPosition = avoidSpeed + 200.0f;
            currentPosition = Vector2.Zero;
            previousPosition = Vector2.Zero;
            seconds = 0;
            whichSkillCheck = 0;
            IsSkillHitFlag = false;
            Skill1MovePosition = Vector2.Zero;
            skill1Combo = 0;
            skillCoolTime = 0;
            angle = 0;
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
            SkillAttack();
            NormalAttack();
            PlayerAvoid();
            motion.Update(gameTime);
            motionRun.Update(gameTime);
            skillmotion.Update(gameTime);
            skillGmotion.Update(gameTime);
            #region Debug確認用
            //Console.WriteLine("HitFlag = " + AttackHitFlag);
            //Console.WriteLine("weakAttackCounter" + weakAttackCounter);
            //Console.WriteLine("ComboFlag = " + ComboFlag);
            //Console.WriteLine("ComboCount= " + comboCount);
            //Console.WriteLine("alpha = " + alpha);
            Console.WriteLine("skill1combo = " + skill1Combo);
            #endregion

            if (AttackHitFlag && comboCount != 0)
            {
                velocity = Vector2.Zero;
            }

            position += velocity;

            if (avoidCoolTime > 20)
            {
                alpha = 1;
                AvoidFlag = false;
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("playerHp", new Vector2(0, 88));
            renderer.DrawTexture("playerHpBar", new Vector2(0, 88));

            

            if (IsSkillHitFlag == true)
            {
                renderer.DrawTexture("black", Vector2.Zero);
                renderer.DrawTexture("pipo-btleffect133", position, motion.DrawingRange());

                
            }
            else if (IsSkillHitFlag == false)
            {

            }

            if (ComboFlag)
            {
                if (comboCount % 2 == 1)
                {
                    renderer.DrawTexture("attack1-anim", position, motion.DrawingRange(), 0);
                    renderer.DrawTexture("VerticalBrade", position);
                }
                else
                {
                    renderer.DrawTexture("attack2-anim", position, motion.DrawingRange(), 0);
                    renderer.DrawTexture("HorizontalBrade", position);
                }
                return;
            }

            //向きらへん
            if (Input.GetLeftStickground(PlayerIndex.One).X == 0 && vecterFlag == true && IsJumpFlag == false
                /*&& !AttackHitFlag && AvoidFlag && ComboFlag && IsJumpFlag && IsSkillHitFlag*/)
            {
                renderer.DrawTexture("idle-anim", position, motion.DrawingRange(), 0);

                sound.StopSEInstance("run");
            }
            else if (Input.GetLeftStickground(PlayerIndex.One).X == 0 && vecterFlag == false && IsJumpFlag == false
                /*&& !AttackHitFlag && AvoidFlag && ComboFlag && IsJumpFlag && IsSkillHitFlag*/)
            {
                renderer.DrawTexture("idle-anim", position, motion.DrawingRange(), 1);

                sound.StopSEInstance("run");
            }


            if (Input.GetLeftStickground(PlayerIndex.One).X != 0 && vecterFlag == true)
            {
                renderer.DrawTexture("run-anim", position, motionRun.DrawingRange(), 0);

                sound.PlaySEInstance("run");
            }
            else if (Input.GetLeftStickground(PlayerIndex.One).X != 0 && vecterFlag == false)
            {
                renderer.DrawTexture("run-anim", position, motionRun.DrawingRange(), 1);

                sound.PlaySEInstance("run");
            }

            if (IsJumpFlag && vecterFlag)
            {
                renderer.DrawTexture("run-anim", position, motionRun.DrawingRange(), 0);

                sound.StopSEInstance("run");
            }
            else if (IsJumpFlag && vecterFlag == false)
            {
                renderer.DrawTexture("run-anim", position, motionRun.DrawingRange(), 1);

                sound.StopSEInstance("run");
            }

            if (AvoidFlag && avoidCoolTime <= 20 && vecterFlag)
            {
                float a = 355;
                int x = 0;
                Rectangle rectR;
                for (int i = 0; i < 20; i++)
                {
                    alpha -= i / 15;
                    rectR = new Rectangle(0, 0, x, 128);
                    renderer.DrawTexture("Avoid2", currentPosition + new Vector2(50 + a, 0), new Vector2(a, 0), rectR, alpha, 0);
                    x += 18 * i;
                }
            }
            else if (AvoidFlag && avoidCoolTime <= 20 && vecterFlag == false)
            {
                float a = 355;
                int x = 355;
                Rectangle rectL;
                for (int i = 0; i < 20; i++)
                {
                    alpha -= i / 2;
                    rectL = new Rectangle(0, 0, x, 128);
                    renderer.DrawTexture("Avoid2", currentPosition + new Vector2(50, 0), new Vector2(a, 0), rectL, alpha, 1);
                    x -= 18 * i;
                }
            }
            //else if (AvoidFlag && avoidCoolTime <= 20 && vecterFlag == false)
            //{
            //    float a = 180;
            //    int x = 0;
            //    Rectangle rectL;
            //    for (int i = 0; i < 20; i++)
            //    {
            //        alpha -= i / 15;
            //        rectL = new Rectangle(0, 0, x, 128);
            //        renderer.DrawTexture("Avoid2", currentPosition + new Vector2(50, 0), new Vector2(a, 0), rectL, alpha , 1);
            //        x += 18 * i;
            //    }
            //}
            renderer.DrawTexture("pipo-btleffect145", Vector2.Zero, skillmotion.DrawingRange(), 0);
        }

        /// <summary>
        /// プレイヤー移動
        /// </summary>
        public void PlayerMove()
        {
            velocity.X = Input.GetLeftStickground(PlayerIndex.One).X * moveSpeed;

            // 角度計算
            stickDirection = Input.GetLeftSticksky(PlayerIndex.One);
            stickAngle = Math.Atan2(stickDirection.Y, stickDirection.X);
            if (stickDirection != Vector2.Zero)
            {
                directionD = stickDirection;
                directionD.Normalize();
                angle = stickAngle;
                //Console.WriteLine("angle = " + angle);
            }

            if (-1.5f <= angle && angle <= 1.5f)
            {
                vecterFlag = true;
            }
            else
            {
                vecterFlag = false;
            }

        }

        /// <summary>
        /// プレイヤー回避
        /// </summary>
        public void PlayerAvoid()
        {
            avoidCoolTime++;

            if (Input.IsButtonDown(PlayerIndex.One, Buttons.Y) && AvoidFlag == false && vecterFlag)
            {
                //velocity.X = Input.GetLeftStickground(PlayerIndex.One).X * moveSpeed * avoidSpeed;
                avoidCoolTime = 0;
                AvoidFlag = true;
                previousPosition = currentPosition;
                currentPosition = position;
                position = new Vector2(position.X + avoidSpeed, position.Y);
                sound.PlaySE("avoid");
            }
            if (Input.IsButtonDown(PlayerIndex.One, Buttons.Y) && AvoidFlag == false && vecterFlag == false)
            {
                //velocity.X = Input.GetLeftStickground(PlayerIndex.One).X * moveSpeed * avoidSpeed;
                avoidCoolTime = 0;
                AvoidFlag = true;
                previousPosition = currentPosition;
                currentPosition = position;
                position = new Vector2(position.X - avoidSpeed, position.Y);
                sound.PlaySE("avoid");
            }
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
            if (avoidCoolTime <= 6)
            {
                velocity.Y = 0.0f;
            }
        }

        /// <summary>
        /// （仮）床との当たり判定
        /// </summary>
        public void GroundHit()
        {
            if (position.Y > Screen.Height - 128)
            {
                velocity.Y = 0.0f;
                position.Y = Screen.Height - 128;
                IsJumpFlag = false;
            }
        }

        /// <summary>
        /// 通常攻撃
        /// </summary>
        public void NormalAttack()
        {
            //Console.WriteLine("comboCnt" + comboCount);
            if (comboCount >= 0 && comboCount <= 3 && Input.IsButtonDown(PlayerIndex.One, Buttons.RightShoulder))
            {
                ComboFlag = true;
                comboCount++;
            }

            if (ComboFlag)
            {
                switch (comboCount)
                {
                    //0.5秒以内に
                    case 1:
                        if (AttackHit(boss) && weakAttackCounter == 1)
                        {
                            Console.WriteLine("N1入った！");
                            Console.WriteLine("comboCnt" + comboCount);
                            AttackHitFlag = true;
                            boss.NormalCollision(this);
                            boss.ReceiveDamage(this);

                            if ((sceneNameT != SceneName.GameTitle))
                            {
                                boss.ReceiveDamage(this);
                            }

                        }
                        else if (!AttackHit(boss))
                        {
                            ComboFlag = false;
                            comboCount = 0;
                        }
                        if (weakAttackCounter > 0 && weakAttackCounter <= 30 && Input.IsButtonDown(PlayerIndex.One, Buttons.RightShoulder))
                        {
                            Console.WriteLine("わーーーーーーーーーーー");
                            weakAttackCounter = 0;
                        }
                        if (weakAttackCounter > 30)
                        {
                            weakAttackCounter = 0;
                            comboCount = 0;
                            ComboFlag = false;
                            AttackHitFlag = false;
                        }
                        break;

                    case 2:
                        if (AttackHit(boss) && weakAttackCounter == 1)
                        {
                            Console.WriteLine("N2入った！");
                            Console.WriteLine("わーーーーーーーーーーー");
                            AttackHitFlag = true;
                            boss.NormalCollision(this);
                            if ((sceneNameT != SceneName.GameTitle))
                            {
                                boss.ReceiveDamage(this);
                            }
                        }
                        if (weakAttackCounter > 0 && weakAttackCounter <= 30 && Input.IsButtonDown(PlayerIndex.One, Buttons.RightShoulder))
                        {
                            weakAttackCounter = 0;
                        }
                        if (weakAttackCounter > 30)
                        {
                            weakAttackCounter = 0;
                            comboCount = 0;
                            ComboFlag = false;
                            AttackHitFlag = false;
                        }
                        break;

                    case 3:
                        if (AttackHit(boss) && weakAttackCounter == 1)
                        {
                            Console.WriteLine("N3入った！");
                            AttackHitFlag = true;
                            boss.NormalCollision(this);
                            if ((sceneNameT != SceneName.GameTitle))
                            {
                                boss.ReceiveDamage(this);
                            }
                        }
                        if (weakAttackCounter > 0 && weakAttackCounter <= 30 && Input.IsButtonDown(PlayerIndex.One, Buttons.RightShoulder))
                        {
                            weakAttackCounter = 0;
                        }
                        if (weakAttackCounter > 30)
                        {
                            weakAttackCounter = 0;
                            comboCount = 0;
                            ComboFlag = false;
                            AttackHitFlag = false;
                        }
                        break;

                    case 4:
                        if (AttackHit(boss) && weakAttackCounter == 1)
                        {
                            Console.WriteLine("N4入った！");
                            AttackHitFlag = true;
                            boss.NormalCollision(this);
                            if ((sceneNameT != SceneName.GameTitle))
                            {
                                boss.ReceiveDamage(this);
                            }
                        }
                        if (weakAttackCounter >= 0 && weakAttackCounter >= 60)
                        {
                            weakAttackCounter = 0;
                            comboCount = 0;
                            ComboFlag = false;
                            AttackHitFlag = false;
                        }
                        break;
                    case 0:
                        break;
                }
                weakAttackCounter++;
            }
            #region　攻撃隠し隠し
            ////N1
            //if (Input.IsButtonDown(PlayerIndex.One, Buttons.RightShoulder))
            //{
            //    if (AttackHit(boss))
            //    {
            //        AttackHitFlag = true;
            //        boss.NormalCollision(this);
            //        boss.ReceiveDamege(this);
            //        ComboFlag = true;
            //    }
            //    weakAttackCounter++;

            //    if (weakAttackCounter > 30)
            //    {
            //        weakAttackCounter = 0;

            //        ComboFlag = false;
            //    }
            //}
            ////N2
            //else if (Input.IsButtonDown(PlayerIndex.One, Buttons.RightShoulder) && ComboFlag == true)
            //{
            //    weakAttackCounter = 0;
            //    if (AttackHit(boss))
            //    {
            //        AttackHitFlag = true;
            //        boss.NormalCollision(this);
            //        boss.ReceiveDamege(this);
            //    }
            //    weakAttackCounter++;

            //    if (weakAttackCounter > 30)
            //    {
            //        weakAttackCounter = 0;

            //        ComboFlag = false;
            //    }
            //}
            ////N3
            //else if (Input.IsButtonDown(PlayerIndex.One, Buttons.RightShoulder) && ComboFlag == true)
            //{
            //    weakAttackCounter = 0;
            //    if (AttackHit(boss))
            //    {
            //        AttackHitFlag = true;
            //        boss.NormalCollision(this);
            //        boss.ReceiveDamege(this);
            //    }
            //    weakAttackCounter++;

            //    if (weakAttackCounter > 30)
            //    {
            //        weakAttackCounter = 0;

            //        ComboFlag = false;
            //    }
            //}
            ////N4
            //else if (Input.IsButtonDown(PlayerIndex.One, Buttons.RightShoulder) && ComboFlag == true)
            //{
            //    weakAttackCounter = 0;
            //    if (AttackHit(boss))
            //    {
            //        AttackHitFlag = true;
            //        boss.NormalCollision(this);
            //        boss.ReceiveDamege(this);
            //    }
            //    weakAttackCounter++;

            //    if (weakAttackCounter > 30)
            //    {
            //        weakAttackCounter = 0;

            //        ComboFlag = false;
            //    }
            //}
            #endregion
        }

        /// <summary>
        /// スキル①or②攻撃
        /// </summary>
        public void SkillAttack()
        {
            //Lボタン + Rボタン（弱攻撃）
            //if ((Input.IsButtonPress(PlayerIndex.One, Buttons.LeftShoulder) && Input.IsButtonDown(PlayerIndex.One, Buttons.RightShoulder))
            //    || Input.IsButtonPress(PlayerIndex.One, Buttons.RightShoulder) && (Input.IsButtonDown(PlayerIndex.One, Buttons.LeftShoulder)))
            //{
            //    whichSkillCheck = 0;
            //}
            if (Input.IsButtonDown(PlayerIndex.One, Buttons.LeftTrigger))
            {
                whichSkillCheck = 1;
            }

            //Lボタン + Xボタン（強攻撃）
            //if (Input.IsButtonDown(PlayerIndex.One, Buttons.LeftShoulder)
            //    && Input.IsButtonDown(PlayerIndex.One, Buttons.X))
            // if ((Input.IsButtonPress(PlayerIndex.One, Buttons.LeftShoulder) && Input.IsButtonDown(PlayerIndex.One, Buttons.X))
            //     || Input.IsButtonPress(PlayerIndex.One, Buttons.X) && (Input.IsButtonDown(PlayerIndex.One, Buttons.LeftShoulder)))
            // {
            //     whichSkillCheck = 1;
            // }
            //if (Input.IsButtonDown(PlayerIndex.One, Buttons.LeftShoulder))
            //{
            //    whichSkillCheck = 2;
            //}

            switch (whichSkillCheck)
            {
                case 1:
                    if (AttackHit(boss))
                    {
                        IsSkillHitFlag = true;
                        boss.NormalCollision(this);
                        boss.Skill1ReceiveDamage(this);
                        Skill1AttackMove();
                    }
                    break;

                    //case 2:
                    //    if (AttackHit(boss))
                    //    {
                    //        IsSkillHitFlag = true;
                    //        boss.NormalCollision(this);
                    //        boss.Skill2ReceiveDamage(this);
                    //    }
                    //    break;
            }
        }

        public void Skill1AttackMove()
        {

            if (IsSkillHitFlag && skill1Combo == 0)
            {
                skill1Combo = 1;
            }


            switch (skill1Combo)
            {
                case 1:
                    Skill1MovePosition = this.position;
                    if (skillCoolTime >= 18)
                    {
                        skill1Combo = 2;
                        skillCoolTime = 0;
                    }
                    break;

                case 2:
                    position = position + new Vector2(400, 400);
                    if (skillCoolTime >= 18)
                    {
                        Console.WriteLine("来た！ ");
                        skill1Combo = 3;
                        skillCoolTime = 0;
                    }
                    break;

                case 3:
                    position = position + new Vector2(100, 400);
                    if (skillCoolTime >= 18)
                    {
                        skill1Combo = 4;
                        skillCoolTime = 0;
                    }
                    break;

                case 4:
                    position = position + new Vector2(400, 200);
                    if (skillCoolTime >= 18)
                    {
                        skill1Combo = 5;
                        skillCoolTime = 0;
                    }
                    break;

                case 5:
                    position = position + new Vector2(250, 600);
                    if (skillCoolTime >= 18)
                    {
                        skill1Combo = 6;
                        skillCoolTime = 0;
                    }
                    break;

                case 6:
                    position = position + new Vector2(250, 0);
                    if (skillCoolTime >= 18)
                    {
                        skill1Combo = 0; ;
                        skillCoolTime = 0;
                    }
                    break;
            }
            skillCoolTime++;
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

        public void GetNowScene(SceneName sceneName)
        {
            this.sceneNameP = sceneName;
        }

        public void GetNowSceneT(SceneName sceneName)
        {
            this.sceneNameT = sceneName;
        }
    }
}
