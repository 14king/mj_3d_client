using UnityEngine;
using System.Collections;
using SLua;

public class AppDelegate : MonoBehaviour
{
	private static LuaState ls_state = new LuaState();
	
	void Start()
	{
		ls_state.doString("print(\"hello lua!\")");
	}
}