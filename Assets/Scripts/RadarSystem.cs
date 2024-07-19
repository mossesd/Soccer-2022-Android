/// <summary>
/// This is Radar System. using to detection an objects and showing on minimap by Tags[]
/// </summary>

using UnityEngine;
using System.Collections;

public enum Alignment { None,LeftTop, RightTop, LeftBot, RightBot ,MiddleTop ,MiddleBot}

public class RadarSystem : MonoBehaviour {

	private Vector2 inposition;
	public float Size = 400; // size of minimap
	public float Distance = 100;// maximum distance of objects
	public float Alpha = 0.5f;
	public Texture2D[] Navtexture;// textutes list
	public string[] EnemyTag;// object tags list
	public Texture2D NavCompass;// compass texture
	public Texture2D NavBG;// background texture
	public Vector2 PositionOffset = new Vector2(0,0);// minimap position offset
	public float Scale = 1;// mini map scale ( Scale < 1 = zoom in , Scale > 1 = zoom out)
	public Alignment PositionAlignment = Alignment.None;// position alignment
	public bool MapRotation;
	public GameObject Player;
	public bool Show = true;
	public Color ColorMult = Color.white;
	
	void Start ()
	{
		Size = Size * Screen.height / 640f;
	}
	

	void Update () {
		if(!Player){
			Player = this.gameObject;
		}
		
		if(Scale<=0){
			Scale = 1;
		}
	
		switch(PositionAlignment){
		case Alignment.None:
			inposition = PositionOffset;
		break;
		case Alignment.LeftTop:
			inposition = Vector2.zero + PositionOffset;
		break;
		case Alignment.RightTop:
			inposition = new Vector2(Screen.width - Size-75,0) + PositionOffset;
		break;
		case Alignment.LeftBot:
			inposition = new Vector2(0,Screen.height - Size) + PositionOffset;
		break;
		case Alignment.RightBot:
			inposition = new Vector2(Screen.width - Size,Screen.height - Size) + PositionOffset;
		break;
		case Alignment.MiddleTop:
			inposition = new Vector2((Screen.width/2) - (Size/2),Size) + PositionOffset;
		break;
		case Alignment.MiddleBot:
			inposition = new Vector2((Screen.width/2) - (Size/2),Screen.height - Size*0.75f) + PositionOffset;
		break;
		}
		
	}
	
	Vector2 ConvertToNavPosition(Vector3 pos)
	{
		if(GameManager.SharedObject().IsFirstHalf == false)
		{
			pos.x *= -1f;
			pos.z *= -1f;
		}

		Vector2 res = Vector2.zero;
		if(Player)
		{
			res.x = inposition.x + ((pos.x+55)/110 * Size);
			res.y = inposition.y + ((-pos.z+37)/74 * (Size*0.684f));
		}
		return res;
	}


	void DrawNav(GameObject[] enemylists,Texture2D navtexture){
		if(Player)
		{
		for(int i=0;i<enemylists.Length;i++)
		{
			//if(Vector3.Distance(Player.transform.position,enemylists[i].transform.position)<= (Distance * Scale))
				{
				Vector2 pos = ConvertToNavPosition(enemylists[i].transform.position);

				//if(Vector2.Distance(pos,(inposition+ new Vector2(Size/2f,Size/2f))) + (navtexture.width/2) < (Size/2f))
					{
					float navscale = Scale;
					if(navscale<1){
						navscale = 1;
					}
						GUI.DrawTexture(new Rect(pos.x - (10*Screen.height/640)/2,pos.y - (10*Screen.height/640)/2,10*Screen.height/640,10*Screen.height/640),navtexture);
				}
			}
		}
		}
	}

	float[] list;

	void OnGUI()
	{
		if(GameManager.SharedObject().IsGameReady == false || PauseController.isPaused)
			return;

		if(NavBG)
			GUI.DrawTexture(new Rect(inposition.x,inposition.y,Size,Size*0.684f),NavBG);

		if(!Show)
			return;
		
		GUI.color = new Color(ColorMult.r,ColorMult.g,ColorMult.b,Alpha);
		if(MapRotation){
			GUIUtility.RotateAroundPivot (-(this.transform.eulerAngles.y), inposition + new Vector2(Size/2f, Size/2f)); 
		}
	
		for(int i=0;i<EnemyTag.Length;i++){
			DrawNav(GameObject.FindGameObjectsWithTag(EnemyTag[i]),Navtexture[i]);
		}

		GUIUtility.RotateAroundPivot ((this.transform.eulerAngles.y), inposition + new Vector2(Size/2f, Size/2f)); 
		if(NavCompass)
		GUI.DrawTexture(new Rect(inposition.x + (Size/2f)-(NavCompass.width/2f),inposition.y + (Size/2f) - (NavCompass.height/2f),NavCompass.width,NavCompass.height),NavCompass);

	}
}



