namespace MM2Randomizer.Enums
{
    public enum ESoundID
    {
        WeaponF         = 0x21,
        HeatmanUnused   = 0x22, // No. Uses all channels.
        WeaponM         = 0x23,
        WeaponP         = 0x24,
        Shotman         = 0x25,
        TakeDamage      = 0x26,
        QuickBeam       = 0x27,
        Refill          = 0x28, 
        MegaLand        = 0x29,  
        WilyDefeat      = 0x2A, // No. Too long for SFX.
        DamageEnemy     = 0x2B, 
        Dragon          = 0x2C, 
        Tink            = 0x2D, 
        CrashAttach     = 0x2E,
        Cursor          = 0x2F,
        TeleportIn      = 0x30,
        WeaponW         = 0x31, 
        Pause           = 0x32,    
        Unknown0        = 0x33,
        BossDoor        = 0x34, // No. Uses music square.
        WeaponH_Charge0 = 0x35, // Yes, but randomize the 3
        WeaponH_Charge1 = 0x36,
        WeaponH_Charge2 = 0x37,
        WeaponH_Shoot   = 0x38,
        FlyBoy          = 0x39,
        TeleportOut     = 0x3A, // No. Uses music square.
        Splash          = 0x3B,
        Yoku            = 0x3C,
        Droplet1        = 0x3D,
        Droplet2        = 0x3E, // No. Uses music square.
        WeaponA         = 0x3F,
        Unknown1        = 0x40, // Yes. Weird fuzz sound?
        Death           = 0x41, // No. Uses music square.
        OneUp           = 0x42,
    }
}
