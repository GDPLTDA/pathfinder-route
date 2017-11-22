using System.Linq;

namespace PathFinder
{
    public static class Extensions
    {
        public static string GetDescricao(this TipoErro erro)
        {
            var memberInfo = typeof(TipoErro).GetMember(erro.ToString())
                                              .FirstOrDefault();

            if (memberInfo != null)
            {
                DescricaoAttribute attribute = (DescricaoAttribute)
                             memberInfo.GetCustomAttributes(typeof(DescricaoAttribute), false)
                                       .FirstOrDefault();
                return attribute.Descricao;
            }
            return "";
        }
    }
}
