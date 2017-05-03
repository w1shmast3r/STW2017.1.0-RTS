using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class Base_NW : NetworkBehaviour 
{
    
    public enum Team { Player1, Player2, Neutral };

    [Header("General")]
    public Team team = Team.Player2;
    public string className = "Unnamed";
    public string description = "Purpose of this unit/structure.";

    [Header("Attributes")]
    public float maxHealth = 1f; //start/max health
    public float armor = 0f; //armor, used for damage reduction
    private float curHealth; //current health

    [Header("VFX")]
    public EffectRecycler.EffectType explosionType = EffectRecycler.EffectType.ExplosionSmall;

    //is unit/structure destroyed
    public bool IsDead { get; set; }

    //navmesh agent for units
    public NavMeshAgent Agent { get; set; }
    
    public Transform MyTransform { get; set; }

    protected LayerMask attackMask;


    // Use this for initialization
    public void Start ()
    {
        Initialize ();
        InitBase();
    }

    public void Initialize ()
    {
        //if ((TeamHandler_NW.teamHandler.unitPlayer1.Contains (GetComponent<NetworkIdentity> ().netId.Value))
        //	|| (TeamHandler_NW.teamHandler.structurePlayer1.Contains (GetComponent<NetworkIdentity> ().netId.Value)))
        //	team = Team.Player1;
        //else if ((TeamHandler_NW.teamHandler.unitPlayer2.Contains (GetComponent<NetworkIdentity> ().netId.Value))
        //	|| (TeamHandler_NW.teamHandler.structurePlayer2.Contains (GetComponent<NetworkIdentity> ().netId.Value)))
        //	team = Team.Player2;

        var netID = GetComponent<NetworkIdentity>().netId.Value;
        team = TeamHandler_NW.teamHandler.GetMyTeam(netID);

    	attackMask = 1 << LayerMask.NameToLayer ("Active");

    	if (team == LevelController.myTeam) {
    		if (GetComponent<NavMeshObstacle> () != null) {
    			GetComponent<NavMeshObstacle> ().enabled = false;
    			Agent = GetComponent<NavMeshAgent> ();
    			Agent.enabled = true;
    			LevelController.RegisterUnit (gameObject);
    			//Agent.SetDestination (LevelController.meetPoint);
                Agent.SetDestination(LevelController.meetPoint);
            }
            else {
    			LevelController.HideMenu ();
    		}
    	}	
    }


    protected void InitBase()
    {
        curHealth = maxHealth;
        Agent = GetComponent<NavMeshAgent>();
        MyTransform = transform;

        Material mat = GetComponent<Renderer>().material;

        Renderer[] visuals = transform.GetComponentsInChildren<Renderer>();

        Color minimapColor = Color.clear;


        switch (team)
        {
            case Team.Neutral:
                gameObject.tag = "Neutral";
                mat.SetColor("_Color", TeamHandler_NW.teamHandler.colorNeutral);
                minimapColor = TeamHandler_NW.teamHandler.colorNeutral;
                break;

            case Team.Player1:
                gameObject.tag = "Player1";
                mat.SetColor("_Color", TeamHandler_NW.teamHandler.colorPlayer1);
                minimapColor = TeamHandler_NW.teamHandler.colorPlayer1;
                break;

            case Team.Player2:
                gameObject.tag = "Player2";
                mat.SetColor("_Color", TeamHandler_NW.teamHandler.colorPlayer2);
                minimapColor = TeamHandler_NW.teamHandler.colorPlayer2;
                break;

            default:
                break;
        }

        foreach (Renderer rend in visuals)
        {
            if (!rend.CompareTag("MiniMapBlob"))
                rend.material = mat;
            else
                rend.material.SetColor("_Color", minimapColor);
        }
    }


    void OnEnable()
    {
        IsDead = false;
    }

    public bool TakeDamage(float damage)
    {
        bool isDestroyed = false;

        //subtract damage from armor value
        if (damage > armor)
            damage -= armor;
        
        //if damage is lower than armor, ensure at least 1 damage is done
        else
            damage = 1f;

        //take damage
        curHealth -= damage;

        //
        if (curHealth <= 0)
        {
            isDestroyed = true;
            IsDead = isDestroyed;
            Destroy();
        }
            

        return isDestroyed;
    }

    //kill off unit
    private void Destroy()
    {
        GameObject explosion = EffectRecycler.effectRecycler.GetEffect(explosionType);
        explosion.transform.position = transform.position + Vector3.up*0.5f;
        explosion.SetActive(true);
        LevelController.playerUnits.Remove(gameObject);
        if (hasAuthority)
            CmdDestroyNW();
    }

    [Command]
    public void CmdDestroyNW()
    {
        Destroy(gameObject);
    }
}
