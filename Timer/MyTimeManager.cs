/*
* ==============================================================================
*
* Filename: MyTimeManager
* Description: 
*
* Version: 1.0
* Created: 2017 05
* Compiler: Visual Studio 2010
*
* Author: RuiZhao
* Company: game95
*
* ==============================================================================
*/
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class KNode<T>
{
    public T Data { set; get; }
    public KNode<T> Next { set; get; }

    public KNode(T item)
    {
        this.Data = item;
        this.Next = null;
    }

    public KNode()
    {
        this.Data = default(T);
        this.Next = null;
    }
}

public class MyTimeManager : MonoBehaviour {

    private static MyTimeManager _instance;
    
    public static MyTimeManager Instance
    {
        get
        {
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;

        //// test 1
        //int timer1 = CreateTimer();
        //AddTimerEvent(timer1, 3, 3, 2, ()=> { Debug.Log(1); });
        //test2
        //int timer1 = CreateTimer(3, 3, 2, () => { Debug.Log(1); });
        //// test3
        //int timer1 = CreateTimer();
        //AddTimerEvent(timer1, 2, 2, 3, () => { Debug.Log(2); });
        //int timer2 = CreateTimer(3, 3, 2, () => { Debug.Log(1); });

        foreach (var per in _timerDic)
        {
            per.Value.Start();
        }
        _start = true;
        _timeFrom = Time.time;
    }
    void OnLevelWasLoaded()
    {
        Debug.Log("A new scene was loaded");
        _instance = this;
    }
    private List<int> _del = new List<int>();
    private int _index = 0;
    private Dictionary<int, MyTimer> _timerDic = new Dictionary<int, MyTimer>();
    private Dictionary<int, MyTimer> _addtimerDic = new Dictionary<int, MyTimer>();
    private bool _start = false;
    private float _timeFrom = 0;

    public int CreateTimer()
    {
        _index++;
        _addtimerDic.Add(_index, new MyTimer(_index));
        return _index;
    }
    public int CreateTimer(float startTime, int time, float perTime, Action<int, object> fun)
    {
        return CreateTimer(startTime, time, perTime, 0, fun);
    }
    public int CreateTimer(float startTime, int time, float perTime, object param, Action<int, object> fun)
    {
        int id = CreateTimer();
        AddTimerEvent(id, startTime, time, perTime, param, fun);
        return id;
    }
    public void AddTimerEvent(int id, float startTime, int time, float perTime, Action<int, object> fun)
    {
        AddTimerEvent(id, startTime, time, perTime, 0, fun);
    }
    public void AddTimerEvent(int id, float startTime, int time, float perTime, object param, Action<int, object> fun)
    {
        if(_timerDic.ContainsKey(id))
        {
            _timerDic[id].AddTimerEvent(startTime, time, perTime, param, fun);
        }
        else
        {
            if (_addtimerDic.ContainsKey(id))
            {
                _addtimerDic[id].AddTimerEvent(startTime, time, perTime, param, fun);
            }
            else
            {
                Debug.LogError("not contain this timer id!");
            }
        }
    }

    public void CloseTimer(int id)
    {
        if (_timerDic.ContainsKey(id))
        {
            _del.Add(id);
        }
        else
        {
            if (_addtimerDic.ContainsKey(id))
            {
                _del.Add(id);
            }
        }
    }

    public void StartTimer(int id)
    {
        if (_timerDic.ContainsKey(id))
        {
            _timerDic[id].Start();
        }
        else
        {
            if (_addtimerDic.ContainsKey(id))
            {
                _addtimerDic[id].Start();
            }
        }
    }

    public void Clear()
    {
        _timerDic.Clear();
        _addtimerDic.Clear();
        _del.Clear();
    }

    public void Restart()
    {
        foreach (var per in _timerDic)
        {
            per.Value.Start();
        }
        _index = 0;
        _start = true;
        _timeFrom = Time.time;
    }

    [SerializeField]
    int _logNum = 0;
    [SerializeField]
    int _diccount = 0;
    [SerializeField]
    int _dic1count = 0;

    private void FixedUpdate()
    {
        if(_start)
        {
            _logNum++;
            float time = Time.time - _timeFrom;
            _timeFrom = Time.time;
            _diccount = _addtimerDic.Count;
            _dic1count = _timerDic.Count;
            if (_addtimerDic.Count > 0)
            {
                foreach (var per in _addtimerDic)
                {
                    _timerDic.Add(per.Key, per.Value);
                }
                _addtimerDic.Clear();
            }

            if (_timerDic.Count > 0)
            {
                foreach (var per in _timerDic)
                {
                    if (!per.Value.DoTime(time))
                    {
                        _del.Add(per.Key);
                    }
                }
            }

            int len = _del.Count;
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    bool delOk = _timerDic.Remove(_del[i]);
                }
                _del.Clear();
            }
        }
    }
}
