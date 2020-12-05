using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpgEngine.Utility
{
    public static class InputHandler
    {
        public static KeyboardState CurrentKeyboardState { get; private set; }
        private static KeyboardState PreviousKeyboardState { get; set; }
        public static Dictionary<Action, ActionKeyMap> ActionKeyMaps { get; private set; }

        public static bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        public static bool WasKeyJustReleased(Keys key)
        {
            return PreviousKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key);
        }

        public static bool WasKeyJustPressed(Keys key)
        {
            return PreviousKeyboardState.IsKeyUp(key) && CurrentKeyboardState.IsKeyDown(key);
        }

        public static bool IsActionPressed(Action action)
        {
            if (ActionKeyMaps.TryGetValue(action, out var actionKeyMap))
            {
                foreach (var key in actionKeyMap.Keys)
                {
                    if (IsKeyPressed(key))
                        return true;
                }
            }

            return false;
        }

        public static bool WasActionJustReleased(Action action)
        {
            if (ActionKeyMaps.TryGetValue(action, out var actionKeyMap))
            {
                foreach (var key in actionKeyMap.Keys)
                {
                    if (WasKeyJustReleased(key))
                        return true;
                }
            }

            return false;
        }

        public static bool WasActionJustPressed(Action action)
        {
            if (ActionKeyMaps.TryGetValue(action, out var actionKeyMap))
            {
                foreach (var key in actionKeyMap.Keys)
                {
                    if (WasKeyJustPressed(key))
                        return true;
                }
            }

            return false;
        }

        public static void Initialize()
        {
            ResetActionMapsToDefault();
        }

        public static void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }

        public static void ResetActionMapsToDefault()
        {
            ActionKeyMaps = new Dictionary<Action, ActionKeyMap>
            {
                [Action.Up] = new ActionKeyMap()
                {
                    Keys = new List<Keys>() { Keys.Up, Keys.W }
                },

                [Action.Down] = new ActionKeyMap()
                {
                    Keys = new List<Keys>() { Keys.Down, Keys.S }
                },

                [Action.Left] = new ActionKeyMap()
                {
                    Keys = new List<Keys>() { Keys.Left, Keys.A }
                },

                [Action.Right] = new ActionKeyMap()
                {
                    Keys = new List<Keys>() { Keys.Right, Keys.D }
                },

                [Action.Action] = new ActionKeyMap()
                {
                    Keys = new List<Keys>() { Keys.Enter, Keys.Space }
                },

                [Action.Back] = new ActionKeyMap()
                {
                    Keys = new List<Keys>() { Keys.Escape }
                },

                [Action.Attack] = new ActionKeyMap()
                {
                    Keys = new List<Keys>() { Keys.LeftControl }
                },

                [Action.Inventory] = new ActionKeyMap()
                {
                    Keys = new List<Keys>() { Keys.Tab }
                },

                [Action.DropItem] = new ActionKeyMap()
                {
                    Keys = new List<Keys>() { Keys.Q }
                },

                [Action.CharacterInfo] = new ActionKeyMap()
                {
                    Keys = new List<Keys>() { Keys.C }
                },

                [Action.Notes] = new ActionKeyMap()
                {
                    Keys = new List<Keys>() { Keys.N }
                },

                [Action.Console] = new ActionKeyMap()
                {
                    Keys = new List<Keys>() { Keys.F12 }
                }
            };
        }

        public enum Action
        {
            Up,
            Down,
            Left,
            Right,
            Action,
            Back,
            Attack,
            Inventory,
            DropItem,
            CharacterInfo,
            Notes,
            Console
        }

        public class ActionKeyMap
        {
            public List<Keys> Keys { get; set; } = new List<Keys>();
        }
    }
}
