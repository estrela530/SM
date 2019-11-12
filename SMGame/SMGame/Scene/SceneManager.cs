using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SMGame.Def;
using SMGame.Device;

using Microsoft.Xna.Framework;

namespace SMGame.Scene
{
    class SceneManager
    {
        // フィールド
        // シーン管理用ディクショナリ
        private Dictionary<SceneName, IScene> scenes =
            new Dictionary<SceneName, IScene>();
        // 現在のシーン
        private IScene currentScene = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SceneManager()
        {

        }

        /// <summary>
        /// シーンの追加
        /// </summary>
        /// <param name="name">シーン名</param>
        /// <param name="scene">追加するシーン</param>
        public void Add(SceneName name, IScene scene)
        {
            // 既にシーン名が登録されていたら
            if (scenes.ContainsKey(name))
            {
                // 何もしない
                return;
            }

            // シーンの追加
            scenes.Add(name, scene);
        }

        public void Update(GameTime gameTime)
        {
            // 今のシーンが空だったら
            if (currentScene == null)
            {
                // 何もせず終了
                return;
            }

            // 今のシーンの更新処理
            currentScene.Update(gameTime);

            // 今のシーンが終了していたら
            if (currentScene.IsEnd())
            {
                // 次のシーンに変更
                Change(currentScene.Next());
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer">レンダラー</param>
        public void Draw(Renderer renderer)
        {
            // 今のシーンが空だったら
            if (currentScene == null)
            {
                // 何もせず終了
                return;
            }

            // 今のシーンの描画処理
            currentScene.Draw(renderer);
        }

        public void Change(SceneName name)
        {
            //何かシーンが登録されていたら
            if (currentScene != null)
            {
                //現在のシーンの終了処理
                currentScene.Shutdown();
            }

            //ディクショナリから次のシーンを取り出し、
            //現在のシーンに設定
            currentScene = scenes[name];

            //シーンの初期化
            currentScene.Initialize();
        }


        public void Initialize()
        {

        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public bool IsEnd()
        {
            throw new NotImplementedException();
        }

        public SceneName Next()
        {
            return SceneName.GamePlay;
        }
    }
}
