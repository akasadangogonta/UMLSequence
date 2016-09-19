using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum letterstatusid {
	notown,
	receive,
	readyet
}

[System.Serializable]
public class letterstatus {
	public const int lettermax=120;
	public int[] status;

	public letterstatus(){
		init();
	}

	public void init(){
		status=new int[lettermax];
		for(int i=0;i<lettermax;i++){
			status[i]=0;
		}
	}

	public letterstatusid get(int no){
		return (letterstatusid)status[no];
	}

	public void set(int no, letterstatusid lid){
		status[no]=(int)lid;
	}

	public bool checkNewLetter(){
		for(int i=0;i<status.Length;i++){
			if(status[i]==(int)letterstatusid.receive){
				return true;
			}
		}
		return false;
	}

	public int add(int no=-1){
		if(no<0){
			for(int i=0;i<status.Length;i++){
				if(status[i]==(int)letterstatusid.notown){
					status[i]=(int)letterstatusid.receive;
					no=i;
					break;
				}
			}
		}else{
			if(no>=0 && no<status.Length){
				status[no]=(int)letterstatusid.receive;
			}
		}
		return no;
	}

	public int getOwnnum(){
		int count=0;
		for(int i=0;i<status.Length;i++){
			if(status[i]>0)count++;
		}
		return count;
	}
}

public class letterdata {
	public int index;
	public string text;
}

[System.Serializable]
public class letterdictionary {
	public List<letterdata> data;
}

/*
public class scene_letter : MonoBehaviour {
	public enum letterinit {
		sceneinit,
		max
	}

	enum seqid {
		init,
		run
	}

	const float ROTANGLE=20f;
	
	letterdictionary _dic;

	public static Vector3 m_rotate=Vector3.zero;

	GameObject[] _objlist;

	static Transform _letterRoot;
	static Transform _contentsRoot;

	seqid _seq;
	static GameObject _body=null;

	Sprite[] _numbersprites;
	UnityEngine.UI.Text _text;


	// Use this for initialization
	void Start () {
		scenemanager.sceneSet(sceneid.letter);
		scenemanager.SetFadeSpeed(0.2f);
		scenemanager.setExecFunc(scenemanager.funcid.term, term);

		_dic = utility.XmlUtil.Deserialize<letterdictionary>("letter");

		_objlist = new GameObject[_dic.data.Count];

		iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", ROTANGLE*2, "time", 0.8f, "looptype",iTween.LoopType.pingPong,"easetype",iTween.EaseType.linear,"onupdate", "updaterotate"));
		scenemanager.setInitFlag((int)letterinit.max);

		Transform canvastrs = transform.FindChild ("Canvas");
		_letterRoot = canvastrs.FindChild ("letterobjroot") ;
		_contentsRoot = GameObject.Find("contentroot").transform;

		_seq = seqid.init;
		_body = gameObject;
		soundmanager.bgmPlay(bgmid.bgm02);
		_numbersprites = Resources.LoadAll<Sprite>("Sprites/letter/letternum");
	}
	
	// Update is called once per frame
	void Update () {
		switch(_seq){
		case seqid.init:
			if(scenemanager.readrecord()!=null){
				letterobj.setupparam param;
				Object 	prefab = (Object)Resources.Load("Prefabs/letter/letterobj");

				for(int i=0;i<_dic.data.Count;i++){
					letterstatusid status = scenemanager.readrecord().m_letter.get(i);
					param.idx=i;
					param.status = status;
					param.letterno = _dic.data[i].index;
					param.numbersprites = _numbersprites;
					_objlist[i]=Instantiate(prefab) as GameObject;
					_objlist[i].transform.SetParent(getLetterParent(status!=letterstatusid.notown));
					_objlist[i].SendMessage("init",param);
					_objlist[i].name=i.ToString();
				}

				_letterRoot.gameObject.SetActive(false);

				scenemanager.unsetInitFlag((int)letterinit.sceneinit);
				scenemanager.setRecordButtonMark(false);
			}
			if(scenemanager.isInitComplete()){
				_seq = seqid.run;
			}
			break;
		case seqid.run:
			break;
		}
	}

	void term(){
	}

	void updaterotate(float val){
		m_rotate.z = val-ROTANGLE;
	}

	public static Transform getLetterParent(bool sw){
		return sw?_contentsRoot:_letterRoot;
	}

	public void onAddButton(){
		int idx = scenemanager.writerecord().m_letter.add();
		if(idx>=0){
			_objlist[idx].transform.SetParent(getLetterParent(true));
			_objlist[idx].SetActive(true);
			_objlist[idx].SendMessage("changeState",letterstatusid.receive);
		}
	}

	public void setLetterText(int idx){
		//_text.text=_dic.data[idx].text;
		letterwindow.disp(_dic.data[idx].text);
	}

	public static void onLetterClick(int idx){
		_body.SendMessage("setLetterText",idx);
	}
}
*/