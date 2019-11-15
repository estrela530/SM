using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMGame.Device;
using SMGame.Def;

namespace SMGame.Character
{
    class Player
    {
        public Vector2 position;
        public Vector2 velocity;
        public int width;
        public int height;
        public float moveSpeed = 7.0f;
        public int HP;
        public bool IsJumpFlag = false;

        public Player(Vector2 position, GameDevice gameDevice, int width, int height)
        {
            this.position = position;
        }

        public void Initialize()
        {
            IsJumpFlag = false;
        }

        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("PlayerTatie", position);
        }

        public void Update(GameTime gameTime)
        {
            PlayerMove();
            PlayerJump();
            GroundHit();

            position = position + velocity;
        }

        public void PlayerMove()
        {
            velocity.X = Input.GetLeftStickground(PlayerIndex.One).X * moveSpeed;

            //position = position + velocity;
        }

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

        public void GroundHit()
        {
            if (position.Y > Screen.Height - 180)
            {
                velocity.Y = 0.0f;
                position.Y = Screen.Height - 180;
                IsJumpFlag = false;
            }            
        }
        
    }
}
