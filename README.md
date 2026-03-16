# topdown-roguelike-unity

This project is a 2D top-down action roguelike developed using Unity.



The game focuses on mouse-aimed combat, multiple weapon types, and AI companion drones that assist the player during dungeon exploration.



Players explore procedurally generated dungeons, fight enemies using different combat styles, and return to town to upgrade equipment and companions.



Core Gameplay Loop



Town(deleted)

→ Prepare equipment and manage drones

→ Enter dungeon

→ Fight enemies

→ Collect loot

→ Return to town

→ Upgrade gear and repeat



The town acts as a central hub where the player prepares for the next dungeon run.



Key Features



Mouse-Aimed Combat



The player attacks in the direction of the mouse cursor.



Controls



WASD : Movement

Mouse : Aim direction

Left Click : Primary attack

1/2/3: Drone Module

Q/E: Player Weapon Module





This system allows precise directional combat and supports different weapon types.



Multi-Weapon System



The player can switch between different weapon types during combat.



Weapon types



Melee

Short-range attacks with high damage.



Ranged

Projectile-based attacks that allow the player to fight enemies from a distance.



Magic

Special abilities such as area attacks or status effects.



Each weapon type encourages different combat strategies.



Companion Drone System



Companion drones automatically assist the player during combat.



Example drone roles



Attack Drone

Automatically attacks nearby enemies.



Support Drone

Heals the player when health is low.



Drone behavior is implemented using a finite state machine.



Example behavior flow



Follow player

→ Detect enemy

→ Execute ability

→ Cooldown

→ Follow player



Enemy AI



Enemies use simple AI behavior patterns.



Example enemy states



Idle

→ Detect player

→ Chase player

→ Attack

→ Return to original position



These behaviors create dynamic combat encounters.



Procedural Dungeon Generation



The dungeon is generated using a room-based procedural system.



Example dungeon flow



Start room

→ Combat rooms

→ Treasure room

→ Boss room



Game System: Chapter < Room < Wave



Each dungeon run generates a different layout, improving replayability.



Technical Implementation



Major gameplay systems implemented in this project



Player movement system

Mouse aiming system

Modular weapon system

Enemy AI system

Companion drone AI

Procedural dungeon generation

Inventory system



Project Architecture



Example project folder structure



Assets

Scripts

Player

Combat

Weapons

AI

Dungeon

Systems



Prefabs

Scenes

Art

Animations

UI



This modular structure helps maintain scalability and code organization.



Tech Stack



Engine

Unity



Language

C#



Tools

Git

GitHub



Development Goals



The goal of this project is to demonstrate



Combat system design

AI behavior implementation

Procedural dungeon generation

Modular system architecture in Unity



This project focuses on gameplay systems and programming rather than large-scale content production.



Future Improvements



Additional weapon types

More drone variants

Advanced enemy AI behaviors

Multiple dungeon themes



Demo



Gameplay video:

