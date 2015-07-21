using System;
using AdvancedCustomRendererTest.Controls;
using AdvancedCustomRendererTest.Droid;
using Android.Graphics.Drawables;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AdvancedListView), typeof(AdvancedListViewRenderer))]

namespace AdvancedCustomRendererTest.Droid
{
    public class AdvancedListViewRenderer 
        : ListViewRenderer
    {
        private int _topOverlayHeight;
        private int _bottomOverlayHeight;
        private Android.Views.View _topOverlayView;
        private Android.Views.View _bottomOverlayView;
        private GradientDrawable _topOverlayMask;
        private GradientDrawable _bottomOverlayMask;

        public AdvancedListViewRenderer ()
        {
        }

        protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged (e);

            if (e.NewElement == null)
            {
                return;
            }

            var element = Element as AdvancedListView;

            if (Control != null)
            {
                UpdateVerticalScrollbar (element);

                Control.Scroll += (object sender, AbsListView.ScrollEventArgs e2) => {
                    //                  System.Diagnostics.Debug.WriteLine ("{0} -> {1}", scrollY, Control.MaxScrollAmount);
                    ReportContentOffset (element);
                    UpdateAffordances ();
                };
                Control.ScrollStateChanged += (object sender, AbsListView.ScrollStateChangedEventArgs e2) => {

                };

                _topOverlayMask = new GradientDrawable ();
                _topOverlayView = new Android.Views.View (Forms.Context);
                _topOverlayView.SetBackgroundDrawable (_topOverlayMask);
                AddView (_topOverlayView);

                _bottomOverlayMask = new GradientDrawable ();
                _bottomOverlayView = new Android.Views.View (Forms.Context);
                _bottomOverlayView.SetBackgroundDrawable (_bottomOverlayMask);
                AddView (_bottomOverlayView);

                UpdateAffordanceColors ();
                RequestLayout ();
            }
        }

        protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged (sender, e);

            if (e.PropertyName == Xamarin.Forms.ListView.ItemsSourceProperty.PropertyName)
            {
                RequestLayout ();
                return;
            }
            if (e.PropertyName == AdvancedListView.TopOverlayOuterColorProperty.PropertyName
                || e.PropertyName == AdvancedListView.TopOverlayInnerColorProperty.PropertyName
                || e.PropertyName == AdvancedListView.BottomOverlayInnerColorProperty.PropertyName
                || e.PropertyName == AdvancedListView.BottomOverlayOuterColorProperty.PropertyName)
            {
                UpdateAffordanceColors ();
                RequestLayout ();
                return;
            }
            if (e.PropertyName == AdvancedListView.TopOverlayHeightPercentProperty.PropertyName
                || e.PropertyName == AdvancedListView.BottomOverlayHeightPercentProperty.PropertyName)
            {
                RequestLayout ();
                return;
            }

            var element = Element as AdvancedListView;
            if (element == null)
            {
                return;
            }

            if (e.PropertyName == AdvancedListView.HasVerticalScrollbarProperty.PropertyName)
            {
                UpdateVerticalScrollbar (element);
                return;
            }
            if (e.PropertyName == AdvancedListView.ContentOffsetProperty.PropertyName)
            {
                //              UpdateContentOffset (element);
                return;
            }
        }

        private void UpdateVerticalScrollbar (AdvancedListView element)
        {
            Control.VerticalScrollBarEnabled = element.HasVerticalScrollbar;
        }

        private void UpdateContentOffset(AdvancedListView element)
        {

        }

        private void ReportContentSize (AdvancedListView element)
        {
            element.SetValue (AdvancedListView.ContentSizeProperty, new Size (Control.Width, Control.MaxScrollAmount));
        }

        private void ReportContentOffset (AdvancedListView element)
        {
            var child = Control.GetChildAt (0);
            var scrollY = -child.Top;
            element.SetValue (AdvancedListView.ContentOffsetProperty, new Xamarin.Forms.Point (0, scrollY));
        }

        protected override void OnLayout (bool changed, int l, int t, int r, int b)
        {
            base.OnLayout (changed, l, t, r, b);

            var element = Element as AdvancedListView;

            ReportContentSize (element);
            ReportContentOffset (element);

            UpdateAffordanceHeights ();
            UpdateAffordances ();
        }

        private void UpdateAffordanceHeights ()
        {
            var element = Element as AdvancedListView;

            if (element == null)
            {
                return;
            }

            _topOverlayHeight = (int)(Height * element.TopOverlayHeightPercent / 100);
            _bottomOverlayHeight = (int)(Height * element.BottomOverlayHeightPercent / 100);
        }

        private void UpdateAffordanceColors ()
        {
            var element = Element as AdvancedListView;

            if (element == null)
            {
                return;
            }

            var topOuterColor = element.TopOverlayOuterColor.ToAndroid ();
            var topInnerColor = element.TopOverlayInnerColor.ToAndroid ();
            var bottomInnerColor = element.BottomOverlayInnerColor.ToAndroid ();
            var bottomOuterColor = element.BottomOverlayOuterColor.ToAndroid ();

            _topOverlayMask.SetColors (new int[] { topOuterColor, topInnerColor });
            _bottomOverlayMask.SetColors (new int[] { bottomInnerColor, bottomOuterColor });
        }

        private void UpdateAffordances ()
        {
            if (Control == null)
            {
                return;
            }

            // Android reports very strange numbers for these properties
            var first = Control.FirstVisiblePosition;
            var last = Control.LastVisiblePosition - 1;
            var count = Control.Count - 2;
            //          System.Diagnostics.Debug.WriteLine ("First: {0}, Last: {1}, Count: {2}", first, last, count);
            _topOverlayView.Layout (0, 0, Width, _topOverlayHeight);
            _bottomOverlayView.Layout (0, Height - _bottomOverlayHeight, Width, Height);
            _topOverlayView.Visibility = (first > 1) ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Invisible;
            _bottomOverlayView.Visibility = (last < count) ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Invisible;
        }
    }}

