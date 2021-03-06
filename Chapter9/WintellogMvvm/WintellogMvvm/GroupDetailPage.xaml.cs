﻿using System;
using System.Collections.Generic;
using PortableWintellog.Data;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WintellogMvvm.Common;
using WintellogMvvm.DataModel;

namespace WintellogMvvm
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class GroupDetailPage
    {
        public GroupDetailPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            App.Instance.Share = Share;
            var group = App.Instance.DataSource.GetGroup((string)navigationParameter);
            DefaultViewModel["Group"] = group;
            DefaultViewModel["Items"] = group.Items;
        }

        private void Share(DataTransferManager dataTransferManager, DataRequestedEventArgs dataRequestedEventArgs)
        {
            var group = DefaultViewModel["Group"] as BlogGroup;
            
            if (group == null)
            {
                return;
            }

            dataRequestedEventArgs.Request.Data.Properties.Title = group.Title;
            dataRequestedEventArgs.Request.Data.SetUri(group.RssUri);
            dataRequestedEventArgs.Request.Data.Properties.Description = "Wintellog RSS feed.";
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            pageState["SelectedGroup"] = ((BlogGroup)DefaultViewModel["Group"]).Id;
        }

        /// <summary>
        /// Invoked when an item is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemId = ((BlogItem)e.ClickedItem).Id;
            Frame.Navigate(typeof(ItemDetailPage), itemId);
        }

        private void Home_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GroupedItemsPage),
                "GroupList");
        }

        private void Pin_Click_1(object sender, RoutedEventArgs e)
        {
            var group = DefaultViewModel["Group"] as BlogGroup;
            
            if (group == null) return;
            
            var title = string.Format("Blog: {0}", group.Title);
            App.Instance.PinToStart(sender,
                                    string.Format("Wintellog.{0}", group.Id.GetHashCode()),
                                    title,
                                    title,
                                    string.Format("Group={0}", group.Id));
        }
    }
}
