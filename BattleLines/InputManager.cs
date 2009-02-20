#region File Description
//-----------------------------------------------------------------------------
// InputManager.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace BattleLines
{
    /// <summary>
    /// This class handles all keyboard and gamepad actions in the game.
    /// </summary>
    public static class InputManager
    {
        #region Action Enumeration


        /// <summary>
        /// The actions that are possible within the game.
        /// </summary>
        public enum Action
        {
            ExitGame,
            Select1,
            Select2,
            Select3,
            Select4,
            SelectCursor,
            MoveTo,
            ActionAt,
            Retreat,
            Advance,
            StatusItemNext,
            StatusItemPrev,
            Chat,
            ViewLeft,
            ViewRight,
            ViewUp,
            ViewDown,
            ZoomOut,
            ZoomIn,
            TotalActionCount
        }


        /// <summary>
        /// Readable names of each action.
        /// </summary>
        private static readonly string[] actionNames = 
            {
                "Exit Game",
                "Select Squad 1",
                "Select Squad 2",
                "Select Squad 3",
                "Select Squad 4",
                "Select At Cursor",
                "Move To Cursor",
                "Action At Cursor",
                "Retreat",
                "Advance",
                "Next Status Item",
                "Previous Status Item",
                "Chat",
                "Move View Left",
                "Move View Right",
                "Move View Up",
                "Move View Down",
                "Zoom Out",
                "Zoom In"
            };

        /// <summary>
        /// Returns the readable name of the given action.
        /// </summary>
        public static string GetActionName(Action action)
        {
            int index = (int)action;

            if ((index < 0) || (index > actionNames.Length))
            {
                throw new ArgumentException("action");
            }

            return actionNames[index];
        }


        #endregion


        #region Support Types


        /// <summary>
        /// GamePad controls expressed as one type, unified with button semantics.
        /// </summary>
        public enum GamePadButtons
        {
            Start,
            Back,
            A,
            B,
            X,
            Y,
            Up,
            Down,
            Left,
            Right,
            LeftShoulder,
            RightShoulder,
            LeftTrigger,
            RightTrigger
        }

        public enum MouseButtons
        {
            Left,
            Right,
            Middle,
            ScrollUp,
            ScrollDown
        }


        /// <summary>
        /// A combination of gamepad and keyboard keys mapped to a particular action.
        /// </summary>
        public class ActionMap
        {
            /// <summary>
            /// List of GamePad controls to be mapped to a given action.
            /// </summary>
            public List<GamePadButtons> gamePadButtons = new List<GamePadButtons>();


            /// <summary>
            /// List of Keyboard controls to be mapped to a given action.
            /// </summary>
            public List<Keys> keyboardKeys = new List<Keys>();

            public List<MouseButtons> mouseButtons = new List<MouseButtons>();
        }


        #endregion


        #region Constants


        /// <summary>
        /// The value of an analog control that reads as a "pressed button".
        /// </summary>
        const float analogLimit = 0.5f;


        #endregion


        #region Keyboard Data


        /// <summary>
        /// The state of the keyboard as of the last update.
        /// </summary>
        private static KeyboardState currentKeyboardState;

        /// <summary>
        /// The state of the keyboard as of the last update.
        /// </summary>
        public static KeyboardState CurrentKeyboardState
        {
            get { return currentKeyboardState; }
        }


        /// <summary>
        /// The state of the keyboard as of the previous update.
        /// </summary>
        private static KeyboardState previousKeyboardState;


        /// <summary>
        /// Check if a key is pressed.
        /// </summary>
        public static bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }


        /// <summary>
        /// Check if a key was just pressed in the most recent update.
        /// </summary>
        public static bool IsKeyTriggered(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key)) &&
                (!previousKeyboardState.IsKeyDown(key));
        }


        #endregion

        #region Mouse Data

        private static MouseState currentMouseState;
        public static MouseState CurrentMouseState
        {
            get { return currentMouseState; }
        }

        private static MouseState previousMouseState;

        public static bool IsMouseButtonPressed(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return IsMouseLeftPressed();
                case MouseButtons.Right:
                    return IsMouseRightPressed();
                case MouseButtons.Middle:
                    return IsMouseMiddlePressed();
            }

            return false;
        }

        public static bool IsMouseLeftPressed()
        {
            return (currentMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool IsMouseRightPressed()
        {
            return (currentMouseState.RightButton == ButtonState.Pressed);
        }

        public static bool IsMouseButtonTriggered(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return IsMouseLeftTriggered();
                case MouseButtons.Right:
                    return IsMouseRightTriggered();
                case MouseButtons.Middle:
                    return IsMouseMiddleTriggered();
                case MouseButtons.ScrollDown:
                    return IsMouseScrollDownTriggered();
                case MouseButtons.ScrollUp:
                    return IsMouseScrollUpTriggered();
            }

            return false;
        }

        public static bool IsMouseMiddlePressed()
        {
            return (currentMouseState.MiddleButton == ButtonState.Pressed);
        }

        public static bool IsMouseLeftTriggered()
        {
            return ((currentMouseState.LeftButton == ButtonState.Pressed) &&
                (previousMouseState.LeftButton == ButtonState.Released));
        }

        public static bool IsMouseRightTriggered()
        {
            return ((currentMouseState.RightButton == ButtonState.Pressed) &&
                (previousMouseState.RightButton == ButtonState.Released));
        }

        public static bool IsMouseMiddleTriggered()
        {
            return ((currentMouseState.MiddleButton == ButtonState.Pressed) &&
                (previousMouseState.MiddleButton == ButtonState.Released));
        }

        public static bool IsMouseScrollDownTriggered()
        {
            return (currentMouseState.ScrollWheelValue <
                    previousMouseState.ScrollWheelValue);
        }

        public static bool IsMouseScrollUpTriggered()
        {
            return (currentMouseState.ScrollWheelValue >
                    previousMouseState.ScrollWheelValue);
        }

        #endregion

        #region GamePad Data


        /// <summary>
        /// The state of the gamepad as of the last update.
        /// </summary>
        private static GamePadState currentGamePadState;

        /// <summary>
        /// The state of the gamepad as of the last update.
        /// </summary>
        public static GamePadState CurrentGamePadState
        {
            get { return currentGamePadState; }
        }


        /// <summary>
        /// The state of the gamepad as of the previous update.
        /// </summary>
        private static GamePadState previousGamePadState;


        #region GamePadButton Pressed Queries


        /// <summary>
        /// Check if the gamepad's Start button is pressed.
        /// </summary>
        public static bool IsGamePadStartPressed()
        {
            return (currentGamePadState.Buttons.Start == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's Back button is pressed.
        /// </summary>
        public static bool IsGamePadBackPressed()
        {
            return (currentGamePadState.Buttons.Back == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's A button is pressed.
        /// </summary>
        public static bool IsGamePadAPressed()
        {
            return (currentGamePadState.Buttons.A == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's B button is pressed.
        /// </summary>
        public static bool IsGamePadBPressed()
        {
            return (currentGamePadState.Buttons.B == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's X button is pressed.
        /// </summary>
        public static bool IsGamePadXPressed()
        {
            return (currentGamePadState.Buttons.X == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's Y button is pressed.
        /// </summary>
        public static bool IsGamePadYPressed()
        {
            return (currentGamePadState.Buttons.Y == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's LeftShoulder button is pressed.
        /// </summary>
        public static bool IsGamePadLeftShoulderPressed()
        {
            return (currentGamePadState.Buttons.LeftShoulder == ButtonState.Pressed);
        }


        /// <summary>
        /// <summary>
        /// Check if the gamepad's RightShoulder button is pressed.
        /// </summary>
        public static bool IsGamePadRightShoulderPressed()
        {
            return (currentGamePadState.Buttons.RightShoulder == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Up on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadUpPressed()
        {
            return (currentGamePadState.DPad.Up == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Down on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadDownPressed()
        {
            return (currentGamePadState.DPad.Down == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Left on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadLeftPressed()
        {
            return (currentGamePadState.DPad.Left == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Right on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadRightPressed()
        {
            return (currentGamePadState.DPad.Right == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's left trigger is pressed.
        /// </summary>
        public static bool IsGamePadLeftTriggerPressed()
        {
            return (currentGamePadState.Triggers.Left > analogLimit);
        }


        /// <summary>
        /// Check if the gamepad's right trigger is pressed.
        /// </summary>
        public static bool IsGamePadRightTriggerPressed()
        {
            return (currentGamePadState.Triggers.Right > analogLimit);
        }


        /// <summary>
        /// Check if Up on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickUpPressed()
        {
            return (currentGamePadState.ThumbSticks.Left.Y > analogLimit);
        }


        /// <summary>
        /// Check if Down on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickDownPressed()
        {
            return (-1f * currentGamePadState.ThumbSticks.Left.Y > analogLimit);
        }


        /// <summary>
        /// Check if Left on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickLeftPressed()
        {
            return (-1f * currentGamePadState.ThumbSticks.Left.X > analogLimit);
        }


        /// <summary>
        /// Check if Right on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickRightPressed()
        {
            return (currentGamePadState.ThumbSticks.Left.X > analogLimit);
        }


        /// <summary>
        /// Check if the GamePadKey value specified is pressed.
        /// </summary>
        private static bool IsGamePadButtonPressed(GamePadButtons gamePadKey)
        {
            switch (gamePadKey)
            {
                case GamePadButtons.Start:
                    return IsGamePadStartPressed();

                case GamePadButtons.Back:
                    return IsGamePadBackPressed();

                case GamePadButtons.A:
                    return IsGamePadAPressed();

                case GamePadButtons.B:
                    return IsGamePadBPressed();

                case GamePadButtons.X:
                    return IsGamePadXPressed();

                case GamePadButtons.Y:
                    return IsGamePadYPressed();

                case GamePadButtons.LeftShoulder:
                    return IsGamePadLeftShoulderPressed();

                case GamePadButtons.RightShoulder:
                    return IsGamePadRightShoulderPressed();

                case GamePadButtons.LeftTrigger:
                    return IsGamePadLeftTriggerPressed();

                case GamePadButtons.RightTrigger:
                    return IsGamePadRightTriggerPressed();

                case GamePadButtons.Up:
                    return IsGamePadDPadUpPressed() ||
                        IsGamePadLeftStickUpPressed();

                case GamePadButtons.Down:
                    return IsGamePadDPadDownPressed() ||
                        IsGamePadLeftStickDownPressed();

                case GamePadButtons.Left:
                    return IsGamePadDPadLeftPressed() ||
                        IsGamePadLeftStickLeftPressed();

                case GamePadButtons.Right:
                    return IsGamePadDPadRightPressed() ||
                        IsGamePadLeftStickRightPressed();
            }

            return false;
        }


        #endregion


        #region GamePadButton Triggered Queries


        /// <summary>
        /// Check if the gamepad's Start button was just pressed.
        /// </summary>
        public static bool IsGamePadStartTriggered()
        {
            return ((currentGamePadState.Buttons.Start == ButtonState.Pressed) &&
              (previousGamePadState.Buttons.Start == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's Back button was just pressed.
        /// </summary>
        public static bool IsGamePadBackTriggered()
        {
            return ((currentGamePadState.Buttons.Back == ButtonState.Pressed) &&
              (previousGamePadState.Buttons.Back == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's A button was just pressed.
        /// </summary>
        public static bool IsGamePadATriggered()
        {
            return ((currentGamePadState.Buttons.A == ButtonState.Pressed) &&
              (previousGamePadState.Buttons.A == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's B button was just pressed.
        /// </summary>
        public static bool IsGamePadBTriggered()
        {
            return ((currentGamePadState.Buttons.B == ButtonState.Pressed) &&
              (previousGamePadState.Buttons.B == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's X button was just pressed.
        /// </summary>
        public static bool IsGamePadXTriggered()
        {
            return ((currentGamePadState.Buttons.X == ButtonState.Pressed) &&
              (previousGamePadState.Buttons.X == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's Y button was just pressed.
        /// </summary>
        public static bool IsGamePadYTriggered()
        {
            return ((currentGamePadState.Buttons.Y == ButtonState.Pressed) &&
              (previousGamePadState.Buttons.Y == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's LeftShoulder button was just pressed.
        /// </summary>
        public static bool IsGamePadLeftShoulderTriggered()
        {
            return (
                (currentGamePadState.Buttons.LeftShoulder == ButtonState.Pressed) &&
                (previousGamePadState.Buttons.LeftShoulder == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's RightShoulder button was just pressed.
        /// </summary>
        public static bool IsGamePadRightShoulderTriggered()
        {
            return (
                (currentGamePadState.Buttons.RightShoulder == ButtonState.Pressed) &&
                (previousGamePadState.Buttons.RightShoulder == ButtonState.Released));
        }


        /// <summary>
        /// Check if Up on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadUpTriggered()
        {
            return ((currentGamePadState.DPad.Up == ButtonState.Pressed) &&
              (previousGamePadState.DPad.Up == ButtonState.Released));
        }


        /// <summary>
        /// Check if Down on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadDownTriggered()
        {
            return ((currentGamePadState.DPad.Down == ButtonState.Pressed) &&
              (previousGamePadState.DPad.Down == ButtonState.Released));
        }


        /// <summary>
        /// Check if Left on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadLeftTriggered()
        {
            return ((currentGamePadState.DPad.Left == ButtonState.Pressed) &&
              (previousGamePadState.DPad.Left == ButtonState.Released));
        }


        /// <summary>
        /// Check if Right on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadRightTriggered()
        {
            return ((currentGamePadState.DPad.Right == ButtonState.Pressed) &&
              (previousGamePadState.DPad.Right == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's left trigger was just pressed.
        /// </summary>
        public static bool IsGamePadLeftTriggerTriggered()
        {
            return ((currentGamePadState.Triggers.Left > analogLimit) &&
                (previousGamePadState.Triggers.Left < analogLimit));
        }


        /// <summary>
        /// Check if the gamepad's right trigger was just pressed.
        /// </summary>
        public static bool IsGamePadRightTriggerTriggered()
        {
            return ((currentGamePadState.Triggers.Right > analogLimit) &&
                (previousGamePadState.Triggers.Right < analogLimit));
        }


        /// <summary>
        /// Check if Up on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickUpTriggered()
        {
            return ((currentGamePadState.ThumbSticks.Left.Y > analogLimit) &&
                (previousGamePadState.ThumbSticks.Left.Y < analogLimit));
        }


        /// <summary>
        /// Check if Down on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickDownTriggered()
        {
            return ((-1f * currentGamePadState.ThumbSticks.Left.Y > analogLimit) &&
                (-1f * previousGamePadState.ThumbSticks.Left.Y < analogLimit));
        }


        /// <summary>
        /// Check if Left on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickLeftTriggered()
        {
            return ((-1f * currentGamePadState.ThumbSticks.Left.X > analogLimit) &&
                (-1f * previousGamePadState.ThumbSticks.Left.X < analogLimit));
        }


        /// <summary>
        /// Check if Right on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickRightTriggered()
        {
            return ((currentGamePadState.ThumbSticks.Left.X > analogLimit) &&
                (previousGamePadState.ThumbSticks.Left.X < analogLimit));
        }


        /// <summary>
        /// Check if the GamePadKey value specified was pressed this frame.
        /// </summary>
        private static bool IsGamePadButtonTriggered(GamePadButtons gamePadKey)
        {
            switch (gamePadKey)
            {
                case GamePadButtons.Start:
                    return IsGamePadStartTriggered();

                case GamePadButtons.Back:
                    return IsGamePadBackTriggered();

                case GamePadButtons.A:
                    return IsGamePadATriggered();

                case GamePadButtons.B:
                    return IsGamePadBTriggered();

                case GamePadButtons.X:
                    return IsGamePadXTriggered();

                case GamePadButtons.Y:
                    return IsGamePadYTriggered();

                case GamePadButtons.LeftShoulder:
                    return IsGamePadLeftShoulderTriggered();

                case GamePadButtons.RightShoulder:
                    return IsGamePadRightShoulderTriggered();

                case GamePadButtons.LeftTrigger:
                    return IsGamePadLeftTriggerTriggered();

                case GamePadButtons.RightTrigger:
                    return IsGamePadRightTriggerTriggered();

                case GamePadButtons.Up:
                    return IsGamePadDPadUpTriggered() ||
                        IsGamePadLeftStickUpTriggered();

                case GamePadButtons.Down:
                    return IsGamePadDPadDownTriggered() ||
                        IsGamePadLeftStickDownTriggered();

                case GamePadButtons.Left:
                    return IsGamePadDPadLeftTriggered() ||
                        IsGamePadLeftStickLeftTriggered();

                case GamePadButtons.Right:
                    return IsGamePadDPadRightTriggered() ||
                        IsGamePadLeftStickRightTriggered();
            }

            return false;
        }


        #endregion


        #endregion


        #region Action Mapping


        /// <summary>
        /// The action mappings for the game.
        /// </summary>
        private static ActionMap[] actionMaps;


        public static ActionMap[] ActionMaps
        {
            get { return actionMaps; }
        }


        /// <summary>
        /// Reset the action maps to their default values.
        /// </summary>
        private static void ResetActionMaps()
        {
            actionMaps = new ActionMap[(int)Action.TotalActionCount];

            actionMaps[(int)Action.ExitGame] = new ActionMap();
            actionMaps[(int)Action.ExitGame].keyboardKeys.Add(
                Keys.Escape);
            actionMaps[(int)Action.ExitGame].gamePadButtons.Add(
                GamePadButtons.Back);

            actionMaps[(int)Action.Select1] = new ActionMap();
            actionMaps[(int)Action.Select1].keyboardKeys.Add(
                Keys.A);
            actionMaps[(int)Action.Select1].keyboardKeys.Add(
                Keys.J);

            actionMaps[(int)Action.Select2] = new ActionMap();
            actionMaps[(int)Action.Select2].keyboardKeys.Add(
                Keys.S);
            actionMaps[(int)Action.Select2].keyboardKeys.Add(
                Keys.K);

            actionMaps[(int)Action.Select3] = new ActionMap();
            actionMaps[(int)Action.Select3].keyboardKeys.Add(
                Keys.D);
            actionMaps[(int)Action.Select3].keyboardKeys.Add(
                Keys.L);

            actionMaps[(int)Action.Select4] = new ActionMap();
            actionMaps[(int)Action.Select4].keyboardKeys.Add(
                Keys.F);
            actionMaps[(int)Action.Select4].keyboardKeys.Add(
                Keys.OemSemicolon);

            actionMaps[(int)Action.SelectCursor] = new ActionMap();
            actionMaps[(int)Action.SelectCursor].mouseButtons.Add(
                MouseButtons.Left);
            actionMaps[(int)Action.SelectCursor].gamePadButtons.Add(
                GamePadButtons.A);

            actionMaps[(int)Action.MoveTo] = new ActionMap();
            actionMaps[(int)Action.MoveTo].mouseButtons.Add(
                MouseButtons.Right);
            actionMaps[(int)Action.MoveTo].gamePadButtons.Add(
                GamePadButtons.B);

            actionMaps[(int)Action.ActionAt] = new ActionMap();
            actionMaps[(int)Action.ActionAt].mouseButtons.Add(
                MouseButtons.Middle);
            actionMaps[(int)Action.ActionAt].gamePadButtons.Add(
                GamePadButtons.X);

            actionMaps[(int)Action.Retreat] = new ActionMap();
            actionMaps[(int)Action.Retreat].keyboardKeys.Add(Keys.LeftShift);
            actionMaps[(int)Action.Retreat].keyboardKeys.Add(Keys.RightShift);

            actionMaps[(int)Action.Advance] = new ActionMap();
            actionMaps[(int)Action.Advance].keyboardKeys.Add(Keys.OemQuotes);
            actionMaps[(int)Action.Advance].keyboardKeys.Add(Keys.Tab);

            actionMaps[(int)Action.StatusItemNext] = new ActionMap();
            actionMaps[(int)Action.StatusItemNext].keyboardKeys.Add(Keys.Space);
            actionMaps[(int)Action.StatusItemNext].gamePadButtons.Add(
                GamePadButtons.RightShoulder);

            actionMaps[(int)Action.StatusItemPrev] = new ActionMap();
            actionMaps[(int)Action.StatusItemPrev].keyboardKeys.Add(Keys.LeftAlt);
            actionMaps[(int)Action.StatusItemPrev].keyboardKeys.Add(Keys.RightAlt);
            actionMaps[(int)Action.StatusItemPrev].gamePadButtons.Add(
                GamePadButtons.LeftShoulder);

            actionMaps[(int)Action.Chat] = new ActionMap();
            actionMaps[(int)Action.Chat].keyboardKeys.Add(Keys.Enter);

            actionMaps[(int)Action.ViewLeft] = new ActionMap();
            actionMaps[(int)Action.ViewLeft].keyboardKeys.Add(
                Keys.Left);
            actionMaps[(int)Action.ViewLeft].gamePadButtons.Add(
                GamePadButtons.Left);

            actionMaps[(int)Action.ViewRight] = new ActionMap();
            actionMaps[(int)Action.ViewRight].keyboardKeys.Add(
                Keys.Right);
            actionMaps[(int)Action.ViewRight].gamePadButtons.Add(
                GamePadButtons.Right);

            actionMaps[(int)Action.ViewUp] = new ActionMap();
            actionMaps[(int)Action.ViewUp].keyboardKeys.Add(
                Keys.Up);
            actionMaps[(int)Action.ViewUp].gamePadButtons.Add(
                GamePadButtons.Up);

            actionMaps[(int)Action.ViewDown] = new ActionMap();
            actionMaps[(int)Action.ViewDown].keyboardKeys.Add(Keys.Down);
            actionMaps[(int)Action.ViewDown].gamePadButtons.Add(GamePadButtons.Down);

            actionMaps[(int)Action.ZoomOut] = new ActionMap();
            actionMaps[(int)Action.ZoomOut].mouseButtons.Add(MouseButtons.ScrollDown);
            actionMaps[(int)Action.ZoomOut].keyboardKeys.Add(Keys.PageUp);
            actionMaps[(int)Action.ZoomOut].gamePadButtons.Add(
                GamePadButtons.LeftTrigger);

            actionMaps[(int)Action.ZoomIn] = new ActionMap();
            actionMaps[(int)Action.ZoomIn].mouseButtons.Add(MouseButtons.ScrollUp);
            actionMaps[(int)Action.ViewDown].keyboardKeys.Add(Keys.PageDown);
            actionMaps[(int)Action.ZoomIn].gamePadButtons.Add(
                GamePadButtons.RightTrigger);
        }


        /// <summary>
        /// Check if an action has been pressed.
        /// </summary>
        public static bool IsActionPressed(Action action)
        {
            return IsActionMapPressed(actionMaps[(int)action]);
        }


        /// <summary>
        /// Check if an action was just performed in the most recent update.
        /// </summary>
        public static bool IsActionTriggered(Action action)
        {
            return IsActionMapTriggered(actionMaps[(int)action]);
        }


        /// <summary>
        /// Check if an action map has been pressed.
        /// </summary>
        private static bool IsActionMapPressed(ActionMap actionMap)
        {
            for (int i = 0; i < actionMap.keyboardKeys.Count; i++)
            {
                if (IsKeyPressed(actionMap.keyboardKeys[i]))
                {
                    return true;
                }
            }
            foreach (MouseButtons mButton in actionMap.mouseButtons)
            {
                if (IsMouseButtonPressed(mButton))
                    return true;
            }
            if (currentGamePadState.IsConnected)
            {
                for (int i = 0; i < actionMap.gamePadButtons.Count; i++)
                {
                    if (IsGamePadButtonPressed(actionMap.gamePadButtons[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Check if an action map has been triggered this frame.
        /// </summary>
        private static bool IsActionMapTriggered(ActionMap actionMap)
        {
            for (int i = 0; i < actionMap.keyboardKeys.Count; i++)
            {
                if (IsKeyTriggered(actionMap.keyboardKeys[i]))
                {
                    return true;
                }
            }
            foreach (MouseButtons mButton in actionMap.mouseButtons)
            {
                if (IsMouseButtonTriggered(mButton))
                    return true;
            }
            if (currentGamePadState.IsConnected)
            {
                for (int i = 0; i < actionMap.gamePadButtons.Count; i++)
                {
                    if (IsGamePadButtonTriggered(actionMap.gamePadButtons[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        #endregion


        #region Initialization


        /// <summary>
        /// Initializes the default control keys for all actions.
        /// </summary>
        public static void Initialize()
        {
            ResetActionMaps();
        }


        #endregion


        #region Updating


        /// <summary>
        /// Updates the keyboard and gamepad control states.
        /// </summary>
        public static void Update()
        {
            // update the keyboard state
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            // update the gamepad state
            previousGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            // mouse 
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }


        #endregion
    }
}
