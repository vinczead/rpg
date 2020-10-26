﻿using Common.Models;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using WorldEditor.Utility;

namespace WorldEditor.DataAccess
{
    public class ScriptFileRepository
    {
        public List<ScriptFile> ScriptFiles { get; set; } = new List<ScriptFile>();
        public List<ScriptFile> GetScriptFiles()
        {
            return new List<ScriptFile>(ScriptFiles);
        }

        public ScriptFile GetScriptFileById(string id)
        {
            var scriptFile = ScriptFiles.FirstOrDefault(scriptFile => scriptFile.Name == id);

            if (scriptFile == null)
                throw new ArgumentException($"No script file was found with id '{id}'.");

            return scriptFile;
        }
        public void AddScriptFile(ScriptFile scriptFile)
        {
            if (scriptFile == null)
                throw new ArgumentNullException("scriptFile");

            ScriptFiles.Add(scriptFile);
            ScriptFileAdded?.Invoke(this, new EntityEventArgs<ScriptFile>(scriptFile));
        }
        public void AddNewScriptFile()
        {
            var i = 1;
            while (ScriptFiles.FirstOrDefault(scriptFile => scriptFile.Name == $"Script{i}") != null)
                i++;

            AddScriptFile(new ScriptFile() { Name = $"Script{i}" });
        }

        public void RemoveScriptFile(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var scriptFile = ScriptFiles.FirstOrDefault(scriptFile => scriptFile.Name == id);
            if (scriptFile == null)
                throw new ArgumentException($"No script file was found with id '{id}'.");

            ScriptFiles.Remove(scriptFile);
            ScriptFileRemoved(this, new EntityEventArgs<ScriptFile>(scriptFile));
        }

        public void RemoveAt(int index)
        {
            if(index < 0 || index >= ScriptFiles.Count)
                throw new ArgumentException($"Invalid index: {index}");

            ScriptFiles.RemoveAt(index);

        }


        public event EventHandler<EntityEventArgs<ScriptFile>> ScriptFileAdded;
        public event EventHandler<EntityEventArgs<ScriptFile>> ScriptFileRemoved;
    }
}
