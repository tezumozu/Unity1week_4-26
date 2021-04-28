using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Player;

namespace GameManagers{
    public enum GameStatus{
        INIT,
        GAME,
        RESULT,
        END
    }
    public class StatusManager : MonoBehaviour{
        static ReactiveProperty<GameStatus> gameStatus = new ReactiveProperty<GameStatus>();
        public static IReadOnlyReactiveProperty<GameStatus> status{
            get {
                return gameStatus;
            }
        }
        void Start(){
            //init
            gameStatus.Where(_ => gameStatus.Value == GameStatus.INIT)
                .Subscribe(status => {
                    Debug.Log("init");
                    Kamo player = GameObject.Find("Kamo").GetComponent<Kamo>();
                    player.child
                        .Where(num => num == 0)
                        .Subscribe(_ => {
                            gameStatus.Value = GameStatus.END;
                        });

                    StartCoroutine("countDown",3.0f);
                });

            //game
            gameStatus.Where(_ => gameStatus.Value == GameStatus.GAME)
                .Subscribe(status => {
                    Debug.Log("start");
                });

            //result
            gameStatus.Where(_ => gameStatus.Value == GameStatus.RESULT)
                .Subscribe(status => {

                });

            //end
            gameStatus.Where(_ => gameStatus.Value == GameStatus.END)
                .Subscribe(status => {

                });

            //ゲーム開始
            gameStatus.Value = GameStatus.INIT;

        }

        IEnumerator countDown(float count){
            while(count > 0){
                count -= Time.deltaTime;
                yield return null;
            }
            gameStatus.Value = GameStatus.GAME;
        }
    }
}

