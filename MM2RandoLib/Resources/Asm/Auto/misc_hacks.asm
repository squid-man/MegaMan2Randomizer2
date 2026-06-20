; This file contains mandatory assorted hacks that were considered too minor to give their own assembly file but have no option to attach actions to. Unlike prepatch.asm, this file is compiled at the same time as everything else, so it has access to .reloc and symbol definitions.

.include "mm2r.inc"

; Disable refight boss music
.segment "BANKB"
.org $a060
	NO_OP 3 ; Was jsr EnqueueSound (stop music)
.segment "BANKE"
.org $83ca
	NO_OP 3 ; Was jsr EnqueueSound (start boss music)
.org $847a
	NO_OP 3 ; Was jsr EnqueueSound (restart stage music)
	
.segment "BANKE"

; Disable Changkey Maker palette swap
.org $a4e6
    ; Stop palette change when enemy appears
	jmp $a555 ; Was jsr $f159
.org $a552
	; Stop palette change on kill/despawn
	NO_OP 3 ; Was jsr $f159

; There are two acid drippers ($72 and $73), with two sound effects. $3e uses square 1, $3d uses square 2. Get rid of $3e as it interferes with the music.
.org $bca7
DripperSfxTable:
	.byte $3d, $3d ; Was $3d, $3e
	
.segment "BANKF"

; Preserve e-tanks on game over
.org $c1bc
	NO_OP 2 ; Was sta NumEtanks

; Woodman's Leaf Shield Attack Nerf
.org $ed5c + $61
	.byte 4