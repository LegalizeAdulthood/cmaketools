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

using Microsoft.VisualStudio.Package;

namespace CMakeTools
{
    /// <summary>
    /// Extension methods that serve as helpers for manipulating strings.
    /// </summary>
    public static class StringHelpers
    {
        /// <summary>
        /// Extract a token from a string.
        /// </summary>
        /// <param name="text">The string from which to extract the token.</param>
        /// <param name="tokenInfo">The token to extract.</param>
        /// <returns>The text of the token.</returns>
        public static string ExtractToken(this string text, TokenInfo tokenInfo)
        {
            return text.Substring(tokenInfo.StartIndex,
                tokenInfo.EndIndex - tokenInfo.StartIndex + 1);
        }
    }
}