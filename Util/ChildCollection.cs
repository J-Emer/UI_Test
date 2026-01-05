using System;
using System.Collections.Generic;
using System.Linq;
using UI.Controls;

namespace UI.Util
{
    public class ChildCollection
    {
        public List<Control> Controls{get; private set;} = new List<Control>();

        public Action OnChildrenChanged;

        public void Add(Control _control)
        {
            Controls.Add(_control);
            OnChildrenChanged?.Invoke();
        }
        public void Remove(Control _control)
        {
            Controls.Remove(_control);
            OnChildrenChanged?.Invoke();
        }    
        public Control Find(string _name)
        {
            return Controls.First(x => x.Name == _name);
        }          
    }

    public class ChildCollection<T> where T : Control
    {
        public List<T> Controls{get; private set;} = new List<T>();

        public Action OnChildrenChanged;

        public void Add(T _control)
        {
            Controls.Add(_control);
            OnChildrenChanged?.Invoke();
        }
        public void Remove(T _control)
        {
            Controls.Remove(_control);
            OnChildrenChanged?.Invoke();
        }    
        public T Find(string _name)
        {
            return (T)Controls.First(x => x.Name == _name);
        } 
    }
}