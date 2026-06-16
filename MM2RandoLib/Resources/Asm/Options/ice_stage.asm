; Pick a random stage to have ice physics.
; Two things need to be done to make this work:
;   1. Replace Flash Man's special tile #3 with ground
;   2. Patch the BG collision check routine to return tile type 7 if it wanted to return tile type 1

.include "mm2r.inc"

.segment "BANKF"

; Very end of bg_collision_check routine
; Was:
;   LDA #$E
;   JSR $C000
;   RTS
.org $CC41
  JSR ice_stage_patch
  JMP $C000 ; aka Switch16kBank

; Overwrite Flash Man's special tile #3 to ground
.org $CC52
  .byte $01

.reloc
ice_stage_patch:
  LDA LevelIdx
  CMP #ICE_STAGE
  BNE @exit
  
  ; Enemies seem to do fine if the ground is ice, but this should help if you run into issues:
  ; Only override for Mega Man. CurObjIdx isn't actually 00, but underflown to FF for him.
  ; LDA CurObjIdx
  ; BPL @exit

  ; $00 contains the actual tile type (not just "special tile #2")
  LDA $00
  CMP #1
  BNE @exit
  
  LDA #7
  STA $00

@exit:
  ; We are responsible for switching back to bank E on the way out.
  LDA #$E
  RTS
