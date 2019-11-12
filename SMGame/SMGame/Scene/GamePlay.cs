using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SMGame.Device;
using SMGame.Character;

namespace SMGame.Scene
{
    class GamePlay : IScene
    {
        private GameDevice gameDevice;
        private bool IsEndFlag;

        //Character関連
        private Boss boss;

        //数値系　
        public Vector2 bossfirstPosition = new Vector2(600, 450);

        
        public GamePlay()
        {
            gameDevice = GameDevice.Instance();

        }

        public void Initialize()
        {
            IsEndFlag = false;

            boss = new Boss(bossfirstPosition, gameDevice, 64, 64);
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin();

            renderer.DrawTexture("backColor", Vector2.Zero);
            boss.Draw(renderer);

            renderer.End();
        }



        public bool IsEnd()
        {
            return IsEndFlag;
        }

        public SceneName Next()
        {
            return SceneName.GameEnding;
        }

        public void Shutdown()
        {

        }

        public void Update(GameTime gameTime)
        {
            boss.Update(gameTime);

            if (Input.GetKeyTrigger(Keys.Space))
            {
                IsEndFlag = true;
            }
        }
    }
}