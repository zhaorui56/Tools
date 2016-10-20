using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class MySceneManager// : MonoBehaviour
{
    // Tree class
    public class TreeNode<T> 
    {
        public T Data { get; set; }
        public TreeNode<T> Parent { get; set; }
        public ICollection<TreeNode<T>> Children { get; set; }

        public TreeNode(T data)
        {
            this.Data = data;
            this.Children = new LinkedList<TreeNode<T>>();
        }

        public TreeNode<T> AddChild(T child)
        {
            TreeNode<T> childNode = new TreeNode<T>(child) { Parent = this };
            this.Children.Add(childNode);
            return childNode;
        }

        public TreeNode<T> GetChild(T child)
        {
            foreach(TreeNode<T> node in Children)
            {
                // 这里就不重载 == 了
                if (node.Data.ToString() == child.ToString())
                    return node;
            }
            return null;
        }

        public TreeNode<T> GetParent()
        {
            return Parent;
        }

        public void DEBUG()
        {
            Debug.Log("INFO: current is " + Data.ToString() + "  parent is " + Parent.Data.ToString() + "  child count is " + Children.Count);

            foreach (TreeNode<T> node in Children)
            {
                Debug.Log(node.Data.ToString());
            }
        }
    }


    private static MySceneManager _this;

    private Dictionary<string, int> _sceneNameDic = new Dictionary<string, int>();
    private TreeNode<string> _node;

    private TreeNode<string> _treeRoot;

    private MySceneManager()
    {
        Debug.Log("MySceneManager AWAKE");
        _treeRoot = new TreeNode<string>("OP_00");
		// example
					TreeNode<string> child = _treeRoot.AddChild("A0_ZhuYe");
					TreeNode<string> child01 = child.AddChild("301_ShiWai");
					TreeNode<string> child02 = child.AddChild("k6_1_ShiWai");
					TreeNode<string> child03 = child.AddChild("K5-JIA");
					TreeNode<string> child04 = child.AddChild("313-k7_ShiWai");

					child.AddChild("3D_ZhuChengXu");
					child.AddChild("ZongZhan_GongKuangChaKan");

					child.AddChild("大区域街景");

					TreeNode<string> child1 = child.AddChild("A6_GongKuangChaKan");
					TreeNode<string> child2 = child.AddChild("A2_场景巡游");
					TreeNode<string> child3 = child.AddChild("A2_SheBeiDingWEI");
					TreeNode<string> child4 = child.AddChild("A1_JiaoChengYanShi");
					TreeNode<string> child5 = child.AddChild("A4_RenWuBianJi"); //   A5_MoNiYanLian_new
					TreeNode<string> child6 = child.AddChild("A3_RenYuanDingWei");

					child1.AddChild("301_GongKuangChaKan");
					child1.AddChild("301_ShiWai");

					child2.AddChild("301_GongKuangChaKan");
					child2.AddChild("301_ShiWai");
					
					TreeNode<string> child41 = child4.AddChild("教学演示列表页");
					TreeNode<string> child411 = child41.AddChild("培训教学");

					child5.AddChild("模拟演练-K6");

        _node = _treeRoot;
    }

    public static MySceneManager GetInstance()
    {
        if (_this == null)
            _this = new MySceneManager();
        return _this;
    }

    public void ChangeScene(string sceneName)
    {
        Debug.Log("ChangeScene " + sceneName);

        // 用于返回主页
        if("A0_ZhuYe" == sceneName)
        {
            _node = _treeRoot.GetChild("A0_ZhuYe");
            SceneManager.LoadScene(sceneName);
            return;
        }
        GC.Collect();

        TreeNode<string> node = _node.GetChild(sceneName);
        if (node != null)
        {
            _node = node;
            //Application.LoadLevel(sceneName);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            if (_node.Parent == null)
            {
                SceneManager.LoadScene(sceneName);

                Debug.Log("Is in edit mode???");
                return;
            }
            // 如果不在子场景中  平级场景中搜索
            TreeNode<string> nodeother = _node.Parent.GetChild(sceneName);
            if (nodeother != null)
            {
                _node = nodeother;
                //Application.LoadLevel(sceneName);
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError(sceneName + " Is not in this tree node!!!");

                _node.DEBUG();
            }
        }
    }

    public void LoadBackScene()
    {
        if(_node == null)
        {
            Debug.Log("_node is null"); //  + _node.GetParent()
            return;
        }

        _node = _node.GetParent();
        if (_node != null)
            SceneManager.LoadScene(_node.Data);
        else
            Debug.LogError("_node is null");
    }
}
