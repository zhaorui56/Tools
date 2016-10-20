using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Collections.Generic;

public class Common
{
    public static bool IsTouchUI(Camera uicamera = null)
    {
        if(uicamera == null)
        {
            uicamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        }
        if(uicamera == null)
        {
            Debug.LogError("cant find UICamera");
            return false;
        }
        Ray ray = uicamera.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线

        return Physics.Raycast(ray, 50, 1 << LayerMask.NameToLayer("000"));
    }
    
    // 二进制序列化
    public static MemoryStream Serialize(object stu)
    {
        MemoryStream stream = new MemoryStream();
        // 构造二进制序列化格式器
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        // 告诉序列化器将对象序列化到一个流中
        binaryFormatter.Serialize(stream, stu);
        return stream;
    }
    // 反序列化二进制
    public static object Deserialize(Stream stream)
    {
        stream.Position = 0;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        return binaryFormatter.Deserialize(stream);
    }

    // MemoryStream to file
    public static bool SaveFile(string path, MemoryStream ms)
    {
        try
        {
            using (FileStream file = new FileStream(path, FileMode.Create, System.IO.FileAccess.Write))
            {
                ms.WriteTo(file);
            }
        }
        catch(InvalidCastException e)
        {
            Debug.LogError(e);
            return false;
        }
        return true;
    }

    // file to MemoryStream 
    public static MemoryStream ReadFile(string path)
    {
        MemoryStream ms = null;
        using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            ms = new MemoryStream();
            byte[] bytes = new byte[file.Length];
            file.Read(bytes, 0, (int)file.Length);
            ms.Write(bytes, 0, (int)file.Length);
        }
        return ms;
    }
    
}
