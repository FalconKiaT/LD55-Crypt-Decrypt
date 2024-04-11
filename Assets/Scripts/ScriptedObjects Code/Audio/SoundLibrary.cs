using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

/// <summary> 
/// <para> Scriptable Object that contains the references to all sounds in game </para> 
/// <para> WARNING: YOU CANT PLAY SOUNDS FROM THIS OBJECT, IT ONLY HOLDS EVENT REFERENCES </para>
/// Create a FMOD.Studio.EventInstance from the references
/// </summary>
[CreateAssetMenu(fileName = "ScriptableSoundLibrary", menuName = "ScriptableObjects/SoundLibrary")]
public class SoundLibrary : ScriptableObject
{
    // TODO: Maybe create a custom editor script for all this

    // VCAs:
    // The string must contain the name assigned to the bus on the FMOD Mixer
    // this is useful if someone decides to change how they are named without re-scripting everything
    [Header("VCA Bus Names")]
    public string masterBus;
    public string musicBus;
    public string sfxBus;

    // Bus Groups allows us to do apply changes to a group of sound effects if playing
    //[Header("Group Bus Names")]
    //[SerializeField] private string onLevelCombatPath;

    // Bus variables
    //public FMOD.Studio.Bus onLevelCombatBus { get; private set; }


    // FMOD Sound References: assign these on the inspector
    [Header("TESTING EFFECTS")]
    [SerializeField] private FMODUnity.EventReference _tempHurt;
    public FMODUnity.EventReference TempHurt { get; private set; }
    [SerializeField] private FMODUnity.EventReference _tempBattleMusic;
    public static FMODUnity.EventReference TempBattleMusic { get; private set; }


    // Function to initialize data, must be called by the sound manager
    public void InitializeLibrary()
    {
        // Fetch Group Buses
        //onLevelCombatBus = FMODUnity.RuntimeManager.GetBus("bus:/" + onLevelCombatPath);

        // TESTING EFFECTS
        TempHurt = _tempHurt;
        TempBattleMusic = _tempBattleMusic;
    }

}


