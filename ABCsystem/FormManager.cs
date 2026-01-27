using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABCsystem
{
    public static class FormManager
    {
        private static Dictionary<Type, Form> _forms = new Dictionary<Type, Form>();

        public static T GetForm<T>() where T : Form, new()
        {
            var type = typeof(T);

            if (_forms.ContainsKey(type))
            {
                if (_forms[type].IsDisposed)
                    _forms[type] = new T();
            }
            else
            {
                _forms[type] = new T();
            }

            return (T)_forms[type];
        }
    }
}
