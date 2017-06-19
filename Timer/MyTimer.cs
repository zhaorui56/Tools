/*
* ==============================================================================
*
* Filename: MyTimer
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


public class MyTimer
{
    private class STUINFO
    {
        public STUINFO(float s, int t, float p, object pa, Action<int, object> f)
        {
            startTime = s;
            time = t;
            perTime = p;
            param = pa;
            fun = f;
        }
        public float startTime;
        public int time;
        public float perTime;
        public object param;
        public Action<int, object> fun;
    }

    private KNode<STUINFO> _node = null;
    private KNode<STUINFO> _current = null;
    private bool _start = false;
    private int _id = 0;

    public int ID { get { return _id; } }
    
    public MyTimer(int id)
    {
        _id = id;
    }

    public void AddTimerEvent(float startTime, int time, float perTime, object param, Action<int, object> fun)
    {
        if(_node == null)
        {
            _node = new KNode<STUINFO>(new STUINFO(startTime, time, perTime, param, fun));
            _current = _node;
        }
        else
        {
            _current.Next = new KNode<STUINFO>(new STUINFO(startTime, time, perTime, param, fun));
            _current = _current.Next;
        }
    }

    public void Start()
    {
        _start = true;
        _current = _node;
        _dotimeLeave = _current.Data.startTime;
    }

    private float _dotimeLeave = 0;
    public bool DoTime(float time)
    {
        bool re = true;
        if (_start)
        {
            _dotimeLeave -= time;
            if (_dotimeLeave <= 0)
            {
                _dotimeLeave = _current.Data.perTime;
                try
                {
                    if (_current.Data.fun != null)
                        _current.Data.fun(_id, _current.Data.param);
                }
                catch
                {
                    re = false;
                }
                _current.Data.time--;
                if (_current.Data.time < 1)
                {
                    _current = _current.Next;
                    if (_current != null)
                    {
                        _dotimeLeave = _current.Data.startTime;
                    }
                    else
                    {
                        re = false;
                    }
                }
            }
        }
        return re;
    }

}
