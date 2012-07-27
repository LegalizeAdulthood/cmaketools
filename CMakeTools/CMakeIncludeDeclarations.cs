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
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.Package;

namespace CMakeTools
{
    /// <summary>
    /// Declarations object for CMake include files.
    /// </summary>
    class CMakeIncludeDeclarations : Declarations
    {
        // Array of include files to be displayed.
        private string[] _includeFiles;

        public CMakeIncludeDeclarations(string sourceFilePath)
        {
            // Find all *.cmake files in the same directory as the current source file,
            // excluding cmake_install.cmake, which is generated by CMake during
            // configuration.
            string dirPath = Path.GetDirectoryName(sourceFilePath);
            IEnumerable<string> files = Directory.EnumerateFiles(dirPath, "*.cmake");
            files = files.Select(Path.GetFileName);
            files = files.Where(x => !x.Equals("cmake_install.cmake"));
            _includeFiles = files.ToArray();
        }

        public override int GetCount()
        {
            return _includeFiles.Length;
        }

        public override string GetDescription(int index)
        {
            return null;
        }

        public override string GetDisplayText(int index)
        {
            return GetName(index);
        }

        public override int GetGlyph(int index)
        {
            // Always return the index for a reference.
            return 193;
        }

        public override string GetName(int index)
        {
            if (index < 0 || index >= _includeFiles.Length)
            {
                return null;
            }
            return _includeFiles[index];
        }
    }
}