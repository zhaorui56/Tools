# PrefabLightmapData
PrefabLightmapData： 动态加载烘焙的脚本，稍微经过了修改

how to use 使用方式 ： 
	1 build one scene,pull some mesh in it
	2 pull this script onto gameobjct as component,set static so it will be baked when Baking
	3 bake
	4 in unity menu  press   "Assets/Check RenderInfo" it will check all render component and set it in Inspector board
	5 in unity menu  press   "Assets/Bake Prefab Lightmaps" it will set Inspector board other info , and save it to Resource file 
	6 pull it to project window,it will saved as a prefab
	7 now ,you can delete baked file in  Hierarchy window, baking your scene or not(set use other LightingData)
	8 write load GameObject script (runtime)
	9 OK
	
	