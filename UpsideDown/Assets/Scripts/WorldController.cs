using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using gamespace;

namespace gamespace
{
    public enum WorldType
    {
        GoodWorld,
        BadWorld,
        NOWORLD
    }
}

public static class WorldController
{
    private static GameObject switchEnergy;
    private static SwitchBarUIBar EnergyBar;

    //public event Action<WorldType> OnWorldChanged;
    public static Action<WorldType> OnWorldChanged;
    public static Action<int> OnPlayerDirectionChanged;

    private static WorldType _current_world;
    private static  WorldType current_world
    {
        get { return _current_world; }
        set
        {
            if (_current_world != value)
            {
                _current_world = value;
                OnWorldChanged(_current_world);
            }
        }
    }

    public static void init(){
        _current_world = WorldType.NOWORLD;
        switchEnergy = GameObject.Find("SwitchChargeHUD");
        EnergyBar = switchEnergy.GetComponent<SwitchBarUIBar>();
    }

    public static void UpdateCurrentWorld(WorldType world){
        current_world = world;
    }

    public static WorldType GetCurrentWorld(){
        return current_world;
    }
    public static bool IsWorldGood(){
        return current_world == WorldType.GoodWorld;
    }
    public static bool IsWorldBad(){
        return current_world == WorldType.BadWorld;
    }
    public static SwitchBarUIBar GetEnergyBar(){
        return EnergyBar;
    }
    public static void change_world()
    {
        switch (current_world)
        {
            case WorldType.GoodWorld:
                // musicManagerObject.playWorld("bad");
                // animator.SetBool("good_world", false);
                UpdateCurrentWorld(WorldType.BadWorld);
                break;
            case WorldType.BadWorld:
                // musicManagerObject.playWorld("good");
                // animator.SetBool("good_world", true);
                UpdateCurrentWorld(WorldType.GoodWorld);
                break;
        }
    }
}
