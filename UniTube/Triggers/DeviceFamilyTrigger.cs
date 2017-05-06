using System;

using Windows.UI.Xaml;

namespace UniTube.Triggers
{
    /// <summary>
    /// Trigger for switching between Windows and Windows Phone
    /// </summary>
    public class DeviceFamilyTrigger : StateTriggerBase, ITriggerValue
    {
        private static string deviceFamily;

        static DeviceFamilyTrigger()
        {
            deviceFamily = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
        }

        public static readonly DependencyProperty DeviceFamilyProperty = DependencyProperty.Register(
            nameof(DeviceFamily), typeof(DeviceFamily), typeof(DeviceFamilyTrigger), new PropertyMetadata(DeviceFamily.Unknown, OnDeviceTypePropertyChanged));

        public DeviceFamily DeviceFamily
        {
            get { return (DeviceFamily)GetValue(DeviceFamilyProperty); }
            set { SetValue(DeviceFamilyProperty, value); }
        }

        private static void OnDeviceTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (DeviceFamilyTrigger)d;
            var val = (DeviceFamily)e.NewValue;
            switch (deviceFamily)
            {
                case "Windows.Mobile":
                    obj.IsActive = (val == DeviceFamily.Mobile);
                    break;
                case "Windows.Desktop":
                    obj.IsActive = (val == DeviceFamily.Desktop);
                    break;
                case "Windows.Team":
                    obj.IsActive = (val == DeviceFamily.Team);
                    break;
                case "Windows.IoT":
                    obj.IsActive = (val == DeviceFamily.IoT);
                    break;
                case "Windows.Holographic":
                    obj.IsActive = (val == DeviceFamily.Holographic);
                    break;
                case "Windows.Xbox":
                    obj.IsActive = (val == DeviceFamily.Xbox);
                    break;
                default:
                    obj.IsActive = (val == DeviceFamily.Unknown);
                    break;
            }           
        }

        #region ITriggerValue

        private bool m_IsActive;

        public bool IsActive
        {
            get { return m_IsActive; }
            private set
            {
                if (m_IsActive != value)
                {
                    m_IsActive = value;
                    SetActive(value);
                    IsActiveChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler IsActiveChanged;

        #endregion ITriggerValue
    }

    public enum DeviceFamily
    {
        Unknown     = 0,
        Desktop     = 1,
        Mobile      = 2,
        Team        = 3,
        IoT         = 4,
        Xbox        = 5,
        Holographic = 6
    }
}
