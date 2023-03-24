﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Publisher.Models
{
    /// <summary>
    /// Dataset metadata extensions
    /// </summary>
    public static class DataSetMetaDataModelEx
    {
        /// <summary>
        /// Clone
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static DataSetMetaDataModel Clone(this DataSetMetaDataModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new DataSetMetaDataModel
            {
                Name = model.Name,
                DataSetClassId = model.DataSetClassId,
                Description = model.Description
            };
        }
    }
}