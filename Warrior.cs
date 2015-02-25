#region Honorbuddy

using Styx;

#endregion

#region System

#endregion

namespace Simcraft
{
    public partial class SimcraftImpl
    {
        private readonly ClusterUnit RavagerCluster = new ClusterUnit(40, 6);

        #region PvE

        [Behavior(WoWClass.Warrior, WoWSpec.WarriorFury, WoWContext.PvE)]
        public void GenerateFuryPvEBehavior()
        {
            actions += Cast(Berserker_Rage, ret => buff[Enrage].down);
            //actions += Cast(Berserker_Rage);
            actions += UseItem(13, ret => cooldowns_enabled);
            actions += Cast(Recklessness,
                ret =>
                    cooldowns_enabled &&
                    (((buff[Bloodbath].up || !talent[Bloodbath].enabled)) || target.time_to_die <= 12 ||
                     talent[Anger_Management].enabled));
            actions += Cast(Avatar, ret => talent[Avatar].enabled && cooldowns_enabled && (buff[Recklessness].up || target.time_to_die <= 30));
            actions += CallActionList("single_target", ret => active_enemies == 1 || !aoe_enabled);
            actions += CallActionList("two_targets", ret => active_enemies == 2 && aoe_enabled);
            actions += CallActionList("three_targets", ret => active_enemies == 3 && aoe_enabled);
            actions += CallActionList("aoe", ret => active_enemies > 3 && aoe_enabled);

            actions.single_target += Cast(Bloodbath, ret => cooldowns_enabled);
            actions.single_target += Cast(Wild_Strike, ret => rage > 110 && target.health.pct > 20);
            actions.single_target += Cast(Bloodthirst,
                ret => (!talent[Unquenchable_Thirst].enabled && rage < 80) || buff[Enrage].down);
            actions.single_target += Cast(Ravager,
                ret =>
                    talent[Ravager].enabled && cooldowns_enabled && aoe_enabled && (buff[Bloodbath].up || !talent[Bloodbath].enabled),
                RavagerCluster);
            actions.single_target += Cast(Execute, ret => buff[Sudden_Death].react);
            actions.single_target += Cast(Siegebreaker);
            actions.single_target += Cast(Storm_Bolt);
            actions.single_target += Cast(Wild_Strike, ret => buff[Bloodsurge].up);
            actions.single_target += Cast(Execute, ret => buff[Enrage].up || target.time_to_die < 12);
            actions.single_target += Cast(Dragon_Roar,
                ret => aoe_enabled && cooldowns_enabled && (buff[Bloodbath].up || !talent[Bloodbath].enabled));
            actions.single_target += Cast(Raging_Blow);
            actions.single_target += Cast(Wild_Strike, ret => buff[Enrage].up && target.health.pct > 20);
            actions.single_target += Cast(Bladestorm, ret => aoe_enabled && cooldowns_enabled);
            actions.single_target += Cast(Shockwave, ret => aoe_enabled && !talent[Unquenchable_Thirst].enabled);
            actions.single_target += Cast(Impending_Victory,
                ret => !talent[Unquenchable_Thirst].enabled && target.health.pct > 20);
            actions.single_target += Cast(Bloodthirst);

            actions.two_targets += Cast(Bloodbath, ret => cooldowns_enabled);
            actions.two_targets += Cast(Ravager,
                ret =>
                    talent[Ravager].enabled && cooldowns_enabled && aoe_enabled && (buff[Bloodbath].up || !talent[Bloodbath].enabled),
                RavagerCluster);
            actions.two_targets += Cast(Dragon_Roar,
                ret => cooldowns_enabled && (buff[Bloodbath].up || !talent[Bloodbath].enabled));
            actions.two_targets += Cast(Bladestorm, ret => cooldowns_enabled && buff[Enrage].up);
            actions.two_targets += Cast(Bloodthirst, ret => buff[Enrage].down || rage < 50 || buff[Raging_BlowEx].down);
            actions.two_targets += Cast(Execute,
                ret => target.health.pct < 20 && Me.IsFacing(Target2) && Target2.IsWithinMeleeRange, Target2);
            actions.two_targets += Cast(Execute, ret => (target.health.pct < 20 || buff[Sudden_Death].react));
            actions.two_targets += Cast(Raging_Blow, ret => buff[Meat_Cleaver].up);
            actions.two_targets += Cast(Whirlwind, ret => !buff[Meat_Cleaver].up);
            actions.two_targets += Cast(Wild_Strike, ret => buff[Bloodsurge].up && rage > 75);
            actions.two_targets += Cast(Bloodthirst);
            actions.two_targets += Cast(Whirlwind, ret => rage > rage.Max - 20);
            actions.two_targets += Cast(Wild_Strike, ret => buff[Bloodsurge].up);

            actions.three_targets += Cast(Bloodbath, ret => cooldowns_enabled);
            actions.three_targets += Cast(Ravager,
                ret =>
                    talent[Ravager].enabled && cooldowns_enabled && aoe_enabled && (buff[Bloodbath].up || !talent[Bloodbath].enabled),
                RavagerCluster);
            actions.three_targets += Cast(Bladestorm, ret => cooldowns_enabled && buff[Enrage].up);
            actions.three_targets += Cast(Bloodthirst, ret => buff[Enrage].down || rage < 50 || buff[Raging_BlowEx].down);
            actions.three_targets += Cast(Raging_Blow, ret => buff[Meat_Cleaver].Stack >= 2);
            actions.three_targets += Cast(Execute, ret => buff[Sudden_Death].react);
            actions.three_targets += Cast(Execute, ret => (target.health.pct < 20 || buff[Sudden_Death].react));
            actions.three_targets += Cast(Execute, ret => target.health.pct < 20 && position_front && target.melee_range,
                Target2);
            actions.three_targets += Cast(Execute, ret => target.health.pct < 20 && position_front && target.melee_range,
                Target3);
            actions.three_targets += Cast(Dragon_Roar,
                ret => cooldowns_enabled && (buff[Bloodbath].up || !talent[Bloodbath].enabled));
            actions.three_targets += Cast(Whirlwind);
            actions.three_targets += Cast(Bloodthirst);
            actions.three_targets += Cast(Wild_Strike, ret => buff[Bloodsurge].up);

            actions.aoe += Cast(Bloodbath, ret => cooldowns_enabled);
            actions.aoe += Cast(Ravager,
                ret =>
                    talent[Ravager].enabled && cooldowns_enabled && aoe_enabled && (buff[Bloodbath].up || !talent[Bloodbath].enabled),
                RavagerCluster);
            actions.aoe += Cast(Raging_Blow, ret => buff[Meat_Cleaver].Stack >= 3 && buff[Enrage].up);
            actions.aoe += Cast(Bloodthirst, ret => buff[Enrage].down || rage < 50 || buff[Raging_BlowEx].down);
            actions.aoe += Cast(Raging_Blow, ret => buff[Meat_Cleaver].Stack >= 3);
            actions.aoe += Cast(Recklessness,
                ret => cooldowns_enabled && cooldown[Bladestorm].up && buff[Enrage].remains > 6);
            actions.aoe += Cast(Bladestorm, ret => cooldowns_enabled && buff[Enrage].remains > 6);
            actions.aoe += Cast(Whirlwind);
            actions.aoe += Cast(Execute, ret => buff[Sudden_Death].react);
            actions.aoe += Cast(Dragon_Roar,
                ret => cooldowns_enabled && (buff[Bloodbath].up || !talent[Bloodbath].enabled));
            actions.aoe += Cast(Bloodthirst);
            actions.aoe += Cast(Wild_Strike, ret => buff[Bloodsurge].up);
        }
        [Behavior(WoWClass.Warrior, WoWSpec.WarriorFury, WoWContext.PvE)]
        public void GenerateArmsPvEBehavior()
        {
            actions += Cast(Berserker_Rage, ret => buff[Enrage].down);
            //actions += Cast(Berserker_Rage);
            actions += UseItem(13, ret => cooldowns_enabled);
            actions += Cast(Recklessness,
                ret =>
                    talent[Bladestorm].enabled && cooldowns_enabled && 
                    (((debuff[Colossus_Smash].Up || !talent[Bloodbath].enabled)) || buff[Bloodbath].up || target.time_to_die <= 10 ));
            actions += Cast(Avatar, ret => talent[Avatar].enabled && cooldowns_enabled && (buff[Recklessness].up || target.time_to_die <= 25));
            actions += CallActionList("single_target", ret => active_enemies == 1 || !aoe_enabled);
            actions += CallActionList("two_targets", ret => active_enemies == 2 && aoe_enabled);
            actions += CallActionList("three_targets", ret => active_enemies == 3 && aoe_enabled);
            actions += CallActionList("aoe", ret => active_enemies >1 && aoe_enabled);

            actions.single_target += Cast(Bloodbath, ret => cooldowns_enabled);
            actions.single_target += Cast(Rend, ret => target.time_to_die>4 && dot[Rend].remains<5.4 && (target.health.pct>20|!debuff[Colossus_Smash].Up));
            actions.single_target += Cast(Ravager,
                ret =>
                    talent[Ravager].enabled && cooldowns_enabled && aoe_enabled && (cooldown[Colossus_Smash].remains <4),
                RavagerCluster);
            actions.single_target += Cast(Execute, ret => buff[Sudden_Death].react);
            actions.single_target += Cast(Colossus_Smash);
            actions.single_target += Cast(Mortal_Strike, ret => target.health.pct >20);
            actions.single_target += Cast(Storm_Bolt, ret => target.health.pct>20 || (target.health.pct<20 && !debuff[Colossus_Smash].Up));          
            actions.single_target += Cast(Execute, ret => buff[Enrage].up || target.time_to_die < 12);
            actions.single_target += Cast(Dragon_Roar,
                ret => aoe_enabled && cooldowns_enabled && debuff[Colossus_Smash].Up || talent[Anger_Management].enabled);
            actions.single_target += Cast(Siegebreaker);
            actions.single_target += Cast(Execute, ret => buff[Sudden_Death].react);
            actions.single_target += Cast(Execute, ret => !buff[Sudden_Death].react & (rage > 72 & cooldown[Colossus_Smash].remains > 6) | debuff[Colossus_Smash].Up | target.time_to_die < 5);
            actions.single_target += Cast(Bladestorm, ret => aoe_enabled && cooldowns_enabled);
            actions.single_target += Cast(Shockwave, ret => aoe_enabled && !talent[Unquenchable_Thirst].enabled);
            actions.single_target += Cast(Impending_Victory,
                ret => rage < 40 & target.health.pct > 20 & cooldown[Colossus_Smash].remains > 1);
            actions.single_target += Cast(Slam, ret => (rage>20 || cooldown[Colossus_Smash].remains >1 ) & target.health.pct>20 & cooldown[Colossus_Smash].remains >1 );
            actions.single_target += Cast(Thunder_Clap, ret => !talent[Slam].enabled & target.health.pct > 20 & (rage >= 40 | debuff[Colossus_Smash].Up) & cooldown[Colossus_Smash].remains > 5);

            actions.aoe += Cast(Sweeping_Strikes);
            actions.aoe += Cast(Rend, ret => dot[Rend].remains <2 & target.time_to_die >4 && dot[Rend].remains<4 && (target.health.pct>20|!debuff[Colossus_Smash].Up));
            actions.aoe += Cast(Rend, ret => dot[Rend].remains <2 & target.time_to_die >8 & !buff[Colossus_Smash].up & talent[Taste_for_Blood].enabled & active_enemies<=2);
            actions.aoe += Cast(Rend, ret => dot[Rend].remains <2 & target.time_to_die >18 & !buff[Colossus_Smash].up & active_enemies<=8);
            actions.aoe += Cast(Ravager,
                ret =>
                    talent[Ravager].enabled && cooldowns_enabled && aoe_enabled && (buff[Colossus_Smash].up || cooldown[Colossus_Smash].remains < 4),
                RavagerCluster);
            actions.aoe += Cast(Bladestorm, ret => ((debuff[Colossus_Smash].Up | cooldown[Colossus_Smash].remains>3) & target.health.pct>20) | (target.health.pct<20 & rage<30 & cooldown[Colossus_Smash].remains>4));
            actions.aoe += Cast(Colossus_Smash, ret => dot[Rend].Ticking);
            actions.aoe += Cast(Recklessness,
                ret => cooldowns_enabled && cooldown[Bladestorm].up && buff[Enrage].remains > 6);
            actions.aoe += Cast(Bladestorm, ret => cooldowns_enabled && buff[Enrage].remains > 6);
            actions.aoe += Cast(Whirlwind);
            actions.aoe += Cast(Execute, ret => (!buff[Sudden_Death].react & active_enemies <= 8 & ((rage > 72 & cooldown[Colossus_Smash].remains > 1) | rage > 80 | target.time_to_die < 5 | debuff[Colossus_Smash].Up)));
            actions.aoe += Cast(Mortal_Strike, ret => target.health.pct > 20 & active_enemies <= 5);
            actions.aoe += Cast(Dragon_Roar,
                ret => cooldowns_enabled && (debuff[Colossus_Smash].Up));
            actions.aoe += Cast(Rend, ret => dot[Rend].remains < 2 & target.time_to_die > 8 & !buff[Colossus_Smash].up & active_enemies >= 9 & rage < 50 & !talent[Taste_for_Blood].enabled);
            actions.aoe += Cast(Whirlwind, ret => target.health.pct > 20 | active_enemies >= 9);
            actions.aoe += Cast(Siegebreaker, ret => talent[Siegebreaker].enabled); 
            actions.aoe += Cast(Storm_Bolt, ret => talent[Storm_Bolt].enabled & cooldown[Colossus_Smash].remains > 4 | debuff[Colossus_Smash].Up);
            actions.aoe += Cast(Shockwave, ret => talent[Shockwave].enabled);
            actions.aoe += Cast(Execute, ret => buff[Sudden_Death].react);
           
        
        
        }
        #endregion
    }
}