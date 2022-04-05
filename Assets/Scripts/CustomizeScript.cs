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
public class item
{
    public item(string _type, string _name, bool _isusing)
    {
        type = _type; name = _name; isusing = _isusing;
    }
    public string type, name;
    public bool isusing;
}
public class CustomizeScript : MonoBehaviour
{
    [SerializeField] string url;
    public List<item> itemlist, savelist;
    public GameObject itemclick;
    public bool done;
    void Start()
    {
        done = false;
        Customizeitemlist();
        StartCoroutine(CustomizeCoroutine("customizeload"));
        ItemJsonToitemlist();
    }
    public void CustomizeItemClick()
    {
        itemclick = EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < itemlist.Count; i++)
        {
            if (itemclick.name.Equals(itemlist[i].name))
            {
                for (int j = 0; j < itemlist.Count; j++)
                {
                    if (itemlist[i].type.Equals(itemlist[j].type))
                    {
                        itemlist[j].isusing = false;
                    }
                }
                itemlist[i].isusing = true;
                break;
            }
        }
    }
    public void CustomizeButton1Click()
    {
        itemlistToItemJson();
        StartCoroutine(CustomizeCoroutine("customizesave"));
    }
    public void Customizeitemlist()
    {
        itemlist.Clear();
        itemlist.Add(new item("ia", "ia1", false)); itemlist.Add(new item("ia", "ia2", false));itemlist.Add(new item("ia", "ia3", false));
        itemlist.Add(new item("ib", "ib1", false)); itemlist.Add(new item("ib", "ib2", false)); itemlist.Add(new item("ib", "ib3", false));
        itemlist.Add(new item("ic", "ic1", false)); itemlist.Add(new item("ic", "ic2", false)); itemlist.Add(new item("ic", "ic3", false));
        itemlist.Add(new item("id", "id1", false)); itemlist.Add(new item("id", "id2", false)); itemlist.Add(new item("id", "id3", false));
        itemlist.Add(new item("ie", "ie1", false)); itemlist.Add(new item("ie", "ie2", false)); itemlist.Add(new item("ie", "ie3", false));
    }
    IEnumerator CustomizeCoroutine(string command)
    {
        WWWForm form = new WWWForm();
        form.AddField("command", command);
        form.AddField("id", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
        form.AddField("password", "");
        form.AddField("nickname", "");
        form.AddField("item", File.ReadAllText(Application.persistentDataPath + "/ItemJson.txt"));
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        string result = UnityWebRequest.UnEscapeURL(www.downloadHandler.text);
        if (command.Contains("load"))
        {
            print(result);
            File.WriteAllText(Application.persistentDataPath + "/ItemJson.txt", result);
            done = true;
        }
    }
    public void ItemJsonToitemlist()
    {
        if (!done) { for (int i = 0; i < 60; i++) { } }
        done = false;
        string rdata = File.ReadAllText(Application.persistentDataPath + "/ItemJson.txt");
        string[] element = rdata.Split(new string[] { "," }, StringSplitOptions.None);
        for (int i = 0; i < element.Length; i++)
        {
            for (int j = 0; j < itemlist.Count; j++)
            {
                if (element[i].Equals(itemlist[j].name))
                {
                    itemlist[j].isusing = true;
                    j = itemlist.Count;
                }
            }
        }
    }
    public void itemlistToItemJson()
    {
        savelist = itemlist.FindAll(x => x.isusing == true);
        StringBuilder wdata = new StringBuilder();
        for(int i = 0; i < savelist.Count; i++)
        {
            wdata.Append(savelist[i].name);
            if (i < savelist.Count - 1)
            {
                wdata.Append(",");
            }
        }
        File.WriteAllText(Application.persistentDataPath + "/ItemJson.txt", wdata.ToString());
    }
}