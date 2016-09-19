using UnityEngine;
using System.Collections;

public enum lithographinit{
	scene,
	max
}

public enum litho_pageid{
	hana,
	kawa,
	taro,
	taka,
	mame,
	max
}

public enum litho_effect{
	off,
	Kir,
	All,
	Bar
}

public enum litho_talkid{
	greet,
	greetcomplete,
	confirm,
	complete0,
	complete1,
	max
}

public enum lithoseid {
	none,
	put,
	eff,
	eff2,
}

[System.Serializable]
public class lithhographdata{
	public int partsnum=0;
	public int completenum=0;
}
/*
public class scene_lithograph : MonoBehaviour {
	public struct lithodata {
		public Sprite basesprite;
		public Sprite namesprite;
	}
	const int LITHOGRAPH_PARTSMAX=16;

	enum seq {
		init,
		fadewait,
		play,
		exchangestart,
		exchange,
		exchangeend,
	}

	enum animseq {
		start,
		reducewait,
		effect,
		effectwait,
		finish,
		finishwait,
		finish2,
		finish2wait,
		end
	}

	int _pagemax;
	int _currentpage;
	GameObject _pageButtonObj;
	int _requestnum;


	lithodata[] _lithodata;

	string[] _path={
		"Sekiban_2D_1_Fairy",
		"Sekiban_2D_2_Kawazu",
		"Sekiban_2D_3_Taro",
		"Sekiban_2D_4_Knight",
		"Sekiban_2D_5_Zokucho"
	};

	int[] _talkid={
		(int)litho_talkid.complete0,
		(int)litho_talkid.complete1,
		-1,
		-1,
		-1
	};


	GameObject _kawazu;
	GameObject lithographroot;
	UnityEngine.UI.Image _baseimage;
	UnityEngine.UI.Image _nameimage;

	public string[] _talktext;
	GameObject _exchangedialog;
	GameObject _litho_peaces;
	int _consumenum;
	Transform _effectroot;
	seq _seq;
	animseq _animseq=(animseq)0;
	float _tmptimer;

	// Use this for initialization
	void Start () {
		scenemanager.sceneSet(sceneid.lithograph);
		scenemanager.SetFadeSpeed(0.5f);
		scenemanager.setExecFunc(scenemanager.funcid.term,term);
		_currentpage=-1;
		_pageButtonObj = GameObject.Find("pagebutton");
		_lithodata=new lithodata[(int)litho_pageid.max];
		for(int i=0;i<(int)litho_pageid.max;i++){
			string name=System.Enum.GetName(typeof(litho_pageid),(int)i);
			_lithodata[i].basesprite = Resources.Load<Sprite>("Sprites/lithograph/"+_path[i]+"/Seki_"+name+"_n");
			_lithodata[i].namesprite = Resources.Load<Sprite>("Sprites/lithograph/"+_path[i]+"/Seki_"+name+"_p_r");
		}
		lithographroot = GameObject.Find("lithograph");
		_baseimage = lithographroot.GetComponent<UnityEngine.UI.Image>();
		_nameimage = lithographroot.transform.FindChild("nameplate").gameObject.GetComponent<UnityEngine.UI.Image>();
		_kawazu = GameObject.Find("kawazu");
		_kawazu.GetComponent<kawazu>().setClickFunc(onkawazuClick);
	
		_talktext= new string[(int)litho_talkid.max] {
			"金の卵を20個集めると\n石版の欠片と交換できるぞい！",
			"流石じゃ！\n良く頑張ったのう！",
			"おお！\n金の卵が{0}個あるぞい",
			"妖精の里に特別に入ることが出来る招待券じゃ！強力な結界も華麗にスルーじゃぞ！",
			"儂の特製魔法陣じゃ！\n樹を降りたくなったら大空へダイブじゃ！"
		};
		_effectroot = GameObject.Find("effect").transform;
		effect(litho_effect.off);
		scenemanager.setInitFlag((int)lithographinit.max);
		_exchangedialog = GameObject.Find("exchangedialog");
		soundmanager.bgmPlay(bgmid.bgm02);
		soundmanager.seName("litho");

		_seq = seq.init;
	}
	
	// Update is called once per frame
	void Update () {
		switch(_seq){
		case seq.init:
			if(scenemanager.checkInitFlags(1<<((int)lithographinit.scene))){
				if(scenemanager.readrecord()!=null){
//	scenemanager.m_savedata.m_lithograph.completenum=5;
					setParam();
					scenemanager.unsetInitFlag((int)lithographinit.scene);
				}
//scenemanager.writerecord().m_stockitem.addValue(stockitemid.egg,400);

				if(scenemanager.readrecord().m_lithograph.completenum<5){
					load((int)scenemanager.readrecord().m_lithograph.completenum);
					/*
					string name="Prefabs/Lithograph/Seki_"+System.Enum.GetName(typeof(litho_pageid),(int)scenemanager.m_savedata.m_lithograph.completenum-1);
					_litho_peaces = Instantiate(Resources.Load(name)) as GameObject;
					_litho_peaces.transform.SetParent(lithographroot.transform.parent);
					_litho_peaces.transform.position = Vector3.zero;
					_litho_peaces.transform.localPosition = Vector3.zero;
					_litho_peaces.transform.localScale = new Vector3(1,1,1);

					int num=scenemanager.readrecord().m_lithograph.partsnum;
					for(int i=0;i<_litho_peaces.transform.childCount;i++){
						_litho_peaces.transform.GetChild(i).gameObject.SetActive(i<num);
					}
				}
			}
			if(scenemanager.isInitComplete()){
				_seq=seq.fadewait;
			}
			break;

		case seq.fadewait:
			if(!scenemanager.isFading()){
				updatePage();
				_seq=seq.play;
			}
			break;

		case seq.play:
			break;

		case seq.exchange:
			if(animation()){
				scenemanager.setScreenBlocker(false);
				setTalk(litho_talkid.greet);
				scenemanager.enableSceneButton(true);
				/*
				if(!scenemanager.readrecord().m_gamesetting.flag.check(settingflag.flagval.releasekanikama)){
					scenemanager.writerecord().m_gamesetting.flag.set(settingflag.flagval.releasekanikama);
				}

				_seq=seq.play;
			}
			break;
		}
	}

	void term(){
		scenemanager.closeYesNoButton();
	}

	void setParam(){
		_pagemax=scenemanager.readrecord().m_lithograph.completenum;
		if(scenemanager.readrecord().m_lithograph.completenum<(int)litho_pageid.max){
			_pagemax++;
		}
		if(_currentpage<0){
			setPage(_pagemax-1,true);
		}
	}

	void updatePage(){
		effect(litho_effect.off);
		_pageButtonObj.transform.GetChild(0).gameObject.SetActive((_currentpage>0));
		_pageButtonObj.transform.GetChild(1).gameObject.SetActive((_currentpage<(_pagemax-1)));
/*
		if(_currentpage<(_pagemax-1) || (scenemanager.m_savedata.m_lithograph.completenum==(int)litho_pageid.max)){
			_baseimage.sprite = _lithodata[_currentpage].basesprite;
			_baseimage.color=new Color(1f,1f,1f,1f);
			effect(litho_effect.Kir);
			if(_litho_peaces!=null && _litho_peaces.gameObject.activeSelf)_litho_peaces.gameObject.SetActive(false);
		}else{
			_baseimage.sprite = null;
			_baseimage.color=new Color(1f,1f,1f,0.05f);
			if(_litho_peaces!=null && !_litho_peaces.gameObject.activeSelf)_litho_peaces.gameObject.SetActive(true);
		}
		_nameimage.sprite = _lithodata[_currentpage].namesprite;

		litho_talkid tid=litho_talkid.greet;
		if(_talkid[_currentpage]>0 && _currentpage<scenemanager.readrecord().m_lithograph.completenum){
			tid=(litho_talkid)_talkid[_currentpage];
		}else if(scenemanager.readrecord().m_lithograph.completenum==(int)litho_pageid.max){
			tid=litho_talkid.greetcomplete;
		}else if(_currentpage==(_pagemax-1) && isEnableExchange()){
			tid=litho_talkid.confirm;
			setExchangeDialog(true);
		}
		setTalk(tid);
	}

	void setPage(int pageno, bool bStart=false){
		_currentpage=pageno;
		updateLithograph();
		if(!bStart)updatePage();
	}

	void updateLithograph(){
		if(_currentpage<(_pagemax-1) || (scenemanager.readrecord().m_lithograph.completenum==(int)litho_pageid.max)){
			_baseimage.sprite = _lithodata[_currentpage].basesprite;
			_baseimage.color=new Color(1f,1f,1f,1f);
			effect(litho_effect.Kir);
			if(_litho_peaces!=null && _litho_peaces.gameObject.activeSelf)_litho_peaces.gameObject.SetActive(false);
			if(_currentpage>1){
				_nameimage.sprite = _lithodata[_currentpage].namesprite;
				_nameimage.color=_baseimage.color;
			}else{
				_nameimage.color=new Color(0f,0f,0f,0.003f);
			}
		}else{
			_baseimage.sprite = null;
			_baseimage.color=new Color(1f,1f,1f,0.003f);
			_nameimage.color=_baseimage.color;
				int num=scenemanager.readrecord().m_lithograph.partsnum;
			if(_litho_peaces!=null && !_litho_peaces.gameObject.activeSelf){
				_litho_peaces.gameObject.SetActive(true);
				for(int i=0;i<16;i++){
					_litho_peaces.transform.GetChild(i).gameObject.SetActive(num>i);
				}
			}
		}

	}


	public void pageForward(){
		if(_currentpage<(_pagemax-1)){
			_currentpage++;
			setPage(_currentpage);
		}
	}

	public void pageBack(){
		if(_currentpage>0){
			_currentpage--;
			setPage(_currentpage);
		}
	}

	void setTalk(litho_talkid tid){
		string text=null;
		if(tid==litho_talkid.confirm){
			int eggnum=scenemanager.readrecord().m_stockitem.getValue(stockitemid.egg);
			int id=(int)tid;
			text=string.Format(_talktext[(int)tid],eggnum);
		}else{
			if(tid<litho_talkid.max){
				text=_talktext[(int)tid];
			}else{
				text="";
			}
		}
		_kawazu.SendMessage("message",text);
	}

	bool isEnableExchange(){
		return scenemanager.readrecord().m_stockitem.getValue(stockitemid.egg)>=LITHOGRAPH_PARTSMAX;
	}

	void setExchangeDialog(bool sw){
		if(sw){
			int leftnum = LITHOGRAPH_PARTSMAX-scenemanager.readrecord().m_lithograph.partsnum;
			int enablenum = scenemanager.readrecord().m_stockitem.getValue(stockitemid.egg)/20;
			_consumenum = (enablenum>leftnum)?leftnum:enablenum;
			exchangedialog.activate(true,_consumenum);
			scenemanager.setScreenBlocker(true);
			scenemanager.setScreenBlockerColor(new Color(1f,1f,1f,0f));
			yesnoconfig conf = new yesnoconfig();
			conf.yesfunc = exchangeYes;
			conf.nofunc = exchangeNo;
			conf.onCloseFunc = exchangeClose;
			conf.pos = new Vector2(0,76f);
			scenemanager.setYesNoButton(conf);
		}else{
			_consumenum=0;
			exchangedialog.activate(false,_consumenum);
		}
	}

	bool exchangeYes(){
		return true;
	}

	bool exchangeNo(){
		_consumenum=0;
		return true;
	}

	bool exchangeClose(){
		exchangedialog.activate(false,0);
		if(_consumenum>0){
			_animseq=animseq.start;
			setTalk(litho_talkid.max);
			scenemanager.setScreenBlocker(true);
			scenemanager.enableSceneButton(false);
			_seq=seq.exchange;
		}
		return true;
	}

	void effect(litho_effect eff){
		if(eff==litho_effect.off){
			foreach(Transform trs in _effectroot){
				trs.gameObject.SetActive(false);
			}
		}else{
			int idx=(int)eff-1;
			_effectroot.GetChild(idx).gameObject.SetActive(true);
		}
	}

	bool animation(){
		bool ret=false;
		switch(_animseq){
		case animseq.start:
			_consumenum--;
			scenemanager.writerecord().m_stockitem.addValue(stockitemid.egg,-20);
			scenemanager.writerecord().m_lithograph.partsnum++;
			int idx = scenemanager.readrecord().m_lithograph.partsnum-1;
			Transform trs=_litho_peaces.transform.GetChild(idx);
			trs.localScale = new Vector3(2f,2f,1f);
			trs.gameObject.SetActive(true);
			iTween.ValueTo(gameObject,iTween.Hash("from",2f,"to",1f,"time",0.3f,"onupdate","onAnimationUpdate","oncomplete","onAnimationEnd"));
			_animseq++;
			break;
		case animseq.effect:
			if(_consumenum>0){
				_animseq=animseq.start;
			}else{
				if(scenemanager.readrecord().m_lithograph.partsnum==16){
					soundmanager.sePlay((int)lithoseid.eff2);
					effect(litho_effect.All);
				}else{
					soundmanager.sePlay((int)lithoseid.eff);
					effect(litho_effect.Bar);
				}
				_tmptimer=0.5f;
				_animseq=animseq.effectwait;
			}
			break;
		case animseq.effectwait:
			_tmptimer-=Time.deltaTime;
			if(_tmptimer<=0){
				_animseq=animseq.finish;
			}
			break;

		case animseq.finish:
			if(scenemanager.readrecord().m_lithograph.partsnum==16){
				//iTween.ValueTo(gameObject,iTween.Hash("from",1.5f,"to",1f,"time",0.5f,"onupdate","onAnimationUpdate","oncomplete","onAnimationEnd"));
				_baseimage.sprite = _lithodata[_currentpage].basesprite;
				_baseimage.color=new Color(1f,1f,1f,1f);
				_tmptimer=1f;
				_animseq=animseq.finishwait;
			}else{
				_animseq=animseq.end;
			}
			break;

		case animseq.finishwait:
			_tmptimer-=Time.deltaTime;
			if(_tmptimer<=0){
				scenemanager.writerecord().m_lithograph.completenum++;
				_pagemax=scenemanager.readrecord().m_lithograph.completenum;
				if(scenemanager.readrecord().m_lithograph.completenum<(int)litho_pageid.max){
					_pagemax++;
				}
				scenemanager.writerecord().m_lithograph.partsnum=0;
				scenemanager.writerecord().m_bookkeeping.addValue((int)bookkeepingid.lithograph,1);

				Destroy(_litho_peaces);
				if(scenemanager.readrecord().m_lithograph.completenum<5){
					load(scenemanager.readrecord().m_lithograph.completenum);
					_litho_peaces.gameObject.SetActive(false);
				}else{
					_litho_peaces=null;
				}
				updatePage();
				_animseq=animseq.end;
			}
			break;

		case animseq.end:
			ret=true;
			break;
		}
		return ret;
	}

	void onAnimationEnd(){
		soundmanager.sePlay((int)lithoseid.put);
		_animseq++;
	}

	void onAnimationUpdate(float ratio){
		int idx = scenemanager.readrecord().m_lithograph.partsnum-1;
		_litho_peaces.transform.GetChild (idx).localScale=new Vector3(ratio,ratio,1f);
	}

	void load(int no){
		string name="Prefabs/Lithograph/Seki_"+System.Enum.GetName(typeof(litho_pageid),no);
		_litho_peaces = Instantiate(Resources.Load(name)) as GameObject;
		_litho_peaces.transform.SetParent(lithographroot.transform.parent);
		_litho_peaces.transform.position = Vector3.zero;
		_litho_peaces.transform.localPosition = Vector3.zero;
		_litho_peaces.transform.localScale = new Vector3(1,1,1);
	}

	public void onkawazuClick(){
		if(isEnableExchange() && !_exchangedialog.activeSelf){
			setExchangeDialog(true);
		}
	}

}
*/