using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;

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


    public void AddUnit(Base_NW.Team team, Transform unit)
    {
        switch (team)
        {
            case Base_NW.Team.Neutral:
                unitNeutral.Add(unit);
                break;

            case Base_NW.Team.Player1:
                //unitPlayer1.Add(unit);
                break;

            case Base_NW.Team.Player2:
                //unitPlayer2.Add(unit);
                break;

            default:
                break;

        }
    }

    public void AddStructure(Base_NW.Team team, Transform structure)
    {
        switch (team)
        {
            case Base_NW.Team.Neutral:
                //structurePlayer1.Add(structure);
                break;

            case Base_NW.Team.Player1:
                //structurePlayer2.Add(structure);
                break;

            case Base_NW.Team.Player2:
                //structureNeutral.Add(structure);
                break;

            default:
                break;

        }
    }

}
