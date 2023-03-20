using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFire : MonoBehaviour
{
    public BossEnemy boss;
    public LDHNetPlayer player;
    public bool isPossible;

    void Awake()
    {
        boss = GameObject.FindGameObjectWithTag("Enemy").GetComponent<BossEnemy>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<LDHNetPlayer>();

    }

    
    void Update()
    {
        
    }
    

    void OnParticleCollision(GameObject other)
    {
        
            boss.Fired();
    }
    
}
