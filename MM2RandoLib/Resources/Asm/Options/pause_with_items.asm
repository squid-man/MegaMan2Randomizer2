; Normally MM2 rejects pause when weapons/items are on the screen. Allow pausing at any time by deleting any weapons/items on the screen on pause.

.include "mm2r.inc"

.segment "BANKE"

.reloc
DeleteBullets: 
	; Slot f only ever contains Flash Man's Time Stopper twinkle effect.
	; That's the one object we actually want to prevent pausing, so check that slot separately.
	lda ObjFlags + $f
	bpl +

	; The fabled double return!
	; This basically tells the game to ignore your start press.
	pla
	pla
	rts

+
	; Delete objects 2-$e
	ldx #$d

-
	lsr ObjFlags + 1, x
	dex
	bne -
	
	rts

.segment "BANKF"

.org $c573
	; Part of the pause handler

	jsr DeleteBullets

	; Delete platforms
	txa
	ldx #$2

-
	sta $5a0, x
	dex
	bpl -

FREE_UNTIL $c57f
