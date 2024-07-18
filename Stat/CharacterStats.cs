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
    public Stat magicResistance;//ħ��

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
    private int   igniteDamage; //�����˺�ֵ

    public System.Action onHealthChanged;

    public bool isDead = false; //��¼��ɫ�Ƿ�����

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

    //����Ե�����ɵ��˺���
    #region Do Damage
    //���㳣���˺�
    public virtual void doDamage(CharacterStats _targetStats)
    {
        //1.���ݵ�������ֵ���жϹ����ܷ�����
        if(TargetCanAvoidAttack(_targetStats)) return;

        //2.����Զ��ֿ���ɵ��˺���ֵ"totalDamage"
        //2.1�����ݽ�ɫ������˺�����������������ɵ��˺�ֵ
        int totalDamage = damage.getValue() + strength.getValue();  //���������ɵ����˺�

        //2.2�����ϱ����˺�
        if (canCrit())
        {
            totalDamage = calculateCriticalDamage(totalDamage);
            Debug.Log("critical Damage" + totalDamage);
        }

        //2.3��ȥ���ֻ��׵ֵ����˺�
        //�������������chill��debuff�����������ף������˺�
        if(_targetStats.isChilled) totalDamage -= Mathf.RoundToInt(_targetStats.armor.getValue() * .8f);
        else totalDamage -= _targetStats.armor.getValue();

        totalDamage -= _targetStats.armor.getValue();               //���ݵ��˻��ף�������ɵ��˺���ֵ
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);    //����˺���ֵ����Ϊ������������������ƶ���

        //3.���գ��ö����⵽�˺�
        _targetStats.takeDamage(totalDamage);
        doMagicDamage(_targetStats);
    }

    //����ħ���˺�
    public virtual void doMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage     = fireDamage.getValue();
        int _iceDamage      = iceDamage.getValue();
        int _lightingDamage = lightingDamage.getValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.getValue();
        totalMagicalDamage -= _targetStats.magicResistance.getValue() + (_targetStats.intelligence.getValue()*3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage,0, int.MaxValue);

        _targetStats.takeDamage(totalMagicalDamage);

        //��ֹ�·���������ѭ��
        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0) return;

        //3.1������������ħ�������У��ĸ���ɵ��˺���ߣ����������Ӧ��DEBUFF
        bool canApplyIgnite  = _fireDamage > _iceDamage      && _fireDamage > _lightingDamage;
        bool canApplyChilled = _iceDamage > _fireDamage      && _iceDamage > _lightingDamage;
        bool canApplyShock   = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        //3.2��������˶��Ԫ���˺���ͬ���������ͨ��ҡɫ�ӵķ�������DEBUFF
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

        //4.Ϊ��ɫװ��DEBUFF
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChilled, canApplyShock);
    }
    #endregion

    //�жϣ���ɫ�Ƿ��ܵ�Ԫ���˺�
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked) return;

        if(_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentDuration; //��ʱ����¼����״̬��ʱ��

            fx.IgniteFXFor(ailmentDuration); //�ý�ɫ���뱻��ȼ״̬ʱ������fx�ж�Ӧ����˸,����Ϊ��˸ʱ��
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


    //����Ӧ�����ܵ��˺�
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

    //�жϽ�ɫ�Ƿ���ܵ��˵��˺�
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvation = _targetStats.evasion.getValue() + _targetStats.agility.getValue();

        if (isShocked) totalEvation += 20;  //�����SHOCK��buff��������������

        if (UnityEngine.Random.Range(0, 100) < totalEvation)
        {
            Debug.Log("Attack avoid");
            return true;
        }

        return false;
    }

    //�жϽ�ɫ�Ƿ����ɱ����˺�
    private bool canCrit()
    {
        int totalCriticalChance = critChance.getValue() + agility.getValue();

        if(UnityEngine.Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    //���㱩���˺�
    private int calculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.getValue() + strength.getValue()) * .01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    //��ȡ����������ֵ
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