; Fixes a frustrating bug where a Sniper Armor can respawn even though its Sniper Joe operator is still alive.
; The game already uses a table of level IDs at $100 to prevent respawns, so it's just a matter of passing the Armor's
; level ID along to the Joe. As always, the devil's in the details.

.include "mm2r.inc"

; Fixtures

LevelIdStash = $05 ; Seems to be safe ¯\_(ツ)_/¯

.segment "BANKE"

; Physics call in state 0, where it's sitting still
.org $b32d
	jsr SniperArmorPatch1 ; Was jsr ObjPhysicsCollisionOnlyEnemy

FREE_UNTIL $b330

; Physics call in states 1-6, where it's jumping or shooting
.org $b3f2
	jsr SniperArmorPatch1 ; Was jsr ObjPhysics

FREE_UNTIL $b3f5

; Just spawned the Sniper Joe child object
.org $b409
	jmp SniperArmorPatch2 ; Was sta GenObjVars + $10, y
	; rts ; Since we're doing a tail call, this byte can be considered free.

FREE_UNTIL $b40d

.reloc
SniperArmorPatch1:
	; X is OBJECT slot (10-1f)
	; ObjPhysics handles unloading the enemy, including clearing out the level ID. So we need to stash it.

	; Annoyingly, this address becomes $f0, which is zp. But this instruction MUST not assmble into a zp instruction.
	; Luckily it does assemble to a 2-byte address, but I don't know how to enforce that.
	lda EnemyLevelIds - $10, x
	sta LevelIdStash

	; Both callsites go here, so check state and replace the correct sprite physics call.
	lda EnemyVars120 - $10, x
	beq +
	jmp ObjPhysics ; Tail call!
+:
	jmp ObjPhysicsCollisionOnlyEnemy ; Tail call!

SniperArmorPatch2:
	; X is parent OBJECT slot (10-1f)
	; Y is child ENEMY slot (0-f)
	; Vanilla code has already checked the carry (indicating failed spawn), so we know this is valid.
	sta GenObjVars + $10, y

	lda LevelIdStash
	sta EnemyLevelIds, y
	
	rts
