using System.Collections.Generic;

namespace JayoPoiyomiPlugin
{
    public class ShaderPropertyDetails : Dictionary<string, string> { }
    public class ShaderPropertyListItem : Dictionary<string, ShaderPropertyDetails> { }
    public class ShaderPropertyListData : Dictionary<string, ShaderPropertyListItem> { }
}
