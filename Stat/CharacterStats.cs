using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelegence,
    vitality,
    damage,
    critChance,
    critPower,
    maxHealth,
    armor,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightingDamage
}

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;
    #region Information
    [Header("Major Stats")] 
    public Stat strength;      //1 point increase damage by 1%
    public Stat agility;       //1 point increase evasion by 1%
    public Stat intelligence;  //1 point increase magic damage + resistance by 1%
    public Stat vitality;      //1 point increase health by 1%

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;

    [Header("Defencive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;//魔抗

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    [Header("BUFF")]
    public bool isIgnited;  //Damage over time
    public bool isChilled;  //Reduce armor 20%
    public bool isShocked;  //reduce acurecy 20%

    [Header("Current Stats")]
    [SerializeField] public int currentHealth;
    #endregion

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;
    [SerializeField] private float ailmentDuration = 4;

    private float igniteDamageCoolDown = .3f;
    private float igniteDamageTimer;
    private int   igniteDamage; //火焰伤害值

    public System.Action onHealthChanged;

    public bool isDead = false; //记录角色是否死亡

    protected virtual void Start()
    {
        critPower.setDefaultValue(150);
        currentHealth = getMaxHealthValue();
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0) isIgnited = false;
        if (chilledTimer < 0) isChilled = false;
        if (shockedTimer < 0) isShocked = false;

        if (igniteDamageTimer < 0 && isIgnited)
        {
            Debug.Log("Take BURN DAMAGE" + igniteDamage);

            DecreaseHealthBy(igniteDamage);

            currentHealth -= igniteDamage;
            if (currentHealth < 0) Die();

            igniteDamageTimer = igniteDamageCoolDown;
        }
    }

    //计算对敌人造成的伤害：
    #region Do Damage
    //计算常规伤害
    public virtual void doDamage(CharacterStats _targetStats)
    {
        //1.根据敌人闪避值，判断攻击能否命中
        if(TargetCanAvoidAttack(_targetStats)) return;

        //2.计算对对手可造成的伤害总值"totalDamage"
        //2.1：根据角色自身的伤害和力量初步计算造成的伤害值
        int totalDamage = damage.getValue() + strength.getValue();  //计算可以造成的总伤害

        //2.2：加上暴击伤害
        if (canCrit())
        {
            totalDamage = calculateCriticalDamage(totalDamage);
            Debug.Log("critical Damage" + totalDamage);
        }

        //2.3减去对手护甲抵挡的伤害
        //如果对手身上有chill的debuff，则削减护甲，增加伤害
        if(_targetStats.isChilled) totalDamage -= Mathf.RoundToInt(_targetStats.armor.getValue() * .8f);
        else totalDamage -= _targetStats.armor.getValue();

        totalDamage -= _targetStats.armor.getValue();               //根据敌人护甲，减少造成的伤害总值
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);    //造成伤害的值不能为负数，这样相对于治疗对手

        //3.最终，让对手遭到伤害
        _targetStats.takeDamage(totalDamage);
        doMagicDamage(_targetStats);
    }

    //计算魔法伤害
    public virtual void doMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage     = fireDamage.getValue();
        int _iceDamage      = iceDamage.getValue();
        int _lightingDamage = lightingDamage.getValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.getValue();
        totalMagicalDamage -= _targetStats.magicResistance.getValue() + (_targetStats.intelligence.getValue()*3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage,0, int.MaxValue);

        _targetStats.takeDamage(totalMagicalDamage);

        //防止下方出现无限循环
        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0) return;

        //3.1计算三种属性魔法攻击中，哪个造成的伤害最高，赋予敌人相应的DEBUFF
        bool canApplyIgnite  = _fireDamage > _iceDamage      && _fireDamage > _lightingDamage;
        bool canApplyChilled = _iceDamage > _fireDamage      && _iceDamage > _lightingDamage;
        bool canApplyShock   = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        //3.2如果出现了多个元素伤害相同的情况，则通过摇色子的方法决定DEBUFF
        while(!canApplyIgnite && !canApplyChilled && !canApplyShock)
        {
            if(UnityEngine.Random.value < .3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChilled, canApplyShock);
                return;
            }

            if(UnityEngine.Random.value < .4f && _iceDamage > 0)
            {
                canApplyChilled = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChilled, canApplyShock);
                return;
            }

            if (UnityEngine.Random.value < .5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChilled, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        //4.为角色装上DEBUFF
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChilled, canApplyShock);
    }
    #endregion

    //判断：角色是否受到元素伤害
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked) return;

        if(_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentDuration; //计时，记录进入状态的时间

            fx.IgniteFXFor(ailmentDuration); //让角色进入被点燃状态时，引用fx中对应的闪烁,参数为闪烁时间
        }

        if(_chill)
        {
            isChilled = _chill;
            chilledTimer = ailmentDuration;

            float slowPercentage = .2f;
            GetComponent<Entity>().slowEntityBy(slowPercentage, ailmentDuration);
            fx.ChillFXFor(ailmentDuration);
        }

        if(_shock)
        {
            isShocked = _shock;
            shockedTimer = ailmentDuration;

            fx.shockFXFor(ailmentDuration);
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;


    //计算应当承受的伤害
    public virtual void takeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        if (currentHealth < 0) Die();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        if (onHealthChanged != null) onHealthChanged();
    }

    protected virtual void Die()
    {
        //throw new NotImplementedException();
    }

    //判断角色是否会躲避敌人的伤害
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvation = _targetStats.evasion.getValue() + _targetStats.agility.getValue();

        if (isShocked) totalEvation += 20;  //如果有SHOCK的buff，则增加命中率

        if (UnityEngine.Random.Range(0, 100) < totalEvation)
        {
            Debug.Log("Attack avoid");
            return true;
        }

        return false;
    }

    //判断角色是否会造成暴击伤害
    private bool canCrit()
    {
        int totalCriticalChance = critChance.getValue() + agility.getValue();

        if(UnityEngine.Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    //计算暴击伤害
    private int calculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.getValue() + strength.getValue()) * .01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    //获取玩家最高生命值
    public int getMaxHealthValue()
    {
        return maxHealth.getValue() + vitality.getValue() * 5;
    }

    public Stat statToModiyfy(StatType _statType)
    {
        if (_statType == StatType.strength)             return strength;
        else if (_statType == StatType.agility)         return agility;
        else if (_statType == StatType.intelegence)     return intelligence;
        else if (_statType == StatType.vitality)        return vitality;
        else if (_statType == StatType.damage)          return damage;
        else if (_statType == StatType.critChance)      return critChance;
        else if (_statType == StatType.critPower)       return critPower;
        else if (_statType == StatType.maxHealth)       return maxHealth;
        else if (_statType == StatType.armor)           return armor;
        else if (_statType == StatType.evasion)         return evasion;
        else if (_statType == StatType.magicRes)        return magicResistance;
        else if (_statType == StatType.fireDamage)      return fireDamage;
        else if (_statType == StatType.iceDamage)       return iceDamage;
        else if (_statType == StatType.lightingDamage)  return lightingDamage;

        return null;
    }

    public Stat getStat(StatType _statType)
    {
        if (_statType == StatType.strength)             return strength;
        else if (_statType == StatType.agility)         return agility;
        else if (_statType == StatType.intelegence)     return intelligence;
        else if (_statType == StatType.vitality)        return vitality;
        else if (_statType == StatType.damage)          return damage;
        else if (_statType == StatType.critChance)      return critChance;
        else if (_statType == StatType.critPower)       return critPower;
        else if (_statType == StatType.maxHealth)       return maxHealth;
        else if (_statType == StatType.armor)           return armor;
        else if (_statType == StatType.evasion)         return evasion;
        else if (_statType == StatType.magicRes)        return magicResistance;
        else if (_statType == StatType.fireDamage)      return fireDamage;
        else if (_statType == StatType.iceDamage)       return iceDamage;
        else if (_statType == StatType.lightingDamage)  return lightingDamage;

        return null;
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }
}