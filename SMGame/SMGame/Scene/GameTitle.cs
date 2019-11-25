using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SMGame.Scene;
using SMGame.Device;
using SMGame.Character.Legs;
using SMGame.Character;
using SMGame.Def;

namespace SMGame.Scene
{
    class GameTitle : IScene
    {
        //フィールド
        // 終了しているかどうか
        private bool IsEndFlag;
        // サウンド
        private Sound sound;
        private Boss boss;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameTitle()
        {
            IsEndFlag = false;
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();

            boss = new Boss(new Vector2(Screen.Width / 2 - 125 / 2, Screen.Height - 250), gameDevice, 125, 200, true);
        }
        public void Draw(Renderer renderer)
        {
            renderer.Begin();
            renderer.DrawTexture("Title", Vector2.Zero);
            boss.Draw(renderer);
            renderer.End();
        }

        public void Initialize()
        {
            IsEndFlag = false;
        }

        /// <summary>
        /// シーンが終了かどうか
        /// </summary>
        /// <returns>シーン終了ならtrue</returns>
        public bool IsEnd()
        {
            return IsEndFlag;
        }

        public SceneName Next()
        {
            return SceneName.GamePlay;
        }

        public void Shutdown()
        {

        }

        public void Update(GameTime gameTime)
        {
            //if(Input.IsButtonDown(PlayerIndex.One, Buttons.RightShoulder))
            //{
            //    sound.PlaySE("run");
            //}
            boss.Update(gameTime);
            if (Input.GetKeyTrigger(Keys.Space) || Input.IsButtonDown(PlayerIndex.One,Buttons.Start))
            {
                IsEndFlag = true;
            }
        }
    }
}
