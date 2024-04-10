using System;
using System.Collections;
using UnityEngine;

// Documentation
#if false
--------------------------------------------------------------------------
Set of useful functions for games, developed by FalconKiat!
Version 1.0 (Although im not tracking changes rn lel)
--------------------------------------------------------------------------

--------------------------------------------------------------------------
                            FKToolsConfig:
All the values that the programmer should change to adapat them to their
game

> gamePausedEval: 
Should evaluate to some static bool value controlled by a game manager.
If not configured, the code will behave like regular coroutine yields
--------------------------------------------------------------------------

--------------------------------------------------------------------------
                             FKRoutines:
This class contains a bunch of useful methos to pass to coroutines
that will respect the pause state of the game, since setting timescale
to 0 can cause unintended repercusions around the game.

> WaitForSecondsPauseAware / NullPauseAware
Both yields will be stopped whenever the gamePauseEval is set to true, and
unlike the regular WaitForSeconds, after the game de-pauses, it will resume 
where it left off, skipping the need of yield breaking the routine or having 
it continue running in the background
--------------------------------------------------------------------------
#endif

namespace FKTools
{
    public class FKToolsConfig
    {
        // FIXME: IMPLEMENT REFERENCE TO MENU/GAME MANAGER
        protected static bool gamePausedEval => false;
        // Should be something like [gamePausedEval => GameManager.isGamePaused;]
        // Where isGamePaused is a static bool inside the class GameManager
    }

    /// <summary>
    /// The FKTools class that contains all the functions for coroutines
    /// </summary>
    public class FKRoutines : FKToolsConfig
    {
        /// <summary>
        /// <para>  Same behaviour as WaitForSeconds, but can be paused in the middle by a bool controlled
        /// by the specified game manager </para> 
        /// Usage: yield return WaitForSecondsPauseAware(seconds);
        /// </summary>
        public static IEnumerator WaitForSecondsPauseAware(float seconds)
        {
            float waitingTimer = 0f;
            // Starts a timer with the passed argument, will not increase if the game pauses
            return new WaitUntil(() =>
            {
                waitingTimer += Time.deltaTime * (gamePausedEval ? 0 : 1);
                return waitingTimer > seconds;
            });
        }

        /// <summary>
        /// <para> Same behaviour as passing null into yield return, but instead it will wait for 
        /// the game to be unpaused next time this yield is called </para>
        /// Usage: yield return NullYieldPauseAware();
        /// </summary>
        public static IEnumerator NullPauseAware()
        {
            if (gamePausedEval) return new WaitUntil(() => !gamePausedEval);
            else return null;
        }
    }
}