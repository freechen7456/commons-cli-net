/**
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.IO;

namespace org.apache.commons.cli
{

    /**
     * <p>
     * Allows Options to be created from a single String.
     * The pattern contains various single character flags and via
     * an optional punctuation character, their expected type.
     * </p>
     *
     * <table border="1">
     * <tr><td>a</td><td>-a flag</td></tr>
     * <tr><td>b@</td><td>-b [classname]</td></tr>
     * <tr><td>c&gt;</td><td>-c [filename]</td></tr>
     * <tr><td>d+</td><td>-d [classname] (creates object via empty contructor)</td></tr>
     * <tr><td>e%</td><td>-e [number] (creates Double/Long instance depeding on existing of a '.')</td></tr>
     * <tr><td>f/</td><td>-f [url]</td></tr>
     * <tr><td>g:</td><td>-g [string]</td></tr>
     * </table>
     *
     * <p>
     * For example, the following allows command line flags of '-v -p string-value -f /dir/file'.
     * The exclamation mark precede a mandatory option.
     * </p>
     * <code>Options options = PatternOptionBuilder.parsePattern("vp:!f/");</code>
     *
     * <p>
     * TODO These need to break out to OptionType and also
     * to be pluggable.
     * </p>
     *
     * @version $Revision: 734339 $, $Date: 2009-01-13 21:56:47 -0800 (Tue, 13 Jan 2009) $
     */
    public class PatternOptionBuilder
    {
        /** String class */
        public static readonly Type STRING_VALUE = typeof(string);

        /** object class */
        public static readonly Type OBJECT_VALUE = typeof(object);

        /** Number class */
        public static readonly Type NUMBER_VALUE = typeof(ValueType);

        /** Date class */
        public static readonly Type DATE_VALUE = typeof(DateTime);

        /** Class class */
        public static readonly Type CLASS_VALUE = typeof(Type);

        /// can we do this one??
        // is meant to check that the file exists, else it errors.
        // ie) it's for reading not writing.

        /** FileInputStream class */
        public static readonly Type EXISTING_FILE_VALUE = typeof(FileStream);

        /** File class */
        public static readonly Type FILE_VALUE = typeof(FileInfo);

        /** File array class */
        public static readonly Type FILES_VALUE = typeof(FileInfo[]);

        /** URL class */
        public static readonly Type URL_VALUE = typeof(Uri);

        /**
         * Retrieve the class that <code>ch</code> represents.
         *
         * @param ch the specified character
         * @return The class that <code>ch</code> represents
         */
        public static object getValueClass(char ch)
        {
            switch (ch)
            {
                case '@':
                    return PatternOptionBuilder.OBJECT_VALUE;
                case ':':
                    return PatternOptionBuilder.STRING_VALUE;
                case '%':
                    return PatternOptionBuilder.NUMBER_VALUE;
                case '+':
                    return PatternOptionBuilder.CLASS_VALUE;
                case '#':
                    return PatternOptionBuilder.DATE_VALUE;
                case '<':
                    return PatternOptionBuilder.EXISTING_FILE_VALUE;
                case '>':
                    return PatternOptionBuilder.FILE_VALUE;
                case '*':
                    return PatternOptionBuilder.FILES_VALUE;
                case '/':
                    return PatternOptionBuilder.URL_VALUE;
            }

            return null;
        }

        /**
         * Returns whether <code>ch</code> is a value code, i.e.
         * whether it represents a class in a pattern.
         *
         * @param ch the specified character
         * @return true if <code>ch</code> is a value code, otherwise false.
         */
        public static bool isValueCode(char ch)
        {
            return ch == '@'
                    || ch == ':'
                    || ch == '%'
                    || ch == '+'
                    || ch == '#'
                    || ch == '<'
                    || ch == '>'
                    || ch == '*'
                    || ch == '/'
                    || ch == '!';
        }

        /**
         * Returns the {@link Options} instance represented by <code>pattern</code>.
         *
         * @param pattern the pattern string
         * @return The {@link Options} instance
         */
        public static Options parsePattern(String pattern)
        {
            char opt = ' ';
            bool required = false;
            object type = null;

            Options options = new Options();

            for (int i = 0; i < pattern.Length; i++)
            {
                char ch = pattern[i];

                // a value code comes after an option and specifies
                // details about it
                if (!isValueCode(ch))
                {
                    if (opt != ' ')
                    {
                        // we have a previous one to deal with
                        options.addOption(OptionBuilder.hasArg(type != null)
                            .isRequired(required)
                            .withType(type)
                            .create(opt));
                        required = false;
                        type = null;
                        opt = ' ';
                    }

                    opt = ch;
                }
                else if (ch == '!')
                {
                    required = true;
                }
                else
                {
                    type = getValueClass(ch);
                }
            }

            if (opt != ' ')
            {
                // we have a final one to deal with
                options.addOption(OptionBuilder.hasArg(type != null)
                    .isRequired(required)
                    .withType(type).create(opt));
            }

            return options;
        }
    }
}