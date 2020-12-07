/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * SPDX-License-Identifier: MIT-0
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this
 * software and associated documentation files (the "Software"), to deal in the Software
 * without restriction, including without limitation the rights to use, copy, modify,
 * merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using Amazon;
using Amazon.Athena;
using AthenaNetCore.BusinessLogic.Extentions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AthenaNetCore.BusinessLogic.Repositories
{
    public abstract class BaseRepository : IBaseRepository
    {
        public BaseRepository()
        {
            //Option 1: Use the default credential provider chain (recommended)
            // to learn more check: https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html
            AmazonAthenaClient = new AmazonAthenaClient(RegionEndpoint.USWest2);

            //Option 2: Hardcode
            //Uncomment if you prefer hardcode your Access key and Secret access instead of using the environment variable
            //AmazonAthenaClient = new AmazonAthenaClient("YOUR_AWS_ACCESS_KEY", "YOUR_AWS_SECRET_ACCESS", RegionEndpoint.USWest2);
        }

        /// <summary>
        /// Instance of the client to be used by repository 
        /// classes to interact with Amazon Athena 
        /// </summary>
        internal IAmazonAthena AmazonAthenaClient { get; }

        /// <summary>
        /// Release compute resources alocated
        /// </summary>
        public void Dispose()
        {
            AmazonAthenaClient?.Dispose();
        }

        public Task<bool> IsTheQueryStillRunning(string queryId)
        {
            return AmazonAthenaClient.IsTheQueryStillRunning(queryId);
        }
    }
}
