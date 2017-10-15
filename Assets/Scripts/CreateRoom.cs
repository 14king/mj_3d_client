using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class CreateRoom : MonoBehaviour {
	// Use this for initialization
	void Start () {
        transform.FindChild("create").GetComponent<Button>().onClick.AddListener(CreateRoomClick);
        transform.FindChild("cancel").GetComponent<Button>().onClick.AddListener(CancelClick);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void CreateRoomClick()
    {
        StartCoroutine(sendCreateRoom());
    }

    private IEnumerator sendCreateRoom() {
        string url = Account.GetInstance().http_url;
        url = url + "create_room?account=" + Account.GetInstance().account_;
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
            if (errmsg == "success")
            {
                Account.GetInstance().room_id = int.Parse(obj["id"].ToString());
                Account.GetInstance().room_number = int.Parse(obj["number"].ToString());
                Account.GetInstance().room_ip = obj["ip"].ToString();
                Account.GetInstance().room_port = int.Parse(obj["port"].ToString());

                NetWork.NetClient.Instance().Connect(Account.GetInstance().room_ip, Account.GetInstance().room_port);
            }
            else
            {
                Debug.Log("创建房间失败：" + errmsg);
            }
        }
    }

    private void CancelClick() {
        gameObject.SetActive(false);
    }
}
