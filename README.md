# Risk of Rain 2 - Skills++ Mod

[![Discord](https://img.shields.io/discord/745162241359478894?color=7289DA&label=modding%20discord&logo=discord&logoColor=white)](https://discord.gg/gGtcnJDnDw) [![source code](https://img.shields.io/static/v1?label=github&logo=github&message=source%20code&color=FFFFFF)](https://github.com/gamrtiem/ror2skillsplusplus/)

## What's new in 0.6.3
- Updated for SOTS phase 3!!

## What's new in 0.6.2
- Added Void fiend upgrades!
- (Hopefully) fixed multiplayer not adding levels for clients

## What's new in 0.6.1
- Re-added configs with support for Risk of Options (sorry controller bros .., looking into how extra skill slots adds controller support!)
- Fixed a bug where skill upgrades wouldn't stay until re-upgraded on the next stage
- Fixed Arti's Snapfreeze upgrade not doing anything

## What's new in 0.6.0
- Updated for Seekers of the Storm! (Expect some bugs (again) (please report them on the github !!))
- Fixed incorrect scaling for MUL-T causing his dash to deal 4x damage
- Added new upgrade for MUL-T's Power Mode
- SOTS survivor support is yet to be added

## What's new in 0.5.0
- Updated the way the mod finds the names of skills to hopefully alleviate future problems with implementing new skills. (For modders: Skill Names now finds the Object.Name of the SkillDef rather than skilldef.skillname. If your mod was using different names for those two, please update your Skills++ integration to use the correct name.) 

- Fixed an error that prevented Engi's turrets from getting their buffs.
- Fixed some tooltip issues regarding Huntress' second utility skill and Void Fiend picking up Acrid's tooltips by mistake.
- Added Not Yet Implemented messages to the unsupported vanilla skills to reduce confusion.

[Full changelog history](https://github.com/gamrtiem/ror2skillsplusplus/blob/main/CHANGELOG.md)

## Installation

Using a mod manager like [r2modman](https://thunderstore.io/package/ebkr/r2modman/) or [gale](https://thunderstore.io/package/Kesomannen/GaleModManager/) is recommended to install mods in general, but if you're running a manual install simply extract the mod's files to your `BepInEx/plugins` folder.

## Usage

While ingame your will earn skill points at certain levels that can be used to purchase upgrades to your characters skills.
By default, your first skill point is earned upon reaching level five, and subsequently rewards every fifth level.

You can change the number of levels between skill points within the gameplay settings in Risk of Rain 2. Changes to this setting will be applied at the start of each run.

![](https://raw.githubusercontent.com/gamrtiem/ror2skillsplusplus/refs/heads/main/Images/levels_per_skillpoint_option.png)

To redeem points open the info screen (hold TAB by default) and click the skill you would like to buy.
Upgrades do not carry over between runs so you will be starting from scratch every time.

1. When you have levelled up enough to buy a skill the icons will change to have a yellow border.
![](https://raw.githubusercontent.com/gamrtiem/ror2skillsplusplus/refs/heads/main/Images/skillpoint_available.png)

1. Opening the info screen will present 'Buy' buttons over the top of skills that can be purchased.
![](https://raw.githubusercontent.com/gamrtiem/ror2skillsplusplus/refs/heads/main/Images/buy_options.png)

1. Clicking on any of the 'Buy' buttons will spend a single point and the skill will upgrade.
![](https://raw.githubusercontent.com/gamrtiem/ror2skillsplusplus/refs/heads/main/Images/skillpoint_spent.png)

## Modded characters

Go and check out all of these characters and give them some love!

If you have added Skills++ to your own modded character let me know and I will add it here.

[![TTGL by Mico27](https://gcdn.thunderstore.io/live/repository/icons/Mico27-TTGL_Mod-0.3.4.png.128x128_q95.jpg)](https://thunderstore.io/package/Mico27/TTGL_Mod/)

### Disabling survivors

Console commands have been added to disable and reenable Skills++ for selected survivors.
You can use this to exclude survivors that conflict with Skills++.

The two commands are:

- `spp_disable_survivor <survivor name>` Disables Skills++ for the named survivor
- `spp_enable_survivor <survivor name>` Re-enables Skills++ for the named survivor

Example usage:

`spp_disable_survivor Artificer`

By default all characters will be enabled for Skills++.
The enable command is there to re-enable a survivor if the conflict no longer exists.

## Feedback and bug reports

The best way to give feedback or raise bugs is through [opening a github issue](https://github.com/gamrtiem/ror2skillsplusplus/issues/new?template=Blank+issue) (please provide a log !!). 
I welcome everyone who uses Skills++ to drop by and share your thoughts.

## FAQ

**When playing multiplayer my/friend's game doesn't work. What is going wrong?**

Skills++ does have multiplayer support but there may still be gaps in the logic.
The best action to resolve this is to raise it in a [github issue](https://github.com/gamrtiem/ror2skillsplusplus/issues/new?template=Blank+issue) with as much info as possible

**Will Skills++ support modded characters?**
  
Yes. There is support for third party code to integrate with the Skills++ system. Guides are available [here](https://github.com/gamrtiem/ror2skillsplusplus/blob/main/Documentation/supporting-modded-characters.md) alongside the mod's source code.

**Skills++ makes the game a cakewalk. Do you recommend any other mods to balance the game?**

I'd highly recommend HIFU's [Inferno](https://thunderstore.io/package/prodzpod/Inferno/) mod. It adds an extra difficulty to the game that should level the playing fields a bit more.

## Special thanks

A very special thanks to the following people. They have been amazing people providing feedback and bug reports for the mod

- K'Not Devourer of Worlds
- Maxi
- TEA
  
---

## Upgrade descriptions

Upgrade descriptions are also shown ingame when hovering over a skill's icon.

![](https://raw.githubusercontent.com/gamrtiem/ror2skillsplusplus/refs/heads/main/Images/skill_upgrade_tooltip.png)

**Jump to character**

[![Commando](https://img.shields.io/static/v1?label=&message=Commando&color=ED9612)](#Commando) [![Huntress](https://img.shields.io/static/v1?label=&message=Huntress&color=D53C3C)](#Huntress) [![Bandit](https://img.shields.io/static/v1?label=&message=Bandit&color=C374C5)](#Bandit) [![MUL-T](https://img.shields.io/static/v1?label=&message=MUL-T&color=D3C450)](#Mul-t) [![Engineer](https://img.shields.io/static/v1?label=&message=Engineer&color=5FE286)](#Engineer) [![Artificer](https://img.shields.io/static/v1?label=&message=Artificer&color=F7C1FD)](#Artificer) [![Mercenary](https://img.shields.io/static/v1?label=&message=Mercenary&color=6CD1EA)](#Mercenary) [![REX](https://img.shields.io/static/v1?label=&message=REX&color=869E54)](#Rex) [![Loader](https://img.shields.io/static/v1?label=&message=Loader&color=6770DE)](#Loader) [![Acrid](https://img.shields.io/static/v1?label=&message=Acrid&color=C9F24D)](#Acrid) [![Captain](https://img.shields.io/static/v1?label=&message=Captain&color=BEBA92)](#Captain) [![Railgunner](https://img.shields.io/static/v1?label=&message=Railgunner&color=ed164d)](#Railgunner) [![Lunar Items](https://img.shields.io/static/v1?label=&message=Lunar%20Items&color=307FFF)](#Lunar-items)

### Commando

| Skill | | Description |
|:-|-|------|
| Double Tap | ![Double tap](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/7/7c/Double_Tap.png) | `+20%` rate of fire and `+15%` recoil reduction per level |
| Phase Round | ![Phase Round](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/3/38/Phase_Round.png) | `+30%` damage and `+30%` projectile velocity per level |
| Phase Blast | ![Phase Blast](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/4/49/Phase_Blast.png) | `+30%` bullets fires and `+20%` blast range per level |
| Tactical Dive | ![Tactical Dive](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/2/25/Tactical_Dive.png) | Grants `+0.75 seconds` of invulnerability per level |
| Tactical Slide | ![Tactical Slide](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/e/e8/Tactical_Slide.png) | `+0.125s` slide duration. Converts `+6%` movespeed while sliding as attack speed, damage and armor |
| Supressive Fire | ![Supressive fire](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/d/db/Suppressive_Fire.png) | `+30%` bullets fired per level |
| Frag Grenade | ![Frag Grenade](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/6/6b/Frag_Grenade.png) | `+20%` explosion damage and `+20%` blast radius per level |

### Huntress

| Skill | | Description |
|:-|-|------|
| Strafe | ![Strafe](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/a/af/Strafe.png) | `+20%` range and `+20%` proc chance per level |
| Flurry | ![Flurry](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/2/24/Flurry.png) | `+10%` range and `+1` arrow fired per level. *Crits fire twice as many arrows* |
| Laser Glaive | ![Laser Glaive](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/f/f6/Laser_Glaive.png) | `+1` bounce, `+10%` damage, and `+10 units` of bounce range per level |
| Blink | ![Blink](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/1/16/Blink.png) | Grants `+1 second` per level or full crit time |
| Phase Blink | ![Phase Blink](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/d/dd/Phase_Blink.png) | Grants `+0.5 seconds` per level or full crit time |
| Arrow Rain | ![Arrow Rain](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/5/59/Arrow_Rain.png) | `+25%` area of effect and `+20%` damage per level |
| Ballista | ![Ballista](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/d/d5/Ballista.png) | `+1` bullet and `+20%` damage per level |

### Bandit

| Skill | | Description |
|:-|-|------|
| Burst | ![Burst](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/4/47/Burst.png) | `+1` bullet and `+5%` damage per level |
| Blast | ![Blast](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/e/e7/Blast.png) | `+20%` damage, `+10%` proc chance, and `+20%` stability per level |
| Serrated Dagger | ![Serrated Dagger](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/b/bb/Serrated_Dagger.png) | `+20%` hitbox size and `+20%` damage per level |
| Serrated Shiv | ![Serrated Shiv](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/8/80/Serrated_Shiv.png) | `+1` projectile per two levels. `+10%` damage per level |
| Smoke Bomb | ![Smoke Bomb](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/e/eb/Smoke_Bomb.png) | `+1` base movement speed while invisible. `+15%` hitbox size and `+20%` damage per level |
| Lights Out | ![Lights Out](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/7/7c/Lights_Out.png) | `+10%` to `+30%` damage per level, based on remaining enemy health |
| Desperado | ![Desperado](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/5/57/Desperado.png) | `+1%` Execute threshold |

### MUL-T

| Skill           |                                                                                                               | Description                                                                                         |
|:----------------|---------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------|
| Auto-Nailgun    | ![Auto-Nailgun](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/e/ec/Auto-Nailgun.png)      | `+20%` damage and `+4` bullets to final burst per level                                             |
| Rebar Puncher   | ![Rebar Puncher](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/1/10/Rebar_Puncher.png)    | `+15%` rate of fire and `+10%` damage per level                                                     |
| Scrap Launcher  | ![Scrap Launcher](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/e/e6/Scrap_Launcher.png)  | `+1` stock, `+20%` damage, and `+15%` blast radius per level                                        |
| Power-Saw       | ![Power-Saw](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/8/8e/Power-Saw.png)            | `+20%` rate of fire, `+20%` damage, and `+30%` blade hitbox size per level                          |
| Blast Cannister | ![Blast Cannister](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/f/f2/Blast_Canister.png) | `+3` child bombs, `+20%` spread radius, `+20%` child blast radius, and `+20%` damage per level      |
| Transport Mode  | ![Transport Mode](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/8/85/Transport_Mode.png)  | `+10%` duration, `+10%` speed, and `+30%` damage per level                                          |
| Swap            | ![Swap](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/8/8d/Retool.png)                    | Upon activating gain `+1 second` of bonus attack speed and `+1 second` equipment cooldown per level |
| Power Mode      | ![Power Mode](https://riskofrain2.wiki.gg/images/7/7c/Power_Mode.png)               | Gain `+25 defense`, `15% damage` and `10% speed` per level                                          |

### Engineer

| Skill | | Description |
|:-|-|------|
| Bouncing Grenades | ![Bouncing Grenades](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/0/08/Bouncing_Grenades.png) | `+1` and `+2` to the minimum and maximum grenades fired per level |
| Pressure Mines | ![Pressure Mines](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/9/9e/Pressure_Mines.png) | `-10%` arming time, `+20%` damage, `+20%` blast radius, and `+20%` trigger radius per level |
| Spider Mines | ![Spider Mines](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/6/61/Spider_Mines.png) | `+30%` activation range and `+20%` damage per level |
| Bubble Shield | ![Bubble Shield](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/d/da/Bubble_Shield.png) | `+15%` duration and `+15%` size per level |
| Thermal Harpoons | ![Thermal Harpoons](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/b/b7/Thermal_Harpoons.png) | `+1` harpoon ammo, `+20%` damage, and `+30%` targetting range per level |
| TR12 Gauss Auto-Turret | ![TR12 Gauss Auto-Turret](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/6/60/TR12_Gauss_Auto-Turret.png) | `+25%` enemy detection range and `+10%` damage per level. `+1` deployable turret per two levels |
| TR58 Carbonizer Turret | ![TR58 Carbonizer Turret](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/4/4f/TR58_Carbonizer_Turret.png) | `+15%` damage, `+25%` proc chance, and `+10%` rate of fire per level. `+1` deployable turret per two levels |

### Artificer

| Skill | | Description |
|:-|-|------|
| Flame bolt | ![Flame bolt](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/5/5e/Flame_Bolt.png) | `+2` stock and `-10%` stock recharge time per level |
| Plasma Bolt | ![Plasma bolt](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/5/57/Plasma_Bolt.png) | `+2` stock and `-10%` stock recharge time per level |
| Charged Nano-Bomb | ![Charged nano-bomb](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/c/c8/Charged_Nano-Bomb.png) | `+20%` damage and `+10%` max charge time per level |
| Charged Nano-Spear | ![Charged nano-spear](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/9/92/Cast_Nano-Spear.png) | `+20%` damage and `+10%` max charge time per level |
| Snapfreeze | ![Snapfreeze](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/c/ce/Snapfreeze.png) | `+30%` wall damage, `+15%` wall duration, and `+20%` wall length per level |
| Flamethrower | ![Flamethrower](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/a/ad/Flamethrower.png) | `+20%` range, `+25%` flame radius, and `+20%` damage per level |
| Ion Surge | ![Ion Surge](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/d/d1/Ion_Surge.png) | `+25%` damage and `+30%` area of effect per level |

### Mercenary

| Skill | | Description |
|:-|-|------|
| Laser Sword | ![Laser Sword](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/e/e2/Laser_Sword.png) | `+20%` damage, and `+15%` attack speed per level |
| Whirlwind | ![Whirlwind](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/e/e0/Whirlwind.png) | `+25%` damage and `+25%` larger hitbox per level |
| Rising Thunder | ![Rising Thunder](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/d/d4/Rising_Thunder.png) | `+25%` damage per level. `+1` stock per two levels |
| Blinding Assault | ![Blinding Assault](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/8/87/Blinding_Assault.png) | `+20%` damage, and `+0.5 seconds` extra delay before reset |
| Focused Assault | ![Focused Assault](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/a/a8/Focused_Assault.png) | `+20%` hitbox size, and `+15%` proc chance per level |
| Eviscerate | ![Eviscerate](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/9/9f/Eviscerate.png) | `+30%` chain range, `+15%` attack speed, and `+10%` proc chance per level |
| Slicing Winds | ![Slicing Winds](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/b/bc/Slicing_Winds.png) | `+20%` attack speed and `+20%` damage per level |

### REX

| Skill | | Description |
|:-|-|------|
| DIRECTIVE: Inject | ![DIRECTIVE: Inject](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/5/57/DIRECTIVE_Inject.png) | `+1` syringe and `+10%` damage per level |
| DIRECTIVE: Drill | ![DIRECTIVE: Drill](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/f/fc/DIRECTIVE_Drill.png) | `+10%` damage, `+10%` duration, and `+20%` radius per level |
| Seed Barrage | ![Seed Barrage](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/b/b0/Seed_Barrage.png) | `+20%` radius and `+20%` damage per level |
| DIRECTIVE: Disperse | ![DIRECTIVE: Disperse](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/2/22/DIRECTIVE_Disperse.png) | `+30%` range, `+20%` angle, `+20%` debuff duration per level |
| Bramble Volley | ![Bramble Volley](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/6/63/Bramble_Volley.png) | `+20%` range, `+20%` angle, `+20%` damage per level |
| DIRECTIVE: Harvest | ![DIRECTIVE: Harvest](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/d/db/DIRECTIVE_Harvest.png) | `+20%` damage, `+10%` radius, `+1%` life steal on marked enemy per level |
| Tangling Growth | ![Tangling Growth](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/9/9d/Tangling_Growth.png) | `+20%` damage, `+20%` radius, `+10%` healing rate per level |

### Loader

| Skill | | Description |
|:-|-|------|
| Knuckleboom | ![Knuckleboom](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/b/b2/Knuckleboom.png) | `+1%` barrier gained and `+20%` damage per level |
| Grapple Fist | ![Grapple Fist](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/2/25/Grapple_Fist.png) | `+30%` hook range per level. |
| Spiked Fist | ![Spiked Fist](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/6/62/Spiked_Fist.png) | `+15%` hook range and `+20%` damage per level |
| Charged Gauntlet | ![Charged Gauntlet](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/7/79/Charged_Gauntlet.png) | `+10%` max charge, `+20%` base damage, and `+15%` velocity based damage per level |
| Thunder Gauntlet | ![Thunder Gauntlet](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/f/f3/Thunder_Gauntlet.png) | `+15%` damage and `+20%` cone range per level |
| M551 Pylon | ![M551 Pylon](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/3/37/M551_Pylon.png) | `+20%` damage, `+20%` range, and `+20%` rate of fire per level |
| Thunder Slam | ![Thunder Slam](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/4/4f/Thunderslam.png) | `+5%` damage, `+10%` to `+100%` range based on travel distance, and `+25%` leap height per level |

### Acrid

| Skill | | Description |
|:-|-|------|
| Vicious Wounds | ![Vicious Wounds](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/5/5a/Vicious_Wounds.png) | `+20%` normal damage, `+25%` combo finisher damage, and `+15%` attack speed per level |
| Neurotoxin | ![Neurotoxin](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/2/2b/Neurotoxin.png) | `+20%` damage, `+30%` projectile speed, and `+30%` blast radius per level. `+1 stock` per two levels |
| Ravenous Bite | ![Ravenous Bite](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/8/8b/Ravenous_Bite.png) | `+20%` damage and `+1` stock per level |
| Caustic Leap | ![Caustic Leap](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/9/9e/Caustic_Leap.png) | `+20%` blast damage, `+30%` acid pool damage, and `+20%` acid pool size per level |
| Frenzied Leap | ![Frenzied Leap](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/9/96/Frenzied_Leap.png) | `+25%` blast damage and `+15%` refunded time per level |
| Epidemic | ![Epidemic](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/9/9d/Epidemic.png) | `+5` infection bounces, `+20%` infection range, and `+20%` on hit damage per level |

### Captain

| Skill | | Description |
|:-|-|------|
| Vulcan Shotgun | ![Vulcan Shotgun](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/b/b9/Vulcan_Shotgun.png) | `+20%` shell count and `+10%` damage per level |
| Power Tazer | ![Power Tazer](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/b/b8/Power_Tazer.png) | `+40%` blast radius and `+20%` damage per level |
| Orbital Probe | ![Orbital Probe](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/4/4e/Orbital_Probe.png) | `+20%` blast radius and `+20%` damage per level |
| OGM-72 'DIABLO' Strike | ![OGM-72 'DIABLO' Strike](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/8/85/OGM-72_%27DIABLO%27_Strike.png) | `-1` second drop time, `-15%` cooldown duration and `+4%` friendly fire prevention range per level |
| Beacon: Healing | ![Beacon: Healing](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/1/1e/Beacon_Healing.png) | `+20%` healing range per level  |
| Beacon: Shocking | ![Beacon: Shocking](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/c/c4/Beacon_Shocking.png) | `+30%` shock range per level |
| Beacon: Resupply | ![Beacon: Resupply](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/6/67/Beacon_Resupply.png) | `+1` resupply stock per level |
| Beacon: Hacking | ![Beacon: Hacking](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/3/38/Beacon_Hacking.png) | `+20%` hacking speed and `+20%` hacking range per level |

### Railgunner

| Skill | | Description |
|:-|-|------|
|
XQR Smart Round System | ![XQR Smart Round System](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/7/71/XQR_Smart_Round_System.png) | `+10%` rate of fire and `+20%` range |
| M99 Sniper | ![M99 Sniper](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/8/88/M99_Sniper.png) | `+1` max empowered round and `+1` stock every 2 levels |
| HH44 Marksman | ![HH44 Marksman](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/e/ec/HH44_Marksman.png) | `+1` temporary armor per kill. Turns into `+0.25s` movement speed boost after zoom out |
| Concussion Device | ![Concussion Device](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/4/48/Concussion_Device.png) | `+0.5s` slowfall on hit and `+15%` throw range |
| Polar Field Device | ![Polar Field Device](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/3/3f/Polar_Field_Device.png) | `+10%` global damage on affected enemies, `+15%` range, and `+15%` throw range  |
| Supercharge | ![Supercharge](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/e/e4/Supercharge.png) | `+10%` proc chance, `+20%` crit multiplier, and `-5%` cooldown duration |
| Cryocharge | ![Cryocharge](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/3/3a/Cryocharge.png) | `+2s` frostfire burn and `-10%` cooldown duration |

### Void Fiend

| Skill              |                                                                                  | Description                                                                          |
|:-------------------|----------------------------------------------------------------------------------|--------------------------------------------------------------------------------------|
| Drown              | ![Drown](https://riskofrain2.wiki.gg/images/thumb/e/e0/Drown.png/120px-Drown.png) | `+1s` of slow on hit and `+10%` damage                                               
| Corrupted Drown    | ![Drown](https://riskofrain2.wiki.gg/images/c/c0/Corrupted_Drown.png) | `+25%` range, `+10%` damage and `+15%` extra speed while in use                       |
| Flood              | ![Flood](https://riskofrain2.wiki.gg/images/f/f7/Flood.png)       | `+15%` faster charge speed, `+10%` damage and `15%` bigger projectile                                |
| Corrupted Flood    | ![Corrupted Flood](https://riskofrain2.wiki.gg/images/7/78/Corrupted_Flood.png) | `+15%` damage and `+10%` less cooldown  |
| Trespass           | ![Trespass](https://riskofrain2.wiki.gg/images/7/7f/Trespass.png) | `+50` armor for `3` seconds                                     |
| Corrupted Trespass | ![Corrupted Trespass](https://riskofrain2.wiki.gg/images/0/04/Corrupted_Trespass.png) | `20%` attack speed and `20%` move speed for `+1` second       |
| Suppress           | ![Suppress](https://riskofrain2.wiki.gg/images/a/ad/Suppress.png) | `+15%` extra health on use              |
| Corrupted Suppress         | ![Corrupted Suppress](https://riskofrain2.wiki.gg/images/7/7b/Corrupted_Suppress.png) | `+15%` extra corruption on use                                    |

### Lunar Items

| Skill | | Description |
|:-|-|------|
| Hungering Gaze | ![Visions of Heresy](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/3/3c/Hungering_Gaze.png) | `+2` stock per level and `+20%` damage per level. As Heretic, `+0.25` base damage per level |
| Slicing Maelstrom | ![Hooks Of Heresy](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/e/e9/Slicing_Maelstrom.png) | `+1` maelstrom tick per second and `+15%` hitbox size per level. As Heretic, `+3` base armor per level |
| Shadowfade | ![Strides Of Heresy](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/c/c8/Shadowfade.png) | `+10%` speed bonus and `+15%` healing ticks. As Heretic, `+3%` max health per level |
| Ruin | ![Heart Of Heresy](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/b/bc/Ruin.png) | `+5%` damage and `+20%` Ruin stack odds per attack, per level. As Heretic, `+0.25` base attack speed per level |
