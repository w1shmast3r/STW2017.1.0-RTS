using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

class HelloWorld : MessageBase
{
    public static short messageID = 101;
    public string test;
}

class DamageTaken: MessageBase
{
    public static short messageID = 102;
    public int damage;
}

class UnitMovement : MessageBase
{
    public static short messageID = 103;
    //which unit
    public Vector3 destination;
}

public class MessageDB : MonoBehaviour {



}
