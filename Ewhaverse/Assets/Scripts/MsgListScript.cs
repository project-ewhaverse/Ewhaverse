using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[System.Serializable]
public class Mmtlist
{
	public Mmtlist(string _rid, string _mid, string _id1, string _id2, string _time, string _mtext)
	{
		rid = _rid; mid = _mid; id1 = _id1; id2 = _id2; time = _time; mtext = _mtext;
	}
	public string rid, mid, id1, id2, time, mtext;
}
public class MsgListScript : MonoBehaviour
{
	public string userName;
	public int listcount;
	public InputField MsgListInputField;
	public GameObject Output, MsgListButton1;
	[SerializeField] string url;
	public List<Mmtlist> mmtlist;
	public List<GameObject> mlb1list = new List<GameObject>();
	void Start()
	{
		Application.runInBackground = true;
		userName = File.ReadAllText(Application.persistentDataPath + "/Sync.txt");
		listcount = 0;
		InvokeRepeating("call", 2, 25);
	}
	IEnumerator McreateCoroutine(string command, string othername)
	{
		WWWForm form = new WWWForm();
		form.AddField("command", command);
		form.AddField("rid", "");
		form.AddField("id1", userName);
		form.AddField("id2", othername);
		UnityWebRequest www = UnityWebRequest.Post(url, form);
		yield return www.SendWebRequest();
		string result = UnityWebRequest.UnEscapeURL(www.downloadHandler.text);
		print(result);
	}
	IEnumerator MlistCoroutine(string command)
	{
		WWWForm form = new WWWForm();
		form.AddField("command", command);
		form.AddField("rid", "");
		form.AddField("id1", userName);
		form.AddField("id2", "");
		UnityWebRequest www = UnityWebRequest.Post(url, form);
		yield return www.SendWebRequest();
		string rdata = www.downloadHandler.text;
		string rdata1 = rdata.Substring(1, rdata.Length - 2);
		string rdata2 = rdata1.Replace("}], [{", "}, {");
		File.WriteAllText(Application.persistentDataPath + "/MmtJson.txt", rdata2);
		mmtlist.Clear();
		mmtlist = JsonConvert.DeserializeObject<List<Mmtlist>>(rdata2);
		if (mmtlist.Count > listcount)
		{
			mmtlistload();
		}
	}
	public void onclick()
	{
		GameObject go = EventSystem.current.currentSelectedGameObject;
		string othername = go.transform.Find("MsgListText1").GetComponent<Text>().text;
		int i = mmtlist.FindIndex(x => x.id1 == othername);
        if (i == -1)
        {
			i = mmtlist.FindIndex(x => x.id2 == othername);
		}
		string rid = mmtlist[i].rid;
		File.WriteAllText(Application.persistentDataPath + "/SyncMsg1.txt", rid);
		File.WriteAllText(Application.persistentDataPath + "/SyncMsg2.txt", othername);
		SceneManager.LoadScene("MsgRoomScene");
	}
	public void onclickcreate()
	{
		if (MsgListInputField.text != "")
		{
			StartCoroutine(McreateCoroutine("mlistcreate", MsgListInputField.text));
			MsgListInputField.text = "";
		}
	}
	void call()
    {
		StartCoroutine(MlistCoroutine("mlistload"));
	}
	public void mmtlistload()
    {
		for (int i = listcount; i < mmtlist.Count; i++)
		{
            if (mmtlist[i].id1 == userName)
            {
				mmtlistpaint(mmtlist[i].id2, mmtlist[i].mtext);
            }
            else
            {
				mmtlistpaint(mmtlist[i].id1, mmtlist[i].mtext);
			}
		}
		listcount = mmtlist.Count;
	}
	public void mmtlistpaint(string othername, string message)
    {
		GameObject newmlb1 = Instantiate(MsgListButton1);
		newmlb1.transform.SetParent(Output.transform);
		newmlb1.transform.position = Output.transform.position + new Vector3(0, 200f, 0);
		newmlb1.GetComponent<Button>().onClick.AddListener(() => onclick());
		mlb1list.Add(newmlb1);
		int i = mlb1list.Count - 1;
		newmlb1.transform.position += new Vector3(0, -50f * i, 0);
		newmlb1.transform.Find("MsgListText1").GetComponent<Text>().text = othername;
		newmlb1.transform.Find("MsgListText2").GetComponent<Text>().text = message;
	}
}