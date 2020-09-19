# Domria: the lands beyond the fog

Architacture code examples from the game that my colleague and I are developing

"**Domria: the lands beyond the fog**" is a strategy game in the veins of God Games, where the player assumes the role of a fictional leader,
indirectly influenceing a population of simulated heroes.
*The game implements:* procedural map generation, AI based NPC's, base building, extensive RPG stats for the heroes, enchantment system and deplomecy systems.

# Files

### Enums
List of enums used across the classes.

### GameMaster
Singleton class for the system that manages players, and keeps track over their actions between themselves, updating their relations chart accordingly.

## Characters

### Entity
The class that all entities in the game world inherit from.

### SkillUser
The class that all entities who could use spells inherit from, such as heroes, monsters and defence buildings.

### NPC
The class for the characters that run around in the game, such as heroes and monsters, most of the RPG stats are stored here.

## Players

### Faction
The base class for all the factions of the game, implements the diplomacy system.

### MOBKingdom
The class for non-playeable factions, generated for every instance of a game.

### Kingdom
The class for the player and rival factions.

## Skills Architecture

### Skills
The base class for all types of skills in the game.

### CharEffect
The class that implements all skills with effect over time

### StatChanger
The class of skills that affect NPC stats.

### Skills Architecture
The class of skills that leave lasting effects on the NPC, such as poison or stun

### OffenceSkill
The class of skills that transfer damage.

### Projectile
The class for the projectiles shot by ranged skills and attacks.

### HealingSkill
The class for skills that heal damage.
