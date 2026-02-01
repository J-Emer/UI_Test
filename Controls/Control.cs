using System;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UI.Util;

namespace UI.Controls
{
    public class Control
    {

#region State
        public string Name{get;set;} = "Control";
        public bool IsActive{get;set;} = true; //update or not update
        public bool IsVisible{get;set;} = true; //draw or not draw
        public bool HasFocus{get;set;} = false; //used by child controls 
        public object UserData{get;set;} = null; //users defined data
#endregion

#region Background
        public Texture2D Texture{get;set;} = AssetLoader.GetPixel();
        public Color BackgroundColor{get;set;} = Color.White;
#endregion

#region Size/Position
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
                HandleDirty();
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
                HandleDirty();
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
                HandleDirty();
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
                HandleDirty();
            }
        }
        public Rectangle SourceRect{get; private set;} = new Rectangle();
        public Vector2 Center => new Vector2(SourceRect.Center.X, SourceRect.Center.Y);
#endregion

#region Cursor        
        protected bool _pMouse = false;
        protected bool _cMouse = false;
#endregion

#region  Border
        public int BorderThickness{get;set;} = 1;
        public Color BorderColor{get;set;} = Color.Black;
#endregion
     
#region Events
        public Action<Control> OnInvalidate;
        public Action OnMouseEnter;
        public Action OnMouseExit;
        public Action OnMouseHover;
        public Action<Control, Input.MouseButton> OnClick;     
#endregion


        private void HandleDirty()
        {
            BeforeInvalidation();
            SourceRect = new Rectangle(X, Y, Width, Height);
            OnInvalidate?.Invoke(this);
            AfterInvalidation();
        }

        public virtual void Update()
        {           
            if(!IsActive){return;} 
            
            _pMouse = _cMouse;
            _cMouse = SourceRect.Contains(Input.MousePos);

            //left mouse click
            if(Input.GetMouseButtonDown(Input.MouseButton.Left) && _cMouse)
            {
                //clicked & hasfocus
                HasFocus = true;
                IsActive = true;
                OnClick?.Invoke(this, Input.MouseButton.Left);
                InternalMouseClick();
            }

            //right mouse click
            if(Input.GetMouseButtonDown(Input.MouseButton.Right) && _cMouse)
            {
                //clicked & hasfocus
                HasFocus = true;
                IsActive = true;
                OnClick?.Invoke(this, Input.MouseButton.Right);
                InternalMouseClick();
            }

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
                HasFocus = false;
            }            

        }
        public virtual void Draw(SpriteBatch _spritebatch, GraphicsDeviceManager graphics)
        {
            if(!IsVisible){return;}

            _spritebatch.Draw(Texture, SourceRect, BackgroundColor);

            //border
            if(BorderThickness == 0){return;}
            _spritebatch.Draw(Texture, new Rectangle(SourceRect.Left, SourceRect.Top, SourceRect.Width, BorderThickness), BorderColor);//top
            _spritebatch.Draw(Texture, new Rectangle(SourceRect.Right - BorderThickness, SourceRect.Top, BorderThickness, SourceRect.Height), BorderColor);//right
            _spritebatch.Draw(Texture, new Rectangle(SourceRect.Left, SourceRect.Bottom - BorderThickness, SourceRect.Width, BorderThickness), BorderColor);//bottom
            _spritebatch.Draw(Texture, new Rectangle(SourceRect.Left, SourceRect.Top, BorderThickness, SourceRect.Height), BorderColor);//left
        
      

        }


        protected virtual void BeforeInvalidation(){}
        protected virtual void AfterInvalidation(){}
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