using Common.Models;
using Common.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Script.Utility
{
    public static class FunctionLibrary
    {
        static Random random = new Random();

        public static double Random(object[] parameters)
        {
            var min = Convert.ToInt32(parameters[0]);
            var max = Convert.ToInt32(parameters[1]);
            return random.Next(min, max);
        }

        public static double StrLength(object[] parameters)
        {
            return parameters[0].ToString().Length;
        }

        public static bool MoveToRegion(object[] parameters)
        {
            var instance = parameters[0] as ThingInstance;
            var region = parameters[1] as Region;
            var x = Convert.ToSingle(parameters[2]);
            var y = Convert.ToSingle(parameters[3]);

            instance.Region.instances.Remove(instance);
            region.instances.Add(instance);
            instance.Region = region;

            instance.Position = new Vector2(x, y);

            return true;
        }

        public static bool SetPos(object[] parameters)
        {
            var instance = parameters[0] as ThingInstance;
            var x = Convert.ToSingle(parameters[1]);
            var y = Convert.ToSingle(parameters[2]);

            instance.Position = new Vector2(x, y);

            return true;
        }

        public static bool SetTile(object[] parameters)
        {
            try { 
                var region = parameters[0] as Region;
                var x = (int)parameters[1];
                var y = (int)parameters[2];
                var tile = parameters[3] as Tile;

                region.Tiles[y][x] = tile;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ShowMessage(object[] parameters)
        {
            try
            {
                var message = parameters[0].ToString();
                EngineVariables.Messages.Enqueue(message);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Spawn(object[] parameters)
        {
            var region = parameters[0] as Region;
            var x = Convert.ToInt32(parameters[1]);
            var y = Convert.ToInt32(parameters[2]);

            var breed = parameters[3] as Thing;

            World.Instance.Spawn(breed.Id, region.Id, new Vector2(x, y));
        }

        public static bool AddItem(object[] parameters)
        {
            var character = parameters[0] as CharacterInstance;
            var itemBreed = parameters[1] as Item;

            var itemInstance = itemBreed.Spawn() as ItemInstance;
            character.Items.Add(itemInstance);

            return true;
        }

        public static bool AddTopic(object[] parameters)
        {
            try
            {
                var character = parameters[0] as CharacterInstance;
                var topicId = parameters[1].ToString();
                var topicText = parameters[2].ToString();
                character.Topics.Add(topicId, topicText);

                return true;
            } catch
            {
                return false;
            }
        }

        public static bool RemoveTopic(object[] parameters)
        {
            try
            {
                var character = parameters[0] as CharacterInstance;
                var topicId = parameters[1].ToString();
                character.Topics.Remove(topicId);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool AddBreedTopic(object[] parameters)
        {
            try
            {
                var character = parameters[0] as Character;
                var topicId = parameters[1].ToString();
                var topicText = parameters[2].ToString();
                character.Topics.Add(topicId, topicText);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool AddBreedItem(object[] parameters)
        {
            try
            {
                var character = parameters[0] as Character;
                var item = parameters[1] as Item;
                character.Items.Add(item);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool AddSpeechText(object[] parameters)
        {
            try
            {
                var text = parameters[0] as string;
                EngineVariables.SpeechTexts.Enqueue(text);

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
