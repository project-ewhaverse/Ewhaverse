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
public class custominfo
{
    public custominfo(string _type, string _name, bool _isusing)
    {
        type = _type; name = _name; isusing = _isusing;
    }
    public string type, name;
    public bool isusing;
}
public class CustomizeScript : MonoBehaviour
{
    [SerializeField] string url;
    public List<custominfo> customlist, confirmlist;
    public String[] typename = { "Skin", "Eye", "Mouse", "Hair", "Top", "Bottom", "Shoes", "Accessory" };
    public GameObject tabtype, viewentity;
    IEnumerator coroutine1, coroutine2;
    public bool done;
    void Start()
    {
        //가입 시 DB에 Ski1,Eye1,Mou1,Hai1,Top1,Bot1,Sho1,Acc1이 저장된다
        //로그인 때에 custominfo에 대한 list를 CustomJson.txt에 저장해둔다
        //CustomJson.txt를 불러와서 customlist에 추가한다
        coroutine1 = CustomCoroutine("startcustom");
        coroutine2 = CustomCoroutine("confirmcustom");
        StartCoroutine(coroutine1);
        //list를 참고하여 캐릭터를 그린다(다른 맵에서도 txt->list->draw)
    }
    IEnumerator CustomCoroutine(string command)
    {
        done = false;
        WWWForm form = new WWWForm();
        form.AddField("command", command);
        form.AddField("id", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
        form.AddField("password", "");
        form.AddField("email", "");
        form.AddField("item", File.ReadAllText(Application.persistentDataPath + "/CustomJson.txt"));
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        string result = www.downloadHandler.text;
        done = true;
        if (command.Contains("startcustom"))
        {
            StartProcess(result);
            StopCoroutine(coroutine1);
        }
        if (command.Contains("confirmcustom"))
        {
            StopCoroutine(coroutine2);
        }
    }
    public void StartProcess(string result)
    {
        //SkiN,~,AccN->customlist
        if (!done)
        {
            for (int i = 0; i < 60; i++) { }
        }
        string tempstring;
        for(int i = 0; i < typename.Length; i++)
        {
            for(int j = 1; j < 6; j++)
            {
                tempstring = typename[i].Substring(0, 3) + j.ToString();
                customlist.Add(new custominfo(typename[i], tempstring, false));
            }
        }
        string[] info = result.Split(new string[] { "," }, StringSplitOptions.None);
        int tempindex;
        for (int i = 0; i < info.Length; i++)
        {
            tempindex = customlist.FindIndex(x => x.name == info[i]);
            customlist[tempindex].isusing = true;
        }
    }
    public void ConfirmProcess()
    {
        //customlist->SkiN,~,AccN
        if (!done)
        {
            for (int i = 0; i < 60; i++) { }
        }
        confirmlist = customlist.FindAll(x => x.isusing == true);
        StringBuilder wdata = new StringBuilder();
        for (int i = 0; i < confirmlist.Count; i++)
        {
            wdata.Append(confirmlist[i].name);
            if (i < confirmlist.Count - 1)
            {
                wdata.Append(",");
            }
        }
        File.WriteAllText(Application.persistentDataPath + "/CustomJson.txt", wdata.ToString());
    }
    public void SelectClicked()
    {
        //처음에는 Model탭과 ViewSkin이 보여진다
        //Model선택: Inspector에서 Model이하 ON, Dress이하 OFF, List이하 ViewSkin만 ON
        //Dress선택: Inspector에서 Model이하 OFF, Dress이하 ON, List이하 ViewTop만 ON
        tabtype = EventSystem.current.currentSelectedGameObject;
        if (tabtype.name == "SelectModel")
        {
            transform.Find("Dress").gameObject.SetActive(false);
            transform.Find("Model").gameObject.SetActive(true);
            TypeClicked(transform.Find("Model").transform.Find("Skin").gameObject);
        }
        if (tabtype.name == "SelectDress")
        {
            transform.Find("Model").gameObject.SetActive(false);
            transform.Find("Dress").gameObject.SetActive(true);
            TypeClicked(transform.Find("Dress").transform.Find("Top").gameObject);
        }
    }
    public void TypeClicked(GameObject obj)
    {
        //Skin선택: List이하 ViewSkin만 ON
        //Eye선택: List이하 ViewEye만 ON
        //Mouse선택: List이하 ViewMouse만 ON
        //Hair선택: List이하 ViewHair만 ON
        //Top선택: List이하 ViewTop만 ON
        //Bottom선택: List이하 ViewBottom만 ON
        //Shoes선택: List이하 ViewShoes만 ON
        //Accessory선택: List이하 ViewAccessory만 ON
        tabtype = EventSystem.current.currentSelectedGameObject;
        if (obj.name == "Skin" || obj.name == "Top")
        {
            tabtype = obj;
        }
        string tmpstring;
        for (int i = 0; i < typename.Length; i++)
        {
            tmpstring = "View" + typename[i];
            transform.Find("List").transform.Find(tmpstring).gameObject.SetActive(false);
        }
        tmpstring = "View" + tabtype.name;
        transform.Find("List").transform.Find(tmpstring).gameObject.SetActive(true);
    }
    public void EntityClicked()
    {
        //ListView~의 엔티티가 클릭되면 리스트에서 isusing을 바꾼다
        viewentity = EventSystem.current.currentSelectedGameObject;
        int tmpindex1;
        tmpindex1 = customlist.FindIndex(x => x.name == viewentity.name);
        int tmpindex2;
        tmpindex2 = tmpindex1 / 5;
        for (int i = 0; i < 5; i++)
        {
            customlist[(tmpindex2 * 5) + i].isusing = false;
        }
        customlist[tmpindex1].isusing = true;
        //캐릭터 미리보기를 다시 그린다
    }
    public void ConfirmClicked()
    {
        //확인되면 customlist의 내용을 CustomJson.txt에 저장한다
        //customlist의 내용을 가공해 DB에 저장한다
        ConfirmProcess();
        StartCoroutine(coroutine2);
        //이전 화면으로 이동한다
    }
    public void CancelClicked()
    {
        //취소되면 이전 화면으로 이동한다
    }
}