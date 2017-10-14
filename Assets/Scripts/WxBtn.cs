using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;

public class WxBtn : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClieck()
    {
        Debug.Log("按钮被按下了！");
        StartCoroutine(SendGet());
    }

    public IEnumerator SendPost(string _url, WWWForm _wForm)
    {
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            Debug.Log(postData.text);
        }
    }

    IEnumerator SendGet()
    {
        string url = Account.GetInstance().http_url;
        url = url + "account=" + Account.GetInstance().account_ + "&password=" + Account.GetInstance().password_;
        WWW getData = new WWW(url);
        yield return getData;
        if (getData.error != null)
        {
            Debug.Log(getData.error);
        }
        else
        {
            Debug.Log(getData.text);
            JObject obj = JObject.Parse(getData.text);

            string errmsg = obj["errmsg"].ToString();
            if(errmsg == "success"){
                string token = obj["token"].ToString();
                Account.GetInstance().token = token;
                Debug.Log("登陆成功：token=" + token);
                SceneManager.LoadScene("hall");
            }
            else{
                Debug.Log("登陆失败：" + errmsg);
            }
        }
    }
}
