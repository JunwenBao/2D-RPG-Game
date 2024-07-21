# 2D-RPG-Game
This is my first game project, developed by Unity 2D engine. I try to use it study components in Unity, game desgin and programming. I really enjoy this journey, an amazing experience.
![屏幕截图 2024-07-21 055624](https://github.com/user-attachments/assets/c1ed6c26-4132-4d8e-bd3c-8aa4acce1c87)

## Player Design
The player have 8 states: Idle, Move, Jump, fall, Squat, Attack, Shoot, Dead. Each state has its corresponding animation and c# file, all animations are controled by Player_AC(Animation Controller). 
<br>
<br>Player's state machine design is as follows:
![屏幕截图 2024-07-21 043654](https://github.com/user-attachments/assets/a22825d3-9e80-49ae-ba49-6d5e9ad2e3ee)
**Class PlayerState**: The base class of all 8 states, it saves a string variable **animBoolName**. Whenever the Player enters a state, the bool variable (which has the same name to animBoolName) inside state machine will be set to true, and then the corresponding animation is played in the state machine.

<br>**Class PlayerStateMachine**: Control the state change. When player need change state, it will call the function **ChangeState()** inside PlayerStateMachine, exit from current state then enter new state.

<br>Player has 3 additional transform: WallCheck, GroundCheck, AttackCheck. WallCheck is used to check whether player is touch the wall, GroundCheck is used to check whether player is standing on the ground. AttackCheck is used to determine whether player can attack enemys.

## Enemy Design
We have 2 kinds enmey: skeleton and minotaur. 

## Stats Design
Each character has four main stats: **major stats, offensive stats, defensive stats, magic stats**. 
<br>(1) Major Stats: This stat include 4 attribute: Strength, Agility, Intelligence, Vitality.
<br>(2) Offensive Stats: This stat include 3 attribute: Damage, Crit Chance, Crit Power.
<br>(3) Defensive Stats: This stat include 4 attribute: Max Health, Armor, Evasion, Magic Resistance.
<br>(4) Magic Stats: This stat include 3 attribute: Fire Damage, Ice Damage, Lightning Damage.

## Items
In this game we have 2 kinds item: **Materials and Equipments**. 
<br>Materials can use to craft equipments. For example, the material animal skin can be used to craft armor.
<br>Equipments can be equipped by player, it will add or decrease some attributes like damage or health.

## Inventory System


## Save&Load System


## UI System


## Future Plan

