using System;
using Microsoft.Graphics.Canvas.Effects;

using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace UniTube.Brushes
{
    public class HostBackdropBrush : XamlCompositionBrushBase
    {
        public static readonly DependencyProperty TintProperty = DependencyProperty.Register(
            nameof(Tint), typeof(Color), typeof(HostBackdropBrush), new PropertyMetadata(default(Color), OnTintChanged));

        public Color Tint
        {
            get { return (Color)GetValue(TintProperty); }
            set { SetValue(TintProperty, value); }
        }

        private static void OnTintChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var hostBackdropBrush = (HostBackdropBrush)d;
            hostBackdropBrush.OnDisconnected();
            hostBackdropBrush.OnConnected();
        }

        protected override void OnConnected()
        {
            Compositor compositor = Window.Current.Compositor;

            bool usingFallback  = !CompositionCapabilities.GetForCurrentView().AreEffectsFast();
            FallbackColor       = Color.FromArgb(255, Tint.R, Tint.G, Tint.B);

            if (usingFallback)
            {
                CompositionBrush = compositor.CreateColorBrush(FallbackColor);
                return;
            }

            var graphicsEffect = new BlendEffect
            {
                Mode        = BlendEffectMode.Overlay,
                Background  = new CompositionEffectSourceParameter("Backdrop"),
                Foreground  = new ColorSourceEffect
                {
                    Name    = "OverlayColor",
                    Color   = Tint
                }
            };

            CompositionEffectFactory effectFactory  = compositor.CreateEffectFactory(graphicsEffect);
            CompositionEffectBrush effectBrush      = effectFactory.CreateBrush();

            CompositionBackdropBrush hostBackdropBrush = compositor.CreateHostBackdropBrush();
            effectBrush.SetSourceParameter("Backdrop", hostBackdropBrush);

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
}
