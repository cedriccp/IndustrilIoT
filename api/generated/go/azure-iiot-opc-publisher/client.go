// Package azureiiotopcpublisher implements the Azure ARM Azureiiotopcpublisher
// service API version v2.
//
// Azure Industrial IoT OPC UA Publisher Service
package azureiiotopcpublisher

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator 1.0.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

import (
    "context"
    "github.com/Azure/go-autorest/autorest"
    "github.com/Azure/go-autorest/autorest/azure"
    "github.com/Azure/go-autorest/autorest/validation"
    "github.com/Azure/go-autorest/tracing"
    "net/http"
)

const (
// DefaultBaseURI is the default URI used for the service Azureiiotopcpublisher
DefaultBaseURI = "http://localhost:9080")

// BaseClient is the base client for Azureiiotopcpublisher.
type BaseClient struct {
    autorest.Client
    BaseURI string
}

// New creates an instance of the BaseClient client.
func New()BaseClient {
    return NewWithBaseURI(DefaultBaseURI, )
}

// NewWithBaseURI creates an instance of the BaseClient client using a custom
// endpoint.  Use this when interacting with an Azure cloud that uses a
// non-standard base URI (sovereign clouds, Azure stack).
func NewWithBaseURI(baseURI string, ) BaseClient {
    return BaseClient{
        Client: autorest.NewClientWithUserAgent(UserAgent()),
        BaseURI: baseURI,
    }
}

    // GetFirstListOfPublishedNodes returns currently published node ids for an
    // endpoint. The endpoint must be activated and connected and the module client
    // and server must trust each other.
        // Parameters:
            // endpointID - the identifier of the activated endpoint.
            // body - the list request
    func (client BaseClient) GetFirstListOfPublishedNodes(ctx context.Context, endpointID string, body PublishedItemListRequestAPIModel) (result PublishedItemListResponseAPIModel, err error) {
        if tracing.IsEnabled() {
            ctx = tracing.StartSpan(ctx, fqdn + "/BaseClient.GetFirstListOfPublishedNodes")
            defer func() {
                sc := -1
                if result.Response.Response != nil {
                    sc = result.Response.Response.StatusCode
                }
                tracing.EndSpan(ctx, sc, err)
            }()
        }
            req, err := client.GetFirstListOfPublishedNodesPreparer(ctx, endpointID, body)
        if err != nil {
        err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "GetFirstListOfPublishedNodes", nil , "Failure preparing request")
        return
        }

                resp, err := client.GetFirstListOfPublishedNodesSender(req)
                if err != nil {
                result.Response = autorest.Response{Response: resp}
                err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "GetFirstListOfPublishedNodes", resp, "Failure sending request")
                return
                }

                result, err = client.GetFirstListOfPublishedNodesResponder(resp)
                if err != nil {
                err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "GetFirstListOfPublishedNodes", resp, "Failure responding to request")
                }

        return
        }

        // GetFirstListOfPublishedNodesPreparer prepares the GetFirstListOfPublishedNodes request.
        func (client BaseClient) GetFirstListOfPublishedNodesPreparer(ctx context.Context, endpointID string, body PublishedItemListRequestAPIModel) (*http.Request, error) {
                pathParameters := map[string]interface{} {
                "endpointId": autorest.Encode("path",endpointID),
                }

            preparer := autorest.CreatePreparer(
        autorest.AsContentType("application/json-patch+json; charset=utf-8"),
        autorest.AsPost(),
        autorest.WithBaseURL(client.BaseURI),
        autorest.WithPathParameters("/v2/publish/{endpointId}",pathParameters),
        autorest.WithJSON(body))
        return preparer.Prepare((&http.Request{}).WithContext(ctx))
        }

        // GetFirstListOfPublishedNodesSender sends the GetFirstListOfPublishedNodes request. The method will close the
        // http.Response Body if it receives an error.
        func (client BaseClient) GetFirstListOfPublishedNodesSender(req *http.Request) (*http.Response, error) {
            sd := autorest.GetSendDecorators(req.Context(), autorest.DoRetryForStatusCodes(client.RetryAttempts, client.RetryDuration, autorest.StatusCodesForRetry...))
                return autorest.SendWithSender(client, req, sd...)
                }

    // GetFirstListOfPublishedNodesResponder handles the response to the GetFirstListOfPublishedNodes request. The method always
    // closes the http.Response Body.
    func (client BaseClient) GetFirstListOfPublishedNodesResponder(resp *http.Response) (result PublishedItemListResponseAPIModel, err error) {
        err = autorest.Respond(
        resp,
        client.ByInspecting(),
        azure.WithErrorUnlessStatusCode(http.StatusOK),
        autorest.ByUnmarshallingJSON(&result),
        autorest.ByClosing())
        result.Response = autorest.Response{Response: resp}
            return
        }

    // GetNextListOfPublishedNodes returns next set of currently published node ids
    // for an endpoint. The endpoint must be activated and connected and the module
    // client and server must trust each other.
        // Parameters:
            // endpointID - the identifier of the activated endpoint.
            // continuationToken - the continuation token to continue with
    func (client BaseClient) GetNextListOfPublishedNodes(ctx context.Context, endpointID string, continuationToken string) (result PublishedItemListResponseAPIModelPage, err error) {
        if tracing.IsEnabled() {
            ctx = tracing.StartSpan(ctx, fqdn + "/BaseClient.GetNextListOfPublishedNodes")
            defer func() {
                sc := -1
                if result.pilram.Response.Response != nil {
                    sc = result.pilram.Response.Response.StatusCode
                }
                tracing.EndSpan(ctx, sc, err)
            }()
        }
                    result.fn = client.getNextListOfPublishedNodesNextResults
        req, err := client.GetNextListOfPublishedNodesPreparer(ctx, endpointID, continuationToken)
        if err != nil {
        err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "GetNextListOfPublishedNodes", nil , "Failure preparing request")
        return
        }

                resp, err := client.GetNextListOfPublishedNodesSender(req)
                if err != nil {
                result.pilram.Response = autorest.Response{Response: resp}
                err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "GetNextListOfPublishedNodes", resp, "Failure sending request")
                return
                }

                result.pilram, err = client.GetNextListOfPublishedNodesResponder(resp)
                if err != nil {
                err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "GetNextListOfPublishedNodes", resp, "Failure responding to request")
                }

        return
        }

        // GetNextListOfPublishedNodesPreparer prepares the GetNextListOfPublishedNodes request.
        func (client BaseClient) GetNextListOfPublishedNodesPreparer(ctx context.Context, endpointID string, continuationToken string) (*http.Request, error) {
                pathParameters := map[string]interface{} {
                "endpointId": autorest.Encode("path",endpointID),
                }

                        queryParameters := map[string]interface{} {
            "continuationToken": autorest.Encode("query",continuationToken),
            }

            preparer := autorest.CreatePreparer(
        autorest.AsGet(),
        autorest.WithBaseURL(client.BaseURI),
        autorest.WithPathParameters("/v2/publish/{endpointId}",pathParameters),
        autorest.WithQueryParameters(queryParameters))
        return preparer.Prepare((&http.Request{}).WithContext(ctx))
        }

        // GetNextListOfPublishedNodesSender sends the GetNextListOfPublishedNodes request. The method will close the
        // http.Response Body if it receives an error.
        func (client BaseClient) GetNextListOfPublishedNodesSender(req *http.Request) (*http.Response, error) {
            sd := autorest.GetSendDecorators(req.Context(), autorest.DoRetryForStatusCodes(client.RetryAttempts, client.RetryDuration, autorest.StatusCodesForRetry...))
                return autorest.SendWithSender(client, req, sd...)
                }

    // GetNextListOfPublishedNodesResponder handles the response to the GetNextListOfPublishedNodes request. The method always
    // closes the http.Response Body.
    func (client BaseClient) GetNextListOfPublishedNodesResponder(resp *http.Response) (result PublishedItemListResponseAPIModel, err error) {
        err = autorest.Respond(
        resp,
        client.ByInspecting(),
        azure.WithErrorUnlessStatusCode(http.StatusOK),
        autorest.ByUnmarshallingJSON(&result),
        autorest.ByClosing())
        result.Response = autorest.Response{Response: resp}
            return
        }

                // getNextListOfPublishedNodesNextResults retrieves the next set of results, if any.
                func (client BaseClient) getNextListOfPublishedNodesNextResults(ctx context.Context, lastResults PublishedItemListResponseAPIModel) (result PublishedItemListResponseAPIModel, err error) {
                req, err := lastResults.publishedItemListResponseAPIModelPreparer(ctx)
                if err != nil {
                return result, autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "getNextListOfPublishedNodesNextResults", nil , "Failure preparing next results request")
                }
                if req == nil {
                return
                }
                resp, err := client.GetNextListOfPublishedNodesSender(req)
                if err != nil {
                result.Response = autorest.Response{Response: resp}
                return result, autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "getNextListOfPublishedNodesNextResults", resp, "Failure sending next results request")
                }
                result, err = client.GetNextListOfPublishedNodesResponder(resp)
                if err != nil {
                err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "getNextListOfPublishedNodesNextResults", resp, "Failure responding to next results request")
                }
                return
                        }

        // GetNextListOfPublishedNodesComplete enumerates all values, automatically crossing page boundaries as required.
        func (client BaseClient) GetNextListOfPublishedNodesComplete(ctx context.Context, endpointID string, continuationToken string) (result PublishedItemListResponseAPIModelIterator, err error) {
            if tracing.IsEnabled() {
                ctx = tracing.StartSpan(ctx, fqdn + "/BaseClient.GetNextListOfPublishedNodes")
                defer func() {
                    sc := -1
                    if result.Response().Response.Response != nil {
                        sc = result.page.Response().Response.Response.StatusCode
                    }
                    tracing.EndSpan(ctx, sc, err)
                }()
         }
            result.page, err = client.GetNextListOfPublishedNodes(ctx, endpointID, continuationToken)
                    return
            }

    // StartPublishingValues start publishing variable node values to IoT Hub. The
    // endpoint must be activated and connected and the module client and server
    // must trust each other.
        // Parameters:
            // endpointID - the identifier of the activated endpoint.
            // body - the publish request
    func (client BaseClient) StartPublishingValues(ctx context.Context, endpointID string, body PublishStartRequestAPIModel) (result PublishStartResponseAPIModel, err error) {
        if tracing.IsEnabled() {
            ctx = tracing.StartSpan(ctx, fqdn + "/BaseClient.StartPublishingValues")
            defer func() {
                sc := -1
                if result.Response.Response != nil {
                    sc = result.Response.Response.StatusCode
                }
                tracing.EndSpan(ctx, sc, err)
            }()
        }
                if err := validation.Validate([]validation.Validation{
                { TargetValue: body,
                 Constraints: []validation.Constraint{	{Target: "body.Item", Name: validation.Null, Rule: true ,
                Chain: []validation.Constraint{	{Target: "body.Item.NodeID", Name: validation.Null, Rule: true, Chain: nil },
                }}}}}); err != nil {
                return result, validation.NewError("azureiiotopcpublisher.BaseClient", "StartPublishingValues", err.Error())
                }

                    req, err := client.StartPublishingValuesPreparer(ctx, endpointID, body)
        if err != nil {
        err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "StartPublishingValues", nil , "Failure preparing request")
        return
        }

                resp, err := client.StartPublishingValuesSender(req)
                if err != nil {
                result.Response = autorest.Response{Response: resp}
                err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "StartPublishingValues", resp, "Failure sending request")
                return
                }

                result, err = client.StartPublishingValuesResponder(resp)
                if err != nil {
                err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "StartPublishingValues", resp, "Failure responding to request")
                }

        return
        }

        // StartPublishingValuesPreparer prepares the StartPublishingValues request.
        func (client BaseClient) StartPublishingValuesPreparer(ctx context.Context, endpointID string, body PublishStartRequestAPIModel) (*http.Request, error) {
                pathParameters := map[string]interface{} {
                "endpointId": autorest.Encode("path",endpointID),
                }

            preparer := autorest.CreatePreparer(
        autorest.AsContentType("application/json-patch+json; charset=utf-8"),
        autorest.AsPost(),
        autorest.WithBaseURL(client.BaseURI),
        autorest.WithPathParameters("/v2/publish/{endpointId}/start",pathParameters),
        autorest.WithJSON(body))
        return preparer.Prepare((&http.Request{}).WithContext(ctx))
        }

        // StartPublishingValuesSender sends the StartPublishingValues request. The method will close the
        // http.Response Body if it receives an error.
        func (client BaseClient) StartPublishingValuesSender(req *http.Request) (*http.Response, error) {
            sd := autorest.GetSendDecorators(req.Context(), autorest.DoRetryForStatusCodes(client.RetryAttempts, client.RetryDuration, autorest.StatusCodesForRetry...))
                return autorest.SendWithSender(client, req, sd...)
                }

    // StartPublishingValuesResponder handles the response to the StartPublishingValues request. The method always
    // closes the http.Response Body.
    func (client BaseClient) StartPublishingValuesResponder(resp *http.Response) (result PublishStartResponseAPIModel, err error) {
        err = autorest.Respond(
        resp,
        client.ByInspecting(),
        azure.WithErrorUnlessStatusCode(http.StatusOK),
        autorest.ByUnmarshallingJSON(&result),
        autorest.ByClosing())
        result.Response = autorest.Response{Response: resp}
            return
        }

    // StopPublishingValues stop publishing variable node values to IoT Hub. The
    // endpoint must be activated and connected and the module client and server
    // must trust each other.
        // Parameters:
            // endpointID - the identifier of the activated endpoint.
            // body - the unpublish request
    func (client BaseClient) StopPublishingValues(ctx context.Context, endpointID string, body PublishStopRequestAPIModel) (result PublishStopResponseAPIModel, err error) {
        if tracing.IsEnabled() {
            ctx = tracing.StartSpan(ctx, fqdn + "/BaseClient.StopPublishingValues")
            defer func() {
                sc := -1
                if result.Response.Response != nil {
                    sc = result.Response.Response.StatusCode
                }
                tracing.EndSpan(ctx, sc, err)
            }()
        }
                if err := validation.Validate([]validation.Validation{
                { TargetValue: body,
                 Constraints: []validation.Constraint{	{Target: "body.NodeID", Name: validation.Null, Rule: true, Chain: nil }}}}); err != nil {
                return result, validation.NewError("azureiiotopcpublisher.BaseClient", "StopPublishingValues", err.Error())
                }

                    req, err := client.StopPublishingValuesPreparer(ctx, endpointID, body)
        if err != nil {
        err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "StopPublishingValues", nil , "Failure preparing request")
        return
        }

                resp, err := client.StopPublishingValuesSender(req)
                if err != nil {
                result.Response = autorest.Response{Response: resp}
                err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "StopPublishingValues", resp, "Failure sending request")
                return
                }

                result, err = client.StopPublishingValuesResponder(resp)
                if err != nil {
                err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "StopPublishingValues", resp, "Failure responding to request")
                }

        return
        }

        // StopPublishingValuesPreparer prepares the StopPublishingValues request.
        func (client BaseClient) StopPublishingValuesPreparer(ctx context.Context, endpointID string, body PublishStopRequestAPIModel) (*http.Request, error) {
                pathParameters := map[string]interface{} {
                "endpointId": autorest.Encode("path",endpointID),
                }

            preparer := autorest.CreatePreparer(
        autorest.AsContentType("application/json-patch+json; charset=utf-8"),
        autorest.AsPost(),
        autorest.WithBaseURL(client.BaseURI),
        autorest.WithPathParameters("/v2/publish/{endpointId}/stop",pathParameters),
        autorest.WithJSON(body))
        return preparer.Prepare((&http.Request{}).WithContext(ctx))
        }

        // StopPublishingValuesSender sends the StopPublishingValues request. The method will close the
        // http.Response Body if it receives an error.
        func (client BaseClient) StopPublishingValuesSender(req *http.Request) (*http.Response, error) {
            sd := autorest.GetSendDecorators(req.Context(), autorest.DoRetryForStatusCodes(client.RetryAttempts, client.RetryDuration, autorest.StatusCodesForRetry...))
                return autorest.SendWithSender(client, req, sd...)
                }

    // StopPublishingValuesResponder handles the response to the StopPublishingValues request. The method always
    // closes the http.Response Body.
    func (client BaseClient) StopPublishingValuesResponder(resp *http.Response) (result PublishStopResponseAPIModel, err error) {
        err = autorest.Respond(
        resp,
        client.ByInspecting(),
        azure.WithErrorUnlessStatusCode(http.StatusOK),
        autorest.ByUnmarshallingJSON(&result),
        autorest.ByClosing())
        result.Response = autorest.Response{Response: resp}
            return
        }

    // Subscribe register a client to receive publisher samples through SignalR.
        // Parameters:
            // endpointID - the endpoint to subscribe to
            // body - the user id that will receive publisher samples.
    func (client BaseClient) Subscribe(ctx context.Context, endpointID string, body string) (result autorest.Response, err error) {
        if tracing.IsEnabled() {
            ctx = tracing.StartSpan(ctx, fqdn + "/BaseClient.Subscribe")
            defer func() {
                sc := -1
                if result.Response != nil {
                    sc = result.Response.StatusCode
                }
                tracing.EndSpan(ctx, sc, err)
            }()
        }
            req, err := client.SubscribePreparer(ctx, endpointID, body)
        if err != nil {
        err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "Subscribe", nil , "Failure preparing request")
        return
        }

                resp, err := client.SubscribeSender(req)
                if err != nil {
                result.Response = resp
                err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "Subscribe", resp, "Failure sending request")
                return
                }

                result, err = client.SubscribeResponder(resp)
                if err != nil {
                err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "Subscribe", resp, "Failure responding to request")
                }

        return
        }

        // SubscribePreparer prepares the Subscribe request.
        func (client BaseClient) SubscribePreparer(ctx context.Context, endpointID string, body string) (*http.Request, error) {
                pathParameters := map[string]interface{} {
                "endpointId": autorest.Encode("path",endpointID),
                }

            preparer := autorest.CreatePreparer(
        autorest.AsContentType("application/json-patch+json; charset=utf-8"),
        autorest.AsPut(),
        autorest.WithBaseURL(client.BaseURI),
        autorest.WithPathParameters("/v2/monitor/{endpointId}/samples",pathParameters))
                if len(body) > 0 {
                preparer = autorest.DecoratePreparer(preparer,
                autorest.WithJSON(body))
                }
        return preparer.Prepare((&http.Request{}).WithContext(ctx))
        }

        // SubscribeSender sends the Subscribe request. The method will close the
        // http.Response Body if it receives an error.
        func (client BaseClient) SubscribeSender(req *http.Request) (*http.Response, error) {
            sd := autorest.GetSendDecorators(req.Context(), autorest.DoRetryForStatusCodes(client.RetryAttempts, client.RetryDuration, autorest.StatusCodesForRetry...))
                return autorest.SendWithSender(client, req, sd...)
                }

    // SubscribeResponder handles the response to the Subscribe request. The method always
    // closes the http.Response Body.
    func (client BaseClient) SubscribeResponder(resp *http.Response) (result autorest.Response, err error) {
        err = autorest.Respond(
        resp,
        client.ByInspecting(),
        azure.WithErrorUnlessStatusCode(http.StatusOK),
        autorest.ByClosing())
        result.Response = resp
            return
        }

    // Unsubscribe unregister a client and stop it from receiving samples.
        // Parameters:
            // endpointID - the endpoint to unsubscribe from
            // userID - the user id that will not receive any more published
            // samples
    func (client BaseClient) Unsubscribe(ctx context.Context, endpointID string, userID string) (result autorest.Response, err error) {
        if tracing.IsEnabled() {
            ctx = tracing.StartSpan(ctx, fqdn + "/BaseClient.Unsubscribe")
            defer func() {
                sc := -1
                if result.Response != nil {
                    sc = result.Response.StatusCode
                }
                tracing.EndSpan(ctx, sc, err)
            }()
        }
            req, err := client.UnsubscribePreparer(ctx, endpointID, userID)
        if err != nil {
        err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "Unsubscribe", nil , "Failure preparing request")
        return
        }

                resp, err := client.UnsubscribeSender(req)
                if err != nil {
                result.Response = resp
                err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "Unsubscribe", resp, "Failure sending request")
                return
                }

                result, err = client.UnsubscribeResponder(resp)
                if err != nil {
                err = autorest.NewErrorWithError(err, "azureiiotopcpublisher.BaseClient", "Unsubscribe", resp, "Failure responding to request")
                }

        return
        }

        // UnsubscribePreparer prepares the Unsubscribe request.
        func (client BaseClient) UnsubscribePreparer(ctx context.Context, endpointID string, userID string) (*http.Request, error) {
                pathParameters := map[string]interface{} {
                "endpointId": autorest.Encode("path",endpointID),
                "userId": autorest.Encode("path",userID),
                }

            preparer := autorest.CreatePreparer(
        autorest.AsDelete(),
        autorest.WithBaseURL(client.BaseURI),
        autorest.WithPathParameters("/v2/monitor/{endpointId}/samples/{userId}",pathParameters))
        return preparer.Prepare((&http.Request{}).WithContext(ctx))
        }

        // UnsubscribeSender sends the Unsubscribe request. The method will close the
        // http.Response Body if it receives an error.
        func (client BaseClient) UnsubscribeSender(req *http.Request) (*http.Response, error) {
            sd := autorest.GetSendDecorators(req.Context(), autorest.DoRetryForStatusCodes(client.RetryAttempts, client.RetryDuration, autorest.StatusCodesForRetry...))
                return autorest.SendWithSender(client, req, sd...)
                }

    // UnsubscribeResponder handles the response to the Unsubscribe request. The method always
    // closes the http.Response Body.
    func (client BaseClient) UnsubscribeResponder(resp *http.Response) (result autorest.Response, err error) {
        err = autorest.Respond(
        resp,
        client.ByInspecting(),
        azure.WithErrorUnlessStatusCode(http.StatusOK),
        autorest.ByClosing())
        result.Response = resp
            return
        }

