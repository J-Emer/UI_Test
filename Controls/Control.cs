using System;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Util;

namespace UI.Controls
{
    public class Control
    {
        public string Name{get;set;} = "Control";
        public bool IsActive{get;set;} = true;
        public bool IsVisible{get;set;} = true;
        public bool HasFocus{get;set;} = false;
        public Texture2D Texture{get;set;} = AssetLoader.GetPixel();
        public bool IsDirty{get; set;} = true;
        private int _x;
        private int _y;
        private int _width;
        private int _height;
        public int X
        {
            get
            {
                return _x;
            }            
            set
            {
                _x = value;
                IsDirty = true; 
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }            
            set
            {
                _y = value;
                IsDirty = true; 
            }
        }

        public int Width
        {
            get
            {
                return _width;
            }            
            set
            {
                _width = value;
                IsDirty = true; 
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }            
            set
            {
                _height = value;
                IsDirty = true; 
            }
        }
        public Rectangle SourceRect{get; private set;} = new Rectangle();
        public Color BackgroundColor{get;set;} = Color.White;
        private bool _pMouse = false;
        private bool _cMouse = false;
        public Action OnMouseEnter;
        public Action OnMouseExit;
        public Action OnMouseHover;
        public Action<Control> OnClick;
        public int BorderThickness{get;set;} = 1;
        public Color BorderColor{get;set;} = Color.Black;
        public object UserData{get;set;} = null;
     

        protected virtual void HandleDirty()
        {
            SourceRect = new Rectangle(X, Y, Width, Height);
            IsDirty = false;
        }
        public virtual void Update()
        {
            if(IsDirty)
            {
                HandleDirty();
            }
            
            if(!IsActive){return;} 
            
            _pMouse = _cMouse;
            _cMouse = SourceRect.Contains(Input.MousePos);

            if(Input.GetMouseButtonDown(Input.MouseButton.Left) && _cMouse)
            {
                //clicked & hasfocus
                HasFocus = true;
                IsActive = true;
                OnClick?.Invoke(this);
                InternalMouseClick();
            }

            // put this here because the control needs to a way to check if it was clicked on. putting the IsActive check at the beginning
            // of Update would perpetually mean this control is not active
            //if(!IsActive){return;} 

            if(!_pMouse && _cMouse)
            {
                //enter
                OnMouseEnter?.Invoke();
                InternalMouseEnter();
            }

            if(_pMouse && _cMouse)
            {
                //hover
                OnMouseHover?.Invoke();
                InternalMouseHover();
            }

            if(_pMouse && !_cMouse)
            {
                //exit
                OnMouseExit?.Invoke();
                InternalMouseExit();
            }

            if(Input.GetMouseButtonDown(Input.MouseButton.Left) && !_cMouse)
            {
                //lost focus
                HasFocus = false;
            }            

        }
        public virtual void Draw(SpriteBatch _spritebatch)
        {
            if(!IsVisible){return;}

            _spritebatch.Draw(Texture, SourceRect, BackgroundColor);

            //border
            _spritebatch.Draw(Texture, new Rectangle(SourceRect.Left, SourceRect.Top, SourceRect.Width, BorderThickness), BorderColor);//top
            _spritebatch.Draw(Texture, new Rectangle(SourceRect.Right - BorderThickness, SourceRect.Top, BorderThickness, SourceRect.Height), BorderColor);//right
            _spritebatch.Draw(Texture, new Rectangle(SourceRect.Left, SourceRect.Bottom - BorderThickness, SourceRect.Width, BorderThickness), BorderColor);//bottom
            _spritebatch.Draw(Texture, new Rectangle(SourceRect.Left, SourceRect.Top, BorderThickness, SourceRect.Height), BorderColor);//left
        }

        protected virtual void InternalMouseEnter(){}
        protected virtual void InternalMouseHover(){}
        protected virtual void InternalMouseExit(){}
        protected virtual void InternalMouseClick(){}





        public override string ToString()
        {
            return $"Type: {this.GetType()} | Name: {Name} | SourceRect: {SourceRect}";
        }


    }
}