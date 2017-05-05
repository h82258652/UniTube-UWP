using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Effects;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace UniTube.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Compositor _compositor;
        SpriteVisual _contentSprite;
        SpriteVisual _paneSprite;

        public MainPage()
        {
            this.InitializeComponent();
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _contentSprite = _compositor.CreateSpriteVisual();
            _paneSprite = _compositor.CreateSpriteVisual();

            _contentSprite.Size = new Vector2((float)ContentBackground.ActualWidth, (float)ContentBackground.ActualHeight);
            _paneSprite.Size = new Vector2((float)PaneBackground.ActualWidth, (float)PaneBackground.ActualHeight);

            ElementCompositionPreview.SetElementChildVisual(ContentBackground, _contentSprite);
            ElementCompositionPreview.SetElementChildVisual(PaneBackground, _paneSprite);

            Matrix5x4 mat = new Matrix5x4
            {
                M11 = 1, M12 = 0, M13 = 0, M14 = 0,
                M21 = 0, M22 = 1, M23 = 0, M24 = 0,
                M31 = 0, M32 = 0, M33 = 1, M34 = 0,
                M41 = 0, M42 = 0, M43 = 0, M44 = 0.5f,
                M51 = 0, M52 = 0, M53 = 0, M54 = 0
            };

            IGraphicsEffect graphicsEffect = new ColorMatrixEffect
            {
                ColorMatrix = mat,
                Source = new CompositionEffectSourceParameter("ImageSource")
            };

            CompositionEffectFactory _effectFactory = _compositor.CreateEffectFactory(graphicsEffect, null);
            CompositionEffectBrush brush = _effectFactory.CreateBrush();

            brush.SetSourceParameter("ImageSource", _compositor.CreateHostBackdropBrush());

            _paneSprite.Brush = _contentSprite.Brush = brush;

            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            TitleBar.Height = coreTitleBar.Height;
            PaneTitleBar.Height = coreTitleBar.Height;
            ContentTitleBar.Height = coreTitleBar.Height;
            Window.Current.SetTitleBar(TitleBar);

            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            PaneTitleBar.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            ContentTitleBar.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar.Height = sender.Height;
            PaneTitleBar.Height = sender.Height;
            ContentTitleBar.Height = sender.Height;
        }

        private void ContentBackground_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_contentSprite != null)
                _contentSprite.Size = e.NewSize.ToVector2();
        }

        private void PaneBackground_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_paneSprite != null)
                _paneSprite.Size = e.NewSize.ToVector2();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }
    }
}
