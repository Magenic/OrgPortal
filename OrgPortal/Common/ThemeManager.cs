using Caliburn.Micro;
using System.Composition;
using System.Globalization;
using Windows.UI;

namespace OrgPortal.Common
{
    public class ThemeManager
    {
        public static Theme CurrentTheme
        {
            get { return Theme.Instance; }
        }
    }

    public class Theme : PropertyChangedBase
    {
        private Theme() { }


        private static Theme _instance;
        public static Theme Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Theme();

                return _instance;
            }
        }
        
        private Color _backgroundColor = Colors.White;
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                NotifyOfPropertyChange(() => BackgroundColor);
            }
        }

        private Color _primaryColor = ColorFromHex("#83C341");
        public Color PrimaryColor
        {
            get { return _primaryColor; }
            set
            {
                _primaryColor = value;
                NotifyOfPropertyChange(() => PrimaryColor);
            }
        }

        private Color _accentColor = ColorFromHex("#83C341");
        public Color AccentColor
        {
            get { return _accentColor; }
            set
            {
                _accentColor = value;
                NotifyOfPropertyChange(() => AccentColor);
            }
        }

        /// <summary>
        /// Convert color from ARGB hex string.
        /// </summary>
        public static Color ColorFromHex(string hexColor)
        {
            if (string.IsNullOrWhiteSpace(hexColor))
                return default(Color);

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            hexColor = hexColor.TrimStart('#');
            int startIndex = 0;

            if (hexColor.Length > 6)
            {
                a = byte.Parse(hexColor.Substring(startIndex, 2), NumberStyles.HexNumber);
                startIndex += 2;
            }

            r = byte.Parse(hexColor.Substring(startIndex, 2), NumberStyles.HexNumber);
            g = byte.Parse(hexColor.Substring(startIndex + 2, 2), NumberStyles.HexNumber);
            b = byte.Parse(hexColor.Substring(startIndex + 4, 2), NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }
    }
}
