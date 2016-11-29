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

using Microsoft.VisualStudio.TestTools.UnitTesting;

using org.apache.commons.cli;

namespace org.apache.commons.cli.bug
{

    /**
     * @author brianegge
     */
    [TestClass]
    public class BugCLI133Test
    {
        [TestMethod]
        public void testOrder()
        {
            Option optionA = new Option("a", "first");
            Options opts = new Options();
            opts.addOption(optionA);
            PosixParser posixParser = new PosixParser();
            CommandLine line = posixParser.parse(opts, null);
            Assert.IsFalse(line.hasOption(null));
        }
    }
}