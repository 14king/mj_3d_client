using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hall : MonoBehaviour {
	Button create_low_btn;
	Button create_middle_btn;
	Button create_high_btn;

	// Use this for initialization
	void Start () {
		create_low_btn = transform.FindChild("create_low_btn").GetComponent<Button>();
		create_middle_btn = transform.FindChild("create_middle_btn").GetComponent<Button>();
		create_high_btn = transform.FindChild("create_high_btn").GetComponent<Button>();

		create_low_btn.onClick.AddListener(OnClickCreateLow);
		create_middle_btn.onClick.AddListener(OnClickCreateMiddle);
		create_high_btn.onClick.AddListener(OnClickCreateHigh);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnClickCreateLow()
	{
		Debug.Log("创建新手房");
        transform.FindChild("create_room_panel").gameObject.SetActive(true);
    }

	private void OnClickCreateMiddle()
	{
		Debug.Log("创建中级房");
	}

	private void OnClickCreateHigh()
	{
		Debug.Log("创建高级房");
	}
}
