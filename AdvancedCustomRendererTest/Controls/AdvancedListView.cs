using System;
using Xamarin.Forms;

namespace AdvancedCustomRendererTest.Controls
{
    public class AdvancedListView
        : ListView
    {
        public static BindableProperty HasVerticalScrollbarProperty = 
            BindableProperty.Create<AdvancedListView, bool> (p => p.HasVerticalScrollbar, true);

        public static BindableProperty IsContentOffsetAnimatedProperty = 
            BindableProperty.Create<AdvancedListView, bool> (p => p.IsContentOffsetAnimated, false);

        public static BindableProperty ContentSizeProperty = 
            BindableProperty.Create<AdvancedListView, Size> (p => p.ContentSize, new Size (0, 0), BindingMode.OneWayToSource);

        public static BindableProperty ContentOffsetProperty = 
            BindableProperty.Create<AdvancedListView, Point> (p => p.ContentOffset, new Point(0, 0));

        public static BindableProperty TopOverlayHeightPercentProperty = 
            BindableProperty.Create<AdvancedListView, float> (p => p.TopOverlayHeightPercent, 20);

        public static BindableProperty BottomOverlayHeightPercentProperty = 
            BindableProperty.Create<AdvancedListView, float> (p => p.BottomOverlayHeightPercent, 20);

        public static BindableProperty TopOverlayOuterColorProperty = 
            BindableProperty.Create<AdvancedListView, Color> (p => p.TopOverlayOuterColor, Color.White.MultiplyAlpha (1));

        public static BindableProperty TopOverlayInnerColorProperty = 
            BindableProperty.Create<AdvancedListView, Color> (p => p.TopOverlayInnerColor, Color.White.MultiplyAlpha (1));

        public static BindableProperty BottomOverlayInnerColorProperty = 
            BindableProperty.Create<AdvancedListView, Color> (p => p.BottomOverlayInnerColor, Color.White.MultiplyAlpha (1));

        public static BindableProperty BottomOverlayOuterColorProperty = 
            BindableProperty.Create<AdvancedListView, Color> (p => p.BottomOverlayOuterColor, Color.White.MultiplyAlpha (1));

        public bool HasVerticalScrollbar
        {
            get
            {
                return (bool)GetValue (HasVerticalScrollbarProperty);
            }
            set
            {
                SetValue (HasVerticalScrollbarProperty, value);
            }
        }

        public bool IsContentOffsetAnimated
        {
            get
            {
                return (bool)GetValue (IsContentOffsetAnimatedProperty);
            }
            set
            {
                SetValue (IsContentOffsetAnimatedProperty, value);
            }
        }

        public Size ContentSize
        {
            get
            {
                return (Size)GetValue (ContentSizeProperty);
            }
        }

        public Point ContentOffset
        {
            get
            {
                return (Point)GetValue (ContentOffsetProperty);
            }
            set
            {
                SetValue (ContentOffsetProperty, value);
            }
        }

        public float TopOverlayHeightPercent
        {
            get
            {
                return (float)GetValue (TopOverlayHeightPercentProperty);
            }
            set
            {
                SetValue (TopOverlayHeightPercentProperty, value);
            }
        }

        public float BottomOverlayHeightPercent
        {
            get
            {
                return (float)GetValue (BottomOverlayHeightPercentProperty);
            }
            set
            {
                SetValue (BottomOverlayHeightPercentProperty, value);
            }
        }

        public Color TopOverlayOuterColor
        {
            get
            {
                return (Color)GetValue (TopOverlayOuterColorProperty);
            }
            set
            {
                SetValue (TopOverlayOuterColorProperty, value);
            }
        }

        public Color TopOverlayInnerColor
        {
            get
            {
                return (Color)GetValue (TopOverlayInnerColorProperty);
            }
            set
            {
                SetValue (TopOverlayInnerColorProperty, value);
            }
        }

        public Color BottomOverlayInnerColor
        {
            get
            {
                return (Color)GetValue (BottomOverlayInnerColorProperty);
            }
            set
            {
                SetValue (BottomOverlayInnerColorProperty, value);
            }
        }

        public Color BottomOverlayOuterColor
        {
            get
            {
                return (Color)GetValue (BottomOverlayOuterColorProperty);
            }
            set
            {
                SetValue (BottomOverlayOuterColorProperty, value);
            }
        }

        public AdvancedListView()
        {
        }
    }
}

