using System;
using Microsoft.Xna.Framework.Graphics;

namespace UI.Controls
{
    public class NumericTextBox : TextBox
    {
        public bool AllowDecimals { get; set; } = true;
        public bool AllowNegative { get; set; } = true;


        protected override bool IsCharAllowed(char c)
        {
            // Digits always allowed
            if (char.IsDigit(c))
                return true;

            // Decimal point
            if (AllowDecimals && c == '.' && !Text.Contains("."))
                return true;

            // Negative sign (only at start)
            if (AllowNegative && c == '-' && _caretIndex == 0 && !Text.StartsWith("-"))
                return true;

            return false;
        }
    }
}
