.include "mm2r.inc"

.segment "BANKE"

.org $8178 ; Part of main game loop
	; 8178  A5 27          LDA $27
	; 817A  29 08          AND #$08
	; 817C  F0 03          BEQ $8181
	; 817E  20 0D 83       JSR DoPauseScreen

	lda CtrlState
	eor #$ff
	and #%00010101 ; Up, select, and A

	jsr PatchCheckPause

	FREE_UNTIL $8181

.reloc
PatchCheckPause:
	; Okay to clobber A, X (Y??)
	bne +

	sta MmState ; Explosion only occurs if MmState == 0
	jmp DoDeath

+
	lda NewButtonsPressed
	and #$8 ; Start
	bne +

	rts

+
	jmp DoPauseScreen
