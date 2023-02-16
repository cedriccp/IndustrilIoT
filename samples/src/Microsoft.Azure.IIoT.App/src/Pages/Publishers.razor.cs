﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.App.Pages {
    using Microsoft.Azure.IIoT.App.Extensions;
    using Microsoft.Azure.IIoT.App.Models;
    using Microsoft.Azure.IIoT.Api.Models;
    using Microsoft.AspNetCore.Components;
    using System;
    using System.Threading.Tasks;

    public partial class Publishers {
        [Parameter]
        public string Page { get; set; } = "1";

        private PagedResult<PublisherModel> PublisherList { get; set; } =
            new PagedResult<PublisherModel>();
        private PagedResult<PublisherModel> _pagedPublisherList =
            new();
        private IAsyncDisposable _publisherEvent;
        private string _tableView = "visible";
        private string _tableEmpty = "displayNone";


        /// <summary>
        /// Notify page change
        /// </summary>
        /// <param name="page"></param>
        public async Task PagerPageChangedAsync(int page) {
            CommonHelper.Spinner = "loader-big";
            StateHasChanged();
            PublisherList = CommonHelper.UpdatePage(RegistryHelper.GetPublisherListAsync, page, PublisherList, ref _pagedPublisherList, CommonHelper.PageLengthSmall);
            NavigationManager.NavigateTo(NavigationManager.BaseUri + "publishers/" + page);
            for (var i = 0; i < _pagedPublisherList.Results.Count; i++) {
                _pagedPublisherList.Results[i] = await RegistryService.GetPublisherAsync(_pagedPublisherList.Results[i].Id);
            }
            CommonHelper.Spinner = string.Empty;
            StateHasChanged();
        }

        /// <summary>
        /// OnInitialized
        /// </summary>
        protected override void OnInitialized() {
            CommonHelper.Spinner = "loader-big";
        }

        /// <summary>
        /// OnAfterRenderAsync
        /// </summary>
        /// <param name="firstRender"></param>
        protected override async Task OnAfterRenderAsync(bool firstRender) {
            if (firstRender) {
                PublisherList = await RegistryHelper.GetPublisherListAsync();
                Page = "1";
                _pagedPublisherList = PublisherList.GetPaged(int.Parse(Page), CommonHelper.PageLengthSmall, PublisherList.Error);
                CommonHelper.Spinner = string.Empty;
                CommonHelper.CheckErrorOrEmpty(_pagedPublisherList, ref _tableView, ref _tableEmpty);
                StateHasChanged();

                _publisherEvent = await RegistryServiceEvents.SubscribePublisherEventsAsync(
                    ev => InvokeAsync(() => PublisherEvent(ev)));
            }
        }

        private Task PublisherEvent(PublisherEventModel ev) {
            _pagedPublisherList = PublisherList.GetPaged(int.Parse(Page), CommonHelper.PageLengthSmall, PublisherList.Error);
            StateHasChanged();
            return Task.CompletedTask;
        }

        public async void Dispose() {
            if (_publisherEvent != null) {
                await _publisherEvent.DisposeAsync();
            }
        }
    }
}