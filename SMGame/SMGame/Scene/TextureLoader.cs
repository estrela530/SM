using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


using SMGame.Device;

namespace SMGame.Scene
{
    class TextureLoader : Loader
    {
        // フィールド
        // レンダラー
        private Renderer renderer;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="resources">アセット名配列</param>
        public TextureLoader(string[,] resources)
            : base(resources)
        {
            // Renderer取得
            renderer = GameDevice.Instance().GetRenderer();
            // ベースクラスの初期化
            base.Initialize();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        public override void Update(GameTime gameTime)
        {
            // 終了フラグを一旦trueにする
            isEndFlag = true;

            // カウンタが最後まで達していない時
            if (counter < maxNum)
            {
                // 画像の読み込み
                renderer.LoadContent(resources[counter, 0],
                    resources[counter, 1]);
                // カウンタを増やす
                counter += 1;
                // 読み込むものがあったので終了フラグをfalseに
                isEndFlag = false;
            }
        }
    }
}
