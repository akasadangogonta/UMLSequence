using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
[System.Serializable]
public class savedata {
	public letterstatus	m_letter;
	public stockitem	m_stockitem;
	public bookkeepingdata	m_bookkeeping;
	public gamesetting m_gamesetting;
	public lithhographdata m_lithograph;
}
*/

[System.Serializable]
public class savedata 
{
	public ObjectsData[] 	m_obj;
	public GeneralData 		m_general;
}


public class record : MonoBehaviour{
	
	public record(){
	}
	
	public static bool prefsave<T>( string prefKey, T serializableObject ){
		MemoryStream memoryStream = new MemoryStream();
		#if UNITY_IPHONE || UNITY_IOS
		System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		#endif
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize( memoryStream, serializableObject );
		
		string tmp = System.Convert.ToBase64String( memoryStream.ToArray() );
		try {
			PlayerPrefs.SetString ( prefKey, tmp );
		} catch( PlayerPrefsException ) {
			return false;
		}
		return true;
	}
	
	public static T prefload<T>( string prefKey ){
		if (!PlayerPrefs.HasKey(prefKey)) return default(T);
		#if UNITY_IPHONE || UNITY_IOS
		System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		#endif
		BinaryFormatter bf = new BinaryFormatter();
		string serializedData = PlayerPrefs.GetString( prefKey );
		
		MemoryStream dataStream = new MemoryStream( System.Convert.FromBase64String(serializedData) );
		T deserializedObject = (T)bf.Deserialize( dataStream );
		
		return deserializedObject;
	}

	public static void save(savedata data) {
		// 保存用クラスにデータを格納.
		prefsave("savedata", data);
		PlayerPrefs.Save();
	}

	public static savedata load() {
		savedata data_tmp=null;
		if(PlayerPrefs.HasKey("savedata")){
			data_tmp = prefload<savedata>("savedata");
		}else{
			data_tmp = new savedata();
			data_tmp.m_general = new GeneralData();
			data_tmp.m_obj = new ObjectsData[0];

			/*
			data_tmp.m_stockitem = new stockitem();
			data_tmp.m_bookkeeping = new bookkeepingdata();
			data_tmp.m_gamesetting = new gamesetting();
			data_tmp.m_lithograph = new lithhographdata(); */
		}
		return data_tmp;
	}
}