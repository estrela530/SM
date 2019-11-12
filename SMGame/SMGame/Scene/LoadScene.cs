using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 書いてない
using Microsoft.Xna.Framework;

using SMGame.Device;
using SMGame.Scene;
using SMGame.Def;

namespace SMGame.Scene
{
    class LoadScene : IScene
    {
        // フィールド
        // レンダラー
        private Renderer renderer;
        // 読み込み用オブジェクト
        private TextureLoader textureLoader;
        private BGMLoader bgmLoader;
        private SELoader seLoader;

        // 全リソース数
        private int totalResourceNum;
        // 終了フラグ
        private bool IsEndFlag;
        //// タイマー
        //private Timer timer;

        #region テクスチャ用
        /// <summary>
        /// テクスチャ読み込み用2次元配列の取得
        /// </summary>
        /// <returns>アセット名の配列</returns>
        private string[,] textureMatrix()
        {
            // テクスチャディレクトリのデフォルトパス
            string path = "./Texture/";
            //string pathAnimation = "./Animation/";

            // 読み込み対象データ
            string[,] data = new string[,]
            {               
                //ここから下追加                
                {"kuribo-",path },
                {"Title",path },
                {"Ending",path },
                {"backColor",path },
                {"kumo",path },
                {"Face",path },
                {"Body",path },
                {"LegT",path },
                {"LegU",path }
                //ここから上追加
            };

            return data;
        }
        #endregion テクスチャ用

        #region BGM用
        /// <summary>
        /// BGM読み込み用2次元配列の取得
        /// </summary>
        /// <returns>アセット名の配列</returns>
        private string[,] BGMMatrix()
        {
            // サウンドディレクトリのデフォルトパス
            string path = "./Sound/";
            // BGM読み込み対象データ
            string[,] data = new string[,]
            {
            };

            return data;
        }
        #endregion BGM用

        #region SE用
        /// <summary>
        /// SE読み込み用2次元配列の取得
        /// </summary>
        /// <returns>アセット名の配列</returns>
        private string[,] SEMatrix()
        {
            // SEディレクトリのデフォルトパス
            string path = "./Sound/";

            // SE読み込み対象データ
            string[,] data = new string[,]
            {

            };

            return data;
        }
        #endregion SE用

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LoadScene()
        {
            // レンダラー取得
            renderer = GameDevice.Instance().GetRenderer();
            // 読み込む対象を取得し、実体生成
            textureLoader = new TextureLoader(textureMatrix());
            bgmLoader = new BGMLoader(BGMMatrix());
            seLoader = new SELoader(SEMatrix());
            IsEndFlag = false;

            //// タイマー生成
            //timer = new CountDownTimer(0.1f);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer">レンダラー</param>
        public void Draw(Renderer renderer)
        {
            // Begin
            renderer.Begin();

            //renderer.DrawTexture("load", new Vector2(20, 20));

            // 現在読み込んでいる数を取得
            int currentCount =
            textureLoader.CurrentCount() +
            bgmLoader.CurrentCount() +
            seLoader.CurrentCount();

            // 読み込むものがあれば描画
            //if (totalResourceNum != 0)
            //{
            //    // 読みこんだ割合
            //    float rate = (float)currentCount / totalResourceNum;
            //    // 数字で描画
            //    renderer.DrawNumber(
            //        "number",
            //        new Vector2(20, 100),
            //        (int)(rate * 100.0f));

            //    // バーで描画
            //    renderer.DrawTexture(
            //        "fade",
            //        new Vector2(0, 500),
            //        null,
            //        0.0f,
            //        Vector2.Zero,
            //        new Vector2(rate * Screen.Width, 20));
            //}

            // 終了
            // すべてのデータを読み込んだか？
            if (textureLoader.IsEnd() &&
            bgmLoader.IsEnd() &&
            seLoader.IsEnd())
            {
                IsEndFlag = true;
            }

            // End
            renderer.End();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            // 終了フラグを継続に設定
            IsEndFlag = false;
            // テクスチャ読み込みオブジェクトを初期化
            textureLoader.Initialize();
            // BGM読み込みオブジェクトを初期化
            bgmLoader.Initialize();
            // SE読み込みオブジェクトを初期化
            seLoader.Initialize();
            // 全リソース数を計算
            totalResourceNum =
            textureLoader.RegistMAXNum() +
            bgmLoader.RegistMAXNum() +
            seLoader.RegistMAXNum();
        }

        /// <summary>
        /// シーン終了かどうか
        /// </summary>
        /// <returns>シーン終了ならtrue</returns>
        public bool IsEnd()
        {
            return IsEndFlag;
        }

        /// <summary>
        /// 次のシーン
        /// </summary>
        /// <returns>次のシーン</returns>

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
            // 演出確認用
            // 一定時間ごとに読み込み
            //timer.Update(gameTime);
            // 時間になっていない時
            //if (timer.IsTime() == false)
            //{
            //    // 終了
            //    return;
            //}
            // 時間になっている時、初期化
            //timer.Initialize();

            // テクスチャから順々に読み込みを行う
            if (textureLoader.IsEnd() == false)
            {
                textureLoader.Update(gameTime);
            }
            else if (bgmLoader.IsEnd() == false)
            {
                bgmLoader.Update(gameTime);
            }
            else if (seLoader.IsEnd() == false)
            {
                seLoader.Update(gameTime);
            }
        }

        public SceneName Next()
        {
            return SceneName.GameTitle;
        }
    }
}
