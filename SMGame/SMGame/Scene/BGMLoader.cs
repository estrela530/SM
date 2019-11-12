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
    class BGMLoader : Loader
    {
        // フィールド
        // サウンド
        private Sound sound;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="resources">アセット名配列</param>
        public BGMLoader(string[,] resources)
            : base(resources)
        {
            // サウンド取得
            sound = GameDevice.Instance().GetSound();
            // ベースクラスの初期化処理
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
            // カウントが最大でない時
            if (counter < maxNum)
            {
                // BGMの読み込み
                sound.LoadBGM(resources[counter, 0],
                    resources[counter, 1]);
                // カウンタを増やす
                counter += 1;
                // 読み込むものがあったので終了フラグをfalseに
                isEndFlag = false;
            }
        }
    }
}