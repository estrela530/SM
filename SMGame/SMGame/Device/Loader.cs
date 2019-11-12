using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 書いてない
using System.Diagnostics;

using Microsoft.Xna.Framework;

namespace SMGame.Device
{
    abstract class Loader
    {
        // フィールド
        // リソースアセット名配列
        protected string[,] resources;
        // ロード用カウンタ
        protected int counter;
        // ロードする最後の番号
        protected int maxNum;
        // 終了フラグ
        protected bool isEndFlag;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="resources">アセット名配列</param>
        public Loader(string[,] resources)
        {
            this.resources = resources;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            // 各変数初期化
            counter = 0;
            isEndFlag = false;
            maxNum = 0;

            // 条件がfalseの時に、エラー分を出す
            Debug.Assert(resources != null,
                "リソースデータ登録情報がおかしいです");
            // 配列からロードする数を取得
            maxNum = resources.GetLength(0);
        }

        /// <summary>
        /// 最大数取得
        /// </summary>
        /// <returns></returns>
        public int RegistMAXNum()
        {
            return maxNum;
        }

        /// <summary>
        /// 現在の登録している番号を取得
        /// </summary>
        /// <returns>今の番号</returns>
        public int CurrentCount()
        {
            return counter;
        }

        /// <summary>
        /// 終了かどうか
        /// </summary>
        /// <returns>終了していたらtrue</returns>
        public bool IsEnd()
        {
            return isEndFlag;
        }

        /// <summary>
        /// 更新（抽象メソッド）
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        public abstract void Update(GameTime gameTime);
    }
}
