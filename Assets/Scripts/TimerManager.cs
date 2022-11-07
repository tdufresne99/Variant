using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [SerializeField] ChangeScene _changeScene;
    [SerializeField] private Text _sSeconds;
    [SerializeField] private Text _tSeconds;
    [SerializeField] private Text _minutes;
    int sSeconds = 9;
    int tSeconds = 5;
    int minutes = 4;

    void Start()
    {
        StartTimer();
    }
    void Update()
    {
        if(sSeconds <= 0 && tSeconds <= 0 && minutes <= 0){
            _changeScene.ChangerSceneAvecInvoke("Lose", 1.5f);
            gameObject.SetActive(false);
        }
    }
    public void StartTimer()
    {
        sSeconds = 9;
        _sSeconds.text = sSeconds + "";

        tSeconds = 5;
        _tSeconds.text = tSeconds + "";

        minutes = 4;
        _minutes.text = minutes + ": ";

        InvokeRepeating("CountDown_sSeconds", 1f, 1f);
    }
    private void CountDown_sSeconds()
    {
        if(minutes <= 0 && tSeconds <= 0 && sSeconds <= 0){
            CancelInvoke();
        } else if(sSeconds <= 0){
            sSeconds = 9;
            _sSeconds.text = sSeconds + "";
            CountDown_tSeconds();
        } else {
            sSeconds --;
            _sSeconds.text = sSeconds + "";
        }
    }
    private void CountDown_tSeconds()
    {
        if(tSeconds <= 0){
            tSeconds = 5;
            _tSeconds.text = tSeconds + "";
            CountDown_minutes();
        } else {
            tSeconds--;
            _tSeconds.text = tSeconds + "";
        }
    }
    private void CountDown_minutes()
    {
        if(minutes <= 0 && tSeconds <= 0 && sSeconds <= 0){
            CancelInvoke();
        } else {
            minutes--;
            _minutes.text = minutes + ": ";
        }
    }
}
