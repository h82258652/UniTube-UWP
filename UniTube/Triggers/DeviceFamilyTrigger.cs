using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public DeviceFamilyTrigger()
        {

        }

        public DeviceFamily DeviceFamily
        {
            get { return (DeviceFamily)GetValue(DeviceFamilyProperty); }
            set { SetValue(DeviceFamilyProperty, value); }
        }

        public static readonly DependencyProperty DeviceFamilyProperty = DependencyProperty.Register(
            nameof(DeviceFamily), typeof(DeviceFamily), typeof(DeviceFamilyTrigger), new PropertyMetadata(DeviceFamily.Unknown, OnDeviceTypePropertyChanged));

        private static void OnDeviceTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (DeviceFamilyTrigger)d;
            var val = (DeviceFamily)e.NewValue;
            if (deviceFamily == "Windows.Mobile")
                obj.IsActive = (val == DeviceFamily.Mobile);
            else if (deviceFamily == "Windows.Desktop")
                obj.IsActive = (val == DeviceFamily.Desktop);
            else if (deviceFamily == "Windows.Team")
                obj.IsActive = (val == DeviceFamily.Team);
            else if (deviceFamily == "Windows.IoT")
                obj.IsActive = (val == DeviceFamily.IoT);
            else if (deviceFamily == "Windows.Holographic")
                obj.IsActive = (val == DeviceFamily.Holographic);
            else if (deviceFamily == "Windows.Xbox")
                obj.IsActive = (val == DeviceFamily.Xbox);
            else
                obj.IsActive = (val == DeviceFamily.Unknown);
            
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
        Unknown = 0,
        Desktop = 1,
        Mobile = 2,
        Team = 3,
        IoT = 4,
        Xbox = 5,
        Holographic = 6
    }
}
