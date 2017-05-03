using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamHandler_NW : NetworkBehaviour
{
    public static TeamHandler_NW teamHandler;

    public Color colorPlayer1;
    public Color colorPlayer2;
    public Color colorNeutral;

    //collection of units for all teams

    public SyncListUInt unitPlayer1 = new SyncListUInt();
    public SyncListUInt unitPlayer2 = new SyncListUInt();
    public List<Transform> unitNeutral = new List<Transform>();

    //collection of structures for all teams
    public SyncListUInt structurePlayer1 = new SyncListUInt();
    public SyncListUInt structurePlayer2 = new SyncListUInt();
    public List<Transform> structureNeutral = new List<Transform>();

    void Awake()
    {
        teamHandler = this;
    }

    public Base_NW.Team GetMyTeam(uint id)
    {
        if (unitPlayer1.Contains(id) || structurePlayer1.Contains(id))
            return Base_NW.Team.Player1;
        else if (unitPlayer2.Contains(id) || structurePlayer2.Contains(id))
            return Base_NW.Team.Player2;
        else return Base_NW.Team.Neutral;
    }
}
