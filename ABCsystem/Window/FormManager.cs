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
        private static readonly Dictionary<Type, Form> _forms = new Dictionary<Type, Form>();

        public static T GetForm<T>() where T : Form, new()
        {
            var type = typeof(T);

            Form form;
            if (_forms.TryGetValue(type, out form))
            {
                if (form != null && !form.IsDisposed)
                    return (T)form;

                _forms.Remove(type);
            }

            var newForm = new T();
            _forms[type] = newForm;

            newForm.FormClosed += (s, e) =>
            {
                Form existing;
                if (_forms.TryGetValue(type, out existing))
                {
                    if (ReferenceEquals(existing, s))
                        _forms.Remove(type);
                }
            };

            return newForm;
        }
    }
}
