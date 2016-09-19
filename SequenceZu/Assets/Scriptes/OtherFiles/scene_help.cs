using UnityEngine;
using System.Collections;

[System.Serializable]
public class settingflag {
	public int m_flag=0;
	public enum flagval{
		releasekanikama	= 0x00000001,
		enablekanikama	= 0x00000002,
		stageclear		= 0x00000004,
		allclear		= 0x00000008,
		firstplay		= 0x00000010,
		mailmark		= 0x00000020,
	}

	public void let(int val){
		m_flag=val;
	}

	public void set(flagval flag){
		m_flag|=(int)flag;
	}

	public void unset(flagval flag){
		m_flag&=~(int)flag;
	}

	public bool check(flagval flag){
		return ((int)m_flag&(int)flag)!=0;
	}
	
}


[System.Serializable]
public class gamesetting {
	public int characterid=0;
	public float bgmvolume=1f;
	public float sevolume=1f;
//	public int enablekanikama=0;
	public long nextlogintime=0;
	public int level=0;
	public settingflag flag;
	public int startinfo=0;
	public int gameovercount=0;
	public gamesetting(){
		flag=new settingflag();
	}

	public void getstartinfo(ref int lastblock, ref int lastenv){
		lastblock=startinfo&0xffff;
		lastenv=(startinfo>>16)&0xffff;
	}

	public void setstartinfo(int lastblock, int lastenv){
		startinfo=lastblock|(lastenv<<16);
	}
}


public enum helpinit{ 
	scene,
	max
}

public enum helppageid{ 
	mainmenu,
	sound,
	tutorial,
	max
}

/*
public class scene_help : MonoBehaviour {

	enum seq {
		init,
		play
	}

	Transform pagetop;
	helppageid _currentPage;

	GameObject _bgmobj;
	GameObject _seobj;
	GameObject _backbutton;

	seq _seq; 
	// Use this for initialization
	void Start () {
		scenemanager.sceneSet(sceneid.help);
		scenemanager.SetFadeSpeed(0.5f);
		scenemanager.setInitFlag((int)helpinit.max);

		pagetop = GameObject.Find("pages").transform;
		Transform sound=pagetop.FindChild("sound");
		_bgmobj=sound.FindChild("bgmvolume").gameObject;
		_seobj=sound.FindChild("sevolume").gameObject;
		soundmanager.bgmPlay(bgmid.bgm02);
		_backbutton = GameObject.Find("backbutton");
		_seq=seq.init;
	}
	
	// Update is called once per frame
	void Update () {
		switch(_seq){
		case seq.init:
			if(scenemanager.checkInitFlags(1<<(int)helpinit.scene)){
				if(scenemanager.readrecord()!=null){
					_bgmobj.SendMessage("set",scenemanager.readrecord().m_gamesetting.bgmvolume);
					_seobj.SendMessage("set",scenemanager.readrecord().m_gamesetting.sevolume);
					scenemanager.unsetInitFlag((int)helpinit.scene);
				}
			}
			if(scenemanager.isInitComplete()){
				soundmanager.bgmPlay(bgmid.bgm01);
				pageSet((int)helppageid.mainmenu);
				_seq=seq.play;
			}
			break;
		case seq.play:
			break;
		}
	}

	public void onClickOriginalpage(){
#if UNITY_IOS
		Application.OpenURL("http://www.amazon.co.jp/dp/4434176978?tag=alphapolcojp-22");
#else
		Application.OpenURL("http://www.alphapolis.co.jp/book/detail/1043038/905/");
#endif
		soundmanager.systemsePlay(systemseid.ok);
	}

	public void onClickCredit(){
		soundmanager.systemsePlay(systemseid.ok);
	}

	public void onClickLawinfo(){
		Application.OpenURL("http://www.alphapolis.co.jp/pages/tokushoho/");
		soundmanager.systemsePlay(systemseid.ok);
	}

	public void onClickBack(){
		pagechange(0);
		soundmanager.systemsePlay(systemseid.ok);
	}

	public void pagechange(int page) {
		pageSet(page);
		soundmanager.systemsePlay(systemseid.ok);
	}

	public void pageSet(int page) {
		_currentPage = (helppageid)page;
		for(int i=0;i<(int)helppageid.max;i++){
			helppageid tmp=(helppageid)i;
			pagetop.GetChild(i).gameObject.SetActive(tmp==(helppageid)page);
		}
		_backbutton.SetActive(_currentPage==helppageid.sound || _currentPage==helppageid.tutorial);
	}

	public void setBgmVolume(float vol) {
		float currentvol=scenemanager.readrecord().m_gamesetting.bgmvolume;
		currentvol+=vol;
		if(currentvol<0){
			currentvol=0;
		}else if(currentvol>1f){
			currentvol=1f;
		}
		if(currentvol!=scenemanager.readrecord().m_gamesetting.bgmvolume){
			scenemanager.writerecord().m_gamesetting.bgmvolume=currentvol;
			_bgmobj.SendMessage("set",currentvol);
			soundmanager.setVolume(soundtype.bgm,currentvol,true);
			soundmanager.systemsePlay(systemseid.ok);
		}
	}

	public void setSeVolume(float vol) {
		float currentvol=scenemanager.readrecord().m_gamesetting.sevolume;
		currentvol+=vol;
		if(currentvol<0){
			currentvol=0;
		}else if(currentvol>1f){
			currentvol=1f;
		}
		if(currentvol!=scenemanager.readrecord().m_gamesetting.sevolume){
			scenemanager.writerecord().m_gamesetting.sevolume=currentvol;
			_seobj.SendMessage("set",currentvol);
			soundmanager.setVolume(soundtype.se,currentvol,true);
			soundmanager.setVolume(soundtype.voice,currentvol,true);
			soundmanager.systemsePlay(systemseid.ok);
		}
	}

}
*/