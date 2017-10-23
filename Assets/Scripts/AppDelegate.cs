using UnityEngine;
using System.Collections;
using XLua;

public class AppDelegate : MonoBehaviour
{
	void Start()
	{
        XLua.LuaEnv luaenv = new XLua.LuaEnv();
        luaenv.DoString("CS.UnityEngine.Debug.Log('hello world')");
        luaenv.Dispose();
    }
}