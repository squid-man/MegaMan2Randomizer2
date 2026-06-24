; Snarpen your snife

.include "mm2r.inc"

; Fixtures

EnemyLevelIds = $100
EnemyStates = $120 ; aka EnemyVars120
SpriteTimers  = $4E0 ; aka GenObjVars
sprite_physics = $EEBA
sprite_physics_collision_only_enemy = $EFB3
level_id_stash = $05 ; Seems to be safe :p

.segment "BANKE"

; Part where the snarmor snits sntill
.org $B32D
  JSR SnarmorPatch ; Was JSR sprite_physics_collision_only_enemy

FREE_UNTIL $B330

; Part where the snarmor wants to snysics
.org $B3F2
  JSR SnarmorPatch ; Was JSR sprite_physics

FREE_UNTIL $B3F5

; Part where the snarmor wants to spawn the snoe
.org $B409
  JMP SnarmorPatch2 ; Was STA SpriteTimers+$10,Y
  ; RTS ; Since we're doing a tail call, this byte can be considered free.

FREE_UNTIL $B40D

.reloc
SnarmorPatch:
  ; X is SPRITE slot (10-1F)
  ; sprite_physics handles unloading the enemy, including nulling out the level ID. So we need to stash it.
  LDA EnemyLevelIds-$10,X ; Annoyingly, this becomes $F0, which is zp. But this instruction MUST not assmble into a zp instruction. Don't know how to enforce that.
  STA level_id_stash

  ; Both callsites go here, so check state and replace the correct sprite physics call.
  LDA EnemyStates-$10,X
  BEQ +
  JMP sprite_physics ; Tail call!
+:
  JMP sprite_physics_collision_only_enemy ; Tail call!

SnarmorPatch2:
  ; X is parent SPRITE slot (10-1F)
  ; Y is child ENEMY slot (0-F)
  ; Vanilla code has already checked the carry so we know this is good
  STA SpriteTimers+$10,Y

  LDA level_id_stash
  STA EnemyLevelIds,Y
  
  RTS
