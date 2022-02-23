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
public class Memtext
{
	public Memtext(string _rid, string _mid, string _id1, string _id2, string _time, string _mtext)
	{
		rid = _rid; mid = _mid; id1 = _id1; id2 = _id2; time = _time; mtext = _mtext;
	}
	public string rid, mid, id1, id2, time, mtext;
}
public class MsgRoomScript : MonoBehaviour
{
	public string rid, userName, otherName;
	public int midcount;
	public InputField inputField;
	public GameObject YellowArea, WhiteArea, DateArea;
	public RectTransform ContentRect;
	public Scrollbar scrollBar;
	AreaScript LastArea;
	[SerializeField] string url;
	public List<Memtext> mtextlist;
	void Start()
	{
		Application.runInBackground = true;
		rid = File.ReadAllText(Application.persistentDataPath + "/SyncMsg1.txt");
		userName = File.ReadAllText(Application.persistentDataPath + "/Sync.txt");
		otherName = File.ReadAllText(Application.persistentDataPath + "/SyncMsg2.txt");
		midcount = 1;
		InvokeRepeating("call", 2, 5);
	}
	IEnumerator MsendCoroutine(string command, string rid, string sender, string receiver, string time, string message)
	{
		WWWForm form = new WWWForm();
		form.AddField("command", command);
		form.AddField("rid", rid);
		form.AddField("mid", "");
		form.AddField("id1", sender);
		form.AddField("id2", receiver);
		form.AddField("time", time);
		form.AddField("mtext", message);
		UnityWebRequest www = UnityWebRequest.Post(url, form);
		yield return www.SendWebRequest();
		string result = UnityWebRequest.UnEscapeURL(www.downloadHandler.text);
		print(result);
	}
	IEnumerator McheckCoroutine(string command)
	{
		WWWForm form = new WWWForm();
		form.AddField("command", command);
		form.AddField("rid", rid);
		form.AddField("mid", midcount.ToString());
		form.AddField("id1", "");
		form.AddField("id2", "");
		form.AddField("time", "");
		form.AddField("mtext", "");
		UnityWebRequest www = UnityWebRequest.Post(url, form);
		yield return www.SendWebRequest();
		string result = UnityWebRequest.UnEscapeURL(www.downloadHandler.text);
		if (result.Contains("Load"))
		{
			StartCoroutine(MloadCoroutine("mtextload"));
		}
	}
	IEnumerator MloadCoroutine(string command)
	{
		WWWForm form = new WWWForm();
		form.AddField("command", command);
		form.AddField("rid", rid);
		form.AddField("mid", "");
		form.AddField("id1", "");
		form.AddField("id2", "");
		form.AddField("time", "");
		form.AddField("mtext", "");
		UnityWebRequest www = UnityWebRequest.Post(url, form);
		yield return www.SendWebRequest();
		File.WriteAllText(Application.persistentDataPath + "/MemJson.txt", www.downloadHandler.text);
		string rdata = File.ReadAllText(Application.persistentDataPath + "/MemJson.txt");
		mtextlist.Clear();
		mtextlist = JsonConvert.DeserializeObject<List<Memtext>>(rdata);
		memtextload();
	}
	public void onendedit()
	{
		if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
		{
			if (!string.IsNullOrEmpty(this.inputField.text))
			{
				sendinput(this.inputField.text);
			}
			this.inputField.text = "";
			this.inputField.Select();
			this.inputField.ActivateInputField();
		}
	}
	public void sendinput(string inputLine)
	{
		DateTime dt = DateTime.Now;
		StartCoroutine(MsendCoroutine("mtextsend", rid, userName, otherName, dt.ToString("yyyy-MM-dd-HH-mm"), inputLine));
	}
	void call()
	{
		StartCoroutine(McheckCoroutine("mtextcheck"));
	}
	public void memtextload()
	{
		for (int i = midcount; i < mtextlist.Count; i++)
		{
			if (mtextlist[i].id1 == userName)
			{
				memtextpaint(true, mtextlist[i].mtext, mtextlist[i].id2, null, mtextlist[i].time);
			}
			else
			{
				memtextpaint(false, mtextlist[i].mtext, mtextlist[i].id1, null, mtextlist[i].time);
			}
		}
		midcount = mtextlist.Count;
	}
	public void memtextpaint(bool isSend, string text, string user, Texture picture, string savedtime)
	{
		//if (text.Trim() == "") return; // 공백 문자열은 제외
		// 현재 스크롤바가 맨 밑에 위치해있는지
		bool isBottom = scrollBar.value <= 0.0001f;
		// 보내는 사람은 노랑, 받는 사람은 흰색영역을 만들어 텍스트 대입
		AreaScript Area = Instantiate(isSend ? YellowArea : WhiteArea).GetComponent<AreaScript>();
		Area.transform.SetParent(ContentRect.transform, false);
		Area.BoxRect.sizeDelta = new Vector2(180, Area.BoxRect.sizeDelta.y);
		Area.TextRect.GetComponent<Text>().text = text;
		Fit(Area.BoxRect);
		// 가로길이 설정 (max 너비에 비해 짧을 경우 대비)
		float X = Area.TextRect.sizeDelta.x + 21;
		float Y = Area.TextRect.sizeDelta.y;
		// 한 줄 초과
		if (Y > 15)
		{
			for (int i = 0; i < 200; i++)
			{
				Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
				Fit(Area.BoxRect);
				if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
			}
		}
		// 한 줄
		else Area.BoxRect.sizeDelta = new Vector2(X, Y);
		// 메세지 옆 시간 대입
		DateTime t = DateTime.ParseExact(savedtime, "yyyy-MM-dd-HH-mm", System.Globalization.CultureInfo.InvariantCulture);
		Area.Time = savedtime;
		Area.User = user;
		int hour = t.Hour;
		if (hour == 0) hour = 12;
		else if (hour > 12) hour -= 12;
		Area.TimeText.text = (t.Hour < 12 ? "오전 " : "오후 ") + hour + ":" + t.ToString("mm");
		// 이전 시간과 같으면 시간꼬리 없애기
		bool isSame = LastArea != null && LastArea.Time == Area.Time && LastArea.User == Area.User;
		if (isSame) LastArea.TimeText.text = "";
		Area.Tail.SetActive(!isSame);
		// - 받은 메세지의 경우 이름, 프로필 사진도 없애기
		if (!isSend)
		{
			//Area.UserImage.gameObject.SetActive(!isSame); - padding 설정때문에 수정중
			//Area.UserText.gameObject.SetActive(!isSame);
			Area.UserText.text = user;
		}
		// 날짜 박스: 이전 것과 날짜가 다르면 날짜영역 보이기
		if (LastArea == null || LastArea.Time.Substring(0, 10) != Area.Time.Substring(0, 10))
		{
			AreaScript dateArea = Instantiate(DateArea).GetComponent<AreaScript>();
			dateArea.transform.SetParent(ContentRect.transform, false);
			dateArea.transform.SetSiblingIndex(dateArea.transform.GetSiblingIndex() - 1);
			string week = "";
			switch (t.DayOfWeek)
			{
				case DayOfWeek.Sunday: week = "일"; break;
				case DayOfWeek.Monday: week = "월"; break;
				case DayOfWeek.Tuesday: week = "화"; break;
				case DayOfWeek.Wednesday: week = "수"; break;
				case DayOfWeek.Thursday: week = "목"; break;
				case DayOfWeek.Friday: week = "금"; break;
				case DayOfWeek.Saturday: week = "토"; break;
			}
			dateArea.TextRect.GetComponent<Text>().text = t.Year + "년 " + t.Month + "월 " + t.Day + "일 " + week + "요일";
		}
		LastArea = Area;
		if (!isSend && !isBottom) return;
		Invoke("ScrollDelay", 0.1f);
	}
	void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);
	void ScrollDelay() => scrollBar.value = 0;
}