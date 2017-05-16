using System;

using Microsoft.Graphics.Canvas.Effects;

using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace UniTube.Brushes
{
    public class AcrylicBrush : XamlCompositionBrushBase
    {
        public bool FallbackForced
        {
            get { return (bool)GetValue(FallbackForcedProperty); }
            set { SetValue(FallbackForcedProperty, value); }
        }

        public static readonly DependencyProperty FallbackForcedProperty = DependencyProperty.Register(
            nameof(FallbackForced), typeof(bool), typeof(AcrylicBrush), new PropertyMetadata(false, OnFallbackForcedChanged));

        private static void OnFallbackForcedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var acrylicBrush = (AcrylicBrush)d;
            acrylicBrush.OnDisconnected();
            acrylicBrush.OnConnected();
        }

        public AcrylicBackgroundSource BackgroundSource
        {
            get { return (AcrylicBackgroundSource)GetValue(BackgroundSourceProperty); }
            set { SetValue(BackgroundSourceProperty, value); }
        }

        public static readonly DependencyProperty BackgroundSourceProperty = DependencyProperty.Register(
            nameof(BackgroundSource), typeof(AcrylicBackgroundSource), typeof(AcrylicBrush), new PropertyMetadata(default(AcrylicBackgroundSource), OnBackgroundSourceChanged));

        private static void OnBackgroundSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var acrylicBrush = (AcrylicBrush)d;
            acrylicBrush.OnDisconnected();
            acrylicBrush.OnConnected();
        }

        public Color TintColor
        {
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
        }

        public static readonly DependencyProperty TintColorProperty = DependencyProperty.Register(
            nameof(TintColor), typeof(Color), typeof(AcrylicBrush), new PropertyMetadata(default(Color), OnTintColorChanged));

        private static void OnTintColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var acrylicBrush = (AcrylicBrush)d;
            acrylicBrush.OnDisconnected();
            acrylicBrush.OnConnected();
        }

        protected override void OnConnected()
        {
            Compositor compositor = Window.Current.Compositor;

            bool usingFallback;

            if (BackgroundSource == AcrylicBackgroundSource.Backdrop)
            {
                usingFallback = FallbackForced ? true : !CompositionCapabilities.GetForCurrentView().AreEffectsSupported();
            }
            else
            {
                usingFallback = FallbackForced ? true : !CompositionCapabilities.GetForCurrentView().AreEffectsFast();
            }

            if (usingFallback)
            {
                CompositionBrush = compositor.CreateColorBrush(FallbackColor);
                return;
            }

            var graphicsEffect = new BlendEffect
            {
                Mode = BlendEffectMode.Overlay,
                Foreground = new ColorSourceEffect
                {
                    Name = "OverlayColor",
                    Color = TintColor
                }
            };

            CompositionBackdropBrush backdropBrush;

            if (BackgroundSource == AcrylicBackgroundSource.Backdrop)
            {
                graphicsEffect.Background = new GaussianBlurEffect
                {
                    BlurAmount = 50,
                    BorderMode = EffectBorderMode.Hard,
                    Name = "Blur",
                    Optimization = EffectOptimization.Speed,
                    Source = new CompositionEffectSourceParameter("Backdrop")
                };

                backdropBrush = compositor.CreateBackdropBrush();
            }
            else
            {
                graphicsEffect.Background = new CompositionEffectSourceParameter("Backdrop");

                backdropBrush = compositor.CreateHostBackdropBrush();
            }

            var effectFactory = compositor.CreateEffectFactory(graphicsEffect);
            var effectBrush = effectFactory.CreateBrush();

            effectBrush.SetSourceParameter("Backdrop", backdropBrush);

            CompositionBrush = effectBrush;
        }

        protected override void OnDisconnected()
        {
            if (CompositionBrush != null)
            {
                CompositionBrush.Dispose();
                CompositionBrush = null;
            }
        }
    }

    public enum AcrylicBackgroundSource
    {
        Backdrop,
        HostBackdrop
    }
}
