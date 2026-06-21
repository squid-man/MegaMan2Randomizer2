.include "mm2r.inc"

MECHA_DRAGON_COOLDOWN_FRAMES = $20

MechaDragonFire = $9035
UpdateMechaDragonMovement = $9165

.segment "BANKB"

.org $902f
	; 902F  8D E1 04       STA GenObjVars + 1
	jsr PatchEndMechaDragonLifeFill

.org $90bc
	; 90BC  20 65 91       JSR UpdateMechaDragonMovement
	jsr PatchUpdateMechaDragonMovement

.org $911b
	; 911B  20 35 90       JSR MechaDragonFire
	jsr PatchMechaDragonFire

.reloc

PatchEndMechaDragonLifeFill:
	; A = 0
	sta GenObjVars + 1
	sta BossVar3

	rts

PatchMechaDragonFire:
	lda BossVar3
	beq +

	rts

+
	lda #MECHA_DRAGON_COOLDOWN_FRAMES
	sta BossVar3

	jmp MechaDragonFire

PatchUpdateMechaDragonMovement:
	jsr UpdateMechaDragonMovement

	dec BossVar3
	bpl +

	inc BossVar3

+
	rts
