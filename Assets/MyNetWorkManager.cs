/************************
 * Title:
 * Function：
 * 	－ 网络管理
 * Used By：	NetworkManager
 * Author:	qwt
 * Date:	
 * Version:	1.0
 * Description:
 *
************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MyNetWorkManager : NetworkManager {

    public Text textIP;
    private int chosenCharacter = 0;

    private string ipAddress;//IP地址

    //子类发送网络消息
    public class NetworkMessage : MessageBase
    {
        public int chosenClass;
    }

    //在服务器端添加playerprefab
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        NetworkMessage message = extraMessageReader.ReadMessage<NetworkMessage>();
        int selectedClass = message.chosenClass;

        if (selectedClass==0)
        {
            GameObject player = Instantiate(Resources.Load("Player")) as GameObject;
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
        if (selectedClass==1)
        {
            GameObject player = Instantiate(Resources.Load("Enemy")) as GameObject;
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
    }

    //当客户端连接时，在客户端添加Player
    public override void OnClientConnect(NetworkConnection conn)
    {
        NetworkMessage test = new NetworkMessage();
        test.chosenClass = chosenCharacter;

        ClientScene.AddPlayer(client.connection, 0, test);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        //base.OnClientSceneChanged(conn);
    }

    //选择角色按钮1
    public void Btn1()
    {
        chosenCharacter = 0;
    }

    //选在角色按钮2
    public void Btn2()
    {
        chosenCharacter = 1;
    }

    //创建游戏按钮事件
    public void StartMyHost()
    {
        SetMyPort();
        NetworkManager.singleton.StartHost();
    }

    //加入游戏按钮事件
    public void JoinGame()
    {
        SetMyPort();
        SetMyIpAddress();
        NetworkManager.singleton.StartClient();
    }

    //设置端口号
    private void SetMyPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    //设置IP地址
    void SetMyIpAddress()
    {
        ipAddress = textIP.text;
        NetworkManager.singleton.networkAddress = ipAddress;
    }
}
