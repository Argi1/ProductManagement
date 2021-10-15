using ProductManagement.Models;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagement.Controllers
{
    [Route("api/group")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IProductRepository repository;

        public GroupController(IProductRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("tree")]
        public IEnumerable<Group> GetGroupsAsTree()
        {
            List<Group> flatGroups = repository.GetGroups().ToList();
            foreach (var group in flatGroups)
            {
                group.Children = flatGroups.Where(child => child.ParentId == group.Id).ToList();
            }

            List<Group> groups = flatGroups.Where(group => group.ParentId == 0).ToList();
            return groups;
        }
    }
}
