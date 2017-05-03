using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamHandler : MonoBehaviour
{

    public static TeamHandler teamHandler;

    public Color colorPlayer1;
    public Color colorPlayer2;
    public Color colorNeutral;

    //collection of units for all teams
    public List<Transform> unitPlayer1 = new List<Transform>();
    public List<Transform> unitPlayer2 = new List<Transform>();
    public List<Transform> unitNeutral = new List<Transform>();

    //collection of structures for all teams
    public List<Transform> structurePlayer1 = new List<Transform>();
    public List<Transform> structurePlayer2 = new List<Transform>();
    public List<Transform> structureNeutral = new List<Transform>();

    void Awake()
    {
        teamHandler = this;
    }
    

    public void AddUnit(Base.Team team, Transform unit)
    {
        switch (team)
        {
            case Base.Team.Neutral:
                unitNeutral.Add(unit);
                break;

            case Base.Team.My:
                unitPlayer1.Add(unit);
                break;

            case Base.Team.Enemy:
                unitPlayer2.Add(unit);
                break;

            default:
                break;

        }
    }

    public void AddStructure(Base.Team team, Transform structure)
    {
        switch (team)
        {
            case Base.Team.Neutral:
                structurePlayer1.Add(structure);
                break;

            case Base.Team.My:
                structurePlayer2.Add(structure);
                break;

            case Base.Team.Enemy:
                structureNeutral.Add(structure);
                break;

            default:
                break;

        }
    }

}
