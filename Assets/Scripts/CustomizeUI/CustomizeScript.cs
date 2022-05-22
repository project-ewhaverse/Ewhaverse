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
using TMPro;
using HSVPicker;
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
    public string[] typename = { "Skin", "Eye", "Mouse", "Hair", "Top", "Bottom", "Shoes", "Accessory" };
    public GameObject tabtype, viewentity;
    IEnumerator coroutine1, coroutine2;
    public bool done;
    [SerializeField] public TMP_Text er, eg, eb, hr, hg, hb;
    void Start()
    {
        //가입 시 DB에 "0.1/0.1/0.1,Eye0,0.1/0.1/0.1,Mou0,HaF0,HaB0,0.1/0.1/0.1,Top0,Bot0,Sho0,Acc0"이 저장된다
        //로그인 때에 DB로부터 CustomJson.txt에 저장해둔다
        //CustomJson.txt의 내용을 가공해서 customlist에 추가한다
        coroutine1 = CustomCoroutine("startcustom");
        coroutine2 = CustomCoroutine("confirmcustom");
        StartCoroutine(coroutine1);
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
            if (i == 0)
            {
                //버튼 이름으로 name 결정할 예정
                customlist.Add(new custominfo("SkinC", "0.1/0.1/0.1", false));
            }
            else if (i == 1)
            {
                for (int j = 0; j <= 4; j++)
                {
                    tempstring = "Eye" + j.ToString();
                    customlist.Add(new custominfo("Eye", tempstring, false));
                }
                //Picker 값으로 name 결정할 예정
                customlist.Add(new custominfo("EyeC", "0.1/0.1/0.1", false));
            }
            else if (i == 2)
            {
                for (int j = 0; j <= 4; j++)
                {
                    tempstring = "Mou" + j.ToString();
                    customlist.Add(new custominfo("Mouse", tempstring, false));
                }
            }
            else if (i == 3)
            {
                for (int j = 0; j <= 4; j++)
                {
                    tempstring = "HaF" + j.ToString();
                    customlist.Add(new custominfo("HairF", tempstring, false));
                }
                for (int j = 0; j <= 4; j++)
                {
                    tempstring = "HaB" + j.ToString();
                    customlist.Add(new custominfo("HairB", tempstring, false));
                }
                customlist.Add(new custominfo("HairC", "0.1/0.1/0.1", false));
            }
            else
            {
                for (int j = 0; j <= 2; j++)
                {
                    tempstring = typename[i].Substring(0, 3) + j.ToString();
                    customlist.Add(new custominfo(typename[i], tempstring, false));
                }
            }
        }
        string[] info = result.Split(new string[] { "," }, StringSplitOptions.None);
        //type-info[0]: SkinC, name-info[0]: "0.1/0.1/0.1", count-info[0]: 1, index-info[0]: 0
        customlist[0].name = info[0];
        customlist[0].isusing = true;
        //info[1]: Eye, Eye0, 5, 1-5
        int tempindex;
        tempindex = customlist.FindIndex(x => x.name == info[1]);
        customlist[tempindex].isusing = true;
        //info[2]: EyeC, "0.1/0.1/0.1", 1, 6
        customlist[6].name = info[2];
        customlist[6].isusing = true;
        //info[3]: Mouse, Mou0, 5, 7-11
        tempindex = customlist.FindIndex(x => x.name == info[3]);
        customlist[tempindex].isusing = true;
        //info[4]: HairF, HaF0, 5, 12-16
        tempindex = customlist.FindIndex(x => x.name == info[4]);
        customlist[tempindex].isusing = true;
        //info[5]: HairB, HaB0, 5, 17-21
        tempindex = customlist.FindIndex(x => x.name == info[5]);
        customlist[tempindex].isusing = true;
        //info[6]: HairC, "0.1/0.1/0.1", 1, 22
        customlist[22].name = info[6];
        customlist[22].isusing = true;
        //info[7]: Top, Top0, 3, 23-25
        tempindex = customlist.FindIndex(x => x.name == info[7]);
        customlist[tempindex].isusing = true;
        //info[8]: Bottom, Bot0, 3, 26-28
        tempindex = customlist.FindIndex(x => x.name == info[8]);
        customlist[tempindex].isusing = true;
        //info[9]: Shoes, Sho0, 3, 29-31
        tempindex = customlist.FindIndex(x => x.name == info[9]);
        customlist[tempindex].isusing = true;
        //info[10]: Accessory, Acc0, 3, 32-34
        tempindex = customlist.FindIndex(x => x.name == info[10]);
        customlist[tempindex].isusing = true;
        //그린다
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
    public void ModelEntityClicked()
    {
        //ListView~의 엔티티가 클릭되면 리스트에서 isusing을 바꾼다
        //info[1]: Eye, Eye0, 5, 1-5
        //info[3]: Mouse, Mou0, 5, 7-11
        //info[4]: HairF, HaF0, 5, 12-16
        //info[5]: HairB, HaB0, 5, 17-21
        viewentity = EventSystem.current.currentSelectedGameObject;
        int tmpindex;
        tmpindex = customlist.FindIndex(x => x.name == viewentity.name);
        if (tmpindex >= 1 & tmpindex <= 5)
        {
            for (int i = 1; i <= 5; i++)
            {
                customlist[i].isusing = false;
            }
        }
        else if (tmpindex >= 7 & tmpindex <= 11)
        {
            for (int i = 7; i <= 11; i++)
            {
                customlist[i].isusing = false;
            }
        }
        else if (tmpindex >= 12 & tmpindex <= 16)
        {
            for (int i = 12; i <= 16; i++)
            {
                customlist[i].isusing = false;
            }
        }
        else if (tmpindex >= 17 & tmpindex <= 21)
        {
            for (int i = 17; i <= 21; i++)
            {
                customlist[i].isusing = false;
            }
        }
        customlist[tmpindex].isusing = true;
        //그린다
    }
    public void DressEntityClicked()
    {
        //ListView~의 엔티티가 클릭되면 리스트에서 isusing을 바꾼다
        //info[7]: Top, Top0, 3, 23-25
        //info[8]: Bottom, Bot0, 3, 26-28
        //info[9]: Shoes, Sho0, 3, 29-31
        //info[10]: Accessory, Acc0, 3, 32-34
        viewentity = EventSystem.current.currentSelectedGameObject;
        int tmpindex1;
        tmpindex1 = customlist.FindIndex(x => x.name == viewentity.name);
        int tmpindex2;
        tmpindex2 = (tmpindex1 - 23) / 3;
        for (int i = 0; i <= 2; i++)
        {
            customlist[(tmpindex2 * 3) + i + 23].isusing = false;
        }
        customlist[tmpindex1].isusing = true;
        //그린다
    }
    public void SkinEntityClicked()
    {
        //ListView~의 엔티티가 클릭되면 리스트에서 name을 바꾼다
        //info[0]: SkinC, "0.1/0.1/0.1", 1, 0
        viewentity = EventSystem.current.currentSelectedGameObject;
        customlist[0].name = viewentity.name;
        //그린다
    }
    public void PickerEntityClicked()
    {
        //ListView~의 엔티티가 클릭되면 리스트에서 name을 바꾼다
        //info[2]: EyeC, "0.1/0.1/0.1", 1, 6
        //info[6]: HairC, "0.1/0.1/0.1", 1, 22
        viewentity = EventSystem.current.currentSelectedGameObject;
        string tempstring;
        string r, g, b;
        if (viewentity.name == "SliderER" | viewentity.name == "SliderEG" | viewentity.name == "SliderEB")
        {
            r = (float.Parse(er.text) / 255).ToString();
            g = (float.Parse(eg.text) / 255).ToString();
            b = (float.Parse(eb.text) / 255).ToString();
            tempstring = r + "/" + g + "/" + b;
            customlist[6].name = tempstring;
        }
        else if (viewentity.name == "SliderHR" | viewentity.name == "SliderHG" | viewentity.name == "SliderHB")
        {
            r = (float.Parse(hr.text) / 255).ToString();
            g = (float.Parse(hg.text) / 255).ToString();
            b = (float.Parse(hb.text) / 255).ToString();
            tempstring = r + "/" + g + "/" + b;
            customlist[22].name = tempstring;
        }
        //그린다
    }
    public void ConfirmClicked()
    {
        //확인되면 customlist의 내용을 가공해 CustomJson.txt에 저장한다
        //CustomJson.txt의 내용도 DB에 저장한다
        ConfirmProcess();
        StartCoroutine(coroutine2);
        //화면 전환한다
    }
    public void CancelClicked()
    {
        //취소되면 화면 전환한다
    }
}