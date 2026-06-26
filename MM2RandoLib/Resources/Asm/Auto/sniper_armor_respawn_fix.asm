; Fixes a frustrating bug where a Sniper Armor can respawn even though its Sniper Joe operator is still alive.
; The game already uses a table of level IDs at $100 to prevent respawns, so it's just a matter of passing the Armor's
; level ID along to the Joe. As always, the devil's in the details.

.include "mm2r.inc"

; Fixtures

LevelIdStash = $05 ; Seems to be safe ¯\_(ツ)_/¯

.segment "BANKE"

; Physics call in state 0, where it's sitting still
.org $B32D
  JSR SniperArmorPatch1 ; Was JSR ObjPhysicsCollisionOnlyEnemy

FREE_UNTIL $B330

; Physics call in states 1-6, where it's jumping or shooting
.org $B3F2
  JSR SniperArmorPatch1 ; Was JSR ObjPhysics

FREE_UNTIL $B3F5

; Just spawned the Sniper Joe child object
.org $B409
  JMP SniperArmorPatch2 ; Was STA GenObjVars+$10,Y
  ; RTS ; Since we're doing a tail call, this byte can be considered free.

FREE_UNTIL $B40D

.reloc
SniperArmorPatch1:
  ; X is OBJECT slot (10-1F)
  ; ObjPhysics handles unloading the enemy, including clearing out the level ID. So we need to stash it.

  ; Annoyingly, this address becomes $F0, which is zp. But this instruction MUST not assmble into a zp instruction.
  ; Luckily it does assemble to a 2-byte address, but I don't know how to enforce that.
  LDA EnemyLevelIds-$10,X
  STA LevelIdStash

  ; Both callsites go here, so check state and replace the correct sprite physics call.
  LDA EnemyVars120-$10,X
  BEQ +
  JMP ObjPhysics ; Tail call!
+:
  JMP ObjPhysicsCollisionOnlyEnemy ; Tail call!

SniperArmorPatch2:
  ; X is parent OBJECT slot (10-1F)
  ; Y is child ENEMY slot (0-F)
  ; Vanilla code has already checked the carry (indicating failed spawn), so we know this is valid.
  STA GenObjVars+$10,Y

  LDA LevelIdStash
  STA EnemyLevelIds,Y
  
  RTS
