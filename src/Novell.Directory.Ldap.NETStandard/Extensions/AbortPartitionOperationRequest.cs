/******************************************************************************
* The MIT License
* Copyright (c) 2003 Novell Inc.  www.novell.com
*
* Permission is hereby granted, free of charge, to any person obtaining  a copy
* of this software and associated documentation files (the Software), to deal
* in the Software without restriction, including  without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to  permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*******************************************************************************/

//
// Novell.Directory.Ldap.Extensions.AbortPartitionOperationRequest.cs
//
// Author:
//   Sunil Kumar (Sunilk@novell.com)
//
// (C) 2003 Novell, Inc (http://www.novell.com)
//

using System;
using System.IO;
using Novell.Directory.Ldap.Asn1;
using Novell.Directory.Ldap.Utilclass;

namespace Novell.Directory.Ldap.Extensions
{
    /// <summary>
    ///     Aborts the last partition operation that was requested on the
    ///     specified partition if the operation is still pending.
    ///     The AbortPartitionRequest extension uses the following OID:
    ///     2.16.840.1.113719.1.27.100.29
    ///     The requestValue has the following format:
    ///     requestValue ::=
    ///     flags       INTEGER
    ///     partitionDN LdapDN.
    /// </summary>
    public class AbortPartitionOperationRequest : LdapExtendedOperation
    {
        /// <summary>
        ///     Constructs an extended operation object for aborting a partition operation.
        /// </summary>
        /// <param name="partitionDn">
        ///     The distinguished name of the replica's
        ///     partition root.
        /// </param>
        /// <param name="flags">
        ///     Determines whether all servers in the replica ring must
        ///     be up before proceeding. When set to zero, the status of the
        ///     servers is not checked. When set to Ldap_ENSURE_SERVERS_UP,
        ///     all servers must be up for the operation to proceed.
        /// </param>
        /// <exception>
        ///     LdapException A general exception which includes an error message
        ///     and an Ldap error code.
        /// </exception>
        public AbortPartitionOperationRequest(string partitionDn, int flags)
            : base(ReplicationConstants.AbortNamingContextOpReq, null)
        {
            try
            {
                if ((object)partitionDn == null)
                {
                    throw new ArgumentException(ExceptionMessages.ParamError);
                }

                var encodedData = new MemoryStream();
                var encoder = new LberEncoder();

                var asn1Flags = new Asn1Integer(flags);
                var asn1PartitionDn = new Asn1OctetString(partitionDn);

                asn1Flags.Encode(encoder, encodedData);
                asn1PartitionDn.Encode(encoder, encodedData);

                SetValue(SupportClass.ToSByteArray(encodedData.ToArray()));
            }
            catch (IOException ioe)
            {
                throw new LdapException(ExceptionMessages.EncodingError, LdapException.EncodingError, null, ioe);
            }
        }
    }
}