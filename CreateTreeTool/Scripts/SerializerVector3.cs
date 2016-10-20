using UnityEngine;

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

[Serializable]
public struct SerializerVector3 : ISerializable
{
    public SerializerVector3(float xf, float yf, float zf)
    {
        x = xf;
        y = yf;
        z = zf;
    }
    public SerializerVector3(SerializationInfo info, StreamingContext context)
    {
        x = info.GetSingle("x");
        y = info.GetSingle("y");
        z = info.GetSingle("z");
    }

    [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("x", x);
        info.AddValue("y", y);
        info.AddValue("z", z);
    }

    public float x;
    public float y;
    public float z;

    public void Fill(Vector3 v3)
    {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }
    public Vector3 FillV3()
    {
        return new Vector3(x, y, z);
    }
}
