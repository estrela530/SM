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
        private Vector2 tipAxis;
        private Vector2 rootAxis;
        private float tipRotate;
        private float rootRotate;
        private int isAttack; //0:攻撃無 1:攻撃待機 2:攻撃中 3:攻撃後
        readonly Vector2 tipTextureSize = new Vector2(70, 330);
        readonly Vector2 rootTextureSize = new Vector2(166, 65);

        private float sec = -2f;
        private float frame = 0.5f;
        Vector2 topPosL;
        Vector2 topPosR;
        Vector2 bottomPosL;
        Vector2 bottomPosR;

        Vector2 playerTopPosL;
        Vector2 playerTopPosR;

        Vector2 center;

        Vector2 targetPos;


        bool isBrake = false;

        /// <summary>
        /// 前右足
        /// </summary>
        public Leg1()
            : base("leg-root", "leg-tip")
        {
            isAttack = 0;
            tipAxis = new Vector2(tipTextureSize.X / 2, 0);
            rootAxis = new Vector2(10, rootTextureSize.Y / 3);
            position = new Vector2(300, Screen.Height / 2 + 10);
            rootPosition = new Vector2(position.X + rootTextureSize.X, position.Y);
            topPosL = new Vector2(position.X - 20, position.Y);
            topPosR = new Vector2(position.X + tipTextureSize.X + 60, position.Y);
            bottomPosL = new Vector2(position.X - 20, position.Y + tipTextureSize.Y - 40);
            bottomPosR = new Vector2(position.X + tipTextureSize.X + 60, position.Y + tipTextureSize.Y - 40);

            center = new Vector2(
                position.X + tipAxis.Y,
                position.Y + tipAxis.Y);
        }
        public override void Initialize()
        {
            isAttack = 0;
        }

        public override void Update(GameTime gameTime)
        {
            Attack();
            rootRotate = SetRotate(position, rootPosition);
            if (isBrake)
            {
                frame = 10f;
            }
        }
        public override void Draw(Renderer renderer)
        {
            if (!isBrake)
            {
                renderer.DrawTexture(rootname, position, rootRotate, rootAxis, Vector2.One);
                renderer.DrawTexture(tipname, position, tipRotate, tipAxis, Vector2.One);
            }
            else
            {
                renderer.DrawTexture(rootname + "-damage", position, rootRotate, rootAxis, Vector2.One);
                renderer.DrawTexture(tipname + "-damage", position, tipRotate, tipAxis, Vector2.One);
            }
            if (Collision.RotateRectangleCollision2(topPosL, bottomPosL, playerTopPosL, playerTopPosR, tipRotate, center, 2) &&
                Collision.RotateRectangleCollision2(topPosR, bottomPosR, playerTopPosL, playerTopPosR, tipRotate, center, 1) &&
                CheckHeight())
            {
                renderer.DrawTexture("leg-root-damage", playerTopPosL);
            }
        }

        public void Attack()
        {
            if (player == null)
            {
                return;
            }

            //攻撃
            switch (isAttack)
            {
                case 0://攻撃後
                case 3:
                    position += new Vector2(
                        (rootPosition.X - rootTextureSize.X + 20 - position.X) / 100f,
                        (rootPosition.Y - 50 - position.Y) / 80);
                    tipRotate += (0 - tipRotate) / 100;

                    if (Math.Abs(tipRotate) > MathHelper.ToDegrees(2) ||
                        Math.Abs(rootPosition.X - rootTextureSize.X + 20 - position.X) > 2 ||
                        Math.Abs(rootPosition.Y - 50 - position.Y) > 2)
                    {
                        break;
                    }
                    sec += 1 / 60f;
                    if (sec > frame)
                    {
                        targetPos = player.position;
                        isAttack++;
                    }
                    break;
                case 1://攻撃準備
                    position.X += 1f;
                    position.Y -= 1f;


                    if (position.Y < rootPosition.Y - 75)
                    {
                        isAttack = 2;
                    }
                    break;
                case 2://攻撃
                    position.X -= 4f;
                    position.Y += 4f;
                    tipRotate += (SetRotate(position, new Vector2(200, Screen.Height)) - MathHelper.ToRadians(90) - tipRotate) / 25;
                    if (position.X < rootPosition.X - rootTextureSize.X)
                    {
                        position.X = rootPosition.X - rootTextureSize.X;
                    }
                    if (position.Y > rootPosition.Y + 50)
                    {
                        isAttack++;
                        sec = 0;
                    }
                    break;
                case 4:
                    position.X += 1f;
                    position.Y -= 1f;


                    if (position.Y < rootPosition.Y - 75)
                    {
                        isAttack = 5;
                    }
                    break;
                case 5:
                    position.X -= 4f;
                    position.Y += 4f;
                    tipRotate += (SetRotate(position, targetPos) - MathHelper.ToRadians(90) - tipRotate) / 25;
                    if (position.X < rootPosition.X - rootTextureSize.X)
                    {
                        position.X = rootPosition.X - rootTextureSize.X;
                    }
                    if (position.Y > rootPosition.Y + 80)
                    {
                        isAttack = 0;
                        sec = 0;
                    }
                    break;
                default:
                    break;
            }

            //プレイヤーの上辺
            playerTopPosL = new Vector2(
                player.position.X, player.position.Y);
            playerTopPosR = new Vector2(
                player.position.X + 128, player.position.Y);
        }

        private float SetRotate(Vector2 my, Vector2 other)
        {
            float rad = (float)Math.Atan2(other.Y - my.Y,
                other.X - my.X);
            return rad;
        }


        private bool CheckHeight()
        {
            if (bottomPosL.Y <= bottomPosR.Y &&
                playerTopPosL.Y < bottomPosL.Y)
            {
                return true;
            }
            else if (bottomPosL.Y > bottomPosR.Y &&
                playerTopPosR.Y < bottomPosR.Y)
            {
                return true;
            }
            return false;
        }
    }
}
