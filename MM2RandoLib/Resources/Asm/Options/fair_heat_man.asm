.include "mm2r.inc"

LfsrState = BossVar3

.segment "BANKB"

.org $80d3
	; Boss' life meter has fully filled
	; 80D3  AD E1 04       LDA $04E1

	jsr InitHeatMan

	FREE_UNTIL $80d6


.org $82ac
	; $d bytes
	; 82AC  A5 4A          LDA $4A ; Random number
	; 82AE  85 01          STA $01
	; 82B0  A9 03          LDA #$03 ; Divide by 3
	; 82B2  85 02          STA $02
	; 82B4  20 4E C8       JSR $C84E
	; 82B7  A6 04          LDX $04

	; 8 bytes
	lda LfsrState
	lsr a
	bcc +

	eor #$b8

+
	; 5 bytes
	jsr GetHeatManDelayCont
	tax
	nop

	FREE_UNTIL $82b9


.reloc

InitHeatMan:
	lda #FAIR_HEAT_MAN_SEED
	sta LfsrState

	lda GenObjVars + 1

	rts

GetHeatManDelayCont:
	sta LfsrState

	eor #%10010010

	cmp #($100 * 2 / 3)
	bcs +

	cmp #($100 / 3)
	lda #$0
	rol a
	
	rts

+
	lda #$2
	rts
