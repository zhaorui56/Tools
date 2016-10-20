using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class EditBigScene : EditorWindow
{
    private Dictionary<string, STUBuildInfo> _tempDic;

    static int _countOvered = 0;
    static int _countStep = 0;
    static int _stepNum = 10;
    static int _count = 0;
    static Transform _tempTransParent;


    static void InitPerObj(GameObject obj)
    {
        Debug.Log("RevertPrefabInstance ReconnectToLastPrefab  " + obj.name);
        PrefabUtility.ReconnectToLastPrefab(obj);
    }

    [MenuItem("HxBigScene/EditBigScene")]
    static void Init()
    {
        _countOvered = 0;
        _countStep = 0;
        _stepNum = 10;
        _count = 0;

        EditBigScene window = (EditBigScene)EditorWindow.GetWindow(typeof(EditBigScene));
    }

    static int _actNum = 0;

    [MenuItem("HxBigScene/Reload")]
    static void Reload()
    {
        if (Selection.objects.Length != 1)
        {
            Debug.LogError("Please select one Object");
            return;
        }

        GameObject del = new GameObject("DEL");
        GameObject add = new GameObject("ADD");
        List<GameObject> delList = new List<GameObject>();

        UnityEngine.Object obj = Selection.objects[0];
        Transform trans = (obj as GameObject).transform;

        int len = trans.childCount;
        _actNum = len;

        for (int i = 0; i < len; i++)
        {
            GameObject objper = trans.GetChild(i).gameObject;
            string name = objper.name;

            int index = name.IndexOf("(Clone)");
            if(index > 0)
            {
                name = name.Substring(0, index);

                _actNum++;

                GameObject newObj = Instantiate(Resources.Load("BigScenePrefabs/" + name)) as GameObject;
                newObj.transform.parent = add.transform;
                newObj.transform.position = objper.transform.position;
                newObj.transform.rotation = objper.transform.rotation;
                newObj.transform.localScale = objper.transform.localScale;

                newObj.name = name + " (" + _actNum + ")";

                delList.Add(objper);
            }
        }

        foreach(GameObject delper in delList)
        {
            delper.transform.parent = del.transform;
        }
        delList.Clear();
    }


    public UnityEngine.Object _source;
    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        _source = EditorGUILayout.ObjectField(_source, typeof(UnityEngine.Object), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _countStep = EditorGUILayout.IntField(_countStep);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _stepNum = EditorGUILayout.IntField(_stepNum);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Next", GUILayout.Height(20)))
        {
            _tempTransParent = (_source as GameObject).transform;
            _count = _tempTransParent.childCount;

            _countStep = _countOvered + _stepNum;
            if (_countStep > _count)
            {
                _countStep = _count;
            }
            //Debug.Log(_countOvered + " " + _countStep + " " + _count);
        }
        if (_countOvered < _countStep && _count > 0)
        {
            for (int i = _countOvered; i < _countStep; i++)
            {
                InitPerObj(_tempTransParent.GetChild(i).gameObject);
                //Debug.Log(_countOvered + " " + _countStep + " " + _count + " | " + _tempTransParent.GetChild(i).gameObject.name);

                DestroyImmediate(GameObject.Find("-=HBD=-"));
            }
            _countOvered = _countStep;

            GC.Collect();
            Resources.UnloadUnusedAssets();

            EditorApplication.RepaintHierarchyWindow();
        }

        if (GUILayout.Button("TestMem", GUILayout.Height(20)))
        {
            // test1
            //SerializerVector3 sv3 = new SerializerVector3(1,2,3);
            //MemoryStream stream = new MemoryStream();
            //// 构造二进制序列化格式器
            //BinaryFormatter binaryFormatter = new BinaryFormatter();
            //// 告诉序列化器将对象序列化到一个流中
            //binaryFormatter.Serialize(stream, sv3);
            //stream.Position = 0;
            //BinaryFormatter bf = new BinaryFormatter();
            //SerializerVector3 svout = (SerializerVector3)bf.Deserialize(stream);
            //Debug.Log(svout.x + " | " + svout.y + " | " + svout.z);

            // test2
            ////STUBuildInfo stu = new STUBuildInfo(new SerializerVector3(1,2,3), new Quaternion(4,5,6,7), new SerializerVector3(8,9,0));
            //STUBuildInfo stu = new STUBuildInfo(new SerializerVector3(1, 2, 3), new SerializerQuaternion(4, 5, 6, 7), new SerializerVector3(8, 9, 0));
            //MemoryStream ms = Common.Serialize(stu);
            //ms.Position = 0;
            //STUBuildInfo outStu = (STUBuildInfo)Common.Deserialize(ms);
            //ms.Close();
            ////Debug.Log(outStu._pos + " | " + outStu._rotate + " | " + outStu._scare);
            //Debug.Log(outStu._pos.x + " | " + outStu._pos.y + " | " + outStu._pos.z );
            //Debug.Log(outStu._rotate.x + " | " + outStu._rotate.y + " | " + outStu._rotate.z + " | " + outStu._rotate.w);
            //Debug.Log(outStu._scare.x + " | " + outStu._scare.y + " | " + outStu._scare.z);

            // test3
            STUBuildInfo stu1 = new STUBuildInfo(new SerializerVector3(1, 2, 3), new SerializerQuaternion(4, 5, 6, 7), new SerializerVector3(8, 9, 0));

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("stu1", stu1);


            MemoryStream ms2 = Common.Serialize(dic);
            ms2.Position = 0;

            Dictionary<string, object> outDic = (Dictionary<string, object>)Common.Deserialize(ms2);
            Debug.Log(outDic.Count);

            foreach (string str in outDic.Keys)
            {
                STUBuildInfo outStu = (STUBuildInfo)outDic[str];
                Debug.Log(outStu._pos.x + " | " + outStu._pos.y + " | " + outStu._pos.z);
                Debug.Log(outStu._rotate.x + " | " + outStu._rotate.y + " | " + outStu._rotate.z + " | " + outStu._rotate.w);
                Debug.Log(outStu._scare.x + " | " + outStu._scare.y + " | " + outStu._scare.z);
            }
        }
        if (GUILayout.Button("AddSelectedSceneInfo", GUILayout.Height(20)))
        {
            // 得到选中物体的名称
            if (Selection.objects.Length != 1)
            {
                Debug.LogError("Please select one Object");
                return;
            }
            string selectedName = Selection.objects[0].name;
            string path = Application.streamingAssetsPath + "/" + selectedName;
            // 查看选中物体名称的文件是否存在 如果不存在创建文件
            // 如果存在 
            if (File.Exists(path))
            {
                MemoryStream ms = Common.ReadFile(path);
                if (ms == null)
                {
                    Debug.LogError("File format Error! " + path);
                }

                // 把选择的物体子物体信息合并 存储到这个文本中
                Dictionary<string, object> outDic = (Dictionary<string, object>)Common.Deserialize(ms);
                outDic.Merge(GetSelectSanObjsInfo());

                Common.SaveFile(path, Common.Serialize(outDic));
                ms.Close();
            }
            // 文件不存在
            else
            {
                // 选中物体信息存储到文件
                Common.SaveFile(path, Common.Serialize(GetSelectSanObjsInfo()));
            }
        }
        if (GUILayout.Button("ClearSelectSceneInfo", GUILayout.Height(20)))
        {
            // 得到选中物体的名称
            if (Selection.objects.Length == 1)
            {
                string selectedName = Selection.objects[0].name;
                // 查看选中物体名称的文件是否存在 如果存在删除文件
                DirectoryInfo mydir = new DirectoryInfo(Application.streamingAssetsPath + "/" + selectedName);
                if (mydir.Exists)
                {
                    mydir.Delete();
                }
            }
        }
        // 检查文件信息
        if (GUILayout.Button("CheckTextInfo", GUILayout.Height(20)))
        {
            if (Selection.objects.Length != 1)
            {
                Debug.LogError("Please select Assets/StreamingAssets/xxx one File");
                return;
            }
            string selectedName = Selection.objects[0].name;
            string path = Application.streamingAssetsPath + "/" + selectedName;

            if (!File.Exists(path))
            {
                Debug.LogError("have no this file " + path);
                return;
            }

            // 打开文本文件
            MemoryStream ms = Common.ReadFile(path);
            Dictionary<string, object> outDic = (Dictionary<string, object>)Common.Deserialize(ms);
            // 解析文件 输出文本信息
            foreach (string str in outDic.Keys)
            {
                STUBuildInfo outStu = (STUBuildInfo)outDic[str];

                Debug.Log(str);
                Debug.Log("pos :" + outStu._pos.x + " | " + outStu._pos.y + " | " + outStu._pos.z);
                Debug.Log("roa :" + outStu._rotate.x + " | " + outStu._rotate.y + " | " + outStu._rotate.z + " | " + outStu._rotate.w);
                Debug.Log("sca :" + outStu._scare.x + " | " + outStu._scare.y + " | " + outStu._scare.z);
            }
            ms.Close();
        }

        //// 检查文件信息
        //if (GUILayout.Button("CreateCreater", GUILayout.Height(20)))
        //{
        //    // 场景中创建一个树木创建者
        //    Instantiate(Resources.Load("Creater"));
        //    creater.AddComponent<TreeCreater>();
        //}
    }
    
    



    // save selected Object info to Dictionary
    private Dictionary<string, object> GetSelectSanObjsInfo()
    {
        if (Selection.objects.Length != 1)
        {
            Debug.LogError("Please select one Object");
            return null;
        }
        Dictionary<string, object> re = new Dictionary<string, object>();

        Transform selectTrans = ((GameObject)(Selection.objects[0])).transform;

        foreach (Transform child in selectTrans)
        {
            Vector3 pos = child.position;
            Quaternion qua = child.rotation;
            Vector3 scare = child.localScale;
            STUBuildInfo stu = new STUBuildInfo(new SerializerVector3(pos.x, pos.y, pos.z), 
                new SerializerQuaternion(qua.x, qua.y, qua.z, qua.w), 
                new SerializerVector3(scare.x, scare.y, scare.z));
            re.Add(child.name, stu);
        }
        return re;
    }

}



[CustomEditor(typeof(TreeCreater))]
public class TreeCreaterEdit : Editor
{
    private TreeCreater _myScript;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        _myScript = (TreeCreater)target;


    }

    public void OnEnable()
    {
        //SceneView.onSceneGUIDelegate += TreeUpdate;
        ActiveEditorTracker.sharedTracker.isLocked = true;
    }

    //// Update is called once per frame
    //void TreeUpdate(SceneView sceneview)
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Event e = Event.current;

    //        Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
    //        Vector3 mousePos = r.origin;

    //        Debug.Log(mousePos);
    //    }
    //}
    private Vector3 _from;
    private Vector3 _to;
    void OnSceneGUI()
    {
        //HandleUtility.AddDefaultControl(controlID);

        // Event.current.type == EventType.MouseDrag || 

        if ((Event.current.type == EventType.MouseDown) && !Event.current.alt)
        {
            _from = SceneView.lastActiveSceneView.camera.ViewportToWorldPoint(Event.current.mousePosition);

            Vector3 pointPos = Event.current.mousePosition;
            Ray r = SceneView.lastActiveSceneView.camera.ScreenPointToRay(new Vector3(pointPos.x, Screen.height - pointPos.y - 40, pointPos.z));

            //GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //obj.transform.position = _from;
            //obj.name = "from";

            RaycastHit hitInfo;
            if (Physics.Raycast(r, out hitInfo, 1000))
            {
                _to = hitInfo.point;
                //GameObject obj1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //obj1.transform.position = _to;
                //obj1.name = "to";
                _myScript.CreateTree(hitInfo.point);
                //Debug.DrawLine(_from, _to);
            }
        }
        //Debug.DrawLine(_from, _to);
    }
}