using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class Base_NW : NetworkBehaviour 
{
    public enum Team { Player1, Player2, Neutral };

    [Header("General")]
	[SyncVar]
    public Team team;
	[SyncVar]
	public Color teamColor;
    public string className = "Unnamed";
    public string description = "Purpose of this unit/structure.";

    [Header("Attributes")]
    public float maxHealth = 1f; //start/max health
    public float armor = 0f; //armor, used for damage reduction
    public HealthBar healthBar;
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

        if (healthBar != null)
            healthBar.Init(maxHealth);
    }

    public void Initialize ()
    {   
    	attackMask = 1 << LayerMask.NameToLayer ("Active");

    	if (team == LevelController.myTeam) {
    		if (GetComponent<NavMeshObstacle> () != null) {
    			GetComponent<NavMeshObstacle> ().enabled = false;
    			Agent = GetComponent<NavMeshAgent> ();
    			Agent.enabled = true;
    			LevelController.RegisterUnit (gameObject);
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

		mat.SetColor("_Color", teamColor);
		minimapColor = teamColor;
        switch (team)
        {
            case Team.Neutral:
                gameObject.tag = "Neutral";
                break;

            case Team.Player1:
                gameObject.tag = "Player1";
                break;

            case Team.Player2:
                gameObject.tag = "Player2";
                break;

            default:
                break;
        }

        foreach (Renderer rend in visuals)
        {
            if (rend.CompareTag("MiniMapBlob"))
                rend.material.SetColor("_Color", minimapColor);
            else if (!rend.CompareTag("HealthBar"))
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

        if (healthBar != null)
            healthBar.AdjustHealth(-damage);

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
        if (GetComponent<Unit_NW>() == null)
        {

            LevelController.GameOver.SetActive(true);
            if (team == LevelController.myTeam)
				LevelController.GameOver.GetComponentInChildren<UnityEngine.UI.Text>().text = "You loose!";
            else
				LevelController.GameOver.GetComponentInChildren<UnityEngine.UI.Text>().text = "You win!";
                
        }

        if (hasAuthority)
            CmdDestroyNW(team);
    }

    [Command]
    public void CmdDestroyNW(Base_NW.Team winner)
    {
        Destroy(gameObject);
    }
}
