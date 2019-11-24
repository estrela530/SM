using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using System.Diagnostics;

namespace SMGame.Device
{
    class Sound
    {
        #region フィールドとコンストラクタ
        // コンテンツ管理
        private ContentManager contentManager;
        // MP3管理用
        private Dictionary<string, Song> bgms;
        // WAV管理用
        private Dictionary<string, SoundEffect> soundEffects;
        // WAVインスタンスの再生管理用ディクショナリ（WAVの高度な利用）
        private Dictionary<string, SoundEffectInstance> seInstances;
        // WAVインスタンスの再生管理用ディクショナリ
        private Dictionary<string, SoundEffectInstance> sePlayDict;
        // 現在再生中のMP3のアセット名
        private string currentBGM;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content">コンテンツ管理</param>
        public Sound(ContentManager content)
        {
            // Game1クラスのコンテンツ管理と紐づけ
            contentManager = content;
            // BGMは繰り返し再生
            MediaPlayer.IsRepeating = true;

            // 各Dictionaryの実体生成
            bgms = new Dictionary<string, Song>();
            soundEffects = new Dictionary<string, SoundEffect>();
            seInstances = new Dictionary<string, SoundEffectInstance>();

            // 再生Listの実体生成
            sePlayDict = new Dictionary<string, SoundEffectInstance>();

            // 何も再生していないのでnullで初期化
            currentBGM = null;
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Unload()
        {
            // ディクショナリをクリア
            bgms.Clear();
            soundEffects.Clear();
            seInstances.Clear();
            sePlayDict.Clear();
        }

        #endregion フィールドとコンストラクタとアンロード

        /// <summary>
        /// Assert用エラーメッセージ
        /// </summary>
        /// <param name="name">使えないであろうアセット名</param>
        /// <returns></returns>
        private string ErrorMessage(string name)
        {
            return "再生する音データのアセット名(" + name + ")がありません" +
                "アセット名の確認、Dictionaryに登録しているか確認してください";
        }

        #region BGM(MP3:MediaPlayer)関連

        /// <summary>
        /// BGM(MP3)の読み込み
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="filepath">ファイルパス</param>
        public void LoadBGM(string name, string filepath = "./")
        {
            // 既に登録されていたら
            if (bgms.ContainsKey(name))
            {
                // ここで終了
                return;
            }
            // MP3の読み込みとDictionaryへ登録
            bgms.Add(name, contentManager.Load<Song>(filepath + name));
        }

        /// <summary>
        /// BGMが停止中かどうか
        /// </summary>
        /// <returns>停止中ならtrue</returns>
        public bool IsStoppedBGM()
        {
            return (MediaPlayer.State == MediaState.Stopped);
        }

        /// <summary>
        /// BGMが再生中かどうか
        /// </summary>
        /// <returns>再生中であればtrue</returns>
        public bool IsPlayingBGM()
        {
            return (MediaPlayer.State == MediaState.Playing);
        }

        /// <summary>
        /// BGMが一時停止中かどうか
        /// </summary>
        /// <returns>一時停止中ならtrue</returns>
        public bool IsPauseBGM()
        {
            return (MediaPlayer.State == MediaState.Paused);
        }

        /// <summary>
        /// BGMを停止
        /// </summary>
        public void StopBGM()
        {
            MediaPlayer.Stop();
            currentBGM = null;
        }


        public void PlayBGM(string name)
        {
            // アセット名がディクショナリに登録されているか？
            Debug.Assert(bgms.ContainsKey(name), ErrorMessage(name));

            // 同じ曲だったら
            if (currentBGM == name)
            {
                // 何もしない
                return;
            }

            // BGMが再生中だったら
            if (IsPlayingBGM())
            {
                // BGM停止
                StopBGM();
            }

            // ボリューム設定（BGMはSEに比べて音量半分が普通）
            MediaPlayer.Volume = 0.5f;

            // 現在のBGM名を設定
            currentBGM = name;

            // 再生開始
            MediaPlayer.Play(bgms[currentBGM]);
        }

        /// <summary>
        /// BGMの一時停止
        /// </summary>
        public void PauseBGM()
        {
            // BGM再生中なら
            if (IsPlayingBGM())
            {
                // 一時停止
                MediaPlayer.Pause();
            }
        }

        /// <summary>
        /// 一時停止からの再生
        /// </summary>
        public void ResumeBGM()
        {
            if (IsPauseBGM())
            {
                MediaPlayer.Resume();
            }
        }

        /// <summary>
        /// BGMループフラグを変更
        /// </summary>
        /// <param name="loopFlag"></param>
        public void ChangeBGMLoopFlag(bool loopFlag)
        {
            MediaPlayer.IsRepeating = loopFlag;
        }
        #endregion BGM(MP3:MediaPlayer)関連


        #region WAV(SE:SoundEffect)関連

        /// <summary>
        /// SEをロード
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="filepath">ファイルパス</param>
        public void LoadSE(string name, string filepath = "./")
        {
            // 既に登録されていたら
            if (soundEffects.ContainsKey(name))
            {
                // 何もせずに終了
                return;
            }

            // ロードして、soundEffectsに登録
            soundEffects.Add(name, contentManager.Load<SoundEffect>(filepath + name));
        }
        
        /// <summary>
        /// SEを再生
        /// </summary>
        /// <param name="name"></param>
        public void PlaySE(string name)
        {
            // アセットが登録されているか？
            Debug.Assert(soundEffects.ContainsKey(name), ErrorMessage(name));

            // 再生
            soundEffects[name].Play();
        }

        public void LoadSEInstance(string name)
        {
            seInstances[name] = soundEffects[name].CreateInstance();
        }

        public void PlaySEInstance(string name)
        {
            // アセットが登録されているか？
            Debug.Assert(seInstances.ContainsKey(name), ErrorMessage(name));

            // 再生
            seInstances[name].Play();
        }


        public void StopSEInstance(string name)
        {
            // アセットが登録されているか？
            Debug.Assert(seInstances.ContainsKey(name), ErrorMessage(name));

            // 再生
            seInstances[name].Stop();
        }
        #endregion WAV(SE:SoundEffect)関連

        #region WAVインスタンス関連

        public void CreateSEInstance(string name)
        {
            // 既に登録されていたら
            if (seInstances.ContainsKey(name))
            {
                // 何もせず終了
                return;
            }

            // WAV用ディクショナリに登録されていないと無理
            Debug.Assert(
                soundEffects.ContainsKey(name),
                "先に" + name + "の読み込み処理を行ってください");

            // WAVデータのインスタンスを生成し、登録
            seInstances.Add(name, soundEffects[name].CreateInstance());
        }

        /// <summary>
        /// SEインスタンスを再生
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="no">番号</param>
        /// <param name="loopFlg">ループするかどうか</param>
        public void PlaySEInstances(string name, int no, bool loopFlg = false)
        {
            Debug.Assert(seInstances.ContainsKey(name),
                ErrorMessage(name));
            // 再生管理用ディクショナリに登録されていたら
            if (sePlayDict.ContainsKey(name + no))
            {
                // 何もしない
                return;
            }
            // seInstacesからnameのデータを取り出す
            var data = seInstances[name];
            // ループフラグ設定
            data.IsLooped = loopFlg;
            // 再生
            data.Play();
            // 再生しているDictionaryに追加
            sePlayDict.Add(name + no, data);
        }

        /// <summary>
        /// 指定SEの停止
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="no">番号</param>
        public void StoppedSE(string name, int no)
        {
            // 再生管理用ディクショナリになければ
            if (sePlayDict.ContainsKey(name + no) == false)
            {
                // 何もしない
                return;
            }
            // もしプレイ中なら
            if (sePlayDict[name + no].State == SoundState.Playing)
            {
                sePlayDict[name + no].Stop();
            }
        }

        /// <summary>
        /// 再生中のSEを全て停止
        /// </summary>
        public void StoppedSE()
        {
            // 再生中SEを1つずつ取り出して繰り返す
            foreach (var se in sePlayDict)
            {
                // もし再生中なら
                if (se.Value.State == SoundState.Playing)
                {
                    // 停止
                    se.Value.Stop();
                }
            }
        }

        /// <summary>
        /// 指定したSEを削除
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="no">番号</param>
        public void RemoveSE(string name, int no)
        {
            // 再生管理用ディクショナリになければ
            if (sePlayDict.ContainsKey(name + no) == false)
            {
                // 何もしない
                return;
            }
            // 指定の要素を削除
            sePlayDict.Remove(name + no);
        }

        /// <summary>
        /// 全てのSEを削除
        /// </summary>
        public void RemoveSE()
        {
            sePlayDict.Clear();
        }

        /// <summary>
        /// 指定したSEを一時停止
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="no">番号</param>
        public void PauseSE(string name, int no)
        {
            // 再生管理用ディクショナリになければ
            if (sePlayDict.ContainsKey(name + no) == false)
            {
                // 何もしない
                return;
            }
            // 再生中なら
            if (sePlayDict[name + no].State == SoundState.Playing)
            {
                sePlayDict[name + no].Pause();
            }
        }

        /// <summary>
        /// 全てのSEを一時停止
        /// </summary>
        public void PauseSE()
        {
            foreach (var se in sePlayDict)
            {
                if (se.Value.State == SoundState.Playing)
                {
                    se.Value.Pause();
                }
            }
        }

        /// <summary>
        /// 指定したSEを一時停止から復帰
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="no">番号</param>
        public void ResumeSE(string name, int no)
        {
            // 再生管理用ディクショナリになければ
            if (sePlayDict.ContainsKey(name + no) == false)
            {
                // 何もしない
                return;
            }
            // 一時停止中なら
            if (sePlayDict[name + no].State == SoundState.Paused)
            {
                // 再開する
                sePlayDict[name + no].Resume();
            }
        }

        /// <summary>
        /// 一時停止中の全てのSEを復帰
        /// </summary>
        public void ResumeSE()
        {
            foreach (var se in sePlayDict)
            {
                if (se.Value.State == SoundState.Paused)
                {
                    se.Value.Resume();
                }
            }
        }

        /// <summary>
        /// SEインスタンスが再生中かどうか
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="no">番号</param>
        /// <returns>再生中ならtrue</returns>
        public bool IsPlayingSEInstance(string name, int no)
        {
            return sePlayDict[name + no].State == SoundState.Playing;
        }

        /// <summary>
        /// SEインスタンスが停止中かどうか
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="no">番号</param>
        /// <returns>停止中ならtrue</returns>
        public bool IsStoppedSEInstance(string name, int no)
        {
            return sePlayDict[name + no].State == SoundState.Stopped;
        }

        /// <summary>
        /// SEインスタンスが一時停止中かどうか
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="no">番号</param>
        /// <returns>一時停止中ならtrue</returns>
        public bool IsPausedSEInstance(string name, int no)
        {
            return sePlayDict[name + no].State == SoundState.Paused;
        }
        #endregion WAVインスタンス関連
    }
}
