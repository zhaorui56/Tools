using UnityEngine;
using System.Collections;

public class TreeCreater : MonoBehaviour {

    public GameObject[] _selectedTreeList;

    [SerializeField]
    private int _selectedIndex = 0;

    [SerializeField]
    private GameObject _selectedTree;


    //void Update()
    //{
    //    if (_selectedIndex < 0)
    //        _selectedIndex = 0;
    //    if (_selectedIndex >= _selectedTreeList.Length)
    //        _selectedIndex = _selectedTreeList.Length - 1;

    //    _selectedTree = _selectedTreeList[_selectedIndex];
    //}

    // Use this for initialization
    void Start () {
	}
    public void CreateTree(Vector3 pos)
    {
        if (_selectedIndex < 0)
            _selectedIndex = 0;
        if (_selectedIndex >= _selectedTreeList.Length)
            _selectedIndex = _selectedTreeList.Length - 1;

        _selectedTree = _selectedTreeList[_selectedIndex];

        if (_selectedTree != null)
        {
            GameObject tree = Instantiate(_selectedTree);
            tree.transform.position = pos;

            float scare = Random.Range(0.9f, 1.1f);
            // 大小浮动-10% - 10%
            tree.transform.localScale = new Vector3(scare, scare, scare);

            int rotate = Random.Range(0, 360);
            // 随机旋转
            tree.transform.localRotation = Quaternion.Euler(0, rotate, 0);

            Debug.Log("OK");
        }
        else
        {
            Debug.LogError("tree is null");
        }
    }
}
