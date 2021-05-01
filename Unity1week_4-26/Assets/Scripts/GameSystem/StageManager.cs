using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Player;

namespace GameManagers{
    public class StageManager : MonoBehaviour{
        [SerializeField]
        Text stageText;

        [SerializeField]
        Text distanceText;
        public static float maxTime{
            get;
            private set;
        }
        public static int stageLevel{
            get;
            private set;
        }//
        public static float time{
            get;
            private set;
        }
        static Subject<Unit> endStage = new Subject<Unit>();
        static public IObservable<Unit> end{
            get{
                return endStage;
            }
        }
        Coroutine stopGame;
        // Start is called before the first frame update
        void Start(){
            maxTime = 30.0f;
            //初期化
            StatusManager.status
                .Where(status => status == GameStatus.INIT)
                .Subscribe(_ => {
                    stageLevel = 0;
                }).AddTo(gameObject);;

            //ゲーム開始前準備
            StatusManager.status
                .Where(status => status == GameStatus.READY)
                .Subscribe(_ => {
                    stageLevel++;
                    stageText.text = "stage " + stageLevel.ToString();
                    time = maxTime;
                    distanceText.text = "30";
                }).AddTo(gameObject);;

            //Game開始
            StatusManager.status
                .Where(status => status == GameStatus.GAME)
                .Subscribe(_ => {
                    stopGame = StartCoroutine("timer");
                }).AddTo(gameObject);;

            //ゲームオーバー時
            Kamo player = GameObject.Find("Kamo").GetComponent<Kamo>();
            player.child
                .Where(num => num == 0)
                .Subscribe(_ => {
                    StopCoroutine(stopGame);
                }).AddTo(gameObject);;
        }

        IEnumerator timer(){
            while(time > 0){
                time -= Time.deltaTime;
                distanceText.text = ((int)time + 1).ToString();
                yield return null;
            }
            distanceText.text = "0";
            endStage.OnNext(Unit.Default);
        }
    }
}
