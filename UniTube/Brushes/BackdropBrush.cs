using Microsoft.Graphics.Canvas.Effects;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace UniTube.Brushes
{
    public class BackdropBrush : XamlCompositionBrushBase
    {
        public static readonly DependencyProperty TintProperty = DependencyProperty.Register(
            nameof(Tint), typeof(Color), typeof(BackdropBrush), new PropertyMetadata(default(Color), OnTintChanged));

        public Color Tint
        {
            get { return (Color)GetValue(TintProperty); }
            set { SetValue(TintProperty, value); }
        }

        private static void OnTintChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var backdropBrush = (BackdropBrush)d;
            backdropBrush.OnDisconnected();
            backdropBrush.OnConnected();
        }

        protected override void OnConnected()
        {
            Compositor compositor = Window.Current.Compositor;

            bool usingFallback  = !CompositionCapabilities.GetForCurrentView().AreEffectsSupported();
            FallbackColor       = Color.FromArgb(255, Tint.R, Tint.G, Tint.B);

            if (usingFallback)
            {
                CompositionBrush = compositor.CreateColorBrush(FallbackColor);
                return;
            }

            var graphicsEffect = new BlendEffect
            {
                Mode        = BlendEffectMode.Overlay,
                Background  = new GaussianBlurEffect
                {
                    Name            = "Blur",
                    Source          = new CompositionEffectSourceParameter("Backdrop"),
                    BlurAmount      = 50f,
                    BorderMode      = EffectBorderMode.Hard,
                    Optimization    = EffectOptimization.Speed
                },
                Foreground = new ColorSourceEffect
                {
                    Name    = "Tint",
                    Color   = Tint
                }
            };

            CompositionEffectFactory effectFactory  = compositor.CreateEffectFactory(graphicsEffect);
            CompositionEffectBrush effectBrush      = effectFactory.CreateBrush();

            CompositionBackdropBrush backdropBrush = compositor.CreateBackdropBrush();
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
}
