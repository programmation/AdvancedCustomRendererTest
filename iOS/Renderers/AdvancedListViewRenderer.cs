using System;
using System.Collections.Generic;
using AdvancedCustomRendererTest.Controls;
using AdvancedCustomRendererTest.iOS;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AdvancedListView), typeof(AdvancedListViewRenderer))]

namespace AdvancedCustomRendererTest.iOS
{
    public class AdvancedListViewRenderer 
        : ListViewRenderer, IUIScrollViewDelegate
    {
//      private UITableViewSource _formsSource;
        private bool _isScrolling;
        private nfloat _topOverlayHeight;
        private nfloat _bottomOverlayHeight;
        private UIView _topOverlayView;
        private CAGradientLayer _topOverlayMask;
        private UIView _bottomOverlayView;
        private CAGradientLayer _bottomOverlayMask;
        private UIImage _backImage;

        public AdvancedListViewRenderer ()
        {
            _topOverlayView = new UIView ();
            _topOverlayMask = new CAGradientLayer ();
            _topOverlayView.Layer.AddSublayer (_topOverlayMask);
            _topOverlayView.UserInteractionEnabled = false;
            _bottomOverlayView = new UIView ();
            _bottomOverlayMask = new CAGradientLayer ();
            _bottomOverlayView.Layer.AddSublayer (_bottomOverlayMask);
            _bottomOverlayView.UserInteractionEnabled = false;
        }

        public override CGRect Frame {
            get {
                return base.Frame;
            }
            set {
                base.Frame = value;
                UpdateAffordanceHeights ();
                UpdateAffordances ();
            }
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

                var formsSource = Control.Source;

                var customSource = new MCListViewSource (formsSource, 
                    (offset) => {
                    //                      Debug.WriteLine("Started scrolling!");
                    _isScrolling = true;
                }, 
                    (offset) => {
                    //                      Debug.WriteLine("Scrolled {0}", offset);
                    if (!_isScrolling) 
                    {                           
                        return;
                    }
                    ReportContentOffset (element);
                    UpdateAffordances ();
                }, 
                    () => {
                    //                      Debug.WriteLine("Stopped scrolling!");
                    _isScrolling = false;
                    ReportContentOffset (element);
                    UpdateAffordances ();
                }
                );

                Control.Source = customSource;

                AddSubview (_topOverlayView);
                AddSubview (_bottomOverlayView);

                UpdateAffordanceColors ();
                SetNeedsLayout ();
            }
        }

        protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged (sender, e);

            if (e.PropertyName == ListView.ItemsSourceProperty.PropertyName)
            {
                SetNeedsLayout ();
                return;
            }
            if (e.PropertyName == AdvancedListView.TopOverlayOuterColorProperty.PropertyName
                || e.PropertyName == AdvancedListView.TopOverlayInnerColorProperty.PropertyName
                || e.PropertyName == AdvancedListView.BottomOverlayInnerColorProperty.PropertyName
                || e.PropertyName == AdvancedListView.BottomOverlayOuterColorProperty.PropertyName)
            {
                UpdateAffordanceColors ();
                SetNeedsLayout ();
                return;
            }
            if (e.PropertyName == AdvancedListView.TopOverlayHeightPercentProperty.PropertyName
                || e.PropertyName == AdvancedListView.BottomOverlayHeightPercentProperty.PropertyName)
            {
                SetNeedsLayout ();
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
            Control.ShowsVerticalScrollIndicator = element.HasVerticalScrollbar;
        }

        private void UpdateContentOffset (AdvancedListView element)
        {
            //          var offset = new CGPoint (element.ContentOffset.X, element.ContentOffset.Y);
            //          Control.SetContentOffset (offset, element.IsContentOffsetAnimated);
        }

        private void ReportContentSize (AdvancedListView element)
        {
            element.SetValue (AdvancedListView.ContentSizeProperty, new Size (Control.ContentSize.Width, Control.ContentSize.Height));
        }

        private void ReportContentOffset (AdvancedListView element)
        {
            element.SetValue (AdvancedListView.ContentOffsetProperty, new Point (Control.ContentOffset.X, Control.ContentOffset.Y));
        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();

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

            _topOverlayHeight = Bounds.Height * element.TopOverlayHeightPercent/100;
            _bottomOverlayHeight = Bounds.Height * element.BottomOverlayHeightPercent/100;
        }

        private void UpdateAffordanceColors ()
        {
            var element = Element as AdvancedListView;

            if (element == null)
            {
                return;
            }

            var topOuterColor = element.TopOverlayOuterColor.ToCGColor ();
            var topInnerColor = element.TopOverlayInnerColor.ToCGColor ();
            var bottomInnerColor = element.BottomOverlayInnerColor.ToCGColor ();
            var bottomOuterColor = element.BottomOverlayOuterColor.ToCGColor ();

            _topOverlayMask.Colors = new CGColor[] { topOuterColor, topInnerColor };
            _bottomOverlayMask.Colors = new CGColor[] { bottomInnerColor, bottomOuterColor };
        }

        private void UpdateAffordances ()
        {
            if (Control == null)
            {
                return;
            }
            //          Debug.WriteLine ("Content: {0} -> {1}", Control.ContentOffset, Control.ContentSize);

            var topFrame = new CGRect (0, 0, Bounds.Width, _topOverlayHeight);
            var bottomFrame = new CGRect (0, Bounds.Height - _bottomOverlayHeight, Bounds.Width, _bottomOverlayHeight);

            var element = Element as AdvancedListView;
            _topOverlayView.Frame = topFrame;
            _bottomOverlayView.Frame = bottomFrame;

            if (false) {
                Element view = element;
                while (view as Page == null) {
                    view = view.Parent;
                }
                var page = view as Page;

                if (_backImage == null && page != null && Window != null) {
                    if (!String.IsNullOrEmpty (page.BackgroundImage)) {
                        {
                            _backImage = UIImage.FromBundle (page.BackgroundImage);
                        }
                    }
                    var referenceView = Window.RootViewController.View;
                    UIGraphics.BeginImageContext(referenceView.Bounds.Size);
                    _backImage.DrawAsPatternInRect(referenceView.Bounds);
                    var image = UIGraphics.GetImageFromCurrentImageContext();
                    UIGraphics.EndImageContext();
                    var imageView = new UIImageView () { Image = image, Frame = referenceView.Bounds };
                    imageView.DrawViewHierarchy (imageView.Bounds, true);

                    var topViewRect = referenceView.ConvertRectFromView (_topOverlayView.Bounds, _topOverlayView);
                    _topOverlayView.RemoveFromSuperview ();
                    _topOverlayView = imageView.ResizableSnapshotView (topViewRect, true, UIEdgeInsets.Zero);
                    _topOverlayView.Layer.AddSublayer (_topOverlayMask);
                    _topOverlayView.UserInteractionEnabled = false;
                    AddSubview (_topOverlayView);
                    BringSubviewToFront (_topOverlayView);

                    var bottomViewRect = referenceView.ConvertRectFromView (_bottomOverlayView.Bounds, _bottomOverlayView);
                    _bottomOverlayView.RemoveFromSuperview ();
                    _bottomOverlayView = imageView.ResizableSnapshotView (bottomViewRect, true, UIEdgeInsets.Zero);
                    _bottomOverlayView.Layer.AddSublayer (_bottomOverlayMask);
                    _bottomOverlayView.UserInteractionEnabled = false;
                    AddSubview (_bottomOverlayView);
                    BringSubviewToFront (_bottomOverlayView);
                }
            }

            _topOverlayView.Frame = topFrame;
            _topOverlayMask.Frame = _topOverlayView.Bounds;
            _bottomOverlayView.Frame = bottomFrame;
            _bottomOverlayMask.Frame = _bottomOverlayView.Bounds;

            _topOverlayView.Hidden = !(Control.ContentOffset.Y > 0);
            _bottomOverlayView.Hidden = !(Control.ContentOffset.Y < (Control.ContentSize.Height - Control.Bounds.Height));
        }
    }

    public class MCListViewSource : UITableViewSource
    {
        private UITableViewSource _formsSource;
        private Action<CGPoint> _onStartScrolling;
        private Action<CGPoint> _onScrolled;
        private Action _onStopScrolling;

        public Dictionary<int, int> Counts { get; set; }

        public MCListViewSource (UITableViewSource formsSource, 
            Action<CGPoint> onStartScrolling, 
            Action<CGPoint> onScrolled,
            Action onStopScrolling
        )
        {
            _formsSource = formsSource;
            _onStartScrolling = onStartScrolling;
            _onScrolled = onScrolled;
            _onStopScrolling = onStopScrolling;
        }

        public override void DraggingStarted (UIScrollView scrollView)
        {
            if (_onStartScrolling != null)
            {
                _onStartScrolling (scrollView.ContentOffset);
            }
        }

        public override void Scrolled (UIScrollView scrollView)
        {
            if (_onScrolled != null)
            {
                _onScrolled (scrollView.ContentOffset);
            }
        }

        public override void DraggingEnded (UIScrollView scrollView, bool willDecelerate)
        {
            if (willDecelerate)
            {
                return;
            }
            if (_onStopScrolling != null)
            {
                _onStopScrolling ();
            }
        }

        public override void DecelerationEnded (UIScrollView scrollView)
        {
            if (_onStopScrolling != null)
            {
                _onStopScrolling ();
            }
        }

        public override void ScrollAnimationEnded (UIScrollView scrollView)
        {
            if (_onStopScrolling != null)
            {
                _onStopScrolling ();
            }
        }

        //      public override void AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
        //      { 
        //          _formsSource.AccessoryButtonTapped (tableView, indexPath);
        //      }

        //      public override UITableViewCellAccessory AccessoryForRow (UITableView tableView, NSIndexPath indexPath)
        //      {
        //          return _formsSource.AccessoryForRow (tableView, indexPath);
        //      }

        //      public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
        //      {
        //          return _formsSource.CanEditRow (tableView, indexPath);
        //      }

        //      public override bool CanMoveRow (UITableView tableView, NSIndexPath indexPath)
        //      {
        //          return _formsSource.CanMoveRow (tableView, indexPath);
        //      }

        //      public override bool CanPerformAction (UITableView tableView, Selector action, NSIndexPath indexPath, NSObject sender)
        //      {
        //          return _formsSource.CanPerformAction (tableView, action, indexPath, sender);
        //      }

        //      public override void CellDisplayingEnded (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        //      {
        //          _formsSource.CellDisplayingEnded (tableView, cell, indexPath);
        //      }

        //      public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        //      {
        //          _formsSource.CommitEditingStyle (tableView, editingStyle, indexPath);
        //      }

        //      public override NSIndexPath CustomizeMoveTarget (UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath proposedIndexPath)
        //      {
        //          return _formsSource.CustomizeMoveTarget (tableView, sourceIndexPath, proposedIndexPath);
        //      }

        //      public override void DidEndEditing (UITableView tableView, NSIndexPath indexPath)
        //      {
        //          _formsSource.DidEndEditing (tableView, indexPath);
        //      }

        //      public override UITableViewRowAction[] EditActionsForRow (UITableView tableView, NSIndexPath indexPath)
        //      {
        //          return _formsSource.EditActionsForRow (tableView, indexPath);
        //      }

        //      public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath)
        //      {
        //          return _formsSource.EditingStyleForRow (tableView, indexPath);
        //      }

        //      public override nfloat EstimatedHeight (UITableView tableView, NSIndexPath indexPath)
        //      {
        //          return _formsSource.EstimatedHeight (tableView, indexPath);
        //      }

        //      public override nfloat EstimatedHeightForFooter (UITableView tableView, nint section)
        //      {
        //          return _formsSource.EstimatedHeightForFooter (tableView, section);
        //      }

        //      public override nfloat EstimatedHeightForHeader (UITableView tableView, nint section)
        //      {
        //          return _formsSource.EstimatedHeightForHeader (tableView, section);
        //      }

        //      public override void FooterViewDisplayingEnded (UITableView tableView, UIView footerView, nint section)
        //      {
        //          _formsSource.FooterViewDisplayingEnded (tableView, footerView, section);
        //      }

        public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
        {
            return _formsSource.GetCell (tableView, indexPath);
        }

        //      public override nfloat GetHeightForFooter (UITableView tableView, nint section)
        //      {
        //          return _formsSource.GetHeightForFooter (tableView, section);
        //      }

        public override nfloat GetHeightForHeader (UITableView tableView, nint section)
        {
            return _formsSource.GetHeightForHeader (tableView, section);
        }

        public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
        {
            return _formsSource.GetHeightForRow (tableView, indexPath);
        }

        //      public override UIView GetViewForFooter (UITableView tableView, nint section)
        //      {
        //          return _formsSource.GetViewForFooter (tableView, section);
        //      }

        public override UIView GetViewForHeader (UITableView tableView, nint section)
        {
            return _formsSource.GetViewForHeader (tableView, section);
        }

        //      public override void HeaderViewDisplayingEnded (UITableView tableView, UIView headerView, nint section)
        //      {
        //          _formsSource.HeaderViewDisplayingEnded (tableView, headerView, section);
        //      }

        //      public override nint IndentationLevel (UITableView tableView, NSIndexPath indexPath)
        //      {
        //          return _formsSource.IndentationLevel (tableView, indexPath);
        //      }

        //      public override void MoveRow (UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
        //      {
        //          _formsSource.MoveRow (tableView, sourceIndexPath, destinationIndexPath);
        //      }

        public override nint NumberOfSections (UITableView tableView)
        {
            return _formsSource.NumberOfSections (tableView);
        }

        //      public override void PerformAction (UITableView tableView, Selector action, NSIndexPath indexPath, NSObject sender)
        //      {
        //          _formsSource.PerformAction (tableView, action, indexPath, sender);
        //      }

        public override void RowDeselected (UITableView tableView, NSIndexPath indexPath)
        {
            _formsSource.RowDeselected (tableView, indexPath);
        }

        //      public override void RowHighlighted (UITableView tableView, NSIndexPath rowIndexPath)
        //      {
        //          _formsSource.RowHighlighted (tableView, rowIndexPath);
        //      }

        public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
        {
            _formsSource.RowSelected (tableView, indexPath);
        }

        public override nint RowsInSection (UITableView tableview, nint section)
        {
            return _formsSource.RowsInSection (tableview, section);
        }

        //      public override void RowUnhighlighted (UITableView tableView, NSIndexPath rowIndexPath)
        //      {
        //          _formsSource.RowUnhighlighted (tableView, rowIndexPath);
        //      }

        //      public override nint SectionFor (UITableView tableView, string title, nint atIndex)
        //      {
        //          return _formsSource.SectionFor (tableView, title, atIndex);
        //      }

        public override string[] SectionIndexTitles (UITableView tableView)
        {
            return _formsSource.SectionIndexTitles (tableView);
        }

        //      public override bool ShouldHighlightRow (UITableView tableView, NSIndexPath rowIndexPath)
        //      {
        //          return _formsSource.ShouldHighlightRow (tableView, rowIndexPath);
        //      }

        //      public override bool ShouldIndentWhileEditing (UITableView tableView, NSIndexPath indexPath)
        //      {
        //          return _formsSource.ShouldIndentWhileEditing (tableView, indexPath);
        //      }

        //      public override bool ShouldShowMenu (UITableView tableView, NSIndexPath rowAtindexPath)
        //      {
        //          return _formsSource.ShouldShowMenu (tableView, rowAtindexPath);
        //      }

        //      public override string TitleForDeleteConfirmation (UITableView tableView, NSIndexPath indexPath)
        //      {
        //          return _formsSource.TitleForDeleteConfirmation (tableView, indexPath);
        //      }

        //      public override string TitleForFooter (UITableView tableView, nint section)
        //      {
        //          return _formsSource.TitleForFooter (tableView, section);
        //      }

        public override string TitleForHeader (UITableView tableView, nint section)
        {
            return _formsSource.TitleForHeader (tableView, section);
        }

        //      public override void WillBeginEditing (UITableView tableView, NSIndexPath indexPath)
        //      {
        //          _formsSource.WillBeginEditing (tableView, indexPath);
        //      }

        //      public override NSIndexPath WillDeselectRow (UITableView tableView, NSIndexPath indexPath)
        //      {
        //          return _formsSource.WillDeselectRow (tableView, indexPath);
        //      }

        //      public override void WillDisplay (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        //      {
        //          _formsSource.WillDisplay (tableView, cell, indexPath);
        //      }

        //      public override void WillDisplayFooterView (UITableView tableView, UIView footerView, nint section)
        //      {
        //          _formsSource.WillDisplayFooterView (tableView, footerView, section);
        //      }

        //      public override void WillDisplayHeaderView (UITableView tableView, UIView headerView, nint section)
        //      {
        //          _formsSource.WillDisplayHeaderView (tableView, headerView, section);
        //      }

        //      public override NSIndexPath WillSelectRow (UITableView tableView, NSIndexPath indexPath)
        //      {
        //          return _formsSource.WillSelectRow (tableView, indexPath);
        //      }
    }}

