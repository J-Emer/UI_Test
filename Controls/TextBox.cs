using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UI.Util;

namespace UI.Controls
{
    public class TextBox : Control
    {
        public string Text { get; set; } = "";
        public SpriteFont Font { get; set; } = AssetLoader.DefaultFont;
        public Color TextColor { get; set; } = Color.Black;
        public int Padding { get; set; } = 4;

        // Caret
        private float _caretTimer = 0f;
        private bool _showCaret = true;
        private const float CARET_BLINK_RATE = 0.5f;
        protected int _caretIndex = 0;

        public Action<string> OnTextChanged;
        private string _previousText = "";

 

        public TextBox() : base()
        {
            Width = 100;
            Height = Font.LineSpacing + Padding * 2;
            BackgroundColor = Color.White;
        }

        public override void Update()
        {
            base.Update();

            if (!HasFocus)
            {
                _showCaret = false;
                return;
            }

            HandleTextInput();
            _caretIndex = Math.Clamp(_caretIndex, 0, Text.Length);
            UpdateCaretBlink();
            DetectTextChange();
        }
        private void DetectTextChange()
        {
            if (_previousText != Text)
            {
                _previousText = Text;
                OnTextChanged?.Invoke(Text);
            }
        }
        private void HandleTextInput()
        {
            // Characters
            bool shift = Input.GetKey(Keys.LeftShift) || Input.GetKey(Keys.RightShift);

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (!Input.GetKeyDown(key))
                    continue;

                // Special keys
                if (HandleSpecialKeys(key))
                {
                    IsDirty = true;
                    return;
                }

                char? c = KeyToChar(key, shift);

                if (c.HasValue)
                {
                    if (IsCharAllowed(c.Value))
                    {
                        Text = Text.Insert(_caretIndex, c.Value.ToString());
                        _caretIndex++;
                    }
                    IsDirty = true;
                }
            }

            // Backspace
            if (Input.GetKeyDown(Keys.Back) && Text.Length > 0)
            {
                Text = Text.Substring(0, Text.Length - 1);
                IsDirty = true;
            }

            // Optional: Enter handling
            if (Input.GetKeyDown(Keys.Enter))
            {
                HasFocus = false;
            }
           
        }
        private bool HandleSpecialKeys(Keys key)
        {
            switch (key)
            {
                case Keys.Back:
                    if (_caretIndex > 0 && Text.Length > 0)
                    {
                        Text = Text.Remove(_caretIndex - 1, 1);
                        _caretIndex--;
                    }
                    return true;

                case Keys.Enter:
                    HasFocus = false;
                    return true;

                case Keys.Space:
                    Text = Text.Insert(_caretIndex, " ");
                    _caretIndex++;
                    return true;

                case Keys.Tab:
                    const string tabSpaces = "    ";
                    Text = Text.Insert(_caretIndex, tabSpaces);
                    _caretIndex += tabSpaces.Length;
                    return true;

                case Keys.Left:
                    _caretIndex = Math.Max(0, _caretIndex - 1);
                    return true;

                case Keys.Right:
                    _caretIndex = Math.Min(Text.Length, _caretIndex + 1);
                    return true;

            }

            return false;
        }
        private char? KeyToChar(Keys key, bool shift)
        {
            // Letters
            if (key >= Keys.A && key <= Keys.Z)
            {
                char c = (char)('a' + (key - Keys.A));
                return shift ? char.ToUpper(c) : c;
            }

            // Numbers (top row)
            if (key >= Keys.D0 && key <= Keys.D9)
            {
                return shift ? ShiftNumber(key): (char)('0' + (key - Keys.D0));
            }

            // Numpad
            if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
            {
                return (char)('0' + (key - Keys.NumPad0));
            }

            // Symbols
            return key switch
            {
                Keys.OemPeriod     => shift ? '>' : '.',
                Keys.Decimal       => '.',
                Keys.OemComma      => shift ? '<' : ',',
                Keys.OemMinus      => shift ? '_' : '-',
                Keys.OemPlus       => shift ? '+' : '=',
                Keys.OemQuestion   => shift ? '?' : '/',
                Keys.OemSemicolon  => shift ? ':' : ';',
                Keys.OemQuotes     => shift ? '"' : '\'',
                Keys.OemOpenBrackets  => shift ? '{' : '[',
                Keys.OemCloseBrackets => shift ? '}' : ']',
                Keys.OemPipe       => shift ? '|' : '\\',
                Keys.OemTilde      => shift ? '~' : '`',
                _ => null
            };
        }
        private char ShiftNumber(Keys key)
        {
            return key switch
            {
                Keys.D1 => '!',
                Keys.D2 => '@',
                Keys.D3 => '#',
                Keys.D4 => '$',
                Keys.D5 => '%',
                Keys.D6 => '^',
                Keys.D7 => '&',
                Keys.D8 => '*',
                Keys.D9 => '(',
                Keys.D0 => ')',
                _ => '\0'
            };
        }
        private void UpdateCaretBlink()
        {
            _caretTimer += Time.DeltaTime;

            if (_caretTimer >= CARET_BLINK_RATE)
            {
                _caretTimer = 0f;
                _showCaret = !_showCaret;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw background + border
            base.Draw(spriteBatch);

            if (Font == null)
                return;
        
            Rectangle textClipRect = new Rectangle(
                SourceRect.X + Padding,
                SourceRect.Y + Padding,
                SourceRect.Width - Padding * 2,
                SourceRect.Height - Padding * 2
            );

            Vector2 textPos = new Vector2(textClipRect.X, textClipRect.Y);
            spriteBatch.DrawString(Font, Text, textPos, TextColor);
            DrawCaret(spriteBatch, textPos);
        }



        private void DrawCaret(SpriteBatch spriteBatch, Vector2 textPos)
        {
            if (!HasFocus || !_showCaret)
                return;

            string leftText =
                _caretIndex > 0
                    ? Text.Substring(0, _caretIndex)
                    : string.Empty;

            float caretX =
                textPos.X + Font.MeasureString(leftText).X;

            Rectangle caretRect = new Rectangle(
                (int)caretX,
                (int)textPos.Y,
                1,
                Font.LineSpacing
            );

            spriteBatch.Draw(Texture, caretRect, TextColor);
        }
        protected override void InternalMouseClick()
        {
            HasFocus = true;
            _caretIndex = Text.Length; 
        }
        protected virtual bool IsCharAllowed(char c)
        {
            return true;
        }

    }
}
