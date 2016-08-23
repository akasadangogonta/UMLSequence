using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum stockitemid{
	nbarrier,
	hbarrier,
	wing,
	maseki,
	coin,
	mana,
	egg,
	//	sbarrier,
	max,
	helperownmax=990
}

public enum shoptalkid{
	greeting,
	thanks,
	notenough,
	max
}

public enum shopresult{
	ok,
	error,
	stockfull,
	notenoughcoin,
}


[System.Serializable]
public class stockitem{
	public int[] itemnun;
	
	public stockitem(){
		itemnun=new int[(int)stockitemid.max];
		for(int i=0;i<(int)stockitemid.max;i++){
			itemnun[i]=0;
		}
	}
	
	public int getValue(stockitemid sid){
		return itemnun[(int)sid];
	}

	public int setValue(stockitemid sid, int value){
		itemnun[(int)sid]=value;
		return itemnun[(int)sid];
	}

	public int addValue(stockitemid sid, int value){
		itemnun[(int)sid]+=value;
		return itemnun[(int)sid];
	}
}

[System.Serializable]
public class shopdata{
	public int index;
	public string name;
	public int icon;
	public int coin;
	public string storeid;
	public int item;
	public int num;
	public string info;
	public string talk;
	private Sprite iconspr;
	private GameObject rootObj;
	private Sprite pricespr;

	public void setIconSprite(Sprite spr){
		iconspr=spr;
	}
	public Sprite getIconSprite(){
		return iconspr;
	}
	public void setPriceSprite(Sprite spr){
		pricespr=spr;
	}
	public Sprite getPriceSprite(){
		return pricespr;
	}
	public void setRoot(GameObject _rootobj){
		rootObj=_rootobj;
	}
	public GameObject getRoot(){
		return rootObj;
	}
}

[System.Serializable]
public class shopdictionary{
	public List<shopdata> data;
}

public enum shopinit{
	scene,
	max
}
/*
public class scene_shop : MonoBehaviour {
	public enum seqid{
		init,
		inputwait,
		confirm,
		menuchange,
		menuchange2,
		max
	}

	public enum contentstypeid{
		coin,
		money,
		max
	}
	

	[SerializeField]
	private Sprite[] iconsprites;

	private Sprite[] priceicon={null,null};

//	[SerializeField]
//	private Sprite[] contentsbuttonspr;

	seqid _seq;
	shopdictionary _dic;
	UnityEngine.UI.Text[] _icontext;
	
	GameObject _menuchange;
	GameObject _itemwindow;
	GameObject _confirmwindow;
	GameObject _kawazu;

	shopdata _currentitem;
	contentstypeid _currentcontents;
	UnityEngine.UI.Image[] _contentsbuttonimage;
	UnityEngine.UI.Scrollbar _scrollbar;

	bool _result;
	shopresult _shopresult;
	Transform _contentsroot;
	Transform _itemroot;

	float _scrollsize;

	List<GameObject> _menuobjlist;
	// Use this for initialization
	void Start () {

		scenemanager.sceneSet(sceneid.shop);
		scenemanager.SetFadeSpeed(0.5f);
		
		_seq=seqid.init;

		_menuobjlist = new List<GameObject>();
		_itemroot = GameObject.Find("itemroot").transform;
		_dic = utility.XmlUtil.Deserialize<shopdictionary>("shop");
		UnityEngine.Object prefab=(UnityEngine.Object)Resources.Load("Prefabs/shop/itemplate");
		_itemwindow = GameObject.Find("itemwindow");
		_contentsroot=_itemwindow.transform.FindChild("content");

		Transform icontrs=GameObject.Find("myitemwindow").transform;
		_icontext=new UnityEngine.UI.Text[2];
		for(int i=0;i<_icontext.Length;i++){
			//if(_iconorder[i]>=0)
			{
				Transform trs=icontrs.GetChild(i);
				_icontext[i]=trs.gameObject.GetComponent<UnityEngine.UI.Text>();
				priceicon[i]=trs.GetChild (0).GetComponent<UnityEngine.UI.Image>().sprite;
			}
		}

		foreach(shopdata sd in _dic.data){
			sd.setIconSprite(iconsprites[sd.icon]);
			sd.setRoot(gameObject);
			GameObject obj=Instantiate(prefab) as GameObject;
			obj.name="item"+sd.index.ToString();
//			obj.SendMessage("init",sd);
			//削obj.transform.SetParent(_itemroot.GetChild((sd.coin==0)?1:0));
			obj.transform.SetParent(_itemroot.GetChild(sd.storeid.Substring(0, 8)!="notpurch"?1:0));//追
			obj.transform.localScale=new Vector3(1f,1f,1f);
			_menuobjlist.Add(obj);
		}
		_scrollbar=GameObject.Find("Scrollbar").GetComponent<UnityEngine.UI.Scrollbar>();
		_scrollbar.value=0f;
		/*
		Transform icontrs=GameObject.Find("icon").transform;
		_icontext=new UnityEngine.UI.Text[(int)stockitemid.max];
		for(int i=0;i<(int)stockitemid.max-1;i++){
			//if(_iconorder[i]>=0)
			{
				Transform trs=icontrs.GetChild(i);
				_icontext[i]=trs.gameObject.GetComponent<UnityEngine.UI.Text>();
			}
		}



		_confirmwindow = GameObject.Find("confirmwindow");
		_confirmwindow.SetActive(false);
		_kawazu = GameObject.Find("kawazu");

		_menuchange = GameObject.Find("menuchange");
		_contentsbuttonimage=new UnityEngine.UI.Image[2];
		_currentcontents=contentstypeid.money;
		for(int i=0;i<2;i++){
			_contentsbuttonimage[i]=_menuchange.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>();
		}
		soundmanager.bgmPlay(bgmid.bgm02);
		scenemanager.setInitFlag((int)shopinit.max);
	}
	
	// Update is called once per frame
	void Update () {
		switch(_seq){
		case seqid.init:
			if(!scenemanager.isInitComplete()){
				if(scenemanager.readrecord()!=null || KawazuStoreScript.instance!=null){
					for(int i=0;i<_dic.data.Count;i++){
						_menuobjlist[i].SendMessage("init",_dic.data[i]);
					}
					_menuobjlist.Clear();
					_menuobjlist=null;

					//UnityEngine.UI.Scrollbar scrollbar=GameObject.Find("Scrollbar").GetComponent<UnityEngine.UI.Scrollbar>();
					//scrollbar.value=1f;

					foreach (var item in StoreInfo.Goods) {
						setShopInfo(item);//ストア情報と関連付け
						//print ("アイテム名:" + item.Name + "　アイテムID：" + item.ItemId + "　値段は" + GetPrice(item.ItemId) + "です");
					}

					Transform content=_itemwindow.transform.FindChild("content");
					for(int i=0;i<content.childCount;i++){
						content.GetChild(i).SendMessage("init",_dic.data[i]);
					}

					for(int i=0;i<_icontext.Length;i++){
						updateIcon((int)stockitemid.coin+i);
					}
					shoptalk(shoptalkid.greeting);
					contentsButton(0);


					//_seq = seqid.inputwait;
					scenemanager.unsetInitFlag((int)shopinit.scene);
				}
			}
			break;
		case seqid.confirm:
			_confirmwindow.SetActive(true);
			break;

		case seqid.menuchange:
			RectTransform rt=_itemwindow.transform.GetChild(0).GetComponent<RectTransform>();

			if(rt.sizeDelta.y>=_scrollsize)
			{
				_seq=seqid.menuchange2;
			}
			break;
		case seqid.menuchange2:
			UnityEngine.UI.ScrollRect scrollRect = _itemwindow.GetComponent<UnityEngine.UI.ScrollRect>();
			scrollRect.verticalNormalizedPosition = 1f;
			_seq=seqid.inputwait;
			break;
		}
	}
	
	void updateIcon(int no) {
		//if(_iconorder[no]>=0)
		{
			int idx = no-(int)stockitemid.coin;
			_icontext[idx].text = "×"+scenemanager.readrecord().m_stockitem.getValue((stockitemid)no).ToString();
		}
	}

	void confirmStart(shopdata data) {
		soundmanager.systemsePlay(systemseid.ok);
		_confirmwindow.SetActive(true);
		_confirmwindow.SendMessage("init",data);
		yesnoconfig config=new yesnoconfig();
		config.yesfunc = onConfirmYes;
		config.nofunc = onConfirmNo;
		//config.intaractive_yes = (scenemanager.readrecord().m_stockitem.getValue((stockitemid)data.item)<(int)stockitemid.helperownmax);
		config.yestext="購入";
		config.notext="やめる";
		config.pos=new Vector2(0,-330f);
		config.onCloseFunc=onConfirmClose;
		scenemanager.setYesNoButton(config);
		_currentitem=data;
		_seq=seqid.confirm;
		_kawazu.SendMessage("message",data.talk);
	}
	
	bool onConfirmYes(){
		_shopresult=shopresult.error;
		_result=checkConfirm(_currentitem);
		if(_result)_shopresult=shopresult.ok;
		return _result;
	}
	
	
	bool onConfirmNo(){
		_currentitem=null;
		return false;
	}
	
	bool onConfirmClose(){
		if(_currentitem!=null){
			shoptalkresult(_shopresult);
			_currentitem=null;
		}else{
			shoptalk(shoptalkid.greeting);
		}
		_confirmwindow.SetActive(false);
		_seq=seqid.inputwait;
		return true;
	}

	bool checkConfirm(shopdata data){
		print (data.name);

		//お助けアイテムの所持数が満杯
		if(data.item<(int)stockitemid.coin){
			if(scenemanager.readrecord().m_stockitem.getValue((stockitemid)data.item)>=(int)stockitemid.helperownmax){
				_shopresult=shopresult.stockfull;
				return false;
			}
		}
		//S""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""追加
		if (_currentcontents == contentstypeid.money)
		{
			if (!KawazuStoreScript.instance.isInitSoomla)
			{
				bool itemHit = false;
				foreach (var itemId in KawazuStoreScript.instance.GetItemsId())
				{
					if (itemId == data.storeid &&
						KawazuStoreScript.instance.GetPurchaseEvent () == purchEvent.none) {
						GameObject obj = GameObject.Find ("game/EventSystem");
						obj.SetActive (false);
						KawazuStoreScript.instance.BuyItem (data.storeid);
						StartCoroutine (WaitReceiveSever (obj, data));
						itemHit = true;
						return true;
					}
				}
				if(itemHit == false) {
					print ("課金アイテムですが、アイテムIDが存在しません");
					return false;
				}
			}
			else
			{
				print ("Soomlaの初期化エラーにより、アイテムの購入ができません");
				return false;
			}
		}
		//E""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""

		int temp = data.coin;
		int val = scenemanager.readrecord().m_stockitem.getValue(stockitemid.coin)-data.coin;
		bool ret=false;
		if(val<0){
			_shopresult=shopresult.notenoughcoin;
		}else{
			scenemanager.writerecord().m_stockitem.setValue(stockitemid.coin,val);
			scenemanager.writerecord().m_stockitem.addValue((stockitemid)data.item,data.num);
			record.save( scenemanager.m_savedata );

			//updateIcon((int)data.item);
			updateIcon((int)stockitemid.coin);
			ret=true;
		}


		return ret;
	}

	//S"""""""""""""""""""""""""""""""""""""""""""""""""""""追加
	IEnumerator WaitReceiveSever(GameObject obj, shopdata tempdata)
	{
		KawazuStoreScript store = KawazuStoreScript.instance;

		int num = 0;
		while (num < 500) 
		{
			num++;
			if (store.GetPurchaseEvent() == purchEvent.none)
			{
				print ("purchEvent.none");
				shoptalk(shoptalkid.greeting);
			}
			else if (store.GetPurchaseEvent() == purchEvent.started)
			{
				print ("purchEvent.started");
				shoptalk(shoptalkid.greeting);
			}
			else if (store.GetPurchaseEvent() == purchEvent.cancel)
			{
				print ("purchEvent.cancel");
				shoptalk(shoptalkid.greeting);
				store.InitPurchaseEvent();

				obj.SetActive(true);
				yield break;
			}
			else if (store.GetPurchaseEvent() == purchEvent.error)
			{
				print ("purchEvent.error");
				shoptalk(shoptalkid.greeting);
				store.InitPurchaseEvent();

				obj.SetActive(true);
				yield break;
			}
			else if (store.GetPurchaseEvent() == purchEvent.succes)
			{
				scenemanager.writerecord().m_stockitem.addValue((stockitemid)tempdata.item,tempdata.num);
				record.save( scenemanager.m_savedata );

				print ("purchEvent.succes");

				shoptalk(shoptalkid.thanks);

				//updateIcon((int)tempdata.item);
				updateIcon((int)stockitemid.coin);
				store.InitPurchaseEvent();

				obj.SetActive(true);
				yield break;
			}
			
			yield return new WaitForSeconds (0.02F);
		}

		print ("サーバから一定時間何の応答も確認できませんでした。");
		shoptalk(shoptalkid.greeting);
		store.InitPurchaseEvent();
		
		obj.SetActive(true);
	}
	//E""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""


	public void onAddcoin(int coin){
		scenemanager.writerecord().m_stockitem.addValue((stockitemid)stockitemid.coin,coin);
		updateIcon((int)stockitemid.coin);
	}
	
	void setShopInfo(VirtualGood vg){
		foreach(shopdata sd in _dic.data){
			if(vg.ItemId==sd.storeid){
				sd.setStoreInfo(vg);
				break;
			}
		}
	}
	
	void shoptalk(shoptalkid id) {
		switch(id){
		case shoptalkid.greeting:
			_kawazu.SendMessage("message","何が欲しいのじゃ？");
			break;
		case shoptalkid.thanks:
			_kawazu.SendMessage("message","毎度ありなのじゃ！");
			break;
		case shoptalkid.notenough:
			_kawazu.SendMessage("message","金が足りんぞい");
			break;
		}
	}

	void shoptalkresult(shopresult id) {
		switch(id){
		case shopresult.ok:
			_kawazu.SendMessage("message","毎度ありなのじゃ！");
			break;
		case shopresult.stockfull:
			_kawazu.SendMessage("message",string.Format("アイテムが満杯じゃ！\nお助けアイテムは{0}個までじゃ！",(int)stockitemid.helperownmax));
			break;
		case shopresult.notenoughcoin:
			_kawazu.SendMessage("message","コインが足りんぞい！");
			break;
		case shopresult.error:
			_kawazu.SendMessage("message","エラーがでたぞい");
			break;
		}
	}


	public void contentsButton(int buttonno) {
		if(_seq<=seqid.inputwait && buttonno!=(int)_currentcontents){
			Transform root=_itemroot.GetChild((int)_currentcontents);
			Transform trs;

			while(_contentsroot.childCount>0){
				trs=_contentsroot.GetChild(0);
				trs.SetParent(root);
				trs.gameObject.SetActive(false);
			}

			root=_itemroot.GetChild(buttonno);

			_scrollsize=0;
			while(root.childCount>0){
				trs=root.GetChild(0);
				trs.gameObject.SetActive(true);
				trs.SetParent(_contentsroot);
				_scrollsize+=134;
			}

			for(int i=0;i<2;i++){
				/*
				int ofs=((int)buttonno==i)?0:1;
				_contentsbuttonimage[i].sprite=contentsbuttonspr[i*2+ofs];

				float color=((int)buttonno==i)?1f:0.5f;
				_contentsbuttonimage[i].color=new Color(color,color,color,color);
			}

			_currentcontents=(contentstypeid)buttonno;
			_seq=seqid.menuchange;
		}
	}
}
*/