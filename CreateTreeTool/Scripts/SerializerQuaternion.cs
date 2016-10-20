using UnityEngine;

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

[Serializable]
public struct SerializerQuaternion : ISerializable
{
    public SerializerQuaternion(float xf, float yf, float zf, float wf)
    {
        x = xf;
        y = yf;
        z = zf;
        w = wf;
    }
    public SerializerQuaternion(SerializationInfo info, StreamingContext context)
    {
        x = info.GetSingle("x");
        y = info.GetSingle("y");
        z = info.GetSingle("z");
        w = info.GetSingle("w");
    }

    [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("x", x);
        info.AddValue("y", y);
        info.AddValue("z", z);
        info.AddValue("w", w);
    }

    public float x;
    public float y;
    public float z;
    public float w;

    public void Fill(Quaternion qua)
    {
        x = qua.x;
        y = qua.y;
        z = qua.z;
        w = qua.w;
    }
    public Quaternion FillV3()
    {
        return new Quaternion(x, y, z, w);
    }
}
