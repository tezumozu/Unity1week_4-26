using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Player;
using Inputs;
using SceneLoads;

namespace GameManagers{
    public enum GameStatus{
        START,//初期状態
        INIT,
        READY,
        GAME,
        RESULT,
        RESET,
        END
    }
    public class StatusManager : MonoBehaviour{
        public static bool gameEnd{
            get;
            private set;
        }

        [SerializeField]
        Text countText;

        IGetButton con;
        static ReactiveProperty<GameStatus> gameStatus = new ReactiveProperty<GameStatus>();
        public static IReadOnlyReactiveProperty<GameStatus> status{
            get {
                return gameStatus;
            }
        }

        void Start(){
            gameStatus.Value = GameStatus.START;//初期値
            //コントローラの取得
            con = GameObject.Find("KeyController").GetComponent<IGetButton>();
            //init
            gameStatus.Where(_ => gameStatus.Value == GameStatus.INIT)
                .Subscribe(status => {
                    gameEnd = false;
                    //ゲームオーバーしたときの処理
                    Kamo player = GameObject.Find("Kamo").GetComponent<Kamo>();
                    player.child
                        .Where(num => num == 0)
                        .Subscribe(_ => {
                            gameEnd = true;
                            gameStatus.Value = GameStatus.RESULT;
                        });
                    
                    //準備状態へ
                    gameStatus.Value = GameStatus.READY;
                }).AddTo(gameObject);

            //READY
            gameStatus.Where(_ => gameStatus.Value == GameStatus.READY)
                .Subscribe(status => {
                    StartCoroutine("countDown",4.0f);
                }).AddTo(gameObject);
            
            //result
            gameStatus.Where(_ => gameStatus.Value == GameStatus.RESULT)
                .Subscribe(status => {
                    if(StageManager.stageLevel == 5){
                        gameEnd = true;
                    }
                }).AddTo(gameObject);

            //reset
            gameStatus.Where(_ => gameStatus.Value == GameStatus.RESET)
                .Subscribe(status => {
                    if(gameEnd){
                        gameStatus.Value = GameStatus.END;
                    }else{
                        gameStatus.Value = GameStatus.READY;
                    }
                }).AddTo(gameObject);

            //end
            gameStatus.Where(_ => gameStatus.Value == GameStatus.END)
                .Subscribe(status => {
                    gameStatus.Value = GameStatus.START;
                    SceneLoader loader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
                    loader.LoadScene("TitleScene");
                }).AddTo(gameObject);

            //ゲーム開始
            gameStatus.Value = GameStatus.INIT;

            //ステージが終了したとき
            StageManager.end.Subscribe(_ => {
                gameStatus.Value = GameStatus.RESULT;
            }).AddTo(gameObject);

            //リザルト中にスタートボタン(spaceキー)を押されたら遷移
            con.pressedIn
                .Where(_ => status.Value == GameStatus.RESULT)
                .Where(button => button == ButtonKind.START)
                .Subscribe(_ => {
                    gameStatus.Value = GameStatus.RESET;
                }).AddTo(gameObject);
            

        }

        IEnumerator countDown(float count){
            countText.gameObject.SetActive(true);
            while(count > 0){
                count -= Time.deltaTime;
                countText.text = ((int)count).ToString();

                if(count < 1){
                    countText.text = "Start!";
                }
                yield return null;
            }
            countText.gameObject.SetActive(false);
            gameStatus.Value = GameStatus.GAME;
        }
    }
}

