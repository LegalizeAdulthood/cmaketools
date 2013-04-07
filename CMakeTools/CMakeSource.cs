﻿/* ****************************************************************************
 * 
 * Copyright (C) 2012-2013 by David Golub.  All rights reserved.
 * 
 * This software is subject to the Microsoft Reciprocal License (Ms-RL).
 * A copy of the license can be found in the License.txt file included
 * in this distribution.
 * 
 * You must not remove this notice, or any other, from this software.
 * 
 * **************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace CMakeTools
{
    /// <summary>
    /// Source object for CMake code.
    /// </summary>
    class CMakeSource : Source
    {
        private struct IncludeCacheEntry
        {
            public string FileName { get; set; }
            public List<string> Variables { get; set; }
            public List<string> EnvVariables { get; set; }
            public List<string> CacheVariables { get; set; }
            public List<string> Functions { get; set; }
            public List<string> Macros { get; set; }
        }

        private Dictionary<string, IncludeCacheEntry> _includeCache;

        public CMakeSource(LanguageService service, IVsTextLines textLines,
            Colorizer colorizer) : base(service, textLines, colorizer)
        {
            _includeCache = new Dictionary<string, IncludeCacheEntry>();
        }

        public override CommentInfo GetCommentFormat()
        {
            // Provide information on how comments are specified in CMake code.
            CommentInfo info = new CommentInfo();
            info.LineStart = "#";
            info.UseLineComments = true;
            return info;
        }

        public override void Completion(IVsTextView textView, TokenInfo info,
            ParseReason reason)
        {
            bool oldValue = LanguageService.Preferences.EnableAsyncCompletion;
            if (reason == ParseReason.MemberSelect ||
                reason == ParseReason.MemberSelectAndHighlightBraces)
            {
                if (info.Token == (int)CMakeToken.VariableStart ||
                    info.Token == (int)CMakeToken.VariableStartEnv)
                {
                    // Disable asynchronous parsing for member selection requests
                    // involving variables.  It doesn't work properly when a parameter
                    // information tool tip is visible.
                    LanguageService.Preferences.EnableAsyncCompletion = false;
                }
            }
            base.Completion(textView, info, reason);
            LanguageService.Preferences.EnableAsyncCompletion = oldValue;
        }

        public override ExpansionProvider GetExpansionProvider()
        {
            return new CMakeExpansionProvider(this);
        }

        /// <summary>
        /// Test whether the given path specifies the name of a CMake file.
        /// </summary>
        /// <param name="path">The path to a file.</param>
        /// <returns>True if the file is a CMake file or false otherwise.</returns>
        public static bool IsCMakeFile(string path)
        {
            bool textFile = true;
            if (Path.GetExtension(path).ToLower() == ".cmake")
            {
                textFile = false;
            }
            else if (Path.GetExtension(path).ToLower() == ".txt")
            {
                if (Path.GetFileName(path).ToLower() == "cmakelists.txt")
                {
                    textFile = false;
                }
            }
            return !textFile;
        }

        /// <summary>
        /// Update the include cache with information on the files referenced by the
        /// specified lines.
        /// </summary>
        /// <param name="lines">A collection of lines.</param>
        public void BuildIncludeCache(IEnumerable<string> lines)
        {
            List<string> includes = CMakeParsing.ParseForIncludes(lines);
            foreach (string include in includes)
            {
                string curFileDir = Path.GetDirectoryName(GetFilePath());
                string path = Path.Combine(curFileDir, include + ".cmake");
                if (!File.Exists(path))
                {
                    path = Path.Combine(CMakePath.FindCMakeModules(),
                        include + ".cmake");
                    if (!File.Exists(path))
                    {
                        path = null;
                    }
                }
                if (path != null)
                {
                    try
                    {
                        string[] includeLines = File.ReadAllLines(path);
                        _includeCache[include] = new IncludeCacheEntry()
                        {
                            FileName = path,
                            Variables = CMakeParsing.ParseForVariables(includeLines),
                            EnvVariables = CMakeParsing.ParseForEnvVariables(
                                includeLines),
                            CacheVariables = CMakeParsing.ParseForCacheVariables(
                                includeLines),
                            Functions = CMakeParsing.ParseForFunctionNames(includeLines,
                                false),
                            Macros = CMakeParsing.ParseForFunctionNames(includeLines,
                                true)
                        };
                    }
                    catch (IOException)
                    {
                        // Just ignore any errors.
                    }
                }
            }
        }

        /// <summary>
        /// Get the variables in the include cache.
        /// </summary>
        /// <returns>A list of variables.</returns>
        public List<string> GetIncludeCacheVariables()
        {
            return GetIncludeCacheItems(x => x.Variables);
        }

        /// <summary>
        /// Get the environment variables in the include cache.
        /// </summary>
        /// <returns>A list of environment variables.</returns>
        public List<string> GetIncludeCacheEnvVariables()
        {
            return GetIncludeCacheItems(x => x.EnvVariables);
        }

        /// <summary>
        /// Get the cache variables in the include cache.
        /// </summary>
        /// <returns>A list of cache variables.</returns>
        public List<string> GetIncludeCacheCacheVariables()
        {
            return GetIncludeCacheItems(x => x.CacheVariables);
        }

        /// <summary>
        /// Get the functions in the include cache.
        /// </summary>
        /// <returns>A list of functions.</returns>
        public List<string> GetIncludeCacheFunctions()
        {
            return GetIncludeCacheItems(x => x.Functions);
        }

        /// <summary>
        /// Get the macros in the include macro.
        /// </summary>
        /// <returns>A list of macros.</returns>
        public List<string> GetIncludeCacheMacros()
        {
            return GetIncludeCacheItems(x => x.Macros);
        }

        private List<string> GetIncludeCacheItems(
            Func<IncludeCacheEntry, IEnumerable<string>> func)
        {
            List<string> variables = new List<string>();
            foreach (IncludeCacheEntry entry in _includeCache.Values)
            {
                variables.AddRange(func(entry));
            }
            return variables;
        }

        /// <summary>
        /// Get the parameters to a specified function defined in a file in the include
        /// cache.
        /// </summary>
        /// <param name="function">
        /// The name of the function for which to find parameters.
        /// </param>
        /// <returns>
        /// A list of the function's parameters or null if the function could not be
        /// found.
        /// </returns>
        public List<string> GetParametersFromIncludeCache(string function)
        {
            string fileName = null;
            IEnumerable<IncludeCacheEntry> entries = _includeCache.Values.Where(
                x => x.Functions.Contains(function) || x.Macros.Contains(function));
            foreach (IncludeCacheEntry entry in entries)
            {
                fileName = entry.FileName;
                break;
            }
            if (fileName != null)
            {
                try
                {
                    string[] includeLines = File.ReadAllLines(fileName);
                    return CMakeParsing.ParseForParameterNames(includeLines, function);
                }
                catch (IOException)
                {
                    // Just ignore any errors.
                }
            }
            return null;
        }
    }
}
