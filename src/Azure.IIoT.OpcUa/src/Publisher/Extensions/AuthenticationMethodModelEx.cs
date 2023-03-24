// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Publisher.Models
{
    using Furly.Extensions.Serializers;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Authentication method model extensions
    /// </summary>
    public static class AuthenticationMethodModelEx
    {
        /// <summary>
        /// Equality comparison
        /// </summary>
        /// <param name="model"></param>
        /// <param name="that"></param>
        /// <returns></returns>
        public static bool IsSameAs(this IEnumerable<AuthenticationMethodModel> model,
            IEnumerable<AuthenticationMethodModel> that)
        {
            if (model == that)
            {
                return true;
            }
            if (model == null || that == null)
            {
                return false;
            }
            if (model.Count() != that.Count())
            {
                return false;
            }
            foreach (var a in model)
            {
                if (!that.Any(b => b.IsSameAs(a)))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Equality comparison
        /// </summary>
        /// <param name="model"></param>
        /// <param name="that"></param>
        /// <returns></returns>
        public static bool IsSameAs(this AuthenticationMethodModel model,
            AuthenticationMethodModel that)
        {
            if (model == that)
            {
                return true;
            }
            if (model == null || that == null)
            {
                return false;
            }
            if (model.Configuration != null && that.Configuration != null)
            {
                if (!VariantValue.DeepEquals(model.Configuration, that.Configuration))
                {
                    return false;
                }
            }
            return
                model.Id == that.Id &&
                model.SecurityPolicy == that.SecurityPolicy &&
                model.CredentialType == that.CredentialType;
        }

        /// <summary>
        /// Deep clone
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static AuthenticationMethodModel Clone(this AuthenticationMethodModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new AuthenticationMethodModel
            {
                Configuration = model.Configuration?.Copy(),
                Id = model.Id,
                SecurityPolicy = model.SecurityPolicy,
                CredentialType = model.CredentialType
            };
        }
    }
}