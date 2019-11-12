using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SMGame.Device
{
    /// <summary>
    /// ゲームデバイスクラス
    /// 継承できないのでsealedで明示的に
    /// </summary>
    sealed class GameDevice
    {
        // 唯一のインスタンス
        private static GameDevice instance;

        // デバイス関連のフィールド
        private Renderer renderer;
        private Sound sound;
        private static Random random;
        private ContentManager content;
        private GraphicsDevice graphics;
        private GameTime gameTime;
        private Vector2 displayModify;//ディスプレイ位置修正


        /// <summary>
        /// コンストラクタ
        /// private宣言で外部からのnew実体生成はさせない
        /// </summary>
        /// <param name="content">コンテンツ管理</param>
        /// <param name="graphics">グラフィックデバイス</param>
        private GameDevice(ContentManager content, GraphicsDevice graphics)
        {
            renderer = new Renderer(content, graphics);
            sound = new Sound(content);
            random = new Random();
            this.content = content;
            this.graphics = graphics;
        }

        /// <summary>
        /// GameDeviceインスタンスの取得
        /// （Game1で使う実体生成用）
        /// </summary>
        /// <param name="content">コンテンツ管理</param>
        /// <param name="graphics">グラフィックデバイス</param>
        /// <returns>GameDeviceインスタンス</returns>
        public static GameDevice Instance(ContentManager content,
            GraphicsDevice graphics)
        {
            // インスタンスがnullだったら
            if (instance == null)
            {
                // 生成する
                instance = new GameDevice(content, graphics);
            }
            return instance;
        }

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <returns>インスタンス</returns>
        public static GameDevice Instance()
        {
            // まだインスタンスが生成されていなければエラー文を出す
            Debug.Assert(instance != null,
                "Game1クラスのInitializeメソッド内で引数付きInstanceメソッドを呼んでください");

            return instance;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        public void Update(GameTime gameTime)
        {
            // デバイスで絶対に1回のみ更新が必要なもの
            Input.Update();
            // ゲーム時間の更新
            this.gameTime = gameTime;
        }

        /// <summary>
        /// レンダラー取得
        /// </summary>
        /// <returns>レンダラー</returns>
        public Renderer GetRenderer()
        {
            return renderer;
        }

        /// <summary>
        /// サウンド取得
        /// </summary>
        /// <returns>サウンド</returns>
        public Sound GetSound()
        {
            return sound;
        }

        /// <summary>
        /// 乱数オブジェクト取得
        /// </summary>
        /// <returns>乱数オブジェクト</returns>
        public Random GetRandom()
        {
            return random;
        }

        /// <summary>
        /// コンテンツ管理取得
        /// </summary>
        /// <returns>コンテンツ管理</returns>
        public ContentManager GetContentManager()
        {
            return content;
        }

        /// <summary>
        /// グラフィックデバイス取得
        /// </summary>
        /// <returns>グラフィックデバイス</returns>
        public GraphicsDevice GetGraphicsDevice()
        {
            return graphics;
        }

        /// <summary>
        /// ゲーム時間の取得
        /// </summary>
        /// <returns>ゲーム時間</returns>
        public GameTime GetGameTime()
        {
            return gameTime;
        }

        public void SetDisplayModify(Vector2 position)
        {
            this.displayModify = position;
        }

        public Vector2 GetDisplayModify()
        {
            return displayModify;
        }
    }
}
