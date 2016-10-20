using UnityEngine;

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

// 一个物体的序列化信息结构体
[Serializable]
public struct STUBuildInfo : ISerializable
{
    //public STUBuildInfo(SerializerVector3 p, Quaternion q, SerializerVector3 s)
    public STUBuildInfo(SerializerVector3 p, SerializerQuaternion q, SerializerVector3 s)
    {
        _pos = p;
        _rotate = q;
        _scare = s;
    }

    public STUBuildInfo(SerializationInfo info, StreamingContext context)
    {
        _pos = (SerializerVector3)info.GetValue("_pos", typeof(SerializerVector3));
        _rotate = (SerializerQuaternion)info.GetValue("_rotate", typeof(SerializerQuaternion));
        _scare = (SerializerVector3)info.GetValue("_scare", typeof(SerializerVector3));
    }

    [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("_pos", _pos);
        info.AddValue("_rotate", _rotate);
        info.AddValue("_scare", _scare);
    }

    public SerializerVector3 _pos;
    public SerializerQuaternion _rotate;
    public SerializerVector3 _scare;
}