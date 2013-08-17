using System.Windows.Automation;

namespace PingPongTest
{
    internal static class ConditionHelper
    {
        internal static PropertyCondition FromName(string name, PropertyConditionFlags propertyConditionFlags = PropertyConditionFlags.None)
        {
            return FromProperty(AutomationElementIdentifiers.NameProperty, name, propertyConditionFlags);
        }

        internal static PropertyCondition FromId(string id, PropertyConditionFlags propertyConditionFlags = PropertyConditionFlags.None)
        {
            return FromProperty(AutomationElementIdentifiers.AutomationIdProperty, id, propertyConditionFlags);
        }

        internal static PropertyCondition FromControlType(ControlType controlType)
        {
            return FromProperty(AutomationElementIdentifiers.ControlTypeProperty, controlType);
        }

        internal static PropertyCondition FromIsEnabled(bool enabled)
        {
            return FromProperty(AutomationElementIdentifiers.IsEnabledProperty, enabled);
        }

        internal static PropertyCondition FromClassName(string className, PropertyConditionFlags propertyConditionFlags = PropertyConditionFlags.None)
        {
            return FromProperty(AutomationElementIdentifiers.ClassNameProperty, className, propertyConditionFlags);
        }

        internal static PropertyCondition FromProperty(AutomationProperty property, object value, PropertyConditionFlags propertyConditionFlags = PropertyConditionFlags.None)
        {
            return new PropertyCondition(property, value, propertyConditionFlags);
        }
    }
}
