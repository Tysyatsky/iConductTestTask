using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class EmployeeNode
{
    [DataMember(Order = 1)]
    public int Id { get; set; }
    
    [DataMember(Order = 2)]
    public string Name { get; set; } = "";
    
    [DataMember(Order = 3)]
    public int? ManagerId { get; set; }

    [DataMember(Order = 4)]
    public List<EmployeeNode> Employees { get; set; } = new List<EmployeeNode>();
}
