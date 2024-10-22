# My 2D RPG Game
**Play on website now:** https://waynets.itch.io/warriors-game
<br>This is my first Unity game project, developed by Unity 2D engine. I try to use it study components in Unity, game desgin and programming. 
<br>The game process is very short becasue it's still a demo. But I will ensure that it will be improved in the future!
<br>Really enjoy game programming, this is an amazing experience.
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
We have 2 kinds enmey: skeleton and minotaur, Each character has its own attack mode.

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
![屏幕截图 2024-07-22 233852](https://github.com/user-attachments/assets/06cce9ca-cd21-4962-ab5d-1089e40b8e44)

## Save&Load System
**Class GameData**: Record many important data like currency, check point, inventory, equipments.
<br>
<br>**interface ISaveManager**: Defined 2 funtions loadData() and saveData(), a lot classes will call this interface. For example: class Inventory will call this interface and override these 2 functions, when player need to save data, class SaveManager will find all classes which used this interface and run saveDta() inside class Inventory. 
<br>
<br>**Class FileDataHandler**: Save the file name and file storage location, responsible for saving the file to the specified location or loading the file from the specified location.
<br>(1)Save Data: Transform object GameData to JASON file, then save it into specific location as .txt file.
<br>(2)Load Data: Find .txt file in specific location and read contents, then transform it to object GameData.
<br>
<br>**Class SaveManager**: Responsible for calling loadData() and saveData() in all instances that use the interface ISaveManager.

## UI System
Designed intuitive UI layouts and guidance systems to help players quickly adapt to and navigate the interface.
<br>Implemented responsive design to support multiple resolutions, ensuring platform compatibility.

## Future Plan
(1) Add 1 to 2 new enemies.
<br>(2) Add 1 to 2 new scenes.
<br>(3) Improve the skill tree.
<br>(4) Improve the Craft.
<br>(5) Add more Skills for player.
