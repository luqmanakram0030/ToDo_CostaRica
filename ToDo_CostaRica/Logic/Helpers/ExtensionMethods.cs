

namespace ToDo_CostaRica.Helpers
{
    public static class ExtensionMethods
    {
        public static T ObtenerRecurso<T>(this string key)
        {
            //bool found = false;

            object retVal = null;

            if (Application.Current.Resources.TryGetValue(key, out retVal))
            {
                //found = true;
                return (T)retVal;
            }
            return default(T);
        }
    }
}
