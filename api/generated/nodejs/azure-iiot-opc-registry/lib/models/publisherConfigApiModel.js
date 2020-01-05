/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for
 * license information.
 *
 * Code generated by Microsoft (R) AutoRest Code Generator 1.0.0.0
 * Changes may cause incorrect behavior and will be lost if the code is
 * regenerated.
 */

'use strict';

/**
 * Default publisher agent configuration
 *
 */
class PublisherConfigApiModel {
  /**
   * Create a PublisherConfigApiModel.
   * @property {object} [capabilities] Capabilities
   * @property {string} [jobCheckInterval] Interval to check job
   * @property {string} [heartbeatInterval] Heartbeat interval
   * @property {number} [maxWorkers] Parallel jobs
   * @property {string} [jobOrchestratorUrl] Job orchestrator endpoint url
   */
  constructor() {
  }

  /**
   * Defines the metadata of PublisherConfigApiModel
   *
   * @returns {object} metadata of PublisherConfigApiModel
   *
   */
  mapper() {
    return {
      required: false,
      serializedName: 'PublisherConfigApiModel',
      type: {
        name: 'Composite',
        className: 'PublisherConfigApiModel',
        modelProperties: {
          capabilities: {
            required: false,
            serializedName: 'capabilities',
            type: {
              name: 'Dictionary',
              value: {
                  required: false,
                  serializedName: 'StringElementType',
                  type: {
                    name: 'String'
                  }
              }
            }
          },
          jobCheckInterval: {
            required: false,
            serializedName: 'jobCheckInterval',
            type: {
              name: 'String'
            }
          },
          heartbeatInterval: {
            required: false,
            serializedName: 'heartbeatInterval',
            type: {
              name: 'String'
            }
          },
          maxWorkers: {
            required: false,
            serializedName: 'maxWorkers',
            type: {
              name: 'Number'
            }
          },
          jobOrchestratorUrl: {
            required: false,
            serializedName: 'jobOrchestratorUrl',
            type: {
              name: 'String'
            }
          }
        }
      }
    };
  }
}

module.exports = PublisherConfigApiModel;
