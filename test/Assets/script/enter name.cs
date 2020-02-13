
using UnityEngine;
using System.Collections;

public class wenzi : MonoBehaviour
{

	//主摄像机对象
	private Camera camera;
	//NPC名称
	public string name;

	//主角对象
	GameObject hero;
	//NPC模型高度
	float npcHeight;

	//默认NPC血值
	private int HP = 100;

	void Start()
	{
		//得到主角对象
		hero = this.gameObject;
		//得到摄像机对象
		camera = Camera.main;

	
	}

	void OnGUI()
	{
		//得到NPC头顶在3D世界中的坐标
		//默认NPC坐标点在脚底下，所以这里加上npcHeight它模型的高度即可
		Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + npcHeight, transform.position.z);
		//根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
		Vector2 position = camera.WorldToScreenPoint(worldPosition);
		//得到真实NPC头顶的2D坐标
		position = new Vector2(position.x, Screen.height - position.y);

		//计算NPC名称的宽高
		Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(this.name));
		//根据tag设置显示颜色
		if (this.tag == "green")
			GUI.color = Color.green;
		else if (this.tag == "red")
			GUI.color = Color.red;
		GUI.skin.label.fontSize = 18;//字体大小
									 //绘制NPC名称
		GUI.Label(new Rect(position.x - (nameSize.x / 2), position.y - nameSize.y, nameSize.x, nameSize.y), name);



	}
}
