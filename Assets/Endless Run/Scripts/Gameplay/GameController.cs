/// <summary>
/// Game controller.
/// This script use for control game loading and spawn character when load complete
/// </summary>

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GameController : MonoBehaviour {
	
	public PatternSystem patSysm; //pattern system
	public CameraFollow cameraFol;	//camera
	public float speedAddEveryDistance = 300;
	public float speedAdd = 0.5f;
	public float speedMax = 20;
	public GameObject playerPref;
	public Vector3 posStart;
	public bool previewProgressBar;
	public bool useShowPercent;
	public Texture2D textureProgressBar_Frame, textureProgressBar_Color,textureProgressBar_BG;
	public Rect rect_progressbar, rect_percent_text;
	
	private bool createGUI;
	private float percentCount;
	private float distanceCheck;
	[HideInInspector]
	public int countAddSpeed;
	private CalOnGUI calOnGUI;
	
	public static GameController instace;
	
	void Start(){
		if(Application.isPlaying == true){
			instace = this;
			calOnGUI = new CalOnGUI();
			StartCoroutine(WaitLoading());
		}
	}
	
	void OnGUI(){
		if(Application.isPlaying == true){
			if(patSysm.loadingComplete == false){
				if(textureProgressBar_BG != null)
				GUI.DrawTexture(new Rect(calOnGUI.SetGUI_Left(rect_progressbar.x, rect_progressbar.y, rect_progressbar.width, rect_progressbar.height)), textureProgressBar_BG);
				
				percentCount = Mathf.Lerp(percentCount, patSysm.loadingPercent, 5 * Time.deltaTime);
				GUI.BeginGroup(new Rect(calOnGUI.SetGUI_Left(rect_progressbar.x, rect_progressbar.y, rect_progressbar.width*(percentCount/100), rect_progressbar.height)));
				if(textureProgressBar_Color == null){
					GUI.Box(new Rect(0,0, calOnGUI.SetGUI_Left(rect_progressbar.x, rect_progressbar.y, rect_progressbar.width, rect_progressbar.height).width, calOnGUI.SetGUI(rect_progressbar.x, rect_progressbar.y, rect_progressbar.width, rect_progressbar.height).height),"");
					
				}else{
					GUI.DrawTexture(new Rect(0,0, calOnGUI.SetGUI_Left(rect_progressbar.x, rect_progressbar.y, rect_progressbar.width, rect_progressbar.height).width, calOnGUI.SetGUI(rect_progressbar.x, rect_progressbar.y, rect_progressbar.width, rect_progressbar.height).height),textureProgressBar_Color);
					
				}
				GUI.EndGroup();
				if(textureProgressBar_Frame != null){
					
					GUI.DrawTexture(new Rect(calOnGUI.SetGUI_Left(rect_progressbar.x, rect_progressbar.y, rect_progressbar.width, rect_progressbar.height)), textureProgressBar_Frame);	
						
				}
				if(useShowPercent)
				GUI.Label(new Rect(calOnGUI.SetGUI(rect_percent_text.x, rect_percent_text.y, rect_percent_text.width , rect_percent_text.height)),percentCount.ToString("0")+"%");
			}
		}else{
			/*
			if(previewProgressBar == true){
				if(calOnGUI == null && createGUI == false){
					calOnGUI = new CalOnGUI();
					createGUI = true;
				}
				if(textureProgressBar_BG != null)
				GUI.DrawTexture(new Rect(calOnGUI.SetGUI_Left(rect_progressbar.x, rect_progressbar.y, rect_progressbar.width, rect_progressbar.height)), textureProgressBar_BG);
				
				GUI.BeginGroup(new Rect(calOnGUI.SetGUI_Left(rect_progressbar.x, rect_progressbar.y, rect_progressbar.width , rect_progressbar.height)));
				if(textureProgressBar_Color == null){
					GUI.Box(new Rect(0,0, calOnGUI.SetGUI_Left(rect_progressbar.x, rect_progressbar.y, rect_progressbar.width, rect_progressbar.height).width, calOnGUI.SetGUI(rect_progressbar.x, rect_progressbar.y, rect_progressbar.width, rect_progressbar.height).height),"");
			
				}else{
					GUI.DrawTexture(new Rect(0,0, calOnGUI.SetGUI_Left(rect_progressbar.x, rect_progressbar.y, rect_progressbar.width, rect_progressbar.height).width, calOnGUI.SetGUI(rect_progressbar.x, rect_progressbar.y, rect_progressbar.width, rect_progressbar.height).height),textureProgressBar_Color);
					
				}
				GUI.EndGroup();
				if(textureProgressBar_Frame != null){
					
					GUI.DrawTexture(new Rect(calOnGUI.SetGUI_Left(rect_progressbar.x, rect_progressbar.y, rect_progressbar.width, rect_progressbar.height)), textureProgressBar_Frame);	
					
				}
				if(useShowPercent)
				GUI.Label(new Rect(calOnGUI.SetGUI(rect_percent_text.x, rect_percent_text.y, rect_percent_text.width , rect_percent_text.height)),"100%");
			}else{
				createGUI = false;	
			}*/
		}
	}
	
	//Loading method
	IEnumerator WaitLoading(){
		Debug.Log("Text: here");
		while(patSysm.loadingComplete == false){
			Debug.Log("Text: 222");
			yield return null;	
		}
		Debug.Log("Text: 1111");
		StartCoroutine(InitPlayer());
	}
	
	//Spawn player method
	IEnumerator InitPlayer(){
		GameObject go = (GameObject)Instantiate(playerPref, posStart, Quaternion.identity);
		cameraFol.target = go.transform;
		yield return 0;
		StartCoroutine(UpdatePerDistance());
	}
	
	//update distance score
	IEnumerator UpdatePerDistance(){
		while(true){
			if(PatternSystem.instance.loadingComplete){
				if(GameAttribute.gameAttribute.pause == false
					&& GameAttribute.gameAttribute.isPlaying == true
					&& GameAttribute.gameAttribute.life > 0){
					if(Controller.instace.transform.position.z > 0){
						GameAttribute.gameAttribute.distance += GameAttribute.gameAttribute.speed * Time.deltaTime;
						distanceCheck += GameAttribute.gameAttribute.speed * Time.deltaTime;
						if(distanceCheck >= speedAddEveryDistance){
							GameAttribute.gameAttribute.speed += speedAdd;
							if(GameAttribute.gameAttribute.speed >= speedMax){
								GameAttribute.gameAttribute.speed = speedMax;	
							}
							countAddSpeed++;
							distanceCheck = 0;
						}
					}
				}
			}
			yield return 0;
		}
	}
	
	//reset game
	public IEnumerator ResetGame(){
		GameAttribute.gameAttribute.isPlaying = false;
		distanceCheck = 0;
		countAddSpeed = 0;
		yield return 0;	
	}
	
}
