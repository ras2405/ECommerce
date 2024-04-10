using System.Reflection;
using LogicInterfaces;

namespace Logic.Utils
{
    public class PromotionHelper
    {
        static public List<IPromotionImplementation> LoadPromotions(string folderPath)
        {
            List<Assembly> promotionAssemblies = new List<Assembly>();

            foreach (string dllPath in Directory.GetFiles(folderPath, "*.dll"))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(dllPath);
                    promotionAssemblies.Add(assembly);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error loading promotion assembly {dllPath}", ex);
                }
            }

            List<IPromotionImplementation> promotions = new List<IPromotionImplementation>();

            foreach (Assembly assembly in promotionAssemblies)
            {
                IEnumerable<Type> promotionTypes = assembly.GetTypes()
                    .Where(type => typeof(IPromotionImplementation).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

                foreach (Type promotionType in promotionTypes)
                {
                    IPromotionImplementation promotionInstance = (IPromotionImplementation)Activator.CreateInstance(promotionType);
                    promotions.Add(promotionInstance);
                }
            }
            return promotions;
        }
    }
}
