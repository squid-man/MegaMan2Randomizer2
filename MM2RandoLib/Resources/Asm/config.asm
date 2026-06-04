.macpack common

; Setup the ROM segments and layout. Because the game views the banks as being $4000 bytes, make the original banks 0-F $4000 bytes, and the expanded banks $2000 bytes. However, to allow some changes to be used on either an unmodified ROM or the expanded ROM, call the last bank BANKF, and have the expanded banks be 1e-3d.

.segment "HEADER" :bank $00 :size $0010 :mem $0000 :off $00000

.segment "BANK0"  :bank $00 :size $4000 :mem $8000 :off $00010
.segment "BANK1"  :bank $01 :size $4000 :mem $8000 :off $04010
.segment "BANK2"  :bank $02 :size $4000 :mem $8000 :off $08010
.segment "BANK3"  :bank $03 :size $4000 :mem $8000 :off $0c010
.segment "BANK4"  :bank $04 :size $4000 :mem $8000 :off $10010
.segment "BANK5"  :bank $05 :size $4000 :mem $8000 :off $14010
.segment "BANK6"  :bank $06 :size $4000 :mem $8000 :off $18010
.segment "BANK7"  :bank $07 :size $4000 :mem $8000 :off $1c010
.segment "BANK8"  :bank $08 :size $4000 :mem $8000 :off $20010
.segment "BANK9"  :bank $09 :size $4000 :mem $8000 :off $24010
.segment "BANKA"  :bank $0a :size $4000 :mem $8000 :off $28010
.segment "BANKB"  :bank $0b :size $4000 :mem $8000 :off $2c010
.segment "BANKC"  :bank $0c :size $4000 :mem $8000 :off $30010
.segment "BANKD"  :bank $0d :size $4000 :mem $8000 :off $34010
.segment "BANKE"  :bank $0e :size $4000 :mem $8000 :off $38010

.segment "BANK1E" :bank $1e :size $2000 :mem $8000 :off $3c010
.segment "BANK1F" :bank $1f :size $2000 :mem $8000 :off $3e010

.segment "BANK20" :bank $20 :size $2000 :mem $8000 :off $40010
.segment "BANK21" :bank $21 :size $2000 :mem $8000 :off $42010
.segment "BANK22" :bank $22 :size $2000 :mem $8000 :off $44010
.segment "BANK23" :bank $23 :size $2000 :mem $8000 :off $46010
.segment "BANK24" :bank $24 :size $2000 :mem $8000 :off $48010
.segment "BANK25" :bank $25 :size $2000 :mem $8000 :off $4a010
.segment "BANK26" :bank $26 :size $2000 :mem $8000 :off $4c010
.segment "BANK27" :bank $27 :size $2000 :mem $8000 :off $4e010
.segment "BANK28" :bank $28 :size $2000 :mem $8000 :off $50010
.segment "BANK29" :bank $29 :size $2000 :mem $8000 :off $52010
.segment "BANK2A" :bank $2a :size $2000 :mem $8000 :off $54010
.segment "BANK2B" :bank $2b :size $2000 :mem $8000 :off $56010
.segment "BANK2C" :bank $2c :size $2000 :mem $8000 :off $58010
.segment "BANK2D" :bank $2d :size $2000 :mem $8000 :off $5a010
.segment "BANK2E" :bank $2e :size $2000 :mem $8000 :off $5c010
.segment "BANK2F" :bank $2f :size $2000 :mem $8000 :off $5e010
.segment "BANK30" :bank $30 :size $2000 :mem $8000 :off $60010
.segment "BANK31" :bank $31 :size $2000 :mem $8000 :off $62010
.segment "BANK32" :bank $32 :size $2000 :mem $8000 :off $64010
.segment "BANK33" :bank $33 :size $2000 :mem $8000 :off $66010
.segment "BANK34" :bank $34 :size $2000 :mem $8000 :off $68010
.segment "BANK35" :bank $35 :size $2000 :mem $8000 :off $6a010
.segment "BANK36" :bank $36 :size $2000 :mem $8000 :off $6c010
.segment "BANK37" :bank $37 :size $2000 :mem $8000 :off $6e010
.segment "BANK38" :bank $38 :size $2000 :mem $8000 :off $70010
.segment "BANK39" :bank $39 :size $2000 :mem $8000 :off $72010
.segment "BANK3A" :bank $3a :size $2000 :mem $8000 :off $74010
.segment "BANK3B" :bank $3b :size $2000 :mem $8000 :off $76010
.segment "BANK3C" :bank $3c :size $2000 :mem $8000 :off $78010
.segment "BANK3D" :bank $3d :size $2000 :mem $8000 :off $7a010

.segment "BANKF"  :bank $0f :size $4000 :mem $c000 :off $7c010

; Specify the unused spaces in the ROM the assembler can put code in

FREE "BANKA" [$bb5e, $c000) ; $4a2 bytes
FREE "BANKB" [$aaaf, $adff] ; $351 bytes
FREE "BANKC" [$9300, $a000) ; $d00 bytes
FREE "BANKD" [$bf88, $bff8) ; $70 bytes

; mm2ft uses e:bd24:be4f
FREE "BANKE" [$8223, $8268) ; $45 bytes freed up by wily_5_respawns.asm
FREE "BANKE" [$be4f, $c000) ; $1b1 bytes

FREE "BANKF" [$ca16, $cb0b] ; $f6 bytes
FREE "BANKF" [$d0d7, $d0f4] ; $1e bytes
FREE "BANKF" [$d192, $d1de] ; $4d bytes
; f:f300:f900 is reserved for DPCM
FREE "BANKF" [$ff87, $ffe0) ; $59 bytes

FREE "BANK1E" [$9a00, $a000) ; $600 bytes
FREE "BANK1F" [$9100, $9200) ; $100 bytes

; 1f:9200:9600 is the uncompressed splash screen
; 1f:9600:a000 are used by duplicate Wily tilesets

; Banks $20:3e are currently reserved for mm2ft
