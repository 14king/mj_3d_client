using UnityEngine;
using System.Collections;

public class Account {

	private static Account instance_;
    public string http_url = "http://222.73.139.48:4001/";
    public string account_ = "yun";
    public string password_ = "fdse";
    public string nickname_ = "";
    public int roomcard_ = 0;
    public string token = "";

   
    public int room_id = 0;           // 房间id
    public int room_number = 0;  // 房间号
    public string room_ip = "";      // 房间网关ip
    public int room_port = 0;         // 房间网关端口

    private Account()
    {

    }

    public static Account GetInstance()
    {
        instance_  = instance_ ?? new Account();
        return instance_;
    }
}
