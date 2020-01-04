using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SMGame.Device;
using SMGame.Scene;

namespace SMGame.Scene
{
    /// <summary>
    /// エンディングクラス
    /// </summary>
    class GameEnding : IScene
    {
        private bool IsEndFlag;//終了フラグ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameEnding()
        {
            IsEndFlag = false;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            renderer.Begin();

            renderer.DrawTexture("gameclear", Vector2.Zero);

            renderer.End();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            IsEndFlag = false;
        }

        /// <summary>
        /// シーン終了か？
        /// </summary>
        /// <returns>シーン終了してたらtrue</returns>
        public bool IsEnd()
        {
            return IsEndFlag;
        }

        /// <summary>
        /// 次のシーンへ
        /// </summary>
        /// <returns>次のシーン名</returns>
        public SceneName Next()
        {
            return SceneName.GameTitle;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Shutdown()
        {
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        public void Update(GameTime gameTime)
        {
            if (Input.GetKeyTrigger(Keys.Space) || Input.IsButtonDown(PlayerIndex.One, Buttons.Start))
            {
                IsEndFlag = true;
            }
        }
    }
}
