; Pick a random stage to have ice physics.
; Two things need to be done to make this work:
;   1. Replace Flash Man's special tile #3 with ground
;   2. Patch the BG collision check routine to return tile type 7 if it wanted to return tile type 1

.include "mm2r.inc"

.segment "BANKF"

; Very end of BG collision check routine
; Was:
;   lda #$e
;   jsr $c000
;   rts
.org $cc41
  jsr IceStagePatch
  jmp $c000 ; aka Switch16kBank

FREE_UNTIL $cc47

; Overwrite Flash Man's special tile #3 to ground
.org $cc52
  .byte $01

FREE_UNTIL $cc53

.reloc
IceStagePatch:
  lda LevelIdx
  cmp #ICE_STAGE
  bne @exit
  
  ; Enemies seem to do fine if the ground is ice, but this should help if you run into issues:
  ; Only override for Mega Man. CurObjIdx isn't actually 00, but underflown to ff for him.
  ; lda CurObjIdx
  ; bpl @exit

  ; $00 contains the actual tile type (not just "special tile #2")
  lda $00
  cmp #1
  bne @exit
  
  lda #7
  sta $00

@exit:
  ; We are responsible for switching back to bank e on the way out.
  lda #$e
  rts
