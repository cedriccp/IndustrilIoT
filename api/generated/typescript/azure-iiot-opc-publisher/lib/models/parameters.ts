/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for
 * license information.
 *
 * Code generated by Microsoft (R) AutoRest Code Generator 1.0.0.0
 * Changes may cause incorrect behavior and will be lost if the code is
 * regenerated.
 */

import * as msRest from "@azure/ms-rest-js";

export const continuationToken: msRest.OperationQueryParameter = {
  parameterPath: "continuationToken",
  mapper: {
    required: true,
    serializedName: "continuationToken",
    type: {
      name: "String"
    }
  }
};
export const endpointId: msRest.OperationURLParameter = {
  parameterPath: "endpointId",
  mapper: {
    required: true,
    serializedName: "endpointId",
    type: {
      name: "String"
    }
  }
};
export const userId: msRest.OperationURLParameter = {
  parameterPath: "userId",
  mapper: {
    required: true,
    serializedName: "userId",
    type: {
      name: "String"
    }
  }
};
