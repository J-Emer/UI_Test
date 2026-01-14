using System.Collections.Generic;
using Microsoft.Xna.Framework;
using UI.Controls;

namespace UI.Util
{
    public abstract class Layout
    {
        public abstract void DoLayout(Rectangle sourceRect, List<Control> controls, int padding);
    }

    public class HorizontalLayout : Layout
    {
        public override void DoLayout(Rectangle sourceRect, List<Control> controls, int padding)
        {
            int xPos = padding + sourceRect.X;
            int yPos = padding + sourceRect.Y;

            foreach (var item in controls)
            {
                item.X = xPos;
                item.Y = yPos;

                yPos += padding + item.Height;
            }
        }
    }

    public class VerticalLayout : Layout
    {
        public override void DoLayout(Rectangle sourceRect, List<Control> controls, int padding)
        {
            int xPos = padding + sourceRect.X;
            int yPos = padding + sourceRect.Y;

            foreach (var item in controls)
            {
                item.X = xPos;
                item.Y = yPos;

                xPos += padding + item.Width;
            }              
        } 
    }

    public class RowLayout : Layout
    {
        public override void DoLayout(Rectangle sourceRect, List<Control> controls, int padding)
        {
            int xPos = padding + sourceRect.X;
            int yPos = padding + sourceRect.Y;

            int width = sourceRect.Width - (padding + padding);

            foreach (var item in controls)
            {
                item.X = xPos;
                item.Y = yPos;

                item.Width = width;

                yPos += padding + item.Height;
            }
        }
    }    

    public class ColumnLayout : Layout
    {
        public override void DoLayout(Rectangle sourceRect, List<Control> controls, int padding)
        {
            int xPos = padding + sourceRect.X;
            int yPos = padding + sourceRect.Y;

            int Height = sourceRect.Height - (padding + padding);

            foreach (var item in controls)
            {
                item.X = xPos;
                item.Y = yPos;

                item.Height = Height;

                xPos += padding + item.Width;
            }
        }
    }   

    public class GridLayout : Layout
    {
        public int Columns{get;set;} = 1;
        public int Rows{get;set;} = 1;

        public GridLayout(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
        }

        public override void DoLayout(Rectangle sourceRect, List<Control> controls, int padding)
        {
            int paddingTotal = (Columns + 1) * padding;
            int size = (sourceRect.Width - paddingTotal) / Columns;

            int xPos = sourceRect.X + padding;
            int yPos = sourceRect.Y + padding;

            int index = 0;

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if(index >= controls.Count){return;}

                    Control item = controls[index];

                    item.X = xPos;
                    item.Y = yPos;
                    item.Width = size;
                    item.Height = size;

                    xPos += size + padding;

                    index += 1;
                }
                
                xPos = sourceRect.X + padding;
                yPos += size + padding;
            }
        }
    }

    //---only lay's out the 1st control
    public class StretchLayout : Layout
    {
        public override void DoLayout(Rectangle sourceRect, List<Control> controls, int padding)
        {
            if(controls.Count <= 0){return;}

            Control item = controls[0];

            item.X = sourceRect.X + padding;
            item.Y = sourceRect.Y + padding;
            item.Width = sourceRect.Width - (padding * 2);
            item.Height = sourceRect.Height - (padding * 2);

            return;
        }
    }



}