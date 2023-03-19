using System.Reflection;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public static class UtilitatiCrossThread
    {
        private delegate void DelegatProprietateCrossThread(Control control,
                                                            string numeProprietate,
                                                            object valoareProprietate);

        public static string PrimesteTextulDinAltThread(Control control)
        {
            string textProprietate = System.String.Empty;
            if (control.InvokeRequired)
            {
                control.Invoke(new MethodInvoker(delegate { textProprietate = control.Text; }));
            }
            else
            {
                textProprietate = control.Text;
            }
            return textProprietate;
        }

        public static void SeteazaProprietateaDinAltThread(Control control,
                                                           string numeProprietate,
                                                           object valoareProprietate)
        {
            if (control.IsDisposed == false)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(new DelegatProprietateCrossThread(SeteazaProprietateaDinAltThread),
                                                new object[] { control, numeProprietate, valoareProprietate });
                }
                else
                {
                    control.GetType().InvokeMember(
                        numeProprietate,
                        BindingFlags.SetProperty,
                        null,
                        control,
                        new object[] { valoareProprietate });
                }
            }
        }
    }
}