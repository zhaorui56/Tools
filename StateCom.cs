using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


/// <summary>
/// 状态切换组件：存储状态和行为.
/// </summary>
public class StateCom : MonoBehaviour
{
	
#if UNITY_EDITOR
	public int _iStateID = -1;
	public int _iStateNum = 0;
#else
	private int _iStateID = -1;
#endif
    private int _oldStateID = -1;
    private Action _updateFun = null;
    private Dictionary<int, STUState> _stateDic = new Dictionary<int, STUState>();
	//private Hashtable _hashState = new Hashtable();
	
	private struct STUState
	{
		public STUState(Action startfun,
                        Action updatefun,
                        Action endfun)
		{
			StartFun = startfun;
			UpdateFun = updatefun;
			EndFun = endfun;
		}

        public Action StartFun;
        public Action UpdateFun;
        public Action EndFun;
	}
	
	
	/** 注册后自己实现状态的跳转 快速注册一个状态  */
    public bool RegStateQ(int ID, Action start = null, Action update = null, Action end = null)
	{
        if (_stateDic.ContainsKey(ID))
            return false;
        _stateDic.Add(ID, new STUState(start, update, end));
#if UNITY_EDITOR
		_iStateNum++;
#endif
		return true;
	}
	
	// 设置状态
	public bool SetState(int iID)
	{
        //Debug.Log(gameObject.name + " SetState " + iID);
		if(_iStateID > -1)
		{
			// 设置事件结束
            STUState stuold = (STUState)(_stateDic[_iStateID]);
            if (stuold.EndFun != null)
                ((Action)(stuold.EndFun))();
            //if(stuold.StartFun != null)
            //{
            //}
		}
		_pause = false;
        STUState stu = (STUState)(_stateDic[iID]);
        _oldStateID = _iStateID;
        _iStateID = iID;
        _updateFun = stu.UpdateFun;
        if (stu.StartFun != null)
            ((Action)(stu.StartFun))();
        else
        {
            Debug.LogError("Error state ID :" + iID + " is NULL " + gameObject.name);
            return false;
        }
        return true;
	}
	public void StopCurrentUpdate()
	{
		_updateFun = null;
	}
	
#if UNITY_EDITOR
	public void OutputInfo()
	{
		STUState stuold = (STUState)(_stateDic[_iStateID]);
		Debug.Log("StateID = " + _iStateID + "  Update null? " + (stuold.UpdateFun == null) + "  _updateFun = " + (_updateFun == null) + " HashNum = " + _stateDic.Count);
	}
#endif

    public int GetState()
	{
		return _iStateID;
	}
	public int GetOldState()
	{
        return _oldStateID;
	}

	public void Clear()
	{
        _stateDic.Clear();
	}

    private bool _pause = false;
    /// <summary> 暂停状态 </summary>
    public void PauseState()
    {
        _pause = true;
    }
    /// <summary> 取消暂停状态 </summary>
    public void UnPauseState()
    {
        _pause = false;
    }

    // void LateUpdate()
	void Update()
    {
        if (_updateFun != null && !_pause)
        {
			_updateFun();
		}
	}
}
