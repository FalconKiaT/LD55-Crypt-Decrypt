using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Documentation
#if false
--------------------------------------------------------------------------
Set of useful functions for games, developed by FalconKiat!
Version 1.0 (Although im not tracking changes rn lel)
--------------------------------------------------------------------------

--------------------------------------------------------------------------
                              FKToolsConfig

All the values that the programmer should change to adapat them to their
game

> gamePausedEval: 
Should evaluate to some static bool value controlled by a game manager.
If not configured, the code will behave like regular coroutine yields
--------------------------------------------------------------------------

--------------------------------------------------------------------------
                               FKRoutines

This class contains a bunch of useful methos to pass to coroutines
that will respect the pause state of the game, since setting timescale
to 0 can cause unintended repercusions around the game.

> WaitForSecondsPauseAware / NullPauseAware
Both yields will be stopped whenever the gamePauseEval is set to true, and
unlike the regular WaitForSeconds, after the game de-pauses, it will resume 
where it left off, skipping the need of yield breaking the routine or having 
it continue running in the background
--------------------------------------------------------------------------

--------------------------------------------------------------------------
                            FKMonoBehaviour

It's the same as using the regular MonoBehaviour class to inherit from in
scripts, but with one caveat. with FKMonoBehaviour, you wouldnt implement
a Update() Function in your script, but rather possibly override two 
functions in the class

FKUpdate: Performs just like the regular Update() Function

FKUpdatePauseAware: Also performs like the Update() function, but will
stop executing if the game is paused
--------------------------------------------------------------------------

--------------------------------------------------------------------------
                                FKAnimator

Another set of functions useful for game pausing, in this case, the tool
wiil keep track of all animator controllers added to the tracking list
and pause them when the function PauseAnimations() is called by a game
manager.
Even though the programmer should take care of removing animator controller
references on destroy, the tool still is capable of working around it.
--------------------------------------------------------------------------

#endif

namespace FKTools
{
    public static class FKToolsConfig
    {
        // FIXME: IMPLEMENT REFERENCE TO MENU/GAME MANAGER
        public static bool gamePausedEval => false;
        // Should be something like [gamePausedEval => GameManager.isGamePaused;]
        // Where isGamePaused is a static bool inside the class GameManager
    }

    /// <summary>
    /// The FKTools class that contains all the functions for coroutines
    /// </summary>
    public static class FKRoutines
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
                waitingTimer += Time.deltaTime * (FKToolsConfig.gamePausedEval ? 0 : 1);
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
            if (FKToolsConfig.gamePausedEval) return new WaitUntil(() => !FKToolsConfig.gamePausedEval);
            else return null;
        }
    }

    /// <summary>
    /// <para> Derived class from MonoBehaviour that contains two update functions, one
    /// acts as the default, and the other only executes if the game is not paused </para>
    /// Inherit from FKMonoBehaviour instead of MonoBehaviour and override the update functions to use
    /// </summary>
    public class FKMonoBehaviour : MonoBehaviour
    {
        // Update function will act weirdly if present in inherited script
        private void Update()
        {
            // Call Regular Update
            FKUpdate();
            // Call Pause Aware Update only if game is not paused
            if (!FKToolsConfig.gamePausedEval) FKUpdatePauseAware();
        }

        // Same as above, but with fixed update
        private void FixedUpdate()
        {
            // Call Regular FixedUpdate
            FKFixedUpdate();
            // Call Pause Aware Fixed Update only if game is not paused
            if (!FKToolsConfig.gamePausedEval) FKFixedUpdatePauseAware();
        }

        /// <summary>
        /// <para> The regular function you would call in a Unity Game script, its the same as Update(). Override to use </para>
        /// WARNING: If you have an Update() function in your script, this wont run properly
        /// </summary>
        public virtual void FKUpdate() { }

        /// <summary>
        /// <para> Performs exactly like Update(), but it wont execute whenever the game is paused. Override to use </para>
        /// WARNING: If you have an Update() function in your script, this wont run properly
        /// </summary>
        public virtual void FKUpdatePauseAware() { }

        /// <summary>
        /// <para> The regular function you would call in a Unity Game script, its the same as FixedUpdate(). Override to use </para>
        /// WARNING: If you have an FixedUpdate() function in your script, this wont run properly
        /// </summary>
        public virtual void FKFixedUpdate() { }

        /// <summary>
        /// <para> Performs exactly like FixedUpdate(), but it wont execute whenever the game is paused. Override to use </para>
        /// WARNING: If you have an FixedUpdate() function in your script, this wont run properly
        /// </summary>
        public virtual void FKFixedUpdatePauseAware() { }
    }

    /// <summary>
    /// <para> Tool class that contains an Animator Pause system, will also pause any controllers added after a pause </para>
    /// A game manager should call Pause/Resume Animations and all GameObjects
    /// with the Animator Controllers adding themselves to the tracking list.
    /// </summary>
    public static class FKAnimator
    {
        private static List<Animator> AnimatorsListening = new List<Animator>();

        /// <summary>
        /// Adds the passed Animator component to the list of Animators that will be paused
        /// by the game
        /// </summary>
        public static void AddToPauseList(Animator input)
        {
            // Only add if not already in the array
            if (!AnimatorsListening.Contains(input))
            {
                AnimatorsListening.Add(input);

                // Set its speed to 0 if the game was paused while added
                if (FKToolsConfig.gamePausedEval) input.speed = 0;
            }
        }

        /// <summary>
        /// <para> Removes the passed Animator component from the list of Animators if present </para>
        /// Should be called on a OnDestroy() function
        /// </summary>
        public static void RemoveFromPauseList(Animator input)
        {
            AnimatorsListening.Remove(input);
        }

        /// <summary>
        /// Function to be called by a game manager to pause all Animators that are
        /// listening to pause
        /// </summary>
        public static void PauseAnimations()
        {
            // List to hold all null references found
            List<Animator> nullComponentsFound = new();
            // Loop to set speed to 0 of all animators listening to pause
            foreach (Animator animator in AnimatorsListening)
            {
                // NOTE: The programmer should take care of removing animator controllers
                // From the list on destroy, but a null check is performed just in case
                if (animator == null)
                {
                    nullComponentsFound.Add(animator);
                    continue;
                }
                animator.speed = 0;
            }
            // Remove null animators found from list
            foreach (Animator animator in nullComponentsFound)
            {
                AnimatorsListening.Remove(animator);
            }
        }

        /// <summary>
        /// Function to be called by a game manager to resume all Animators that were
        /// paused before
        /// </summary>
        public static void ResumeAnimations()
        {
            // List to hold all null references found
            List<Animator> nullComponentsFound = new();
            // Loop to set speed to 0 of all animators listening to pause
            foreach (Animator animator in AnimatorsListening)
            {
                // NOTE: The programmer should take care of removing animator controllers
                // From the list on destroy, but a null check is performed just in case
                if (animator == null)
                {
                    nullComponentsFound.Add(animator);
                    continue;
                }
                animator.speed = 1;
            }
            // Remove null animators found from list
            foreach (Animator animator in nullComponentsFound)
            {
                AnimatorsListening.Remove(animator);
            }
        }
    }
}