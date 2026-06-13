; The mm2r.inc virtual file will be constructed by appending this file after all the programmatically defined options definitions. All global variable and constant definitions that may be needed by multiple assembly files should be here.

.macpack common

;;;;;;;;;; Hardware registers

; Standard registers

PPUCTRL = $2000
PPUMASK = $2001
PPUSTATUS = $2002
OAMADDR = $2003
OAMDATA = $2004
PPUSCROLL = $2005
PPUADDR = $2006
PPUDATA = $2007
OAMDMA = $4014

JOY1 = $4016
JOY2 = $4017

; MMC3 registers

BankCtrlReg = $9ffc
BankReg = $9ffd
MirrorReg = $bffc
PrgRamProtectReg = $bffd
IrqCounterReg = $dffc
IrqReloadReg = $dffd
IrqDisableReg = $fffc
IrqEnableReg = $fffd

SWITCH_BANK_8 = 6
SWITCH_BANK_A = 7

MIRROR_VERTICAL = 0
MIRROR_HORIZONTAL = 1

;;;;;;;;;; Global variables and constants

FrameCtr = $1c
CtrlState = $23
LevelIdx = $2a
CurObjIdx = $2b
MmState = $2c
IframesLeft = $4b
WpnsAcquiredMask = $9a
ItemsAcquiredMask = $9b
ItemEnergies = $9c
NumEtanks = $a7
CurItemIdx = $a9 ; 0 if none
Temp = $fd

EnemyVars110 = $110
EnemyVars120 = $120
ObjSpriteIds = $400
ObjFlags = $420
GenObjVars = $4e0
ObjAnimVars = $680
ObjFrameCtrs = $6a0
ObjHitPoints = $6c0

BossBehavior = $b1
BossVar = $b2
BossVar2 = $b4
BossVar3 = $5ab

NUM_ITEMS = $b
MAX_ENERGY = $1c

; Object IDs

CLOWN_GEAR_OBJ_ID = $29
PRESS_OBJ_ID = $30
BLOCKY_OBJ_ID = $31
PIPI_EGG_OBJ_ID = $3a
PRESS_CHAIN_OBJ_ID = $52

; Track IDs

ENERGY_FILL_SFX_ID = $28

;;;;;;;;;; ROM data addresses

;;; TODO: Pass this from code
WEAPON_TO_STAGE_INDEX_MAP_ADDR = $bff8 ; Bank d

StageItemRewardMasks = $c281
EnemyContactDamageTable = $ed5c

;;;;;;;;;; Function addresses

DrawTitleLogoSprites = $a60f ; Bank d

WaitForNmiOutOfGame = $c0ab
EnqueueSound = $c051
DoDeath = $c10b
LoadStageTileset = $c45d
DoPauseScreen = $c573
SpawnEnemyFromEnemy = $f159

; Macros

.macro SKIP count
	.org * + count
.endmacro

.macro NO_OP count
	.res count, $ea
.endmacro

.macro REL_ORG_BASE base
	.define REL_ORG_BASE_VALUE base
.endmacro

.macro REL_ORG offs
	.org (REL_ORG_BASE_VALUE) + (offs)
.endmacro

.macro REL_TILE x, y
	.org (REL_ORG_BASE_VALUE) + (x) + (y) * 32
.endmacro

.macro TITLE_REL_TILE x, y
	REL_TILE (x), (29 - (y))
.endmacro

.macro REL_RECT_FILL xstart, ystart, xend, yend, stride, value
	.repeat (yend) - (ystart), yoffs
		REL_ORG ((ystart) + yoffs) * stride + (xstart)
		.res (xend) - (xstart), (value)
	.endrepeat
.endmacro

.macro REL_INV_RECT_FILL xstart, ystart, xend, yend, width, height, value
	REL_RECT_FILL (xstart), ((height) - 1 - (yend)), (xend), ((height) - 1 -(ystart)), (width), (value)
.endmacro

.macro REL_TILE_RECT_FILL xstart, ystart, xend, yend, value, yflip
	.if (yflip)
		REL_INV_RECT_FILL (xstart), (ystart), (xend), (yend), 32, 30, (value)
	.else
		REL_RECT_FILL (xstart), (ystart), (xend), (yend), 32, (value)
	.endif
.endmacro
