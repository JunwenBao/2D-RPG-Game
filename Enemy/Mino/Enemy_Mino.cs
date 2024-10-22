using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class Enemy_Mino : Enemy
{
    public Mino_Idle_State   idleState    { get; set; }
    public Mino_Move_State   moveState    { get; set; }
    public Mino_Attack_State attackState  { get; set; }
    public Mino_Battle_State battleState  { get; set; }
    public Mino_Die_State    dieState     { get; set; }

    protected override void Awake()
    {
        base.Awake();

        idleState   = new Mino_Idle_State  (this, stateMachine, "Idle", this);
        moveState   = new Mino_Move_State  (this, stateMachine, "Move", this);
        attackState = new Mino_Attack_State(this, stateMachine, "Attack", this);
        battleState = new Mino_Battle_State(this, stateMachine, "Move", this);
        dieState    = new Mino_Die_State   (this, stateMachine, "Die", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        if(GetComponent<EnemyStats>().getCurrentHealth() < 0 && !isDead)
        {
            stateMachine.ChangeState(dieState);
            isDead = true;
        }
    }
}
