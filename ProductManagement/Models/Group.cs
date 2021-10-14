using System.Collections.Generic;

namespace ProductManagement.Models
{
    public class Group
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public string GroupName { get; set; }
        public List<Group> Children { get; set; }
    }
}
