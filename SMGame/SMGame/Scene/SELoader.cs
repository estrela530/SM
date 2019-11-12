using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

// 書いてない
using SMGame.Device;

namespace SMGame.Scene
{
    class SELoader : Loader
    {
        // フィールド
        // サウンド
        private Sound sound;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="resources">アセット名の配列</param>
        public SELoader(string[,] resources)
            : base(resources)
        {
            // サウンドの取得
            sound = GameDevice.Instance().GetSound();
            // ベースの初期化
            base.Initialize();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        public override void Update(GameTime gameTime)
        {
            // 終了フラグをとりあえずtrueにする
            isEndFlag = true;
            // カウンタが最後まで達していなければ
            if (counter < maxNum)
            {
                // SEの読み込み
                sound.LoadSE(resources[counter, 0],
                    resources[counter, 1]);
                // カウンタを1増やす
                counter += 1;
                // 読み込むものがあったので終了フラグをhufalseにする
                isEndFlag = false;
            }
        }
    }
}
