﻿/* ****************************************************************************
 * 
 * Copyright (C) 2012 by David Golub.  All rights reserved.
 * 
 * This software is subject to the Microsoft Reciprocal License (Ms-RL).
 * A copy of the license can be found in the License.txt file included
 * in this distribution.
 * 
 * You must not remove this notice, or any other, from this software.
 * 
 * **************************************************************************/

using System.Collections.Generic;
using Microsoft.VisualStudio.Package;

namespace CMakeTools
{
    class CMakeSubcommandMethods : Methods
    {
        // Parameters to the CMAKE_POLICY(GET) command.
        private static string[] _cmakePolicyGetParams = new string[]
        {
            "policy_number",
            "output_variable"
        };

        // Parameters to the CMAKE_POLICY(PUSH) and CMAKE_POLICY(POP) commands.
        private static string[] _cmakePolicyPush = new string[] {};
        private static string[] _cmakePolicyPop = _cmakePolicyPush;

        // Parameters to the CMAKE_POLICY(SET) command.
        private static string[] _cmakePolicySetParams = new string[]
        {
            "policy_number",
            "behavior"
        };

        // Parameters to the CMAKE_POLICY(VERSION) command.
        private static string[] _cmakePolicyVersionParams = new string[]
        {
            "version_number"
        };

        // Map from subcommands of the CMAKE_POLICY command to parameters.
        private static Dictionary<string, string[]> _cmakePolicySubcommands =
            new Dictionary<string, string[]>
        {
            { "GET",        _cmakePolicyGetParams },
            { "POP",        _cmakePolicyPop },
            { "PUSH",       _cmakePolicyPush },
            { "SET",        _cmakePolicySetParams },
            { "VERSION",    _cmakePolicyVersionParams }
        };

        // Map from subcommands of the DEFINE_PROPERTY command to parameters.
        private static Dictionary<string, string[]> _definePropertySubcommands =
            new Dictionary<string, string[]>
        {
            { "CACHED_VARIABLE",    null },
            { "DIRECTORY",          null },
            { "GLOBAL",             null },
            { "SOURCE",             null },
            { "TARGET",             null },
            { "TEST",               null },
            { "VARIABLE",           null }
        };

        // Parameters to the EXPORT(PACKAGE) command.
        private static string[] _exportPackageParams = new string[]
        {
            "name"
        };

        // Map from subcommands of the EXPORT command to parameters.
        private static Dictionary<string, string[]> _exportSubcommands =
            new Dictionary<string, string[]>
        {
            { "PACKAGE",    _exportPackageParams },
            { "TARGETS",    null }
        };

        // Parameters to the FILE(APPEND) command.
        private static string[] _fileAppendParams = new string[]
        {
            "filename",
            "message"
        };

        // Parameters to the FILE(DOWNLOAD) command.
        private static string[] _fileDownloadParams = new string[]
        {
            "url",
            "filename"
        };

        // Parameters to the FILE(GLOB) command.
        private static string[] _fileGlobParams = new string[]
        {
            "variable",
            "glob1 glob2 ..."
        };

        // Parameters to the FILE(GLOB_RECURSE) command.
        private static string[] _fileGlobRecurseParams = _fileGlobParams;

        // Parameters to the FILE(MAKE_DIRECTORY) command.
        private static string[] _fileMakeDirectoryParams = new string[]
        {
            "directory1 directory2 ..."
        };

        // Parameters to the FILE(READ) command.
        private static string[] _fileReadParams = new string[]
        {
            "filename",
            "variable"
        };

        // Parameters to the FILE(MD5) command and the various other hashing
        // commands.
        private static string[] _fileMD5Params = _fileReadParams;
        private static string[] _fileSHA1Params = _fileReadParams;
        private static string[] _fileSHA224Params = _fileReadParams;
        private static string[] _fileSHA256Params = _fileReadParams;
        private static string[] _fileSHA384Params = _fileReadParams;
        private static string[] _fileSHA512Params = _fileReadParams;

        // Parameters to the FILE(RELATIVE_PATH) command.
        private static string[] _fileRelativePathParams = new string[]
        {
            "variable",
            "directory",
            "filename"
        };

        // Parameters to the FILE(REMOVE) command.
        private static string[] _fileRemoveParams = new string[]
        {
            "filename1 filename2 ..."
        };

        // Parameters to the FILE(REMOVE_RECURSE) command.
        private static string[] _fileRemoveRecurseParams = _fileRemoveParams;

        // Parameters to the FILE(RENAME) command.
        private static string[] _fileRenameParams = new string[]
        {
            "old_name",
            "new_name"
        };

        // Parameters to the FILE(STRINGS) command.
        private static string[] _fileStringsParams = _fileReadParams;

        // Parameters to the FILE(TO_CMAKE_PATH) command.
        private static string[] _fileToCMakePathParams = new string[]
        {
            "path",
            "variable"
        };

        // Parameters to the FILE(TO_NATIVE_PATH) command.
        private static string[] _fileToNativePathParams = _fileToCMakePathParams;

        // Parameters to the FILE(UPLOAD) command.
        private static string[] _fileUploadParams = new string[]
        {
            "filename",
            "url"
        };

        // Parameters to the FILE(WRITE) command.
        private static string[] _fileWriteParams = _fileAppendParams;

        // Map from subcommands of the FILE command to parameters.
        private static Dictionary<string, string[]> _fileSubcommands =
            new Dictionary<string, string[]>
        {
            { "APPEND",         _fileAppendParams },
            { "DOWNLOAD",       _fileDownloadParams },
            { "GLOB",           _fileGlobParams },
            { "GLOB_RECURSE",   _fileGlobRecurseParams },
            { "MAKE_DIRECTORY", _fileMakeDirectoryParams },
            { "MD5",            _fileMD5Params },
            { "READ",           _fileReadParams },
            { "RELATIVE_PATH",  _fileRelativePathParams },
            { "REMOVE",         _fileRemoveParams },
            { "REMOVE_RECURSE", _fileRemoveRecurseParams },
            { "RENAME",         _fileRenameParams },
            { "SHA1",           _fileSHA1Params },
            { "SHA224",         _fileSHA224Params },
            { "SHA256",         _fileSHA256Params },
            { "SHA384",         _fileSHA384Params },
            { "SHA512",         _fileSHA512Params },
            { "STRINGS",        _fileStringsParams },
            { "TO_CMAKE_PATH",  _fileToCMakePathParams },
            { "TO_NATIVE_PATH", _fileToNativePathParams },
            { "UPLOAD",         _fileUploadParams },
            { "WRITE",          _fileWriteParams }
        };

        // Map from subcommands of the INSTALL command to parameters.
        private static Dictionary<string, string[]> _installSubcommands =
            new Dictionary<string, string[]>
        {
            { "CODE",       null },
            { "DIRECTORY",  null },
            { "EXPORT",     null },
            { "FILES",      null },
            { "PROGRAMS",   null },
            { "SCRIPTS",    null },
            { "TARGETS",    null }
        };

        // Parameters to the LIST(APPEND) command.
        private static string[] _listAppendParams = new string[]
        {
            "list",
            "element1 element2 ..."
        };

        // Parameters to the LIST(FIND) command.
        private static string[] _listFindParams = new string[]
        {
            "list",
            "value",
            "output_variable"
        };

        // Parameters to the LIST(GET) command.
        private static string[] _listGetParams = new string[]
        {
            "list",
            "index",
            "output_variable"
        };

        // Parameters to the LIST(INSERT) command.
        private static string[] _listInsertParams = new string[]
        {
            "list",
            "index",
            "element1 element2 ..."
        };

        // Parameters to the LIST(LENGTH) command.
        private static string[] _listLengthParams = new string[]
        {
            "list",
            "output_variable"
        };

        // Parameters to the LIST(REMOVE_AT) command.
        private static string[] _listRemoveAtParams = new string[]
        {
            "list",
            "index1 index2 ..."
        };

        // Parameters to the LIST(REMOVE_DUPLICATES) command.
        private static string[] _listRemoveDuplicatesParams = new string[]
        {
            "list"
        };

        // Parameters to the LIST(REMOVE_ITEM) command.
        private static string[] _listRemoveItemParams = new string[]
        {
            "list",
            "value1 value2 ..."
        };

        // Parameters to the LIST(REVERSE) command.
        private static string[] _listReverseParams = _listRemoveDuplicatesParams;

        // Parameters to the LIST(SORT) command.
        private static string[] _listSortParams = _listRemoveDuplicatesParams;

        // Map from subcommands of the LIST command to parameters.
        private static Dictionary<string, string[]> _listSubcommands =
            new Dictionary<string, string[]>
        {
            { "APPEND",             _listAppendParams },
            { "FIND",               _listFindParams },
            { "GET",                _listGetParams },
            { "INSERT",             _listInsertParams },
            { "LENGTH",             _listLengthParams },
            { "REMOVE_AT",          _listRemoveAtParams },
            { "REMOVE_DUPLICATES",  _listRemoveDuplicatesParams },
            { "REMOVE_ITEM",        _listRemoveItemParams },
            { "REVERSE",            _listReverseParams },
            { "SORT",               _listSortParams }
        };

        // Map from subcommands of the SET_PROPERTY command to parameters.
        private static Dictionary<string, string[]> _setPropertySubcommands =
            new Dictionary<string, string[]>
        {
            { "CACHE",      null },
            { "DIRECTORY",  null },
            { "GLOBAL",     null },
            { "SOURCE",     null },
            { "TARGET",     null },
            { "TEST",       null }
        };

        // Parameters to the STRING(ASCII) command.
        private static string[] _stringAsciiParams = new string[]
        {
            "number",
            "output_variable"
        };

        // Parameters to the STRING(CONFIGURE) command.
        private static string[] _stringConfigureParams = new string[]
        {
            "string",
            "output_variable"
        };

        // Parameters to the STRING(FIND) command.
        private static string[] _stringFindParams = new string[]
        {
            "string",
            "substring",
            "output_variable"
        };

        // Parameters to the STRING(LENGTH) command.
        private static string[] _stringLengthParams = _stringConfigureParams;

        // Parameters to the STRING(MD5) command and the various other hashing
        // commands.
        private static string[] _stringMD5Params = new string[]
        {
            "output_variable",
            "input"
        };
        private static string[] _stringSHA1Params = _stringMD5Params;
        private static string[] _stringSHA224Params = _stringMD5Params;
        private static string[] _stringSHA256Params = _stringMD5Params;
        private static string[] _stringSHA384Params = _stringMD5Params;
        private static string[] _stringSHA512Params = _stringMD5Params;

        // Parameters to the STRING(RANDOM) command.
        private static string[] _stringRandomParams = new string[]
        {
            "output_variable"
        };

        // Parameters to the STRING(REPLACE) command.
        private static string[] _stringReplaceParams = new string[]
        {
            "match_string",
            "replace_string",
            "output_variable",
            "input1 input2 ..."
        };

        // Parameters to the STRING(STRIP) command.
        private static string[] _stringStripParams = _stringConfigureParams;

        // Parameters to the STRING(SUBSTRING) command.
        private static string[] _stringSubstringParams = new string[]
        {
            "string",
            "begin_index",
            "length",
            "output_variable"
        };

        // Parameters to the STRING(TOLOWER) and STRING(TOUPPER) commands.
        private static string[] _stringToLowerParams = _stringConfigureParams;
        private static string[] _stringToUpperParams = _stringConfigureParams;

        // Map from subcommands of the STRING command to parameters.
        private static Dictionary<string, string[]> _stringSubcommands =
            new Dictionary<string, string[]>
        {
            { "ASCII",      _stringAsciiParams },
            { "CONFIGURE",  _stringConfigureParams },
            { "FIND",       _stringFindParams },
            { "LENGTH",     _stringLengthParams },
            { "MD5",        _stringMD5Params },
            { "RANDOM",     _stringRandomParams },
            { "REGEX",      null },
            { "REPLACE",    _stringReplaceParams },
            { "SHA1",       _stringSHA1Params },
            { "SHA224",     _stringSHA224Params },
            { "SHA256",     _stringSHA256Params },
            { "SHA384",     _stringSHA384Params },
            { "SHA512",     _stringSHA512Params },
            { "STRIP",      _stringStripParams },
            { "SUBSTRING",  _stringSubstringParams },
            { "TOLOWER",    _stringToLowerParams },
            { "TOUPPER",    _stringToUpperParams }
        };

        // Map from commands to subcommands.
        private static Dictionary<CMakeCommandId, Dictionary<string, string[]>> _allSubcommands =
            new Dictionary<CMakeCommandId, Dictionary<string, string[]>>
        {
            { CMakeCommandId.CMakePolicy,       _cmakePolicySubcommands },
            { CMakeCommandId.DefineProperty,    _definePropertySubcommands },
            { CMakeCommandId.Export,            _exportSubcommands },
            { CMakeCommandId.File,              _fileSubcommands },
            { CMakeCommandId.Install,           _installSubcommands },
            { CMakeCommandId.List,              _listSubcommands },
            { CMakeCommandId.SetProperty,       _setPropertySubcommands },
            { CMakeCommandId.String,            _stringSubcommands }
        };

        // The command and subcommand for which the given instance will provie
        // parameters.
        private CMakeCommandId _id;
        private string _subcommand;

        private CMakeSubcommandMethods(CMakeCommandId id, string subcommand)
        {
            _id = id;
            _subcommand = subcommand;
        }

        public override int GetCount()
        {
            // Only provide a single signature.
            return 1;
        }

        public override string GetDescription(int index)
        {
            return null;
        }

        public override string GetName(int index)
        {
            // Look up the name of the command and combine it with the subcommand.
            return CMakeKeywords.GetCommandFromId(_id) + "(" + _subcommand;
        }

        public override int GetParameterCount(int index)
        {
            // Look up the number of parameters that the command takes.
            if (!_allSubcommands.ContainsKey(_id) ||
                !_allSubcommands[_id].ContainsKey(_subcommand) ||
                _allSubcommands[_id][_subcommand] == null)
            {
                return 0;
            }
            return _allSubcommands[_id][_subcommand].Length;
        }

        public override void GetParameterInfo(int index, int parameter, out string name,
            out string display, out string description)
        {
            // Look up the name of the requested parameter.
            if (!_allSubcommands.ContainsKey(_id) ||
                !_allSubcommands[_id].ContainsKey(_subcommand) ||
                _allSubcommands[_id][_subcommand] == null)
            {
                name = null;
                display = null;
                description = null;
                return;
            }
            name = _allSubcommands[_id][_subcommand][parameter];
            display = _allSubcommands[_id][_subcommand][parameter];
            description = null;
        }

        public override string GetType(int index)
        {
            // CMake commands don't have return types.
            return null;
        }

        public override string Delimiter
        {
            get
            {
                // In CMake, spaces instead of commas are used to separate arguments.
                return " ";
            }
        }

        public override string OpenBracket
        {
            get
            {
                // An opening parenthesis separates the command from the subcommand, and
                // whitespace separates the subcommand from its first parameter.
                return " ";
            }
        }

        /// <summary>
        /// Check whether the specified command takes subcommands.
        /// </summary>
        /// <param name="id">Identifier of the command to check.</param>
        /// <returns>True if the command takes subcommands or false otherwise.</returns>
        public static bool HasSubcommands(CMakeCommandId id)
        {
            return _allSubcommands.ContainsKey(id);
        }

        /// <summary>
        /// Get a methods object containing the parameters for a given CMake subcommand.
        /// </summary>
        /// <param name="id">The identifier of a CMake command.</param>
        /// <param name="subcommand">The name of a subcommand.</param>
        /// <returns>
        /// A methods object or null if there is no parameter information.
        /// </returns>
        public static Methods GetSubcommandParameters(CMakeCommandId id,
            string subcommand)
        {
            if (!_allSubcommands.ContainsKey(id) ||
                !_allSubcommands[id].ContainsKey(subcommand) ||
                _allSubcommands[id][subcommand] == null)
            {
                return null;
            }
            return new CMakeSubcommandMethods(id, subcommand);
        }

        /// <summary>
        /// Get the subcommands of a given command.
        /// </summary>
        /// <param name="id">The identifier of a CMake command.</param>
        /// <returns>A collection of names of subcommands.</returns>
        public static IEnumerable<string> GetSubcommands(CMakeCommandId id)
        {
            if (!_allSubcommands.ContainsKey(id))
            {
                return null;
            }
            return _allSubcommands[id].Keys;
        }

        /// <summary>
        /// Get a collection of commands that should trigger member selection because
        /// they have subcommands.
        /// </summary>
        /// <returns>A collection of command identifiers.</returns>
        public static IEnumerable<CMakeCommandId> GetMemberSelectionTriggers()
        {
            return _allSubcommands.Keys;
        }
    }
}
