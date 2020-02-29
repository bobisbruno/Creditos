using System.Collections.Generic;
using AdminInformes.Entidades;

namespace AdminInformes.Negocio
{
    public class MenuDeTableros
    {
    public List<ItemMenuTablero> ObtenerTableros()
        {
            AccesoDatos.Tableros t = new AccesoDatos.Tableros();
            List<ItemMenuTablero> resultado = t.ObtenerTableros();
            return resultado;
        }
    }
}