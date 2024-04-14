using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandOption : MonoBehaviour
{
    [SerializeField] private CommandType commandType;

    // Function
    public CommandType getCommand()
    {
        return commandType;
    }
}
