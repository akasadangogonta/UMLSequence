using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum bookkeepingid{
	maxdistance,//最高到達点
	maxfloor,//最終到達層
	growth,//大樹の生育状態
	totaldistance,//総登頂距離
	coin,//最高獲得コイン
	mana,//最高獲得マナ
	egg,//最高獲得金の卵
	totalcoin,//総獲得コイン
	totalmana,//総獲得マナ
	totalegg,//総獲得マナ
	lithograph,//完成した石版の枚数
	totalbarrier,//生涯結界使用回数
	totalhbarrier,//生涯四重結界使用回数
	totalwing,//生涯魔法の翼使用回数
	totalmaseki,//生涯魔石使用回数
	max
}

[System.Serializable]
public class bookkeepingformat {
	public int index;
	public string label;
	public string unit;
	private long value;

	public void setValue(long val){
		value=val;
	}

	public long getValue(){
		return value;
	}
}

[System.Serializable]
public class bookkeepingdictionary {
	public List<bookkeepingformat> data;
}

[System.Serializable]
public class bookkeepingdata {

	public long[] value;

	public bookkeepingdata(){
		value = new long[(int)bookkeepingid.max];
		for(int i=0;i<(int)bookkeepingid.max;i++){
			value[i]=0;
		}
		//value[(int)bookkeepingid.growth]=(long)gamemanager.cleardistance_initialize;
	}

	public long setValue(int idx, long val){
		value[idx]=val;
		return value[idx];
	}

	public long addValue(int idx, long val){
		value[idx]+=val;
		return value[idx];
	}

	public long getValue(int idx){
		return value[idx];
	}

}

public enum bookkeepinginit{
	scene,
	tombo,
	max
}
/*

public class scene_bookkeeping : MonoBehaviour {

	enum seq{
		init,
		layout,
		waitlayout,
		fadewait,
		play,
		rankingstart,
		rankingwait,
		rankingend,
	}

	Transform _content;
	bookkeepingdictionary _dic;
	stagegrowth _stagegrowth=null;

	seq _seq;

	GameObject _kawazu;
	GameObject _rankingdialog;
	UnityEngine.UI.Text _rankingtext;
	//float _tmptime;
	bool _rankingsend;

	// Use this for initialization
	void Start () {
		scenemanager.sceneSet(sceneid.bookkeeping);
		scenemanager.SetFadeSpeed(0.5f);
		_dic = utility.XmlUtil.Deserialize<bookkeepingdictionary>("bookkeeping");
		_content=GameObject.Find("content").transform;
		scenemanager.setInitFlag((int)bookkeepinginit.max);

		UnityEngine.Object prefab=(UnityEngine.Object)Resources.Load("Prefabs/bookkeeping/plate");
		foreach(bookkeepingformat bf in _dic.data){
			GameObject obj=Instantiate(prefab) as GameObject;
			obj.transform.SetParent(_content);
			obj.transform.localScale=new Vector3(1f,1f,1f);
		}
		scenemanager.setInitFlag((int)bookkeepinginit.max);

		string xmlname = string.Format("stagegrowth{0}", 0); 
//		_stagegrowth=utility.XmlUtil.Deserialize<stagegrowth>(xmlname);

		_kawazu=GameObject.Find("kawazu").gameObject;
		_kawazu.GetComponent<kawazu>().setClickFunc(onkawazuClick);
		_rankingdialog = GameObject.Find("rankingdialog").gameObject;
		_rankingtext=GameObject.Find("valuetext").GetComponent<UnityEngine.UI.Text>();
		_rankingdialog.SetActive(false);

		soundmanager.bgmPlay(bgmid.bgm02);
		_seq=seq.init;
	}
	
	// Update is called once per frame
	void Update () {
		switch(_seq){
		case seq.init:
			if(scenemanager.checkInitFlags(1<<(int)bookkeepinginit.scene) && scenemanager.readrecord()!=null){
				foreach(bookkeepingformat bf in _dic.data){
					long value=scenemanager.readrecord().m_bookkeeping.getValue(bf.index);
					bf.setValue(value);
					GameObject obj=_content.GetChild(bf.index).gameObject;
					obj.SendMessage("init",bf);
					_seq=seq.layout;
				}
			}
			break;
		case seq.layout:
			GameObject.Find("Scrollbar").GetComponent<UnityEngine.UI.Scrollbar>().value=1f;
			scenemanager.unsetInitFlag((int)bookkeepinginit.scene);
			_seq=seq.fadewait;
			break;
		case seq.waitlayout:
			if(scenemanager.isInitComplete()){
				_seq=seq.fadewait;
			}
			break;
		case seq.fadewait:
			if(!scenemanager.isFading()){
				_kawazu.SendMessage("message","更なる高みを目指すのじゃ！");
				_kawazu.SendMessage("setAutoCloseTime",2f);
				_seq=seq.play;
			}
			break;
		case seq.play:
			break;
		case seq.rankingstart:
			//最高到達距離
			long distance=scenemanager.readrecord().m_bookkeeping.getValue((int)bookkeepingid.maxdistance);
			//最高獲得コイン
			long coin=scenemanager.readrecord().m_bookkeeping.getValue((int)bookkeepingid.coin);
			//最高獲得マナ
			long mana=scenemanager.readrecord().m_bookkeeping.getValue((int)bookkeepingid.mana);
			//
			//ランキング送信開始処理
			//
			//
			Transform contents=_rankingdialog.transform.GetChild(0);
			contents.GetChild(1).GetComponent<UnityEngine.UI.Text>().text="";
			_rankingtext.text="送信中";
			
			//iOS GameCenter Google Play Serviceに送るスコア
			RankingScoreManager.m_Instance.RegisterDistanceScore((int)distance);
			RankingScoreManager.m_Instance.RegisterCoinScore	((int)coin);
			RankingScoreManager.m_Instance.RegisterManaScore	((int)mana);

			//send score
			RankingScoreManager.m_Instance.ReportDistanceScore	();
			RankingScoreManager.m_Instance.ReportCoinScore 	   	();
			RankingScoreManager.m_Instance.ReportManaScore		();


			RankingScoreManager.m_Instance.ShowLeaderBoard();	

			//_tmptime=2f;
			_seq=seq.rankingwait;
			break;
		case seq.rankingwait:
			//
			//ランキング送信終了チェック
			//
			if(true){
				_seq=seq.rankingend;
			}
			break;
		case seq.rankingend:
			_rankingdialog.SetActive(false);
			scenemanager.setFrontBlocker(false);
			_seq=seq.play;
			break;
		}
	}

	public bool onRankingYesClick(){
		scenemanager.setFrontBlocker(true);
		_rankingsend=true;
		return true;
	}

	public bool onRankingNoClick(){
		return true;
	}

	public bool onRankingYesnoCloseClick(){
		_kawazu.SendMessage("message","");
		if(_rankingsend){
			_seq=seq.rankingstart;
		}else{
			_rankingdialog.SetActive(false);
		}
		return true;
	}

	public void onkawazuClick(){
		_kawazu.SendMessage("message","更なる高みを目指すのじゃ！");
		_kawazu.SendMessage("setAutoCloseTime",2f);
	}


	public void onRankingClick(){
		long val=scenemanager.readrecord().m_bookkeeping.getValue((int)bookkeepingid.maxdistance);
		//if(val>0)
		{
			yesnoconfig config= new yesnoconfig();
			config.yesfunc = onRankingYesClick;
			config.nofunc = onRankingNoClick;
			config.onCloseFunc = onRankingYesnoCloseClick;
			scenemanager.setYesNoButton(config);
			_rankingdialog.SetActive(true);
			_rankingtext.text=string.Format("{0}ｍ",val);
			Transform contents=_rankingdialog.transform.GetChild(0);
			contents.GetChild(1).GetComponent<UnityEngine.UI.Text>().text="記録をランキング登録します\n\nよろしいですか？";
			_rankingsend=false;
		}
	}

}
*/