using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using SMGame.Device;

namespace SMGame.Scene
{
    /// <summary>
    /// シーンインターフェース
    /// </summary>
    interface IScene
    {
        // 初期化
        void Initialize();
        // 更新
        void Update(GameTime gameTime);
        // 描画
        void Draw(Renderer renderer);
        // 終了
        void Shutdown();

        // シーン管理用
        // シーンが終わっているかどうか
        bool IsEnd();
        // 次のシーン取得
        SceneName Next();
    }
}
