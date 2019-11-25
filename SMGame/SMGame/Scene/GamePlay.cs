using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SMGame.Device;
using SMGame.Character;
using SMGame.Def;

namespace SMGame.Scene
{
    class GamePlay : IScene
    {
        private GameDevice gameDevice;
        private bool IsEndFlag;

        //Character関連
        private Boss boss;
        private Player player;

        //数値系　
        public Vector2 bossPosition = new Vector2(450, 420);
        public Vector2 playerFirstPosition = new Vector2(Screen.Width / 4, Screen.Height - 128);
        private Sound sound;

        public GamePlay()
        {
            gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();

        }

        public void Initialize()
        {
            IsEndFlag = false;

            boss = new Boss(bossPosition, gameDevice, 344, 217);
            player = new Player(playerFirstPosition, gameDevice, 128, 128, boss);
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin();

            renderer.DrawTexture("back", Vector2.Zero);

            boss.Draw(renderer);
            player.Draw(renderer);

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
            if (boss.player == null)
            {
                boss.GetPlayer(player);
            }
            sound.PlayBGM("BossBGM");
            boss.Update(gameTime);
            player.Update(gameTime);

            if (Input.GetKeyTrigger(Keys.Space) || Input.IsButtonDown(PlayerIndex.One, Buttons.Start))
            {
                IsEndFlag = true;
            }

            if (boss.IsDead())
            {
                IsEndFlag = true;
            }
        }
    }
}